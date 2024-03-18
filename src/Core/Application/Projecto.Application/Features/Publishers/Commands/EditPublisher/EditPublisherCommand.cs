using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Projecto.Application.Dtos.Publisher;
using Projecto.Application.Services.ImageService;

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

            var publisher = await _context.Publisher.
                Include(p=>p.Logo).FirstOrDefaultAsync(p=>p.Id == request.Id, cancellationToken)
                            ??throw new PublisherNotFoundException("No publisher with such id!");
            _mapper.Map(request.PublisherDto, publisher);
            if (request.PublisherDto.PublisherLogo is not null)
            {
                if (publisher.Logo is not null)
                {
                    await _imageService.DeleteImage("Publishers", publisher.Logo.FileName);
                }

                var imageFileName = await _imageService.CreateImageAsync("Publishers", request.PublisherDto.PublisherLogo);
                publisher.Logo = (new PublisherImage() { FileName = imageFileName });
            }
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

}
