namespace Projecto.Application.Features.Developers.Commands.Edit
{
    public record EditDeveloperCommand(int? Id, UpdateDeveloperDto DeveloperDto) : IRequest;
    public class EditDeveloperCommandHandler : IRequestHandler<EditDeveloperCommand>
    {
        private readonly IDataContext _context;
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;
        public EditDeveloperCommandHandler(IDataContext context, IMapper mapper, IImageService imageService)
        {
            _context = context;
            _mapper = mapper;
            _imageService = imageService;
        }
        public async Task Handle(EditDeveloperCommand request, CancellationToken cancellationToken)
        {
            var developer = await _context.Developers.FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken) 
                ?? throw new DeveloperNotFoundException("There is no developer with such id!");
            developer = _mapper.Map(request.DeveloperDto, developer);
            if(request.DeveloperDto.Logo is not null)
            {
                if(developer.Logo is not null)
                {
                    await _imageService.DeleteImage("Developers", developer.Logo.FileName);
                }
                var image = await _imageService.CreateImageAsync("Developers", request.DeveloperDto.Logo);
                developer.Logo = new DeveloperImage { FileName = image };
            }
            _context.MarkAsModified(developer);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
