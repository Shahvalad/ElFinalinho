using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Application.Features.Listings.Queries.GetById
{
    public record GetListingByIdQuery(int id) : IRequest<Listing>;
    public class GetListingByIdQueryHandler : IRequestHandler<GetListingByIdQuery, Listing>
    {
        private readonly IDataContext _context;

        public GetListingByIdQueryHandler(IDataContext context)
        {
            _context = context;
        }

        public async Task<Listing> Handle(GetListingByIdQuery request, CancellationToken cancellationToken)
        {
            return await _context.Listings
                .AsNoTracking()
                .Include(l => l.User)
                .Include(l => l.MarketItem)
                    .ThenInclude(mi => mi.TarotCard)
                .Include(l => l.MarketItem)
                    .ThenInclude(mi => mi.Listings)
                        .ThenInclude(li => li.User)
                        .ThenInclude(li => li.ProfilePicture)
                .FirstOrDefaultAsync(l => l.Id == request.id, cancellationToken);
        }
    }
}
