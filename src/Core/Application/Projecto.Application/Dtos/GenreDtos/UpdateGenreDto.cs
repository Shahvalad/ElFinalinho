using System.ComponentModel.DataAnnotations;

namespace Projecto.Application.Dtos.GenreDtos
{
    public class UpdateGenreDto
    {
        [Required] 
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }
}
