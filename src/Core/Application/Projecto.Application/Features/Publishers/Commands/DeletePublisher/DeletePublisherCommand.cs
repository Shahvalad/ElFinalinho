namespace Projecto.Application.Features.Publishers.Commands.DeletePublisher
{
    public record DeletePublisherCommand(int? Id) : IRequest;

    public class DeletePublisherCommandHandler : IRequestHandler<DeletePublisherCommand>
    {
        private readonly IDataContext _context;
        private readonly IImageService _imageService;

        public DeletePublisherCommandHandler(IDataContext context, IImageService imageService)
        {
            _context = context;
            _imageService = imageService;
        }

        public async Task Handle(DeletePublisherCommand request, CancellationToken cancellationToken)
        {
            var publisher = await _context.Publishers.Include(p=>p.Logo).FirstOrDefaultAsync(p=>p.Id == request.Id, cancellationToken)
                            ?? throw new PublisherNotFoundException("No publisher with such id!");
            if (publisher.Logo is not null)
            {
                var result = await _imageService.DeleteImage("Publishers", publisher.Logo.FileName);
                if (!result)
                {
                    throw new Exception("Failed to delete existing image.");
;               }
            }
            _context.Publishers.Remove(publisher);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
