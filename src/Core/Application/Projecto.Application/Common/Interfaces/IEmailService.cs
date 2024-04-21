using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Application.Common.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string template);
        Task SendReceiptEmailAsync(string to, string ticketId,List<CartItem> cartItems);
        Task SendGameKeysEmailAsync(string to, List<GameKey> gameKeys);
    }
}
