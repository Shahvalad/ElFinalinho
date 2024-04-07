using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Application.Features.Games.Queries.GetByName
{
    public record GetGameByNameQuery(string searchTerm) : IRequest<List<GetGameDto>>;
    public class GetGameByNameQueryHandler : IRequestHandler<GetGameByNameQuery, List<GetGameDto>>
    {
        private readonly IDataContext _context;
        private readonly IMapper _mapper;

        public GetGameByNameQueryHandler(IMapper mapper, IDataContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<List<GetGameDto>> Handle(GetGameByNameQuery request, CancellationToken cancellationToken)
        {
            var games = await _context.Games
                .AsNoTracking()
                .Include(g => g.Developer)
                .Include(g => g.Publisher)
                .Include(g => g.Images)
                .Include(g => g.GameGenres)
                    .ThenInclude(gg => gg.Genre)
                .Where(g => g.Name.ToLower().Contains(request.searchTerm.ToLower()))
                .ToListAsync();

            var getGameDtos = _mapper.Map<List<GetGameDto>>(games);
            foreach (var dto in getGameDtos)
            {
                dto.Images = games.FirstOrDefault(g => g.Id == dto.Id)?.Images.Select(i => new GameImage() { FileName = i.FileName, IsCoverImage = i.IsCoverImage }).ToList();
                if (games.FirstOrDefault(g => g.Id == dto.Id).StockCount > 0)
                    dto.InStock = true;
                dto.CoverImageFileName = games.FirstOrDefault(g => g.Id == dto.Id).Images.FirstOrDefault(i => i.IsCoverImage).FileName;
            }
            return getGameDtos;
        }
    }
}
