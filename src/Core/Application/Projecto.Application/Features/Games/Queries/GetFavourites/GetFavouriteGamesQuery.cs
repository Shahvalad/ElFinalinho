using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Application.Features.Games.Queries.GetFavourites
{
    public record GetFavouriteGamesQuery(string UserId) : IRequest<IEnumerable<GetGameDto>>;
    public class GetFavouriteGamesQueryHandler : IRequestHandler<GetFavouriteGamesQuery, IEnumerable<GetGameDto>>
    {
        private readonly IDataContext _context;
        private readonly UserManager<AppUser> _userManager;

        public GetFavouriteGamesQueryHandler(IDataContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IEnumerable<GetGameDto>> Handle(GetFavouriteGamesQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            var userFavouriteGames = await _context.UserFavouriteGames
                   .Include(ufg => ufg.Game)
                   .ThenInclude(g => g.Images)
                   .Where(ufg => ufg.UserId == user.Id)
                   .ToListAsync(); 
            var games = new List<GetGameDto>();
            games.AddRange(userFavouriteGames.Select(x => new GetGameDto
            {
                Id = x.GameId,
                Name = x.Game.Name,
                Description = x.Game.Description,
                Publisher = x.Game.Publisher,
                Developer = x.Game.Developer,
                Price=x.Game.Price,
                CoverImageFileName = x.Game.Images.FirstOrDefault(x => x.IsCoverImage).FileName
            }));
            return games;
        }
    }
}
