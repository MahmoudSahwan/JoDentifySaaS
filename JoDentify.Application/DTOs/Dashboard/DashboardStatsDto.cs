namespace JoDentify.Application.DTOs.Dashboard
{
    public class DashboardStatsDto
    {
        public int TotalPatients { get; set; }
        public int NewPatientsThisMonth { get; set; }

        public int TodayAppointments { get; set; }
        public int UpcomingAppointments { get; set; }

        public decimal TotalRevenue { get; set; }
        public decimal MonthlyRevenue { get; set; }
        public decimal TotalOutstanding { get; set; }
    }
}