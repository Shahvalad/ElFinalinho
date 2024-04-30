using Projecto.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Application.Features.AdminDashboardUser.Commands.CreateAdmin
{
    public record CreateAdminCommand(string Username, string Email, string Password) : IRequest<Result<AppUser>>;
    public class CreateAdminCommandHandler : IRequestHandler<CreateAdminCommand, Result<AppUser>>
    {
        private readonly UserManager<AppUser> _userManager;

        public CreateAdminCommandHandler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Result<AppUser>> Handle(CreateAdminCommand request, CancellationToken cancellationToken)
        {
            var user = new AppUser
            {
               UserName = request.Username,
               Email = request.Email,
               IsAdmin = true
            };
            if (_userManager.Users.Any(u=>u.UserName.ToLower()==request.Username))
            {
                return Result<AppUser>.Failure(new[] { "Username already exists" });
            }
            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Admin");
                return Result<AppUser>.Success(user);
            }
            return Result<AppUser>.Failure(result.Errors.Select(e => e.Description).ToList());
        }
    }

}
