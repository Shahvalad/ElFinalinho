using Projecto.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Domain.Models
{
    public class Listing : BaseAuditableEntity
    {
        public string UserId { get; set; }
        public AppUser User { get; set; }
        public int MarketItemId { get; set; }
        public MarketItem MarketItem { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public ListingStatus Status { get; set; } = ListingStatus.Active;
    }
}
