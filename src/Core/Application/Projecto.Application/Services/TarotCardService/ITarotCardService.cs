using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Application.Services.TarotCardService
{
    public interface ITarotCardService
    {
        Task<TarotCard> AssignRandomCardToUser(string userId);
    }
}
