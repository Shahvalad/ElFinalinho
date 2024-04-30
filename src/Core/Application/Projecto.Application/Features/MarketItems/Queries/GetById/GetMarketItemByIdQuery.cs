using Projecto.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Application.Features.MarketItems.Queries.GetById
{
    public record GetMarketItemByIdQuery(int? id) : IRequest<MarketItem>;
    public class GetMarketItemByIdQueryHandler : IRequestHandler<GetMarketItemByIdQuery, MarketItem>
    {
        private readonly IDataContext _context;

        public GetMarketItemByIdQueryHandler(IDataContext context)
        {
            _context = context;
        }

        public async Task<MarketItem> Handle(GetMarketItemByIdQuery request, CancellationToken cancellationToken)
        {
            var marketItem = await _context.MarketItems
                .AsNoTracking()
                .Include(mi => mi.TarotCard)
                .Include(mi => mi.Listings)
                .Include(mi=>mi.Listings)
                    .ThenInclude(l=>l.User)
                    .ThenInclude(l=>l.ProfilePicture)
                .FirstOrDefaultAsync(mi => mi.TarotCard.Id == request.id, cancellationToken);
            marketItem.Listings = marketItem.Listings.
                Where(l => l.Status == ListingStatus.Active)
                .OrderBy(l=>l.Price)
                .ToList();
            marketItem.Quantity = marketItem.Listings.Count();
            if (marketItem.Listings.Any())
            {
                marketItem.StartingPrice = marketItem.Listings.Min(l => l.Price);
            }
            else
            {
                marketItem.StartingPrice = 0;
            }
            return marketItem;
        }
    }
}
