namespace Projecto.Application.Features.Developers.Commands.Delete
{
    public record DeleteDeveloperCommand(int? Id) : IRequest;
    public class DeleteDeveloperCommandHandler : IRequestHandler<DeleteDeveloperCommand>
    {
        private readonly IDataContext _context;
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;
        public DeleteDeveloperCommandHandler(IDataContext context, IMapper mapper, IImageService imageService)
        {
            _context = context;
            _mapper = mapper;
            _imageService = imageService;
        }

        public async Task Handle(DeleteDeveloperCommand request, CancellationToken cancellationToken)
        {
            if (request.Id is null)
            {
                throw new ArgumentNullException(nameof(request.Id), "Id cannot be null.");
            }

            var developer = await GetDeveloperAsync(request.Id.Value, cancellationToken);
            await DeleteImageAsync(developer);
            await DeleteDeveloper(developer, cancellationToken);
        }

        private async Task<Developer> GetDeveloperAsync(int id, CancellationToken cancellationToken)
        {
            var developer = await _context.Developers.Include(d => d.Logo).FirstOrDefaultAsync(p => p.Id == id, cancellationToken) ??
                throw new DeveloperNotFoundException("There is no developer with such id!");
            return developer;
        }

        private async Task DeleteImageAsync(Developer developer)
        {
            if (developer.Logo is not null)
            {
                var result = await _imageService.DeleteImage("Developers", developer.Logo.FileName);
                if (!result)
                {
                    throw new ImageDeletionException("Failed to delete existing image.");
                }
            }
        }

        private async Task DeleteDeveloper(Developer developer, CancellationToken cancellationToken)
        {
            _context.Developers.Remove(developer);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}