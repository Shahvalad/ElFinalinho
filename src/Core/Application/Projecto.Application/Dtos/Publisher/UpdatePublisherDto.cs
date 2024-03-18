using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Projecto.Application.Dtos.Publisher
{
    public class UpdatePublisherDto
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }

        [Display(Name = "Publisher Logo")]
        public IFormFile? PublisherLogo { get; set; }
        public string? PublisherLogoFileName { get; set; }
    }
}
