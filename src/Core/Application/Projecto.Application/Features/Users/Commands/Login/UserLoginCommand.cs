using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Application.Features.Users.Commands.Login
{
    public record UserLoginCommand(string UserName, string Password, bool RememberMe) : IRequest<LoginResult>;
    public class LoginCommandHandler : IRequestHandler<UserLoginCommand, LoginResult>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public LoginCommandHandler(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<LoginResult> Handle(UserLoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user != null)
            {
                if (!await _userManager.IsEmailConfirmedAsync(user))
                {
                    return new LoginResult { Status = LoginStatus.EmailNotConfirmed };
                }

                var result = await _signInManager.PasswordSignInAsync(request.UserName, request.Password, request.RememberMe, false);

                if (result.Succeeded)
                {
                    return new LoginResult { Status = LoginStatus.Success };
                }
            }

            return new LoginResult { Status = LoginStatus.Failure };
        }
    }
}
