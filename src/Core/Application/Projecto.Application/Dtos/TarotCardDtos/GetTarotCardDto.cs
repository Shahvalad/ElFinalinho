using Projecto.Domain.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Application.Dtos.TarotCardDtos
{
    public record GetTarotCardDto(int Id, 
        string Name, 
        string Description, 
        string Image, 
        Rarity Rarity, 
        double DropRate, 
        int OwnedCount);
}
