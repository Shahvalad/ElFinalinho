using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Projecto.Application.Common.Models;

namespace Projecto.Application.Features.Friends.Queries.GetSentFriendshipRequests
{
    public record GetSentFriendshipRequestsQuery(string UserId) : IRequest<Result<List<GetProfileDto>>>;

    public class
        GetSentFriendshipRequestsQueryHandler : IRequestHandler<GetSentFriendshipRequestsQuery,
        Result<List<GetProfileDto>>>
    {
        private readonly IDataContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;

        public GetSentFriendshipRequestsQueryHandler(IDataContext context, IMapper mapper,
            UserManager<AppUser> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<Result<List<GetProfileDto>>> Handle(GetSentFriendshipRequestsQuery request,
            CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
            {
                return Result<List<GetProfileDto>>.Failure(new string[] { "No such user!" });
            }

            var profiles = user.SentFriendRequests
                .Where(x => !x.IsAccepted)
                .Select(x => x.User)
                .Select(x => _mapper.Map<GetProfileDto>(x))
                .ToList();

            return Result<List<GetProfileDto>>.Success(profiles);
        }
    }

}
