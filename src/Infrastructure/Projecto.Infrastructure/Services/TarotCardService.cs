using Projecto.Application.Services.TarotCardService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Infrastructure.Services
{
    public class TarotCardService : ITarotCardService
    {
        private readonly IDataContext _context;

        public TarotCardService(IDataContext context)
        {
            _context = context;
        }

        public async Task<TarotCard> AssignRandomCardToUser(string userId)
        {
            var tarotCards = await _context.TarotCards.ToListAsync();

            var totalDropRate = tarotCards.Sum(card => card.DropRate);

            var random = new Random();
            var randomNumber = random.NextDouble() * totalDropRate;

            var cumulative = 0.0;
            TarotCard droppedCard = null;
            foreach (var card in tarotCards)
            {
                cumulative += card.DropRate;

                if (randomNumber <= cumulative)
                {
                    var userTarotCard = new UserTarotCard
                    {
                        UserId = userId,
                        TarotCardId = card.Id
                    };

                    await _context.UserTarotCards.AddAsync(userTarotCard);
                    await _context.SaveChangesAsync(CancellationToken.None);

                    droppedCard = card;
                    break;
                }
            }
            return droppedCard;
        }

    }
}
