using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Domain.Models
{
    public class TarotCard : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public Rarity Rarity { get; set; }
        public double DropRate { get; set; }
        public List<UserTarotCard> UserTarotCards { get; set; } = new List<UserTarotCard>();
        public List<Listing> Listings { get; set; } = new List<Listing>();
    }
}
