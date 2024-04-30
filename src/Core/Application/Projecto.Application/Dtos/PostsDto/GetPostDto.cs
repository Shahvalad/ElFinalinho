using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Application.Dtos.PostsDto
{
    public record GetPostDto(int Id, string Image, string Description);
}
