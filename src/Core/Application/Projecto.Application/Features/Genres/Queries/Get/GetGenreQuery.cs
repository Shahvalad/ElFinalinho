namespace Projecto.Application.Features.Genres.Queries.Get
{
    public record GetGenreQuery(int? Id, bool IsTracking) : IRequest<GetGenreDto>;

    public class GetGenreQueryHandler : IRequestHandler<GetGenreQuery, GetGenreDto>
    {
        private readonly IDataContext _context;
        private readonly IMapper _mapper;

        public GetGenreQueryHandler(IDataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<GetGenreDto> Handle(GetGenreQuery request, CancellationToken cancellationToken)
        {
            var genre = new Genre();
            if (request.IsTracking)
            {
                genre = await _context.Genre.FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken) ?? throw new GenreNotFoundException("There is no genre with such id!");
            }
            genre = await _context.Genre.AsNoTracking().FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken) ?? throw new GenreNotFoundException("There is no genre with such id!");
            return _mapper.Map<GetGenreDto>(genre);
        }
    }
}
