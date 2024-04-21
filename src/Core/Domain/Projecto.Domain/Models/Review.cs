using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Domain.Models
{
    public class Review : BaseAuditableEntity
    {
        public bool IsLiked { get; set; }
        [MaxLength(500)]
        [Required]
        public string Comment { get; set; } = default!;
        public int GameId { get; set; } 
        public Game Game { get; set; } = default!;
        public string UserId { get; set; } = default!;
        public AppUser User { get; set; } = default!;
    }
}
