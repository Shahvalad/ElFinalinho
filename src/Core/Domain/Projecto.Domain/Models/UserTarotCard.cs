using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Domain.Models
{
    public class UserTarotCard
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; }

        public int TarotCardId { get; set; }
        public TarotCard TarotCard { get; set; }
        public bool IsDisplayedOnProfile { get; set; }
    }
}
