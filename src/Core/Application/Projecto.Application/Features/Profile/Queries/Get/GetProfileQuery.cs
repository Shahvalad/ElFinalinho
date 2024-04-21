using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Projecto.Application.Dtos.ProfileDtos;
using System.Security.Claims;

namespace Projecto.Application.Features.Profile.Queries.Get
{
    public record GetProfileQuery(ClaimsPrincipal User) : IRequest<GetProfileDto>;
    public class GetProfileQueryHandler : IRequestHandler<GetProfileQuery, GetProfileDto>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IDataContext _context;
        public GetProfileQueryHandler(UserManager<AppUser> userManager, IDataContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<GetProfileDto> Handle(GetProfileQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users.Include(u => u.ProfilePicture)
                .FirstOrDefaultAsync(u => u.UserName == request.User.Identity.Name, cancellationToken);
            var userGames = await _context.UserGames
                .Where(g => g.UserId == user.Id)
                .Include(g => g.Game)
                .Include(g => g.Game.Images)
                .ToListAsync(cancellationToken);

            List<GetGameDto> games = userGames.Select(g => new GetGameDto
            {
                Id = g.Game.Id,
                Name = g.Game.Name,
                Description = g.Game.Description,
                Developer = g.Game.Developer,
                Publisher = g.Game.Publisher,
                StockCount = g.Game.StockCount,
                Images = g.Game.Images.Select(i => new GameImage() { FileName = i.FileName, IsCoverImage = i.IsCoverImage }).ToList(),
                InStock = g.Game.StockCount > 0,
                CoverImageFileName = g.Game.Images.FirstOrDefault(i => i.IsCoverImage)!.FileName
            }).ToList();

            return new GetProfileDto(
                user.UserName,
                user.FirstName,
                user.LastName,
                user.Bio,
                user.Email,
                user.ProfilePicture?.FileName ?? "",
                user.MemberSince,
                games);
        }
    }
}
