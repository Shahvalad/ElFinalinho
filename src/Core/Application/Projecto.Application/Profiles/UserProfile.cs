using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Projecto.Application.Dtos.ProfileDtos;

namespace Projecto.Application.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<AppUser, GetProfileDto>().ReverseMap();
            CreateMap<AppUser, UserProfileDto>().ReverseMap();
        }
    }
}
