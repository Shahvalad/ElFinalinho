using Projecto.Application.Features.Profile.Queries.Get;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Application.Features.Profile.Queries.GetWithMessageStatus
{
   public record GetProfileWithMessageStatusQuery(string UserId) : IRequest<Result<UserProfileWithMessageStatusDto>>;

    public class GetProfileWithMessageStatusQueryHandler : IRequestHandler<GetProfileWithMessageStatusQuery, Result<UserProfileWithMessageStatusDto>>
    {
        private readonly IDataContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;

        public GetProfileWithMessageStatusQueryHandler(IDataContext context, IMapper mapper, UserManager<AppUser> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<Result<UserProfileWithMessageStatusDto>> Handle(GetProfileWithMessageStatusQuery request,
       CancellationToken cancellationToken)
        {
            var user = await _userManager.Users
                .Include(u => u.ProfilePicture)
                .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

            if (user == null)
            {
                return Result<UserProfileWithMessageStatusDto>.Failure(new[] { "FAIl" });
            }

            var userGames = await _context.UserGames
                .Where(g => g.UserId == user.Id)
                .Include(g => g.Game)
                .Include(g => g.Game.Images)
                .ToListAsync(cancellationToken);

            var userFriendships = await _context.Friendships
                .Where(f => f.RequesterId == request.UserId && f.IsAccepted ||
                                    f.UserId == request.UserId && f.IsAccepted)
                .ToListAsync(cancellationToken);

            var userFriendIds = userFriendships
                .Select(f => f.RequesterId == request.UserId ? f.UserId : f.RequesterId)
                .Distinct()
                .ToList();

            var userFriends = await _userManager.Users
                .Include(u => u.ProfilePicture)
                .Where(u => userFriendIds.Contains(u.Id))
                .ToListAsync(cancellationToken);

            var userchatprofiles = userFriends.Select(u => new UserChatProfileDto
            (
                u.Id,
                u.UserName ?? "",
                u.ProfilePicture?.FileName ?? "",
                _context.Messages.Any(m => m.RecipientUsername == user.UserName && m.SenderUsername == u.UserName && !m.IsRead)
            )).ToList();

            List<GetGameDto> games = userGames.Select(g => new GetGameDto
            {
                Id = g.Game.Id,
                Name = g.Game.Name,
                Description = g.Game.Description,
                Developer = g.Game.Developer,
                Publisher = g.Game.Publisher,
                StockCount = g.Game.StockCount,
                Images = g.Game.Images.Select(i => new GameImage()
                { FileName = i.FileName, IsCoverImage = i.IsCoverImage }).ToList(),
                InStock = g.Game.StockCount > 0,
                CoverImageFileName = g.Game.Images.FirstOrDefault(i => i.IsCoverImage)?.FileName
            }).ToList();

            return Result<UserProfileWithMessageStatusDto>.Success(new UserProfileWithMessageStatusDto
            {
                UserName = user.UserName,
                Bio = user.Bio ?? "",
                ProfilePictureName = user.ProfilePicture?.FileName ?? "",
                MemberSince = user.MemberSince,
                Games = games,
                Friends = userchatprofiles,
                HasFriends = userchatprofiles.Any()
            });
        }

    }
}
