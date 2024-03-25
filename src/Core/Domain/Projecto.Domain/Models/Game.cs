using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Projecto.Domain.Models.Common;

namespace Projecto.Domain.Models
{
    public class Game : BaseAuditableEntity
    {
        [Required]
        public string Name { get; set; } = null!;

        [Range(5, double.MaxValue, ErrorMessage = "Price must be greater than or equal to 5.")]
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public int ViewCount { get; set; }
        public int PurchaseCount { get; set; }
        public List<GameImage> Images { get; set; } = new List<GameImage>();

        //Navigation Properties 
        public int? PublisherId { get; set; }
        public Publisher? Publisher { get; set; }
        public int DeveloperId { get; set; }
        public Developer Developer { get; set; } = null!;

        public List<GameGenre> GameGenres { get; set; } = new List<GameGenre>();

    }
}
