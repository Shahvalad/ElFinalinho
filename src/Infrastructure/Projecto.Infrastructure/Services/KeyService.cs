using Projecto.Application.Common.Interfaces;
using Projecto.Application.Services.KeyService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Infrastructure.Services
{
    public class KeyService : IKeyService
    {
        private readonly IDataContext _context;

        public KeyService(IDataContext context)
        {
            _context = context;
        }

        public string AssignKeyToUser(string username)
        {
            var key = _context.GameKeys.FirstOrDefault(k => k.IsAssigned == false);
            if (key == null)
            {
                return null;
            }
            key.IsAssigned = true;
            key.AssignedTo = username;
            _context.Games.Find(key.GameId).StockCount--;
            _context.SaveChangesAsync(cancellationToken:CancellationToken.None);

            return key.Value;
        }
    }
}
