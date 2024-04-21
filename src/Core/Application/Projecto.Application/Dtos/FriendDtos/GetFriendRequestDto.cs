using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Application.Dtos.FriendDtos
{
    public record GetFriendRequestDto(int RequestId, string ProfilePictureName, string Username);
}
