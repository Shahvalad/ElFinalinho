using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Projecto.Application.Dtos.FriendDtos;

namespace Projecto.Application.Features.Friends.Queries.GetAll
{
    public record GetAllFriendsQuery(string UserId) : IRequest<Result<List<GetFriendDto>>>;
    public class GetAllFriendsQueryHandler : IRequestHandler<GetAllFriendsQuery, Result<List<GetFriendDto>>>
    {
        private readonly IDataContext _context;
        private readonly UserManager<AppUser> _userManager;
        public GetAllFriendsQueryHandler(IDataContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<Result<List<GetFriendDto>>> Handle(GetAllFriendsQuery request, CancellationToken cancellationToken)
        {
            var friends = await _context.Friendships
                .Where(f => f.UserId == request.UserId && f.IsAccepted || f.RequesterId == request.UserId && f.IsAccepted)
                .ToListAsync(cancellationToken);

            var userIds = friends.Select(f => f.UserId == request.UserId ? f.RequesterId : f.UserId).ToList();

            var users = await _userManager.Users
                .Where(u => userIds.Contains(u.Id))
                .Include(u => u.ProfilePicture)
                .ToListAsync(cancellationToken);

            var friendDtos = users
                .Select(u => new GetFriendDto(u.Id,u.ProfilePicture?.FileName ?? "profile.png", u.UserName))
                .ToList();

            return Result<List<GetFriendDto>>.Success(friendDtos);
        }

    }
}
