namespace Projecto.Infrastructure.Services
{
    public class KeyService : IKeyService
    {
        private readonly IDataContext _context;
        private readonly IEmailService _emailService;
        private readonly UserManager<AppUser> _userManager;

        public KeyService(IDataContext context, UserManager<AppUser> userManager, IEmailService emailService)
        {
            _context = context;
            _userManager = userManager;
            _emailService = emailService;
        }

        public async Task<string> AssignKeyToUser(string username, int gameId)
        {
            var game = await GetGame(gameId);
            var key = GetAvailableKeyForGame(gameId);
            var user = await GetUser(username);

            AssignKeyToUser(key, username);
            UpdateGameStock(game);
            await AddGameToUserGames(user, gameId);

            await _context.SaveChangesAsync(cancellationToken: CancellationToken.None);
            return key.Value;
        }

        private async Task<Game> GetGame(int gameId)
        {
            var game = await _context.Games.FindAsync(gameId);
            if (game == null || game.StockCount == 0)
            {
                throw new InsufficientStockException("No stock or game not found!");
            }
            return game;
        }

        private GameKey GetAvailableKeyForGame(int gameId)
        {
            var key = _context.GameKeys.Where(g => g.GameId == gameId)
                .FirstOrDefault(k => k.IsAssigned == false);
            if (key == null)
            {
                throw new InsufficientStockException("No keys left for this game.");
            }
            return key;
        }


        private async Task<AppUser> GetUser(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                throw new UserNotFoundException("User not found!");
            }
            return user;
        }

        private void AssignKeyToUser(GameKey key, string username)
        {
            key.IsAssigned = true;
            key.AssignedTo = username;
        }

        private void UpdateGameStock(Game game)
        {
            game.StockCount--;
            game.PurchaseCount++;
        }

        private async Task AddGameToUserGames(AppUser user, int gameId)
        {
            var userOwnedGame = await _context.UserGames
                .FirstOrDefaultAsync(ug => ug.UserId == user.Id && ug.GameId == gameId);

            if (userOwnedGame == null)
            {
                userOwnedGame = new UserGame { GameId = gameId, UserId = user.Id };
                await _context.UserGames.AddAsync(userOwnedGame);
            }
        }
    }
}
