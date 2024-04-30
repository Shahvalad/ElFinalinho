using Projecto.Application.Dtos.UserDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Application.Features.Users.Queries.GetAll
{
    public record GetAllUsersQuery(List<string> Roles) : IRequest<List<GetUserDto>>;
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, List<GetUserDto>>
    {
        private readonly UserManager<AppUser> _userManager;

        public GetAllUsersQueryHandler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<List<GetUserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _userManager.Users
                .Where(u => request.Roles.Contains(u.IsAdmin ? "Admin" : (u.IsModerator ? "Moderator" : "User")))
                .Select(u => new GetUserDto
                (
                    u.Id,
                    u.UserName,
                    u.Email,
                    !u.IsBanned,
                    u.IsAdmin ? "Admin" : (u.IsModerator ? "Moderator" : "User")
                ))
                .ToListAsync(cancellationToken);
            return users;
        }
    }
}
