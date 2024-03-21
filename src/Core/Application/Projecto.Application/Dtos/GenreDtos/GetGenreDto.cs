namespace Projecto.Application.Dtos.GenreDtos
{
    public class GetGenreDto
    {
        public int Id { get; init; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

    }
}
