using Projecto.Application.Dtos.TarotCardDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Application.Features.TarotCards.Queries.GetById
{
    public record GetTarotCardByIdQuery(int Id) : IRequest<GetTarotCardDto>;
    public class GetTarotCardByIdQueryHandler : IRequestHandler<GetTarotCardByIdQuery, GetTarotCardDto>
    {
        private readonly IDataContext _context;
        private readonly IMapper _mapper;
        public GetTarotCardByIdQueryHandler(IDataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<GetTarotCardDto> Handle(GetTarotCardByIdQuery request, CancellationToken cancellationToken)
        {
            var tarotCard = await _context.TarotCards
                .Include(x => x.UserTarotCards)
                    .ThenInclude(utc => utc.User)  // Load the User for each UserTarotCard
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (tarotCard is null)
                throw new TarotCardNotFoundException($"Tarot Card with id {request.Id} not found");

            var ownedCount = tarotCard.UserTarotCards.Select(utc => utc.UserId).Distinct().Count();

            var getTarotCardDto = new GetTarotCardDto
            (
                tarotCard.Id,
                tarotCard.Name,
                tarotCard.Description,
                tarotCard.Image,
                tarotCard.Rarity,
                tarotCard.DropRate,
                ownedCount
            );
            return getTarotCardDto;
        }


    }
}
