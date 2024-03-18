using MediatR;
using Microsoft.EntityFrameworkCore;
using Projecto.Application.Common.Interfaces;
using Projecto.Application.Dtos.Genre;
using Projecto.Domain.Exceptions;

namespace Projecto.Application.Features.Genres.Commands.EditGenre
{
    public record EditGenreCommand(int? Id, UpdateGenreDto GenreDto) : IRequest<UpdateGenreDto>;

    public class EditGenreCommandHandler : IRequestHandler<EditGenreCommand, UpdateGenreDto>
    {
        private readonly IDataContext _context;
        private readonly IMapper _mapper;
        public EditGenreCommandHandler(IDataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UpdateGenreDto> Handle(EditGenreCommand request, CancellationToken cancellationToken)
        {
            var genre = await _context.Genre.FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken) ?? throw new GenreNotFoundException("There is no genre with such id!");
            genre = _mapper.Map(request.GenreDto, genre);
            await _context.SaveChangesAsync(cancellationToken);
            return _mapper.Map<UpdateGenreDto>(genre);
        }
    }
}
