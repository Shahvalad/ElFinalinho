using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Application.Features.Communities.Commands.LikePost
{
    public record LikeCommunityPostCommand(string UserId, int PostId) : IRequest;
    public class LikeCommunityPostCommandHandler:IRequestHandler<LikeCommunityPostCommand>
    {
        private readonly IDataContext _context;
        private readonly UserManager<AppUser> _userManager;

        public LikeCommunityPostCommandHandler(IDataContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task Handle(LikeCommunityPostCommand request, CancellationToken cancellationToken)
        {

            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
                throw new UserNotFoundException("No such user!");

            var post = await _context.CommunityPosts
                .Include(p=>p.LikedByUsers)
                .FirstOrDefaultAsync(p=>p.Id == request.PostId, cancellationToken);

            if (post == null)
                throw new PostNotFoundException("No such post");

            var userLikesPost = post.LikedByUsers
                .FirstOrDefault(ulp => ulp.UserId == request.UserId && ulp.PostId == request.PostId);

            if (userLikesPost != null)
            {
                post.LikedByUsers.Remove(userLikesPost);
                post.LikesCount--;
            }
            else
            {
                userLikesPost = new UserLikesPost
                {
                    UserId = request.UserId,
                    PostId = request.PostId
                };
                post.LikedByUsers.Add(userLikesPost);
                post.LikesCount++;
            }


            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
