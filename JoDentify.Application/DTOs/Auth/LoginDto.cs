using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoDentify.Application.DTOs.Auth
{
    // الكلاس ده بيمثل الداتا اللي هنستقبلها من المستخدم وهو بيعمل لوجين
    public class LoginDto
    {
        [Required(ErrorMessage = "اسم المستخدم أو البريد الإلكتروني مطلوب")]
        public string UserNameOrEmail { get; set; }

        [Required(ErrorMessage = "كلمة المرور مطلوبة")]
        public string Password { get; set; }
    }
}

