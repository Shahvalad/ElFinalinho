using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Projecto.Application.Common.Models;
using Projecto.Application.Dtos.FriendDtos;

namespace Projecto.Application.Features.Friends.Queries.GetReceivedFriendshipRequests
{
    public record GetReceivedFriendshipRequestsQuery(string UserId) : IRequest<Result<List<GetFriendRequestDto>>>;

    public class GetReceivedFriendshipRequestsQueryHandler : IRequestHandler<GetReceivedFriendshipRequestsQuery, Result<List<GetFriendRequestDto>>>
    {
        private readonly IDataContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        public GetReceivedFriendshipRequestsQueryHandler(IDataContext context, IMapper mapper, UserManager<AppUser> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<Result<List<GetFriendRequestDto>>> Handle(GetReceivedFriendshipRequestsQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users.Include(u => u.ReceivedFriendRequests)
                .SingleOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);
            if (user == null)
            {
                return Result<List<GetFriendRequestDto>>.Failure(new string[] { "No such user!" });
            }

            var requesterUsers = user.ReceivedFriendRequests
                .Where(x => !x.IsAccepted)
                .Select(f => _userManager.Users.Include(u=>u.ProfilePicture).FirstOrDefault(u => u.Id == f.RequesterId))
                .ToList();

            var getFriendRequestsDto = requesterUsers
                .Select(profile => new GetFriendRequestDto(
                    _context.Friendships
                        .Where(f => f.UserId == request.UserId && !f.IsAccepted)
                        .Select(f => f.Id)
                        .FirstOrDefault(),
                            profile.ProfilePicture?.FileName ?? "",
                            profile.UserName)).ToList();

            return Result<List<GetFriendRequestDto>>.Success(getFriendRequestsDto);
        }
    }

}
