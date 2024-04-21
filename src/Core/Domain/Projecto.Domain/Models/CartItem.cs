namespace Projecto.Domain.Models
{
    public class CartItem
    {
        public Game Game{ get; set; }
        private int _quantity;
        public int Quantity
        {
            get { return _quantity; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Quantity cannot be negative.");
                _quantity = value;
            }
        }
    }
}
