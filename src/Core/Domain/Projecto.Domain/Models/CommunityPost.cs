using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Domain.Models
{
    public class CommunityPost : BaseAuditableEntity
    {
        public string? Title { get; set; }
        public bool isSpoiler { get; set; }
        public int CommunityId { get; set; }
        public int LikesCount { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; }
        public Community Community { get; set; }
        public CommunityPostImage CommunityPostImage { get; set; }
        public List<UserLikesPost>? LikedByUsers { get; set; } = new();
    }

}
