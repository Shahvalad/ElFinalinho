using Projecto.Application.Dtos.ReviewDtos;

namespace Projecto.MVC.ViewModels
{
    public class GameDetailsVM
    {
        public GetGameDto Game { get; set; }
        public ReviewCreateDto NewReview { get; set; }
        public ReviewVM PaginatedReviews { get; set; }
        public double AverageRating { get; set; }
        public int ReviewsCount { get; set; }
    }
}
