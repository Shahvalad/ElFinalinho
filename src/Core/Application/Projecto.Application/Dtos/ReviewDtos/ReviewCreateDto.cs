using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Application.Dtos.ReviewDtos
{
    public record ReviewCreateDto(bool IsLiked, string Comment, string UserId, int GameId);
}
