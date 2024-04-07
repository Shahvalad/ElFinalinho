using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Application.Features.Carts.Commands.AddToCart
{
    public class AddToCartCommand : IRequest
    {
        public int GameId { get; set; }
        public Cart CurrentCart { get; set; }
        public int Quantity { get; set; }
    }
    public class AddToCartCommandHandler : IRequestHandler<AddToCartCommand>
    {
        private readonly IDataContext _context;
        public AddToCartCommandHandler(IDataContext context)
        {
            _context = context;
        }
        public async Task Handle(AddToCartCommand request, CancellationToken cancellationToken)
        {
            var game = await _context.Games.FindAsync(request.GameId)??throw new GameNotFoundException("No game with such id!");
            if(request.CurrentCart == null)
            {
                request.CurrentCart = new Cart();
            }
           
            if (request.CurrentCart.CartItems.Any(x => x.Game.Id == game.Id))
            {
                if (game.StockCount < request.CurrentCart.CartItems.First(x => x.Game.Id == game.Id).Quantity + request.Quantity)
                {
                    request.CurrentCart.CartItems.First(x => x.Game.Id == game.Id).Quantity = game.StockCount;
                }
                else
                {
                    request.CurrentCart.CartItems.First(x => x.Game.Id == game.Id).Quantity++;
                }
            }
            else
            {
                if (game.StockCount < request.Quantity)
                {
                    throw new Exception("Not enough stock!");
                }
                request.CurrentCart.CartItems.Add(new CartItem { Game = game, Quantity = request.Quantity });
            }

        }
       
    }
}
