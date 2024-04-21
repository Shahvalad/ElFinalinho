namespace Projecto.Application.Features.Friends.Commands.DeclineFriendRequest
{
    public record DeclineFriendRequestCommand(int RequestId) : IRequest<Result<Friendship>>;

    public class DeclineFriendRequestCommandHandler : IRequestHandler<DeclineFriendRequestCommand, Result<Friendship>>
    {
        private readonly IDataContext _context;

        public DeclineFriendRequestCommandHandler(IDataContext context)
        {
            _context = context;
        }

        public async Task<Result<Friendship>> Handle(DeclineFriendRequestCommand request, CancellationToken cancellationToken)
        {
            var friendshipRequest = _context.Friendships.FirstOrDefault(f => f.Id == request.RequestId);
            if (friendshipRequest == null)
            {
                return Result<Friendship>.Failure(new []{ "Friendship request not found" });
            }
            _context.Friendships.Remove(friendshipRequest);
            await _context.SaveChangesAsync(cancellationToken);
            return Result<Friendship>.Success(friendshipRequest);
        }
    }
}
