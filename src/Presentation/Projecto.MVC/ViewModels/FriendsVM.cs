using Projecto.Application.Dtos.FriendDtos;
using Projecto.Application.Dtos.ProfileDtos;

namespace Projecto.MVC.ViewModels
{
    public class FriendsVM
    {
        public List<GetFriendRequestDto> ReceivedFriendshipRequests { get; set; } = new List<GetFriendRequestDto>();
        public List<GetProfileDto> SentFriendshipRequests { get; set; } = new List<GetProfileDto>();
        public List<GetFriendDto> Friends { get; set; } = new List<GetFriendDto>();
        public bool IsReceivedFriendshipRequestsEmpty => ReceivedFriendshipRequests.Count == 0;
        public bool IsSentFriendshipRequestsEmpty => SentFriendshipRequests.Count == 0;
    }
}
