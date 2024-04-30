namespace Projecto.MVC.ViewModels
{
    public class SellYourItemVM
    {
        public MarketItem MarketItem { get; set; }
        public decimal AverageSellingPrice { get; set; }
        public decimal LowestSellingPrice { get; set; }
        public decimal HighestSellingPrice { get; set; }
    }
}
