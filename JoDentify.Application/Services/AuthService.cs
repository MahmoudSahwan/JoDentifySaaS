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

    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IRepository<Clinic> _clinicRepository;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            IMapper mapper,
            IConfiguration configuration,
            IRepository<Clinic> clinicRepository)
        {
            _userManager = userManager;
            _mapper = mapper;
            _configuration = configuration;
            _clinicRepository = clinicRepository;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto)
        {
            var user = _mapper.Map<ApplicationUser>(registerDto);
            user.UserName = registerDto.Email;

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                return new AuthResponseDto
                {
                    IsAuthenticated = false,
                    Message = $"Registration failed: {string.Join(", ", errors)}",
                };
            }

            var newClinic = new Clinic
            {
                Name = registerDto.ClinicName,
                OwnerId = user.Id
            };

            try
            {
                await _clinicRepository.AddAsync(newClinic);
            }
            catch (Exception)
            {
                await _userManager.DeleteAsync(user);
                return new AuthResponseDto
                {
                    IsAuthenticated = false,
                    Message = "Registration failed: Could not create the clinic."
                };
            }

            user.ClinicId = newClinic.Id;
            var updateResult = await _userManager.UpdateAsync(user);

            if (!updateResult.Succeeded)
            {
              
                await _clinicRepository.DeleteAsync(newClinic.Id);
               

                await _userManager.DeleteAsync(user);
                return new AuthResponseDto
                {
                    IsAuthenticated = false,
                    Message = "Registration failed: Could not link user to the clinic."
                };
            }

            return await GenerateJwtToken(user, newClinic.Id);
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.UserNameOrEmail);
            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(loginDto.UserNameOrEmail);
            }

            if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                return new AuthResponseDto
                {
                    IsAuthenticated = false,
                    Message = "Invalid credentials"
                };
            }

            return await GenerateJwtToken(user, user.ClinicId);
        }

       
        private Task<AuthResponseDto> GenerateJwtToken(ApplicationUser user, Guid? clinicId)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddDays(7);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                new Claim("uid", user.Id)
            };

            if (clinicId.HasValue)
            {
                claims.Add(new Claim("clinicId", clinicId.Value.ToString()));
            }

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            var response = new AuthResponseDto
            {
                IsAuthenticated = true,
                Message = "Authentication successful",
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                ExpiresOn = expires
            };

            
            return Task.FromResult(response);
        }
    }
}