using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Application.Features.Friends.Commands.RemoveFriend
{
    public record RemoveFriendCommand(string UserId, string FriendId) : IRequest<Result<Friendship>>;

    public class RemoveFriendCommandHandler : IRequestHandler<RemoveFriendCommand, Result<Friendship>>
    {
        private readonly IDataContext _context;

        public RemoveFriendCommandHandler(IDataContext context)
        {
            _context = context;
        }

        public async Task<Result<Friendship>> Handle(RemoveFriendCommand request, CancellationToken cancellationToken)
        {
            var friendship = await _context.Friendships
                .Where(f => f.UserId == request.UserId && f.RequesterId == request.FriendId || 
                            f.UserId == request.FriendId && f.RequesterId==request.UserId)
                .FirstOrDefaultAsync(cancellationToken);
            if (friendship == null)
            {
                return Result<Friendship>.Failure(new[]{ "Friendship not found" });
            }
            _context.Friendships.Remove(friendship);
            await _context.SaveChangesAsync(cancellationToken);

            return Result<Friendship>.Success(friendship);
        }
    }
}
