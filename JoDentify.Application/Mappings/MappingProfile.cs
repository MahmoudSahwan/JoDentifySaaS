using AutoMapper;
using JoDentify.Application.DTOs.Auth;
using JoDentify.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoDentify.Application.Mappings
{
    // ده الكلاس اللي فيه "خريطة" التحويل
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // هنا بنعرف الـ AutoMapper
            // بنقوله: إزاي تحول من RegisterDto (مصدر) إلى ApplicationUser (هدف)
            CreateMap<RegisterDto, ApplicationUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName));
            // ملحوظة: مش بنعمل مابينج للـ Password لإن الـ Identity بيعملها Hash بطريقة خاصة
        }
    }
}

