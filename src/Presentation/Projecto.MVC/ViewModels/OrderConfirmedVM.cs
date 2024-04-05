namespace Projecto.MVC.ViewModels
{
    public class OrderConfirmedVM
    {
        public string OrderId { get; set; }
        public long? AmountPaid { get; set; }
        public List<GameKey> GameKeys { get; set; }
    }
}
