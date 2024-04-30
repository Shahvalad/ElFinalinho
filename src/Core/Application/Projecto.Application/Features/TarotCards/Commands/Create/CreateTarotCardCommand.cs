using Projecto.Application.Dtos.TarotCardDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Application.Features.TarotCards.Commands.Create
{
    public record CreateTarotCardCommand(CreateTarotCardDto CreateTarotCardDto) : IRequest;
    public class CreateTarotCardCommandHandler : IRequestHandler<CreateTarotCardCommand>
    {
        private readonly IDataContext _context;
        private readonly IImageService _imageService;

        public CreateTarotCardCommandHandler(IDataContext context, IImageService imageService)
        {
            _context = context;
            _imageService = imageService;
        }

        public async Task Handle(CreateTarotCardCommand request, CancellationToken cancellationToken)
        {
            var tarotCard = new TarotCard
            {
                Name = request.CreateTarotCardDto.Name,
                Description = request.CreateTarotCardDto.Description,
                DropRate = request.CreateTarotCardDto.DropRate,
                Rarity = request.CreateTarotCardDto.Rarity,
                Image = await _imageService.CreateImageAsync("TarotCards",request.CreateTarotCardDto.Image)
            };
            await _context.TarotCards.AddAsync(tarotCard, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
