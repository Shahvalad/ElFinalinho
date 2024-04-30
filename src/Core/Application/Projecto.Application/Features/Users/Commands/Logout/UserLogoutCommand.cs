using Projecto.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Application.Features.Users.Commands.Logout
{
    public class UserLogoutCommand : IRequest;
    public class UserLogoutCommandHandler : IRequestHandler<UserLogoutCommand>
    {
        private readonly SignInManager<AppUser> _signInManager;

        public UserLogoutCommandHandler(SignInManager<AppUser> signInManager)
        {
            _signInManager = signInManager;
        }

        public async Task Handle(UserLogoutCommand request, CancellationToken cancellationToken)
        {
            await _signInManager.SignOutAsync();
        }
    }
}
