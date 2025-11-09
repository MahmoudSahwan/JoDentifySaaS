using AutoMapper;
using JoDentify.Application.DTOs.Dashboard;
using JoDentify.Application.Interfaces;
using JoDentify.Core;
using JoDentify.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Globalization;

namespace JoDentify.Application.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly ApplicationDbContext _context;
        private readonly Guid? _clinicId;
        private readonly IMapper _mapper;

        public DashboardService(
            ApplicationDbContext context,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;

            var user = httpContextAccessor.HttpContext?.User;
            var clinicIdClaim = user?.Claims.FirstOrDefault(c => c.Type == "clinicId");
            if (clinicIdClaim != null && Guid.TryParse(clinicIdClaim.Value, out Guid clinicId))
            {
                _clinicId = clinicId;
            }
            else
            {
                _clinicId = null;
            }
        }

        private void CheckClinicAccess()
        {
            if (_clinicId == null)
            {
                throw new UnauthorizedAccessException("User is not associated with a clinic.");
            }
        }

        // (دي الدالة الرئيسية)
        public async Task<DashboardStatsDto> GetDashboardStatsAsync()
        {
            CheckClinicAccess();
            var clinicId = _clinicId.Value;

            // --- (هنا التصليح النهائي) ---
            // (هنلغي Task.WhenAll وهنشغلهم واحد ورا التاني)

            // --- 1. حساب الكروت ---
            var visitorsToday = await _context.Appointments
                .Where(a => a.ClinicId == clinicId && a.StartTime.Date == DateTime.UtcNow.Date)
                .CountAsync();

            var totalPatients = await _context.Patients
                .Where(p => p.ClinicId == clinicId)
                .CountAsync();

            var apptConfirmation = await _context.Appointments
                .Where(a => a.ClinicId == clinicId && a.Status == AppointmentStatus.Confirmed)
                .CountAsync();

            var overduePayment = await _context.Invoices
                .Where(i => i.ClinicId == clinicId && i.Status == InvoiceStatus.Pending && i.DueDate < DateTime.UtcNow)
                .SumAsync(i => i.TotalAmount - i.AmountPaid);

            // --- 2. حساب الرسوم البيانية ---
            var patientStats = await _context.Invoices
                .Where(i => i.ClinicId == clinicId)
                .GroupBy(i => i.Status)
                .Select(g => new ChartDataDto
                {
                    Name = g.Key.ToString(),
                    Value = g.Count()
                }).ToListAsync();

            var sixMonthsAgo = DateTime.UtcNow.AddMonths(-6);
            var revenueStatsResult = await _context.PaymentTransactions
                .Where(pt => pt.ClinicId == clinicId && pt.PaymentDate > sixMonthsAgo)
                .GroupBy(pt => new { pt.PaymentDate.Year, pt.PaymentDate.Month })
                .Select(g => new
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Value = g.Sum(pt => pt.Amount)
                })
                .OrderBy(g => g.Year).ThenBy(g => g.Month)
                .ToListAsync();

            // --- 3. حساب الجدول ---
            var recentAppointments = await _context.Appointments
                .Where(a => a.ClinicId == clinicId)
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .OrderByDescending(a => a.StartTime)
                .Take(5)
                .Select(a => new RecentAppointmentDto
                {
                    PatientName = a.Patient != null ? a.Patient.FullName : "N/A",
                    AppointmentDate = a.StartTime,
                    Service = a.Title,
                    DoctorName = a.Doctor != null ? a.Doctor.FullName : "N/A",
                    Status = a.Status.ToString()
                }).ToListAsync();

            // --- (نهاية التصليح) ---

            var revenueStats = revenueStatsResult
                .Select(g => new ChartDataDto
                {
                    Name = new DateTime(g.Year, g.Month, 1).ToString("MMM", CultureInfo.InvariantCulture),
                    Value = g.Value
                }).ToList();

            var dashboardStats = new DashboardStatsDto
            {
                VisitorsToday = visitorsToday,
                TotalPatients = totalPatients,
                ApptConfirmation = apptConfirmation,
                OverduePayment = overduePayment,
                PatientStats = patientStats,
                RevenueStats = revenueStats,
                RecentAppointments = recentAppointments
            };

            return dashboardStats;
        }
    }
}