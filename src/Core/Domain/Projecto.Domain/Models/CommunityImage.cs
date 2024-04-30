using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Domain.Models
{
    public class CommunityImage : BaseImage
    {
        public int CommunityId { get; set; }
        public Community Community { get; set; }
    }
}
