using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Application.Features.Games.Queries.GetRandomGamesQuery
{
    public record GetRandomGamesQuery() : IRequest<List<GetGameDto>>;
    public class GetRandomGamesQueryHandler : IRequestHandler<GetRandomGamesQuery, List<GetGameDto>>
    {
        private readonly IDataContext _context;

        public GetRandomGamesQueryHandler(UserManager<AppUser> userManager, IDataContext context)
        {
            _context = context;
        }

        public async Task<List<GetGameDto>> Handle(GetRandomGamesQuery request, CancellationToken cancellationToken)
        {
            var games = await _context.Games
                .AsNoTracking().
                Where(g=>g.Id == 2||g.Id==5)
                .Include(g=>g.Images)
                .ToListAsync(cancellationToken);

            var gameDtos = games.Select(g => new GetGameDto
            {
                Id = g.Id,
                Name = g.Name,
                Description = g.Description,
                Price = g.Price,
                StockCount = g.StockCount,
                Images = g.Images,
            }).ToList();
            return gameDtos; 
        }
    }
}
