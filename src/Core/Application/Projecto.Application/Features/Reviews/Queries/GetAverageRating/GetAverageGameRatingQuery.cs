using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Application.Features.Reviews.Queries.GetAverageRating
{
    public record GetAverageGameRatingQuery(int id) : IRequest<double>;
    public class GetAverageGameRatingQueryHandler : IRequestHandler<GetAverageGameRatingQuery, double>
    {
        private readonly IDataContext _context;

        public GetAverageGameRatingQueryHandler(IDataContext context)
        {
            _context = context;
        }

        public async Task<double> Handle(GetAverageGameRatingQuery request, CancellationToken cancellationToken)
        {
            var reviews = await _context.Reviews
                .AsNoTracking()
                .Where(g => g.GameId == request.id)
                .ToListAsync(cancellationToken);

            if (!reviews.Any())
            {
                return 0; 
            }

            double averageRating = reviews.Average(r => r.IsLiked ? 5 : 1);
            return averageRating;
        }
    }
}
