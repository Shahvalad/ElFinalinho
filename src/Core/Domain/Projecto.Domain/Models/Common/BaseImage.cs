using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace Projecto.Domain.Models.Common
{
    public abstract class BaseImage
    {
        public int Id { get; set; }
        public string FileName { get; set; } = null!;

        [NotMapped]
        [Display(Name = "Image File")]
        public IFormFile ImageFile { get; set; } = null!;
    }
}
