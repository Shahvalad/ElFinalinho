namespace Projecto.Application.Features.Publishers.Commands.EditPublisher
{
    public record EditPublisherCommand(int? Id, UpdatePublisherDto PublisherDto) : IRequest;

    public class EditPublicCommandHandler : IRequestHandler<EditPublisherCommand>
    {
        private readonly IDataContext _context;
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;
        public EditPublicCommandHandler(IDataContext context, IMapper mapper, IImageService imageService)
        {

            _context = context;
            _mapper = mapper;
            _imageService = imageService;
        }

        public async Task Handle(EditPublisherCommand request, CancellationToken cancellationToken)
        {
            var publisher = await _context.Publishers.
                Include(p=>p.Logo).FirstOrDefaultAsync(p=>p.Id == request.Id, cancellationToken)
                            ??throw new PublisherNotFoundException("No publisher with such id!");

            publisher = _mapper.Map(request.PublisherDto, publisher);
            if (request.PublisherDto.Logo is not null && publisher.Logo is not null)
            {
                var deleteResult = await _imageService.DeleteImage("Publishers", publisher.Logo.FileName);
                if (!deleteResult)
                {
                    throw new Exception("Failed to delete existing image.");
                }
                var imageFileName = await _imageService.CreateImageAsync("Publishers", request.PublisherDto.Logo);
                publisher.Logo = (new PublisherImage() { FileName = imageFileName });
            }
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

}
