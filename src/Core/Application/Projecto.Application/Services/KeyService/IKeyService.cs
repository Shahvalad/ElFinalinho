using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Application.Services.KeyService
{
    public interface IKeyService
    {
        string AssignKeyToUser(string username);
    }
}
