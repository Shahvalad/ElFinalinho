
namespace Projecto.Application.Features.Publishers.Commands.CreatePublisher
{
    public record CreatePublisherCommand(CreatePublisherDto PublisherDto) : IRequest<int>;

    public class CreatePublisherCommandHandler(IDataContext context, IMapper mapper, IImageService imageService)
        : IRequestHandler<CreatePublisherCommand, int>
    {
        public async Task<int> Handle(CreatePublisherCommand request, CancellationToken cancellationToken)
        {
            var publisher = mapper.Map<Publisher>(request.PublisherDto);
            if (request.PublisherDto.Logo is not null)
            {
                var imageFileName = await imageService.CreateImageAsync("Publishers", request.PublisherDto.Logo);
                publisher.Logo = new PublisherImage() { FileName = imageFileName };
            }
            await context.Publisher.AddAsync(publisher, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            return publisher.Id;
        }
    }

}
