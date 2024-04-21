using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Domain.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string SenderUsername { get; set; }
        public string RecipientUsername { get; set; }
        public string Text { get; set; }
        public bool IsRead { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
