namespace Projecto.MVC.ViewModels
{
    public class MarketVM
    {
        public List<MarketItem> MarketItems { get; set; } = new List<MarketItem>();
        public List<Listing> UsersPendingListings { get; set; } = new List<Listing>();
    }
}
