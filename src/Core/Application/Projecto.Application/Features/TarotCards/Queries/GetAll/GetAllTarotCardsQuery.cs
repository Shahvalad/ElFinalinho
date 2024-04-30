using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Application.Features.TarotCards.Queries.GetAll
{
    public record GetAllTarotCardsQuery() : IRequest<List<TarotCard>>;
    public class GetAllTarotCardsQueryHandler : IRequestHandler<GetAllTarotCardsQuery, List<TarotCard>>
    {
        private readonly IDataContext _context;

        public GetAllTarotCardsQueryHandler(IDataContext context)
        {
            _context = context;
        }

        public async Task<List<TarotCard>> Handle(GetAllTarotCardsQuery request, CancellationToken cancellationToken)
        {
            return await _context.TarotCards
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
    }
}
