namespace Projecto.Application.Features.Games.Queries.GetAll
{
    public record GetGamesQuery : IRequest<IEnumerable<GetGameDto>>;
    public class GetGamesQueryHandler : IRequestHandler<GetGamesQuery, IEnumerable<GetGameDto>>
    {
        private readonly IDataContext _context;
        private readonly IMapper _mapper;

        public GetGamesQueryHandler(IDataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetGameDto>> Handle(GetGamesQuery request, CancellationToken cancellationToken)
        {
            var games = await _context.Games
                .AsNoTracking().
                    Include(g => g.Developer).
                    Include(g => g.Publisher).
                    Include(g => g.Images).
                    Include(g => g.GameGenres).
                        ThenInclude(gg => gg.Genre).
                ToListAsync(cancellationToken);
            var getGameDtos = _mapper.Map<IEnumerable<GetGameDto>>(games);
            if (games.Any())
            {
                foreach (var dto in getGameDtos)
                {
                    dto.Images = games.FirstOrDefault(g => g.Id == dto.Id)?.Images.Select(i => new GameImage() { FileName = i.FileName, IsCoverImage = i.IsCoverImage }).ToList();
                    if (games.FirstOrDefault(g => g.Id == dto.Id)!.StockCount > 0)
                        dto.InStock = true;
                    dto.CoverImageFileName = games.FirstOrDefault(g => g.Id == dto.Id)!.Images.FirstOrDefault(i => i.IsCoverImage)!.FileName;
                }
            }
            return getGameDtos;
        }
    }
}
