using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Application.Features.Communities.Commands.DislikePost
{
    public record DislikeCommunityPostCommand(string UserId, int PostId) : IRequest;
    public class DislikeCommunityPostCommandHandler : IRequestHandler<DislikeCommunityPostCommand>
    {
        private readonly IDataContext _context;
        private readonly UserManager<AppUser> _userManager;
        public DislikeCommunityPostCommandHandler(IDataContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task Handle(DislikeCommunityPostCommand request, CancellationToken cancellationToken)
        {
            var post = await _context.CommunityPosts
                .Include(p => p.LikedByUsers)
                .FirstOrDefaultAsync(p => p.Id == request.PostId, cancellationToken);

            if (post is null)
                throw new PostNotFoundException("Post not found!");

            var user = await _userManager.FindByIdAsync(request.UserId);

            if (user is null)
                throw new UserNotFoundException("User not found!");


            var userLikesPost = post.LikedByUsers
                .FirstOrDefault(ulp => ulp.UserId == request.UserId && ulp.PostId == request.PostId);


            if (userLikesPost != null)
            {
                post.LikedByUsers.Remove(userLikesPost);
                post.LikesCount++;
            }
            else
            {
                userLikesPost = new UserLikesPost
                {
                    UserId = request.UserId,
                    PostId = request.PostId
                };
                post.LikedByUsers.Add(userLikesPost);
                post.LikesCount--;
            }

            await _context.SaveChangesAsync(cancellationToken);
        }

    }
}
