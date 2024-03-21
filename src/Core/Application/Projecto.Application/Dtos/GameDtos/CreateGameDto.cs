using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Projecto.Application.Dtos.GameDtos
{
    public class CreateGameDto
    {
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }

        [Display(Name = "Cover Image")]
        public IFormFile CoverImage { get; set; }
        public string? CoverImageFileName { get; set; }
        public ICollection<IFormFile>? Images { get; set; }
        public List<string> ImageFileNames { get; set; } = new List<string>();
        public string? Description { get; set; }

        [Display(Name = "Publisher")]
        public int? PublisherId { get; set; }

        [Display(Name = "Developer")]
        public int DeveloperId { get; set; }

        [Display(Name = "Genres")]
        public List<int>? SelectedGenres { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }
    }
}