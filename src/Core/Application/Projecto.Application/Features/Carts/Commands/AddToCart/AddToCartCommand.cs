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
        public Cart CurrentCart { get; set; } = null!;
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
            var game = await _context.Games.FindAsync(request.GameId) ?? throw new GameNotFoundException("No game with such id!");
            request.CurrentCart ??= new Cart();
            var cartItem = FindCartItem(request.CurrentCart, game.Id);

            if (cartItem == null)
            {
                AddNewCartItem(game, request.CurrentCart, request.Quantity);
            }
            else
            {
                UpdateCartItemQuantity(game, cartItem, request.Quantity);
            }
        }
        private CartItem? FindCartItem(Cart cart, int gameId)
        {
            return cart.CartItems.SingleOrDefault(x => x.Game.Id == gameId);
        }
        private void UpdateCartItemQuantity(Game game, CartItem cartItem, int quantity)
        {
            if (game.StockCount < cartItem.Quantity + quantity)
            {
                cartItem.Quantity = game.StockCount;
            }
            else
            {
                cartItem.Quantity += quantity;
            }
        }

        private void AddNewCartItem(Game game, Cart cart, int quantity)
        {
            if (game.StockCount < quantity)
            {
                throw new InsufficientStockException("Not enough stock!");
            }
            cart.CartItems.Add(new CartItem { Game = game, Quantity = quantity });
        }
    }
}
