using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Application.Dtos.TarotCardDtos
{
    public record CreateTarotCardDto(string Name, string Description, IFormFile Image,double DropRate, Rarity Rarity);
}
