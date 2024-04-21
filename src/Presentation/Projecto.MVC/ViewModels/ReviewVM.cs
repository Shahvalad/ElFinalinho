namespace Projecto.MVC.ViewModels
{
    public class ReviewVM
    {
        public List<Review> Reviews { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

    }
}
