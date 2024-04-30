using Projecto.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Application.Features.Listings.Commands.Create
{
    public record CreateListingCommand(string UserId, int CardId, decimal Price) : IRequest<Result<TarotCard>>;

    public class CreateListingCommandHandler : IRequestHandler<CreateListingCommand, Result<TarotCard>>
    {
        private readonly IDataContext _context;
        private readonly UserManager<AppUser> _userManager;
        public CreateListingCommandHandler(IDataContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<Result<TarotCard>> Handle(CreateListingCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == request.UserId);
            var marketItem = await _context.MarketItems
                .Include(mi => mi.TarotCard)
                .Include(mi => mi.Listings)
                .FirstOrDefaultAsync(mi => mi.TarotCard.Id == request.CardId, cancellationToken);

            if (user == null || marketItem == null)
            {
                return Result<TarotCard>.Failure(new[] { "User or market item not found" });
            }

            var userTarotCard = await _context.UserTarotCards
                .FirstOrDefaultAsync(utc => utc.UserId == user.Id && utc.TarotCardId == marketItem.TarotCard.Id, cancellationToken);

            if (userTarotCard == null)
            {
                return Result<TarotCard>.Failure(new[] { "The card is not in user's collection" });
            }

            _context.UserTarotCards.Remove(userTarotCard);

            var listing = new Listing
            {
                UserId = user.Id,
                MarketItem = marketItem,
                Quantity = 1,
                Price = request.Price,
                Status = ListingStatus.Active
            };

            marketItem.Listings.Add(listing);
            marketItem.Quantity = marketItem.Listings.Count();
            marketItem.StartingPrice = marketItem.Listings.Min(l => l.Price);
            await _context.SaveChangesAsync(cancellationToken);

            return Result<TarotCard>.Success(marketItem.TarotCard);
        }

    }
}
