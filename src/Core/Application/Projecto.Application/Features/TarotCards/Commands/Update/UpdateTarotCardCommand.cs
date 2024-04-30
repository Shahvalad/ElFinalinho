using Projecto.Application.Dtos.TarotCardDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Application.Features.TarotCards.Commands.Update
{
    public record UpdateTarotCardCommand(UpdateTarotCardDto UpdateTarotCardDto, int Id) : IRequest;
    public class UpdateTarotCardCommandHandler : IRequestHandler<UpdateTarotCardCommand>
    {
        private readonly IDataContext _context;
        private readonly IImageService _imageService;

        public UpdateTarotCardCommandHandler(IDataContext context, IImageService imageService)
        {
            _context = context;
            _imageService = imageService;
        }

        public async Task Handle(UpdateTarotCardCommand request, CancellationToken cancellationToken)
        {
            var tarotCard = await _context.TarotCards.FindAsync(request.Id, cancellationToken);

            if (tarotCard is null)
                throw new TarotCardNotFoundException("There is no such tarot card");

            tarotCard.Name = request.UpdateTarotCardDto.Name;
            tarotCard.Description = request.UpdateTarotCardDto.Description;
            tarotCard.DropRate = request.UpdateTarotCardDto.DropRate;
            tarotCard.Rarity = request.UpdateTarotCardDto.Rarity;

            if (request.UpdateTarotCardDto.Image is not null)
            {
                await _imageService.DeleteImage("TarotCards", tarotCard.Image);
                tarotCard.Image = await _imageService.CreateImageAsync("TarotCards", request.UpdateTarotCardDto.Image);
            }
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
