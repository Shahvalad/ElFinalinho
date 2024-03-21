namespace Projecto.Application.Features.Publishers.Queries.GetPublisher
{
    public record GetPublisherQuery(int? Id) : IRequest<GetPublisherDto>;

    public class GetPublisherQueryHandler : IRequestHandler<GetPublisherQuery, GetPublisherDto>
    {
        private readonly IDataContext _context;
        private readonly IMapper _mapper;

        public GetPublisherQueryHandler(IDataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<GetPublisherDto> Handle(GetPublisherQuery request,
            CancellationToken cancellationToken)
        {
            var publishers = await _context.Publisher
                .Include(p => p.Logo)
                .Where(p => p.Id == request.Id)
                .Select(p => _mapper.Map<GetPublisherDto>(p))
                .FirstOrDefaultAsync(cancellationToken) ?? throw new PublisherNotFoundException("There is no publisher with such id!");

            return publishers;
        }
    }

}
