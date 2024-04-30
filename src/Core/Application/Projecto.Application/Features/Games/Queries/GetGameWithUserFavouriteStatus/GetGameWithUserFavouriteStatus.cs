using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Application.Features.Games.Queries.GetGameWithUserFavouriteStatus
{
    public record GetGameWithUserFavouriteStatus(int? Id, string UserId) : IRequest<GetGameDto>;
    public class GetGameWithUserFavouriteStatusHandler : IRequestHandler<GetGameWithUserFavouriteStatus, GetGameDto>
    {
        private readonly IDataContext _context;
        private readonly IMapper _mapper;

        public GetGameWithUserFavouriteStatusHandler(IMapper mapper, IDataContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<GetGameDto> Handle(GetGameWithUserFavouriteStatus request, CancellationToken cancellationToken)
        {
            var game = await _context.Games
                .AsNoTracking()
                .Include(g => g.Developer)
                .Include(g => g.Publisher)
                .Include(g => g.Images)
                .Include(g => g.UserFavouriteGames)
                    .ThenInclude(ufg => ufg.User)
                .Include(g => g.GameGenres)
                    .ThenInclude(gg => gg.Genre)
                .Include(g=>g.Reviews)
                    .ThenInclude(r=>r.User)
                    .FirstOrDefaultAsync(g=>g.Id==request.Id, cancellationToken)??throw new GameNotFoundException("There is no game with such id!");

            var gameDto = _mapper.Map<GetGameDto>(game);
            gameDto.Images = game.Images.Where(i=>i.IsCoverImage==false).Select(i => new GameImage() { FileName = i.FileName, IsCoverImage = false }).ToList();
            if (game.StockCount > 0)
                gameDto.InStock = true;
            gameDto.Reviews = game.Reviews.OrderByDescending(r => r.CreatedAt).ToList();
            gameDto.CoverImageFileName = game.Images.FirstOrDefault(i => i.IsCoverImage)!.FileName;
            gameDto.IsFavourite = game.UserFavouriteGames.Any(ufg => ufg.UserId == request.UserId);
            return gameDto;
        }
    }
}
