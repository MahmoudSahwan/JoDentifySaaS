using Microsoft.AspNetCore.Identity; // ده أهم using
using System.ComponentModel.DataAnnotations;

namespace JoDentify.Core
{
    // اتأكد إنه بيرث من IdentityUser
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty; // تم التعديل هنا

        // ممكن نضيف خصائص تانية هنا قدام زي "RoleInClinic"
    }
}