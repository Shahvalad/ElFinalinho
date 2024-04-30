using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Application.Features.Posts.Queries.GetPostDisplayedOnProfile
{
    public record GetPostDisplayedOnProfileQuery(string UserId) : IRequest<CommunityPost>;
    public class GetPostDisplayedOnProfileQueryHandler : IRequestHandler<GetPostDisplayedOnProfileQuery, CommunityPost>
    {
        private readonly UserManager<AppUser> _userManager;

        public GetPostDisplayedOnProfileQueryHandler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<CommunityPost> Handle(GetPostDisplayedOnProfileQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users
                .Include(u => u.DisplayedPost)
                .ThenInclude(p => p.CommunityPostImage)
                .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

            return user?.DisplayedPost;
        }
    }
}
