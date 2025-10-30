using JoDentify.Application.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoDentify.Application.Interfaces
{
    // ده العقد اللي الـ AuthService هيلتزم بيه
    public interface IAuthService
    {
        // عملية التسجيل
        Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto);

        // عملية تسجيل الدخول
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
    }
}
