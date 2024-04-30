using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Application.Features.TarotCards.Queries.GetUserTarotCards
{
    public record GetUserTarotCardsQuery(string UserId) : IRequest<Result<List<UserTarotCard>>>;
    public class GetUserTarotCardsQueryHandler : IRequestHandler<GetUserTarotCardsQuery, Result<List<UserTarotCard>>>
    {
        private readonly IDataContext _context;

        public GetUserTarotCardsQueryHandler(IDataContext context)
        {
            _context = context;
        }

        public async Task<Result<List<UserTarotCard>>> Handle(GetUserTarotCardsQuery request, CancellationToken cancellationToken)
        {
            var usersTarotCards = await _context.UserTarotCards
                .Where(utc => utc.UserId == request.UserId)
                .Include(utc => utc.TarotCard)
                .GroupBy(utc=>utc.TarotCard.Name)
                .Select(g=>g.First())
                .ToListAsync(cancellationToken);

            return Result<List<UserTarotCard>>.Success(usersTarotCards);
        }
    }
}
