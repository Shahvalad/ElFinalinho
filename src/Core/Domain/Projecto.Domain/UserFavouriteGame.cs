using Projecto.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Domain
{
    public class UserFavouriteGame
    {
        public string UserId { get; set; }
        public int GameId { get; set; }
        public AppUser User { get; set; }
        public Game Game { get; set; }
    }
}
