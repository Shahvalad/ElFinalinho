using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Application.Features.Games.Queries.GetPopulars
{
    public record GetPopularGamesQuery() : IRequest<List<GetGameDto>>;
    public class GetPopularGamesQueryHandler : IRequestHandler<GetPopularGamesQuery, List<GetGameDto>>
    {
        private readonly IDataContext _context;
        private readonly IMapper _mapper;

        public GetPopularGamesQueryHandler(IDataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<GetGameDto>> Handle(GetPopularGamesQuery request, CancellationToken cancellationToken)
        {
            var games = await _context.Games
                .AsNoTracking()
                .Include(g => g.Developer)
                .Include(g => g.Images)
                .Include(g => g.GameGenres)
                    .ThenInclude(gg => gg.Genre)
                .Include(g => g.Reviews)
                    .ThenInclude(r => r.User)
                .OrderByDescending(g => g.Reviews.Count)
                .Take(8)
                .ToListAsync(cancellationToken);

            var gameDtos = _mapper.Map<List<GetGameDto>>(games);
            if (games.Any())
            {
                foreach (var dto in gameDtos)
                {
                    dto.Images = games.FirstOrDefault(g => g.Id == dto.Id)?.Images.Select(i => new GameImage() { FileName = i.FileName, IsCoverImage = i.IsCoverImage }).ToList();
                    if (games.FirstOrDefault(g => g.Id == dto.Id)!.StockCount > 0)
                        dto.InStock = true;
                    dto.CoverImageFileName = games.FirstOrDefault(g => g.Id == dto.Id)!.Images.FirstOrDefault(i => i.IsCoverImage)!.FileName;
                }
            }

            return gameDtos;
        }
    }
}
