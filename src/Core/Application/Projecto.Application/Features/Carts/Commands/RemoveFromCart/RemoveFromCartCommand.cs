using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Application.Features.Carts.Commands.RemoveFromCart
{
    public class RemoveFromCartCommand : IRequest
    {
        public int GameId { get; set; }
        public Cart CurrentCart { get; set; }
    }

    public class RemoveFromCartCommandHandler : IRequestHandler<RemoveFromCartCommand>
    {
        public Task Handle(RemoveFromCartCommand request, CancellationToken cancellationToken)
        {
            if (request.CurrentCart == null)
            {
                throw new CartNotFoundException("Cart is empty.");
            }
            var cartItem = request.CurrentCart.CartItems.SingleOrDefault(x => x.Game.Id == request.GameId) 
                ?? throw new CartItemNotFoundException("No such item in cart.");

            if (cartItem.Quantity == 1)
            {
                request.CurrentCart.CartItems.Remove(cartItem);
            }
            else
            {
                cartItem.Quantity--;
            }
            return Task.CompletedTask;
        }
    }
}
