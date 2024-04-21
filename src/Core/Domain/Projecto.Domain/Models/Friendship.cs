namespace Projecto.Domain.Models
{
    public class Friendship
    {
        public int Id { get; set; }
        public AppUser Requester { get; set; }
        public string RequesterId { get; set; }
        public AppUser User { get; set; }
        public string UserId { get; set; }
        public bool IsAccepted { get; set; }
    }
}
