namespace Projecto.Application.Features.Genres.Commands.Delete
{
    public record DeleteGenreCommand(int? Id) : IRequest;
    public class DeleteGenreCommandHandler : IRequestHandler<DeleteGenreCommand>
    {
        private readonly IDataContext _context;

        public DeleteGenreCommandHandler(IDataContext context)
        {
            _context = context;
        }

        public async Task Handle(DeleteGenreCommand request, CancellationToken cancellationToken)
        {
            var genre = await _context.Genre.FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken) ?? throw new GenreNotFoundException("There is no genre with such id!");
            _context.Genre.Remove(genre);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
