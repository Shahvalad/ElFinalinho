using Projecto.Application.Dtos.PostsDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Application.Features.Communities.Queries.GetUsersCommunityPosts
{
    public record GetUsersCommunityPostsQuery(string UserId) : IRequest<List<GetPostDto>>;
    public class GetUsersCommunityPostsQueryHandler : IRequestHandler<GetUsersCommunityPostsQuery, List<GetPostDto>>
    {
        private readonly IDataContext _context;

        private readonly UserManager<AppUser> _userManager;
        public GetUsersCommunityPostsQueryHandler(IDataContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<List<GetPostDto>> Handle(GetUsersCommunityPostsQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users
                .AsNoTracking()
                .Include(u => u.Posts)
                .ThenInclude(p=>p.CommunityPostImage)
                .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);
            var posts = user.Posts.ToList();
            if(posts.Count == 0)
            {
                return new List<GetPostDto>();
            }
            var getPostDtos = posts.Select(post => new GetPostDto
            (
                post.Id,
                post.CommunityPostImage.FileName,
                post.Title
            )).ToList();

            return getPostDtos;
        }
    }


}
