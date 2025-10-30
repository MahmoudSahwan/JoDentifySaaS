using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoDentify.Application.DTOs.Auth
{
    // الكلاس ده بيمثل الداتا اللي هنستقبلها من المستخدم وهو بيسجل
    public class RegisterDto
    {
        [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
        [EmailAddress(ErrorMessage = "صيغة البريد الإلكتروني غير صحيحة")]
        public string Email { get; set; }

        [Required(ErrorMessage = "اسم المستخدم مطلوب")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "كلمة المرور مطلوبة")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "كلمة المرور يجب أن تكون 6 أحرف على الأقل")]
        public string Password { get; set; }

        [Required(ErrorMessage = "الاسم بالكامل مطلوب")]
        public string FullName { get; set; } // دي الخاصية الإضافية اللي في ApplicationUser
    }
}

