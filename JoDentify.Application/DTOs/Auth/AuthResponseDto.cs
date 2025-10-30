using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoDentify.Application.DTOs.Auth
{
    // الكلاس ده بيمثل الرد اللي هنرجعه للمستخدم بعد التسجيل أو اللوجين
    public class AuthResponseDto
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
