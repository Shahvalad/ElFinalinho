using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Application.Dtos.CommunityDtos
{
    public record CreateCommunityPostDto(
        string Title, 
        IFormFile Image, 
        bool isSpoiler = false);
}
