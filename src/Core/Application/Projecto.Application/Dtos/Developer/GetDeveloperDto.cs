using Microsoft.AspNetCore.Http;

namespace Projecto.Application.Dtos.Developer
{
    public class GetDeveloperDto
    {
        public int Id { get; init; }
        public string Name { get; set; } = null!;
        public IFormFile? Logo { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
