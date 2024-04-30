using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Application.Features.Reviews.Queries.GetCount
{
    public record GetAllReviewsCount(int id) : IRequest<int>;
    public class GetAllReviewsCountHandler : IRequestHandler<GetAllReviewsCount, int>
    {
        private readonly IDataContext _context;

        public GetAllReviewsCountHandler(IDataContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(GetAllReviewsCount request, CancellationToken cancellationToken)
        {
            var reviews = await _context.Reviews
                .AsNoTracking()
                .Where(r => r.GameId == request.id)
                .CountAsync(cancellationToken);

            return reviews;
        }
    }
}
