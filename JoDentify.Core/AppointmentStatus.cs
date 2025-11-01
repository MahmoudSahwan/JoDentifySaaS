namespace JoDentify.Core
{
    
    public enum AppointmentStatus
    {
        Scheduled,  // 0 - (اتحجز)
        Confirmed,  // 1 - (اتأكد عليه)
        Completed,  // 2 - (خلص)
        Canceled,   // 3 - (المريض لغاه)
        NoShow      // 4 - (المريض مجاش)
    }
}