namespace Projecto.MVC.ViewModels
{
    public class CartVM
    {
        public Cart? Cart { get; set; }
        public decimal UserBalance { get; set; }
        public bool CartHasItems => Cart?.CartItems.Any() ?? false;
    }
}
