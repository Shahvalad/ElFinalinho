namespace Projecto.Application.Features.Games.Commands.AddKeys
{
    public record AddKeysCommand(int? Id, List<string> Keys) : IRequest;
    public class AddKeysCommandHandler : IRequestHandler<AddKeysCommand>
    {
        private readonly IDataContext _context;
        public AddKeysCommandHandler(IDataContext context)
        {
            _context = context;
        }
        public async Task Handle(AddKeysCommand request, CancellationToken cancellationToken)
        {
            var game = await _context.Games.Include(g=>g.GameKeys).FirstOrDefaultAsync(g=>g.Id == request.Id)
                ??throw new GameNotFoundException("No game with such id!");

            foreach (var key in request.Keys)
            {
                if (game.GameKeys.Any(k => k.Value == key)) continue;
                game.GameKeys.Add(new GameKey { Value = key });
                game.StockCount++;
            }
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

}
