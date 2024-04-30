using Projecto.Domain.Enums;

namespace Projecto.Application.Features.Listings.Queries.GetUsersPendingListings
{
    public record GetUsersPendingListingsQuery(string UserId) : IRequest<List<Listing>>;
    public class GetUsersPendingListingsQueryHandler : IRequestHandler<GetUsersPendingListingsQuery, List<Listing>>
    {
        private readonly IDataContext _context;

        public GetUsersPendingListingsQueryHandler(IDataContext context)
        {
            _context = context;
        }

        public async Task<List<Listing>> Handle(GetUsersPendingListingsQuery request, CancellationToken cancellationToken)
        {
            return await _context.Listings
                .AsNoTracking()
                .Include(l => l.MarketItem)
                .ThenInclude(mi => mi.TarotCard)
                .Where(l => l.UserId == request.UserId && l.Status == ListingStatus.Active)
                .ToListAsync(cancellationToken);
        }
    }
}
