using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Application.Dtos.MessageDtos
{
    public class MessageDto
    {
        public string Text { get; set; }
        public bool IsSent { get; set; }
        public string UserProfilePictureName { get; set; } = "default.png";
        public bool IsRead { get; set; }
    }
}
