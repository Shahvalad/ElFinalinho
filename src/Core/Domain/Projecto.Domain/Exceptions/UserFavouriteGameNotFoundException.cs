using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Domain.Exceptions
{
    public class UserFavouriteGameNotFoundException : Exception
    {
        public UserFavouriteGameNotFoundException(string message) : base(message)
        {
            
        }
    }
}
