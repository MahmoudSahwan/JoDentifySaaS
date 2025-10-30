using AutoMapper;
using JoDentify.Application.DTOs.Auth;
using JoDentify.Application.Interfaces;
using JoDentify.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JoDentify.Application.Services
{
    // ده التنفيذ الفعلي لـ IAuthService
    // ده "العقل" بتاع عملية التسجيل واللوجين
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<ApplicationUser> userManager, IMapper mapper, IConfiguration configuration)
        {
            _userManager = userManager;
            _mapper = mapper;
            _configuration = configuration;
        }

        // --- عملية التسجيل ---
        public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto)
        {
            // 1. حول الـ DTO لـ Entity باستخدام AutoMapper
            var user = _mapper.Map<ApplicationUser>(registerDto);

            // 2. حاول تسجل المستخدم في قاعدة البيانات
            var result = await _userManager.CreateAsync(user, registerDto.Password);

            // 3. لو التسجيل فشل (زي اسم مستخدم مكرر)
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return new AuthResponseDto { IsSuccess = false, Message = $"Registration failed: {errors}" };
            }

            // 4. لو التسجيل نجح، اعمل Token
            return GenerateJwtToken(user);
        }

        // --- عملية تسجيل الدخول ---
        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {
            // 1. دور على المستخدم بالاسم أو الايميل
            var user = await _userManager.FindByNameAsync(loginDto.UserNameOrEmail);
            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(loginDto.UserNameOrEmail);
            }

            // 2. لو المستخدم مش موجود أصلاً
            if (user == null)
            {
                return new AuthResponseDto { IsSuccess = false, Message = "Invalid username or password." };
            }

            // 3. اتأكد إن الباسورد صح
            var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!isPasswordValid)
            {
                return new AuthResponseDto { IsSuccess = false, Message = "Invalid username or password." };
            }

            // 4. لو الباسورد صح، اعمل Token
            return GenerateJwtToken(user);
        }

        // --- وظيفة صناعة الـ Token ---
        private AuthResponseDto GenerateJwtToken(ApplicationUser user)
        {
            // 1. جيب الـ Secret Key من appsettings.json
            var jwtKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            // 2. حضّر "البيانات" اللي هتتخزن جوه الـ Token
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id), // User ID
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // Token ID
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim("fullname", user.FullName) // معلومة إضافية
            };

            // 3. حضّر الـ Token
            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(Convert.ToInt32(_configuration["JWT:DurationInDays"])), // مدة الصلاحية
                signingCredentials: new SigningCredentials(jwtKey, SecurityAlgorithms.HmacSha256)
            );

            // 4. اكتب الـ Token في شكل String
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            // 5. رجع الرد الناجح
            return new AuthResponseDto
            {
                IsSuccess = true,
                Message = "Authentication successful!",
                Token = tokenString,
                ExpiresAt = token.ValidTo
            };
        }
    }
}

