using Projecto.Application.Dtos.UserDtos;

namespace Projecto.Application.Features.Users.Queries.GetById
{
    public record GetUserByIdQuery(string UserId) : IRequest<GetUserDto>;
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, GetUserDto>
    {
        private readonly UserManager<AppUser> _userManager;

        public GetUserByIdQueryHandler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<GetUserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users
                .Where(u => u.Id == request.UserId)
                .Select(u => new GetUserDto
                (
                 u.Id,
                 u.UserName,
                 u.Email,
                 !u.IsBanned,
                 u.IsAdmin ? "Admin" : (u.IsModerator ? "Moderator" : "User")
                 ))
                .FirstOrDefaultAsync(cancellationToken);
            return user;
        }
    }
}
