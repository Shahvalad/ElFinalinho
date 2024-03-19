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
            if (request.PublisherDto == null)
            {
                throw new ArgumentNullException(nameof(request.PublisherDto));
            }

            var publisher = await _context.Publisher.
                Include(p=>p.Logo).FirstOrDefaultAsync(p=>p.Id == request.Id, cancellationToken)
                            ??throw new PublisherNotFoundException("No publisher with such id!");

            if (publisher.Logo is not null)
            {
                var deleteResult = await _imageService.DeleteImage("Publishers", publisher.Logo.FileName);
                if (!deleteResult)
                {
                    throw new Exception("Failed to delete existing image.");
                }
            }
            _mapper.Map(request.PublisherDto, publisher);

            if (request.PublisherDto.Logo is not null)
            {
                var imageFileName = await _imageService.CreateImageAsync("Publishers", request.PublisherDto.Logo);
                publisher.Logo = (new PublisherImage() { FileName = imageFileName });
            }

            _context.MarkAsModified(publisher);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

}
