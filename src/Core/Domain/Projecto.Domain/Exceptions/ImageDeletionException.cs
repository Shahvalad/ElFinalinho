using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Domain.Exceptions
{
    public class ImageDeletionException : Exception
    {
        public ImageDeletionException(string message) : base(message)
        {
        }
    }
}
