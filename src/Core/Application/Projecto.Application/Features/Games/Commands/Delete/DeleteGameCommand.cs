namespace Projecto.Application.Features.Games.Commands.Delete
{
    public record DeleteGameCommand(int? Id) : IRequest;
    public class DeleteGameCommandHandler : IRequestHandler<DeleteGameCommand>
    {
        private readonly IDataContext _context;
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;

        public DeleteGameCommandHandler(IMapper mapper, IDataContext context, IImageService imageService)
        {
            _mapper = mapper;
            _context = context;
            _imageService = imageService;
        }

        public async Task Handle(DeleteGameCommand request, CancellationToken cancellationToken)
        {
            var game = await _context.Games.Include(g=>g.Images).FirstOrDefaultAsync(g => g.Id == request.Id) ??
                throw new GameNotFoundException("There is no game with such id!");

            foreach(var image in game.Images)
            {
               await _imageService.DeleteImage("Games", image.FileName);
            }

            _context.Games.Remove(game);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
