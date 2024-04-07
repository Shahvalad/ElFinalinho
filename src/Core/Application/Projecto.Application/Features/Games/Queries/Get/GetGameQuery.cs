namespace Projecto.Application.Features.Games.Queries.Get
{
    public record GetGameQuery(int? Id) : IRequest<GetGameDto>;
    public class GetGameQueryHandler : IRequestHandler<GetGameQuery, GetGameDto>
    {
        private readonly IDataContext _context;
        private readonly IMapper _mapper;

        public GetGameQueryHandler(IMapper mapper, IDataContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<GetGameDto> Handle(GetGameQuery request, CancellationToken cancellationToken)
        {
            var game = await _context.Games
                .AsNoTracking()
                .Include(g => g.Developer)
                .Include(g => g.Publisher)
                .Include(g => g.Images)
                .Include(g=>g.UserFavouriteGames)
                    .ThenInclude(ufg=>ufg.User)
                .Include(g=>g.GameGenres)
                    .ThenInclude(gg=>gg.Genre)
                .FirstOrDefaultAsync(g => g.Id == request.Id)??throw new GameNotFoundException("There is no game with such id!");

            var getGameDto = _mapper.Map<GetGameDto>(game);
            getGameDto.Images = game.Images.Select(i => new GameImage() { FileName = i.FileName, IsCoverImage = i.IsCoverImage }).ToList();
            if(game.StockCount > 0)
                getGameDto.InStock = true;
            getGameDto.CoverImageFileName = game.Images.FirstOrDefault(i => i.IsCoverImage).FileName;
            return getGameDto;
        }
    }
}
