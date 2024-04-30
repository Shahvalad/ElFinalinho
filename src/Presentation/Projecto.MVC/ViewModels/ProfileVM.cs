namespace Projecto.MVC.ViewModels
{
    public class ProfileVM
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Bio { get; set; }
        public DateTime RegistrationDate { get; set; }
        public List<Game> Games { get; set; } = new List<Game>();
        public CommunityPost DisplayedPost { get; set; }
    }
}
