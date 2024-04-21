using Projecto.Application.Dtos.ReviewDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Application.Features.Reviews.Queries.GetAll
{
    public record GetReviewsQuery(int GameId, int Page) : IRequest<PaginatedReviewsDto>;

    public class GetReviewsQueryHandler : IRequestHandler<GetReviewsQuery, PaginatedReviewsDto>
    {
        private readonly IDataContext _context;
        private readonly IMapper _mapper;

        public GetReviewsQueryHandler(IMapper mapper, IDataContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<PaginatedReviewsDto> Handle(GetReviewsQuery request, CancellationToken cancellationToken)
        {
            var reviews = await _context.Reviews
                .AsNoTracking()
                .Include(r => r.User)
                    .ThenInclude(u=>u.ProfilePicture)
                .Where(r => r.GameId == request.GameId)
                .OrderByDescending(r => r.CreatedAt)
                .Skip((request.Page - 1) * 5)
                .Take(5)
                .ToListAsync(cancellationToken);

            var totalReviews = await _context.Reviews
                .AsNoTracking()
                .Where(r => r.GameId == request.GameId)
                .CountAsync(cancellationToken);

            var paginatedReviewsDto = new PaginatedReviewsDto()
            {
                Reviews = reviews.ToList(),
                CurrentPage = request.Page,
                TotalPages = (int)Math.Ceiling(totalReviews / 5.0)
            };
            return paginatedReviewsDto;
        }
    }

    
}
