using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Projecto.Application.Dtos.Publisher
{
    public class CreatePublisherDto
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }

        [Display(Name = "Publisher Logo")]
        public IFormFile? Logo { get; set; }
    }
}
