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
            var developer = await _context.Developers.Include(d=>d.Logo).FirstOrDefaultAsync(p=>p.Id == request.Id, cancellationToken) ??
                throw new DeveloperNotFoundException("There is no developer with such id!");
            if(developer.Logo is not null)
            {
                var result = await _imageService.DeleteImage("Developers", developer.Logo.FileName);
                if(!result)
                {
                    throw new Exception("Failed to delete existing image.");
                }
            }
            _context.Developers.Remove(developer);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
