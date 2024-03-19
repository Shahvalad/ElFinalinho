
namespace Projecto.Application.Features.Developers.Commands.Create
{
    public record CreateDeveloperCommand(CreateDeveloperDto DeveloperDto) : IRequest<int>;
    public class CreateDeveloperCommandHandler : IRequestHandler<CreateDeveloperCommand, int>
    {
        private readonly IDataContext _context;
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;
        public CreateDeveloperCommandHandler(IDataContext context, IMapper mapper, IImageService imageService)
        {
            _context = context;
            _mapper = mapper;
            _imageService = imageService;
        }

        public async Task<int> Handle(CreateDeveloperCommand request, CancellationToken cancellationToken)
        {
            var developer = _mapper.Map<Developer>(request.DeveloperDto);
            if(request.DeveloperDto.Logo is not null)
            {
                var image = await _imageService.CreateImageAsync("Developers", request.DeveloperDto.Logo);
                developer.Logo = new DeveloperImage { FileName = image };
            }
            await _context.Developer.AddAsync(developer, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return developer.Id;
        }
    }
}
