using System.Collections.Generic;

namespace JoDentify.Application.DTOs.Dashboard
{
    public class DashboardStatsDto
    {
        // (1. الكروت اللي فوق)
        public int VisitorsToday { get; set; }
        public int TotalPatients { get; set; }
        public int ApptConfirmation { get; set; }
        public decimal OverduePayment { get; set; }

        // (2. الرسوم البيانية)
        public List<ChartDataDto> PatientStats { get; set; } = new List<ChartDataDto>(); // (لـ Pie Chart)
        public List<ChartDataDto> RevenueStats { get; set; } = new List<ChartDataDto>(); // (لـ Line Chart)

        // (3. الجدول)
        public List<RecentAppointmentDto> RecentAppointments { get; set; } = new List<RecentAppointmentDto>();
    }
}