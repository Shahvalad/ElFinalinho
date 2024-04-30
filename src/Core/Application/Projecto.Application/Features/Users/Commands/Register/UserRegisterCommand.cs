using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Application.Features.Users.Commands.Register
{
    public record UserRegisterCommand(string UserName, string Email, string Password) : IRequest<Result<AppUser>>;
    public class RegisterCommandHandler : IRequestHandler<UserRegisterCommand, Result<AppUser>>
    {
        private readonly UserManager<AppUser> _userManager;

        public RegisterCommandHandler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Result<AppUser>> Handle(UserRegisterCommand request, CancellationToken cancellationToken)
        {
            var user = new AppUser { UserName = request.UserName, Email = request.Email, Balance = 0, TotalSpendings = 0, MemberSince = DateTime.Now };
            var identityResult = await _userManager.CreateAsync(user, request.Password);
            if (identityResult.Succeeded)
            {
                return Result<AppUser>.Success(user);
            }
            return Result<AppUser>.Failure(identityResult.Errors.Select(e => e.Description));
        }
    }
}
