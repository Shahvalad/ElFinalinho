using Projecto.Domain.Models;

namespace Projecto.Application.Features.Genres.Commands.CreateGenre
{

    public record CreateGenreCommand(CreateGenreDto GenreDto) : IRequest;

    public class CreateGenreCommandHandler : IRequestHandler<CreateGenreCommand>
    {
        
        private readonly IDataContext _context;
        private readonly IMapper _mapper;
        public CreateGenreCommandHandler(IDataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task Handle(CreateGenreCommand request, CancellationToken cancellationToken)
        {
            var genre = _mapper.Map<Genre>(request.GenreDto);
            await _context.Genre.AddAsync(genre, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
