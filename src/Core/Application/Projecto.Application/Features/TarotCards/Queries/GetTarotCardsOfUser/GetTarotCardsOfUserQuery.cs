using Microsoft.VisualBasic;
using Projecto.Application.Dtos.TarotCardDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Application.Features.TarotCards.Queries.GetTarotCardsOfUser
{
    public record GetTarotCardsOfUserQuery(string UserId) : IRequest<Result<List<GetTarotCardDto>>>;
    public class GetTarotCardsOfUserQueryHandler : IRequestHandler<GetTarotCardsOfUserQuery, Result<List<GetTarotCardDto>>>
    {
        private readonly IDataContext _context;

        public GetTarotCardsOfUserQueryHandler(IDataContext context)
        {
            _context = context;
        }

        public async Task<Result<List<GetTarotCardDto>>> Handle(GetTarotCardsOfUserQuery request, CancellationToken cancellationToken)
        {
            var usersTarotCards = await _context.UserTarotCards
                .Where(utc => utc.UserId == request.UserId)
                .Include(utc => utc.TarotCard)
                .ToListAsync(cancellationToken);

            var userTarotCardDtos = usersTarotCards.Select(usersTarotCards => new GetTarotCardDto
            (
               usersTarotCards.TarotCard.Id,
               usersTarotCards.TarotCard.Name,
               usersTarotCards.TarotCard.Description,
               usersTarotCards.TarotCard.Image,
               usersTarotCards.TarotCard.Rarity,
               usersTarotCards.TarotCard.DropRate,
               0
             )).ToList();

            return Result<List<GetTarotCardDto>>.Success(userTarotCardDtos);
        }
    }
}
