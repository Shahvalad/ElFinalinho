using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Application.Features.Games.Queries.RecommendGames
{
    public record RandomRecommendGamesQuery(string userId) : IRequest<List<GetGameDto>>;
    public class RecommendGamesToUserQueryHandler : IRequestHandler<RandomRecommendGamesQuery, List<GetGameDto>>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IDataContext _context;

        public RecommendGamesToUserQueryHandler(UserManager<AppUser> userManager, IDataContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<List<GetGameDto>> Handle(RandomRecommendGamesQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users
                .Include(u=>u.UserFavouriteGames)
                .ThenInclude(ufg=>ufg.Game)
                .ThenInclude(g=>g.GameGenres)
                .Include(u=>u.UserGames)
                .FirstOrDefaultAsync(u => u.Id == request.userId, cancellationToken);

            var favoriteGenres = user.UserFavouriteGames
                .SelectMany(ufg => ufg.Game.GameGenres.Select(gg => gg.GenreId))
                .Distinct();

            if(favoriteGenres.Count() == 0)
            {
                return new List<GetGameDto>();
            }
            var playedGames = user.UserGames.Select(ug => ug.GameId);

            var recommendedGames = _context.Games
                .Include(g=>g.GameGenres)
                .ThenInclude(gg=>gg.Genre)
                .Include(g=>g.Images)
                .Include(g=>g.Developer)
                .Include(g=>g.Publisher)
                .Include(g=>g.UserGames)
                .ThenInclude(ug=>ug.User)
                .Include(g=>g.UserFavouriteGames)
                .ThenInclude(ufg=>ufg.User)
                .Where(g => !playedGames.Contains(g.Id) && g.GameGenres.Any(gg => favoriteGenres.Contains(gg.GenreId)))
                .ToList();
            var gameDtos = recommendedGames.Select(g => new GetGameDto
            {
                Id = g.Id,
                Name = g.Name,
                Description = g.Description,
                Price = g.Price,
                StockCount = g.StockCount,
                Images = g.Images,
                Developer = g.Developer,
                Publisher = g.Publisher,
                GameGenres = g.GameGenres,
                CoverImageFileName = g.Images.Where(i=>i.IsCoverImage).Select(i=>i.FileName).FirstOrDefault(),
            }).Take(4).ToList();

            return gameDtos;
        }
    }
}
