using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Domain.Exceptions
{
    public class InsufficientStockException : Exception
    {
        public InsufficientStockException() : base() { }
        public InsufficientStockException(string message) : base(message) { }
        public InsufficientStockException(string message, Exception inner) : base(message, inner) { }
    }
}
