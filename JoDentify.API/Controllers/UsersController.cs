using AutoMapper;
using JoDentify.Application.DTOs.User;
using JoDentify.Core;
using JoDentify.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JoDentify.API.Controllers
{
    [Authorize]
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly Guid? _clinicId;

        public UsersController(ApplicationDbContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;

            var clinicIdClaim = httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "clinicId");
            if (clinicIdClaim != null && Guid.TryParse(clinicIdClaim.Value, out Guid clinicId))
            {
                _clinicId = clinicId;
            }
            else
            {
                _clinicId = null;
            }
        }

        [HttpGet("doctors")]
        public async Task<IActionResult> GetClinicDoctors()
        {
            if (_clinicId == null)
            {
                return Unauthorized("User is not associated with a clinic.");
            }

            var users = await _context.Users
                .Where(u => u.ClinicId == _clinicId.Value)
                .AsNoTracking()
                .ToListAsync();

            var userDtos = _mapper.Map<IEnumerable<UserDto>>(users);
            return Ok(userDtos);
        }
    }
}