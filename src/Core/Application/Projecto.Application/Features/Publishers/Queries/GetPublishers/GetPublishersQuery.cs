namespace Projecto.Application.Features.Publishers.Queries.GetPublishers
{
    public record GetPublishersQuery() : IRequest<IEnumerable<GetPublisherDto>>;

    public class GetPublisherQueryHandler : IRequestHandler<GetPublishersQuery, IEnumerable<GetPublisherDto>>
    {
        private readonly IDataContext _context;
        private readonly IMapper _mapper;

        public GetPublisherQueryHandler(IDataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetPublisherDto>> Handle(GetPublishersQuery request, CancellationToken cancellationToken)
        {
            var publishers = await _context.Publisher.Include(p=>p.Logo).AsNoTracking().ToListAsync(cancellationToken);
            return _mapper.Map<IEnumerable<GetPublisherDto>>(publishers);
        }
    }
}
