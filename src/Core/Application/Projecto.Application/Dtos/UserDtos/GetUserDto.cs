using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Application.Dtos.UserDtos
{
    public record GetUserDto(
        string UserId, 
        string UserName, 
        string Email, 
        bool IsActive, 
        string Role);
}
