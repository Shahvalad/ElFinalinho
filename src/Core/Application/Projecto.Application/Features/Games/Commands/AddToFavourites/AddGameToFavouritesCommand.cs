using Microsoft.AspNetCore.Identity;
using Projecto.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Application.Features.Games.Commands.AddToFavourites
{
    public record AddGameToFavouritesCommand(string UserId, int GameId) : IRequest;
    public class AddGameToFavouritesCommandHandler : IRequestHandler<AddGameToFavouritesCommand>
    {
        private readonly IDataContext _context;
        private readonly UserManager<AppUser> _userManager;

        public AddGameToFavouritesCommandHandler(IDataContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task Handle(AddGameToFavouritesCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId) ?? throw new UserNotFoundException("User not found!");
            var userFavouriteGame = new UserFavouriteGame
            {
                UserId = user.Id,
                GameId = request.GameId
            };
            await _context.UserFavouriteGames.AddAsync(userFavouriteGame);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
