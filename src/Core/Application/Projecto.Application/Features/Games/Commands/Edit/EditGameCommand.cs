namespace Projecto.Application.Features.Games.Commands.Edit
{
    public record EditGameCommand(int? Id, UpdateGameDto UpdateGameDto) : IRequest;
    public class EditGameCommandHandler : IRequestHandler<EditGameCommand>
    {
        private readonly IDataContext _context;
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;

        public EditGameCommandHandler(IImageService imageService, IMapper mapper, IDataContext context)
        {
            _imageService = imageService;
            _mapper = mapper;
            _context = context;
        }

        public async Task Handle(EditGameCommand request, CancellationToken cancellationToken)
        {
            if (request.UpdateGameDto.CoverImage is not null)
            {
                var coverImageName = await _imageService.CreateImageAsync("Games", request.UpdateGameDto.CoverImage);
                request.UpdateGameDto.CoverImageFileName = coverImageName;
            }
            await HandleGameEditing(request.Id, request.UpdateGameDto);
            await _context.SaveChangesAsync(cancellationToken);
        }

        private async Task HandleGameEditing(int? id, UpdateGameDto updateGameDto)
        {
            var game = await GetGameByIdAsync(id);

            if (updateGameDto.Images is not null && updateGameDto.Images.Any())
            {
                foreach (var image in updateGameDto.Images)
                {
                    var imageFileName = await _imageService.CreateImageAsync("Games", image);
                    updateGameDto.ImageFileNames.Add(imageFileName);
                }
            }
            game.Name = updateGameDto.Name;
            game.Description = updateGameDto.Description;
            game.PublisherId = updateGameDto.PublisherId;
            game.DeveloperId = updateGameDto.DeveloperId;
            game.IsDeleted = !updateGameDto.IsActive;
            game.Description = updateGameDto.Description;
            game.Price = updateGameDto.Price;

            game.Developer = await _context.Developers.FindAsync(updateGameDto.DeveloperId) ?? 
                             throw new DeveloperNotFoundException("No such developer");
            game.GameGenres = updateGameDto.SelectedGenres?.Select(genreId => new GameGenre { GenreId = genreId }).ToList();

            var existingImages = await _context.GameImages
                .Where(img => img.GameId == game.Id)
                .ToListAsync();

            var newImages = updateGameDto.ImageFileNames?
                .Where(imageFileName => imageFileName != null)
                .Select(imageFileName => new GameImage { FileName = imageFileName })
                .ToList();

            var mergedImages = existingImages.Concat(newImages ?? Enumerable.Empty<GameImage>()).ToList();

            game.Images = mergedImages;

            if (updateGameDto.CoverImageFileName is not null)
            {
                var existingCoverImage = mergedImages.FirstOrDefault(img => img.FileName == updateGameDto.CoverImageFileName);
                var existingCoverImageName = game.Images.Where(im => im.IsCoverImage == true).Select(im => im.FileName)
                    .FirstOrDefault();

                if (existingCoverImageName != null) await _imageService.DeleteImage("Games", existingCoverImageName);

                game.Images.Remove(game.Images.FirstOrDefault(im => im.IsCoverImage == true && im.GameId == id) ?? throw new ImageDeletionException("Error while deleting image!"));
                if (existingCoverImage == null)
                {
                    game.Images.Add(new GameImage { FileName = updateGameDto.CoverImageFileName, IsCoverImage = true });
                }
            }

            game.UpdatedAt = DateTime.UtcNow;
        }

        private async Task<Game> GetGameByIdAsync(int? id)
        {
            if (id is null) return new Game();
            var game = await _context.Games
                .Include(g => g.Developer)
                .Include(g => g.Publisher)
                .Include(g => g.Images)
                .Include(g => g.GameGenres)
                .ThenInclude(gg => gg.Genre)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (game is not null)
            {
                return game;
            }
            return new Game();
        }

    }
}
