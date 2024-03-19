
namespace Projecto.Application.Features.Publishers.Commands.CreatePublisher
{
    public record CreatePublisherCommand(CreatePublisherDto PublisherDto) : IRequest<int>;

    public class CreatePublisherCommandHandler : IRequestHandler<CreatePublisherCommand, int>
    {
        private readonly IDataContext _context;
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;
        public CreatePublisherCommandHandler(IDataContext context, IMapper mapper, IImageService imageService)
        {
            _context = context;
            _mapper = mapper;
            _imageService = imageService;
        }

        public async Task<int> Handle(CreatePublisherCommand request, CancellationToken cancellationToken)
        {
            var publisher = _mapper.Map<Publisher>(request.PublisherDto);
            if (request.PublisherDto.Logo is not null)
            {
                var imageFileName = await _imageService.CreateImageAsync("Publishers", request.PublisherDto.Logo);
                publisher.Logo = new PublisherImage() { FileName = imageFileName };
            }
            await _context.Publisher.AddAsync(publisher, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return publisher.Id;
        }
    }

}
