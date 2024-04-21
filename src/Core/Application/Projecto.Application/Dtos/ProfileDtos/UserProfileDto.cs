using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Application.Dtos.ProfileDtos
{
    public class UserProfileDto
    {
        public string UserName { get; set; } = null!;
        public string? Bio { get; set; }
        public string? ProfilePictureName { get; set; }
        public DateTime MemberSince { get; set; }
        public List<GetGameDto> Games { get; set; } = new List<GetGameDto>();
        public List<GetProfileDto> Friends { get; set; } = new List<GetProfileDto>();
    }
        
}
