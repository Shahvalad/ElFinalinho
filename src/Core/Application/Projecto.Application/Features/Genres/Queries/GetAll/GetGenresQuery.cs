namespace Projecto.Application.Features.Genres.Queries.GetAll
{
    public record GetGenresQuery() : IRequest<IEnumerable<GetGenreDto>>;

    public class GetGenresQueryHandler : IRequestHandler<GetGenresQuery, IEnumerable<GetGenreDto>>
    {
        private readonly IDataContext _context;
        private readonly IMapper _mapper;
        public GetGenresQueryHandler(IDataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetGenreDto>> Handle(GetGenresQuery request, CancellationToken cancellationToken)
        {
            var genres = await _context.Genres.AsNoTracking().ToListAsync(cancellationToken);
            return _mapper.Map<IEnumerable<GetGenreDto>>(genres);
        }
    }
}
