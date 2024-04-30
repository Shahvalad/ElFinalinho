using Projecto.Application.Common.Models;

namespace Projecto.Application.Features.Communities.Queries.GetById
{
    public record GetCommunityByIdQuery(int? id) : IRequest<Result<CommunityWithPostsDto>>;
    public class GetCommunityByIdQueryHandler : IRequestHandler<GetCommunityByIdQuery, Result<CommunityWithPostsDto>>
    {
        private readonly IDataContext _context;
        public GetCommunityByIdQueryHandler(IDataContext context)
        {
            _context = context;
        }
        public async Task<Result<CommunityWithPostsDto>> Handle(GetCommunityByIdQuery request, CancellationToken cancellationToken)
        {
            var community = await _context.Communities
                .AsNoTracking()
                .Include(c => c.Image)
                .Include(c => c.Posts)
                    .ThenInclude(p=>p.CommunityPostImage)
                .Include(c => c.Posts)
                .ThenInclude(p=>p.User)
                    .ThenInclude(p=>p.ProfilePicture)
                .Include(c => c.Posts)
                .ThenInclude(c=>c.LikedByUsers)
                .FirstOrDefaultAsync(c => c.Id == request.id, cancellationToken);

            if (community is null)
                return Result<CommunityWithPostsDto>.Failure(new[] { "Community not found!" });

            var communityWithPostsDto = new CommunityWithPostsDto
                    (community.Id, community.Name, community.Threads, community.Image.FileName, community.Posts);

            return Result<CommunityWithPostsDto>.Success(communityWithPostsDto);
        }

    }
}
