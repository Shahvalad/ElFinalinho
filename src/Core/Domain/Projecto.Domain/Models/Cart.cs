namespace Projecto.Domain.Models
{
    public class Cart
    {
        public List<CartItem> CartItems { get; set; } = new List<CartItem>();
        public decimal TotalPrice => CartItems.Sum(x => x.Game.Price * x.Quantity);
    }
}
