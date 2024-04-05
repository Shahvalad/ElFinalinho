using Microsoft.AspNetCore.Http;
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

        public async Task<string> AssignKeyToUser(string username, int gameId)
        {
            if(await _context.Games.FindAsync(gameId) == null)
            {
                throw new Exception("Game not found!");
            }
            if(_context.Games.Find(gameId).StockCount==0)
            {
                throw new Exception("NO STOCK!");
            }
            var key = _context.GameKeys.Where(g=>g.GameId==gameId).FirstOrDefault(k => k.IsAssigned == false);
            if (key == null)
            {
                throw new Exception("KEY IS NULL");
            }
            key.IsAssigned = true;
            key.AssignedTo = username;
            _context.Games.Find(gameId).StockCount--;
            await _context.SaveChangesAsync(cancellationToken:CancellationToken.None);
            return key.Value;
        }
    }
}
