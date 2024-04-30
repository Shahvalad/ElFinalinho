using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Application.Features.Posts.Commands.SetDisplayedPost
{
    public record SetDisplayedPostCommand(string UserId, int? PostId) : IRequest;

    public class SetDisplayedPostCommandHandler : IRequestHandler<SetDisplayedPostCommand>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IDataContext _context;
        public SetDisplayedPostCommandHandler(UserManager<AppUser> userManager, IDataContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task Handle(SetDisplayedPostCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users.Include(u => u.DisplayedPost).FirstOrDefaultAsync(u => u.Id == request.UserId);
            if (user == null)
            {
                throw new UserNotFoundException("No user with such id!");
            }
            user.DisplayedPostId = request.PostId;
            _context.Entry(user).State = EntityState.Modified; // Explicitly mark the entity as modified
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
