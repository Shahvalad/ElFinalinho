using Projecto.Application.Dtos.ProfileDtos;

namespace Projecto.MVC.ViewModels
{
    public class ProfileViewModel
    {
        public GetProfileDto User { get; set; }
        public CommunityPost? DisplayedPost { get; set; }
    }
}
