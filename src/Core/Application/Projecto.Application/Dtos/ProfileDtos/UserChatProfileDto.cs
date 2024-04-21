using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Application.Dtos.ProfileDtos
{
    public record UserChatProfileDto
    (
         string Id,
         string UserName,
         string? ProfilePictureName,
         bool HasUnreadMessages
        );
}
