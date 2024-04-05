using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Application.Services.KeyService
{
    public interface IKeyService
    {
        Task<string> AssignKeyToUser(string username, int gameId);
    }
}
