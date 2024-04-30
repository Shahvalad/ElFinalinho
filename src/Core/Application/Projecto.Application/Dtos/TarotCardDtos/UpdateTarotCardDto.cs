using Microsoft.AspNetCore.Http;

namespace Projecto.Application.Dtos.TarotCardDtos
{
    public record UpdateTarotCardDto
        (string Name, 
        string Description, 
        IFormFile? Image, 
        double DropRate, 
        Rarity Rarity);
}
