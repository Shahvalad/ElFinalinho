using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Projecto.Application.Dtos.Publisher
{
    public class GetPublisherDto
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
