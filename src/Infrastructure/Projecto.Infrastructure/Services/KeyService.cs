using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Projecto.Application.Common.Interfaces;
using Projecto.Application.Services.KeyService;
using Projecto.Domain.Exceptions;
using Projecto.Domain.Models;
using Stripe;
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
        private readonly UserManager<AppUser> _userManager;
        public KeyService(IDataContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
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
            if(key == null)
            {
                throw new Exception("No keys left for this game.");
            }
            key.IsAssigned = true;
            key.AssignedTo = username;

            var game = await _context.Games.FindAsync(gameId) ?? throw new GameNotFoundException("There is no game with such id!");
            game.StockCount--;
            game.PurchaseCount++;
            var user = await _userManager.FindByNameAsync(username);
            var userGame = await _context.UserGames
                .FirstOrDefaultAsync(ug => ug.UserId == user.Id && ug.GameId == gameId);
            if (userGame == null)
            {
                userGame = new UserGame { GameId = gameId, UserId = user.Id };
                await _context.UserGames.AddAsync(userGame);
            }


            await _context.SaveChangesAsync(cancellationToken:CancellationToken.None);
            return key.Value;
        }
    }
}
