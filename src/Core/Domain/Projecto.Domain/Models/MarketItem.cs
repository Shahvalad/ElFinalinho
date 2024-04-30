using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Domain.Models
{
    public class MarketItem : BaseEntity
    {
        public int Quantity { get; set; }
        public decimal StartingPrice { get; set; }
        public TarotCard TarotCard { get; set; }
        public List<Listing> Listings { get; set; } = new List<Listing>();
    }
}
