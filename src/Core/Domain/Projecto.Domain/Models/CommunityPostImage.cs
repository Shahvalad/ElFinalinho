using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Domain.Models
{
    public class CommunityPostImage : BaseImage
    {
        public int CommunityPostId { get; set; }
        public CommunityPost CommunityPost { get; set; }
    }
}
