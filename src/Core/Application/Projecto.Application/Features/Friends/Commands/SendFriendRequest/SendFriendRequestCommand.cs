using Projecto.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Application.Features.Friends.Commands.SendFriendRequest
{
    public record SendFriendRequestCommand(string Username, string SenderId) : IRequest<Result<Friendship>>;
    public class SendFriendRequestCommandHandler : IRequestHandler<SendFriendRequestCommand, Result<Friendship>>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IDataContext _context;
        public SendFriendRequestCommandHandler(UserManager<AppUser> userManager, IDataContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<Result<Friendship>> Handle(SendFriendRequestCommand request, CancellationToken cancellationToken)
        {
            var receiver = await _userManager.FindByNameAsync(userName: request.Username);
            if (receiver == null)
            {
                return Result<Friendship>.Failure(new string[] { "No user with such nickname" });
            }

            var sender = await _userManager.FindByIdAsync(request.SenderId);
            if (sender == null)
            {
                return Result<Friendship>.Failure(new string[] { "No such user!" });
            }

            var friendships = _context.Friendships
                .Where(f => (f.UserId == receiver.Id && f.RequesterId == sender.Id) || (f.UserId == sender.Id && f.RequesterId == receiver.Id))
                .ToList();

            if (receiver.Id == sender.Id)
            {
                return Result<Friendship>.Failure(new string[] { "You can't send a friend request to yourself!" });
            }

            if (friendships.Any(f => f.IsAccepted == false))
            {
                return Result<Friendship>.Failure(new string[] { "You already sent this request!" });
            }

            if (friendships.Any(f => f.IsAccepted == true))
            {
                return Result<Friendship>.Failure(new string[] { "This user is already in your friends list!" });
            }

            var friendship = new Friendship
            {
                RequesterId = sender.Id,
                UserId = receiver.Id,
                IsAccepted = false
            };

            sender.SentFriendRequests.Add(friendship);
            receiver.ReceivedFriendRequests.Add(friendship);
            await _context.Friendships.AddAsync(friendship, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return Result<Friendship>.Success(message: "Friend request sent!");
        }

    }
}
