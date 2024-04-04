using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Domain.Models
{
    public class CartItem
    {
        public Game Game { get; set; }
        public int Quantity { get; set; }
    }
}
