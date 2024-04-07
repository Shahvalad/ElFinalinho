using Projecto.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Application.Features.Games.Commands.RemoveFromFavourites
{
    public record RemoveFromFavouritesCommand(int GameId, string UserId) : IRequest;
    public class RemoveFromFavouritesCommandHandler : IRequestHandler<RemoveFromFavouritesCommand>
    {
        private readonly IDataContext _context;
        public RemoveFromFavouritesCommandHandler(IDataContext context)
        {
            _context = context;
        }

        public async Task Handle(RemoveFromFavouritesCommand request, CancellationToken cancellationToken)
        {
            var userFavouriteGame = await _context.UserFavouriteGames
                .FirstOrDefaultAsync(ufg => ufg.UserId == request.UserId && ufg.GameId == request.GameId);
            if (userFavouriteGame == null)
            {
                throw new UserFavouriteGameNotFoundException("This user has no such favourite game!");
            }
            _context.UserFavouriteGames.Remove(userFavouriteGame);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
