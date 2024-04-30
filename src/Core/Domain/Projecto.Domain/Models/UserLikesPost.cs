using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Domain.Models
{
    public class UserLikesPost
    {
        public string UserId { get; set; }
        public AppUser User { get; set; }
        public int PostId { get; set; }
        public CommunityPost Post { get; set; }

    }
}
