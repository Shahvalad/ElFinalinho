using Projecto.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Application.Features.MarketItems.Queries.GetAll
{
    public record GetAllMarketItemsQuery() : IRequest<List<MarketItem>>;
    public class GetAllMarketItemsQueryHandler : IRequestHandler<GetAllMarketItemsQuery, List<MarketItem>>
    {
        private readonly IDataContext _context;

        public GetAllMarketItemsQueryHandler(IDataContext context)
        {
            _context = context;
        }

        public async Task<List<MarketItem>> Handle(GetAllMarketItemsQuery request, CancellationToken cancellationToken)
        {
            var marketItems = await _context.MarketItems
                .AsNoTracking()
                .Include(mi=>mi.TarotCard)
                .Include(mi=>mi.Listings)
                .ToListAsync(cancellationToken);
            foreach(var marketItem in marketItems)
            {
                marketItem.Listings = marketItem.Listings.Where(l => l.Status == ListingStatus.Active).ToList();
                marketItem.Quantity = marketItem.Listings.Count();
                if (marketItem.Listings.Any())
                {
                    marketItem.StartingPrice = marketItem.Listings.Min(l => l.Price);
                }
                else
                {
                    marketItem.StartingPrice = 0;
                }
            }
            return marketItems.OrderByDescending(mi=>mi.Quantity).ToList();
        }
    }
}
