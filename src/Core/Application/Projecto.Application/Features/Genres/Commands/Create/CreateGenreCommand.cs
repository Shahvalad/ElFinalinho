using Microsoft.AspNetCore.Http;

namespace Projecto.Application.Features.Genres.Commands.Create
{

    public record CreateGenreCommand(CreateGenreDto GenreDto) : IRequest<int>;

    public class CreateGenreCommandHandler : IRequestHandler<CreateGenreCommand, int>
    {
        private readonly IDataContext _context;
        private readonly IMapper _mapper;
        public CreateGenreCommandHandler(IDataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateGenreCommand request, CancellationToken cancellationToken)
        {
            var genre = _mapper.Map<Genre>(request.GenreDto);
            await _context.Genre.AddAsync(genre, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return genre.Id;

        }
    }
}
