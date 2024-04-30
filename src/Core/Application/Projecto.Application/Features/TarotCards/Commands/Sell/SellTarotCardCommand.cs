using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Application.Features.TarotCards.Commands.Sell
{
    public record SellTarotCardCommand(string UserId, int CardId) : IRequest<Result<TarotCard>>;
    public class SellTarotCardCommandHandler : IRequestHandler<SellTarotCardCommand, Result<TarotCard>>
    {
        private readonly IDataContext _context;

        public SellTarotCardCommandHandler(IDataContext context)
        {
            _context = context;
        }

        public async Task<Result<TarotCard>> Handle(SellTarotCardCommand request, CancellationToken cancellationToken)
        {
            var userTarotCard = await _context.UserTarotCards
                .Where(utc => utc.UserId == request.UserId && utc.TarotCardId == request.CardId)
                .Include(utc => utc.TarotCard)
                .FirstOrDefaultAsync(cancellationToken);

            if (userTarotCard == null)
            {
                return Result<TarotCard>.Failure(new[] { "Tarot card not found" });
            }

            _context.UserTarotCards.Remove(userTarotCard);
            await _context.SaveChangesAsync(cancellationToken);

            return Result<TarotCard>.Success(userTarotCard.TarotCard);
        }
    }
}
