namespace Projecto.Application.Features.Games.Commands.Delete
{
    public record DeleteGameImageCommand(string FileName) : IRequest<int>;
    public class DeleteGameImageCommandHandler : IRequestHandler<DeleteGameImageCommand,int>
    {
        private readonly IDataContext _context;
        private readonly IImageService _imageService;

        public DeleteGameImageCommandHandler(IDataContext context, IImageService imageService)
        {
            _context = context;
            _imageService = imageService;
        }

        public async Task<int> Handle(DeleteGameImageCommand request, CancellationToken cancellationToken)
        {
            int id = _context.GameImages.Where(img => img.FileName == request.FileName).Select(img => img.GameId).FirstOrDefault();
            _context.GameImages.RemoveRange(_context.GameImages.Where(img => img.FileName == request.FileName));

            await _imageService.DeleteImage("Games", request.FileName);
            await _context.SaveChangesAsync(cancellationToken);
            return id;
        }
    }
}
