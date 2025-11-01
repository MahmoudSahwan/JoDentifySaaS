using AutoMapper;
using JoDentify.Application.DTOs.Dashboard;
using JoDentify.Application.Interfaces;
using JoDentify.Core;
using JoDentify.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace JoDentify.Application.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly Guid? _clinicId;

        public DashboardService(
            ApplicationDbContext context,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
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

        public async Task<DashboardStatsDto> GetDashboardStatsAsync()
        {
            CheckClinicAccess();

            var now = DateTime.UtcNow;
            var today = now.Date;
            var startOfMonth = new DateTime(now.Year, now.Month, 1);

            var patientQuery = _context.Patients.Where(p => p.ClinicId == _clinicId.Value);
            var appointmentQuery = _context.Appointments.Where(a => a.ClinicId == _clinicId.Value);
            var paymentQuery = _context.PaymentTransactions.Where(pt => pt.ClinicId == _clinicId.Value);
            var invoiceQuery = _context.Invoices.Where(i => i.ClinicId == _clinicId.Value);

            var totalPatients = await patientQuery.CountAsync();

            var newPatientsThisMonth = await patientQuery
                .CountAsync(p => p.CreatedAt >= startOfMonth);

            var todayAppointments = await appointmentQuery
                .CountAsync(a => a.StartTime.Date == today && a.Status != AppointmentStatus.Canceled);

            var upcomingAppointments = await appointmentQuery
                .CountAsync(a => a.StartTime > now && a.Status == AppointmentStatus.Scheduled);

            var totalRevenue = await paymentQuery
                .SumAsync(pt => pt.Amount);

            var monthlyRevenue = await paymentQuery
                .Where(pt => pt.PaymentDate >= startOfMonth)
                .SumAsync(pt => pt.Amount);

            var totalOutstanding = await invoiceQuery
                .Where(i => i.Status == InvoiceStatus.Pending || i.Status == InvoiceStatus.PartiallyPaid)
                .SumAsync(i => i.TotalAmount - i.AmountPaid);

            return new DashboardStatsDto
            {
                TotalPatients = totalPatients,
                NewPatientsThisMonth = newPatientsThisMonth,
                TodayAppointments = todayAppointments,
                UpcomingAppointments = upcomingAppointments,
                TotalRevenue = totalRevenue,
                MonthlyRevenue = monthlyRevenue,
                TotalOutstanding = totalOutstanding
            };
        }

        public async Task<IEnumerable<RecentPatientDto>> GetRecentPatientsAsync(int count = 5)
        {
            CheckClinicAccess();

            var recentPatients = await _context.Patients
                .Where(p => p.ClinicId == _clinicId.Value)
                .OrderByDescending(p => p.CreatedAt)
                .Take(count)
                .ToListAsync();

            return _mapper.Map<IEnumerable<RecentPatientDto>>(recentPatients);
        }
    }
}