using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Application.Features.Listings.Commands.Cancel
{
    public record CancelListingCommand(int id) : IRequest;
    public class CancelListingCommandHandler : IRequestHandler<CancelListingCommand>
    {
        private readonly IDataContext _context;

        public CancelListingCommandHandler(IDataContext context)
        {
            _context = context;
        }

        public async Task Handle(CancelListingCommand request, CancellationToken cancellationToken)
        {
            var listing = await _context.Listings
                .Include(li=>li.MarketItem)
                    .ThenInclude(li=>li.TarotCard)
                .Include(li=>li.User)
                .FirstOrDefaultAsync(l => l.Id == request.id, cancellationToken);

            if (listing == null)
            {
                throw new Exception(nameof(Listing));
            }
            UserTarotCard userTarotCard = new UserTarotCard 
            { 
                IsDisplayedOnProfile=false, 
                TarotCardId=listing.MarketItem.TarotCard.Id, 
                UserId=listing.UserId
            };
            _context.Listings.Remove(listing);
            listing.MarketItem.Quantity = listing.MarketItem.Listings.Count();
            listing.MarketItem.StartingPrice = listing.MarketItem.Listings.Min(l => l.Price);
            await _context.UserTarotCards.AddAsync(userTarotCard,cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

        }
    }
}
