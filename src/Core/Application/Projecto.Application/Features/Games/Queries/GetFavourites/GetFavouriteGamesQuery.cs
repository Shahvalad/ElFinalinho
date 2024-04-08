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
        private readonly IMapper _mapper;

        public GetFavouriteGamesQueryHandler(IDataContext context, UserManager<AppUser> userManager, IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetGameDto>> Handle(GetFavouriteGamesQuery request, CancellationToken cancellationToken)
        {
            var user = await GetUser(request.UserId);
            if (user == null)
            {
                return Enumerable.Empty<GetGameDto>();
            }
            var userFavouriteGames = await GetUserFavouriteGames(user.Id);
            var gameDtos = _mapper.Map<IEnumerable<GetGameDto>>(userFavouriteGames);

            foreach (var gameDto in gameDtos)
            {
                gameDto.CoverImageFileName = _context.Games
                    .Include(g => g.Images)
                    .FirstOrDefault(g => g.Id == gameDto.Id)
                    ?.Images.FirstOrDefault(i => i.IsCoverImage)?.FileName;
            }
            return gameDtos;

        }

        private async Task<AppUser> GetUser(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        private async Task<IEnumerable<Game>> GetUserFavouriteGames(string userId)
        {
            return await _context.UserFavouriteGames
                   .Include(ufg => ufg.Game)
                   .ThenInclude(g => g.Images)
                   .Where(ufg => ufg.UserId == userId)
                   .Select(ufg => ufg.Game)
                   .ToListAsync();
        }
    }
}