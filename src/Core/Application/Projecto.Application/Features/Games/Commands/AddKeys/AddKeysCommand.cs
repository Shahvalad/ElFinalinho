namespace Projecto.Application.Features.Games.Commands.AddKeys
{
    public record AddKeysCommand(int? id, List<string> Keys) : IRequest;
    public class AddKeysCommandHandler : IRequestHandler<AddKeysCommand>
    {
        private readonly IDataContext _context;
        public AddKeysCommandHandler(IDataContext context)
        {
            _context = context;
        }
        public async Task Handle(AddKeysCommand request, CancellationToken cancellationToken)
        {
            var game = await _context.Games.FindAsync(request.id)??throw new GameNotFoundException("No game with such id!");
            foreach (var key in request.Keys)
            {
                if (game.GameKeys.Any(k => k.Value == key)) continue;
                game.GameKeys.Add(new GameKey { Value = key });
            }
            game.StockCount = game.StockCount + request.Keys.Count;
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

}
