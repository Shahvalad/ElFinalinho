using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Projecto.Application.Dtos.DeveloperDtos
{
    public class UpdateDeveloperDto
    {
        [Required]
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public IFormFile? Logo { get; set; }
    }
}
