using Projecto.Application.Common.Models;

namespace Projecto.Application.Features.Friends.Commands.AcceptFriendRequest
{
    public record AcceptFriendRequestCommand(int FriendRequestId) : IRequest<Result<Friendship>>;

    public class AcceptFriendRequestCommandHandler : IRequestHandler<AcceptFriendRequestCommand, Result<Friendship>>
    {
        private readonly IDataContext _context;

        public AcceptFriendRequestCommandHandler(IDataContext context)
        {
            _context = context;
        }

        public async Task<Result<Friendship>> Handle(AcceptFriendRequestCommand request, CancellationToken cancellationToken)
        {
            var friendShip = await _context.Friendships.FirstOrDefaultAsync(f => f.Id == request.FriendRequestId, cancellationToken);
            if (friendShip == null)
            {
                return Result<Friendship>.Failure(new []{ "Friend request not found" });
            }
            friendShip.IsAccepted = true;
            await _context.SaveChangesAsync(cancellationToken);
            return Result<Friendship>.Success(friendShip);
        }
    }
}
