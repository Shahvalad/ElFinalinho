namespace Projecto.Application.Features.Games.Commands.Create
{
    public record CreateGameCommand(CreateGameDto CreateGameDto) : IRequest<int>;
    public class CreateGameHandler : IRequestHandler<CreateGameCommand, int>
    {
        private readonly IDataContext _context;
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;

        public CreateGameHandler(IDataContext context, IMapper mapper, IImageService imageService)
        {
            _context = context;
            _mapper = mapper;
            _imageService = imageService;
        }

        public async Task<int> Handle(CreateGameCommand request, CancellationToken cancellationToken)
        {
            var game = _mapper.Map<Game>(request.CreateGameDto);

            if (request.CreateGameDto.Images is not null && request.CreateGameDto.Images.Any())
            {
                foreach (var image in request.CreateGameDto.Images)
                {
                    var imageName = await _imageService.CreateImageAsync("Games", image);
                    game.Images.Add(new GameImage { FileName = imageName });
                }
            }

            if (request.CreateGameDto.CoverImage is not null)
            {
                var coverImageName = await _imageService.CreateImageAsync("Games", request.CreateGameDto.CoverImage);
                game.Images.Add(new GameImage { FileName = coverImageName, IsCoverImage = true });
            }

            if (request.CreateGameDto.SelectedGenres is not null && request.CreateGameDto.SelectedGenres.Any())
            {
                game.GameGenres = request.CreateGameDto.SelectedGenres.Select(genreId => new GameGenre { GenreId = genreId }).ToList();
            }

            game.Developer = await _context.Developers.FirstOrDefaultAsync(g=>g.Id == request.CreateGameDto.DeveloperId) ?? 
                throw new GameNotFoundException("There is no game with such id!");

            await _context.Games.AddAsync(game);
            await _context.SaveChangesAsync(cancellationToken);
            return game.Id;
        }
    }
}
