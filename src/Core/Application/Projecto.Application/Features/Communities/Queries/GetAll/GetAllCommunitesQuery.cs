using Projecto.Application.Dtos.CommunityDtos;
namespace Projecto.Application.Features.Communities.Queries.GetAll
{
    public record GetAllCommunitesQuery() : IRequest<List<CommunityDto>>;
    public class GetAllCommunitesQueryHandler : IRequestHandler<GetAllCommunitesQuery, List<CommunityDto>>
    {
        private readonly IDataContext _context;

        public GetAllCommunitesQueryHandler(IDataContext context)
        {
            _context = context;
        }

        public async Task<List<CommunityDto>> Handle(GetAllCommunitesQuery request, CancellationToken cancellationToken)
        {
            var communities = await _context.Communities
                .AsNoTracking()
                .Include(c => c.Image)
                .Select(c => new CommunityDto(c.Id, c.Name, c.Threads, c.Image.FileName))
                .ToListAsync(cancellationToken);
            return communities;
        }
    }
}
