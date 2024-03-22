using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Application.Dtos.GameDtos
{
    public class GetGameDto
    {
        public int Id { get; init; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public List<GameImage>? Images { get; set; }
        public List<GameGenre> GameGenres { get; set; } = new List<GameGenre>();
        public Publisher? Publisher { get; set; }
        public Developer Developer { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsActive { get; set; }
    }
}
