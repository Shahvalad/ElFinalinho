namespace Projecto.Domain.Models
{
    public class AppUserProfilePicture : BaseImage
    {
        public string UserId { get; set; }
        public AppUser User { get; set; } = null!;
    }
}
