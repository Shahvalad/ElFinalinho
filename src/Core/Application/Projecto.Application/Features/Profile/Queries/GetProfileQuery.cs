using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Projecto.Application.Dtos.ProfileDtos;
using System.Security.Claims;

namespace Projecto.Application.Features.Profile.Queries
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
            var user = await _userManager.GetUserAsync(request.User);
            return new GetProfileDto(
                user.FirstName,
                user.LastName,
                user.Bio,
                user.Email,
                user.MemberSince,
                await _context.UserGames
                    .Where(g => g.UserId == user.Id)
                    .Select(g => g.Game).ToListAsync());
        }
    }
}
