using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Application.Features.AdminDashboardUser.Commands.Logout
{
    public record LogoutCommand(): IRequest<Result<AppUser>>;
    public class LogoutCommandHandler : IRequestHandler<LogoutCommand, Result<AppUser>>
    {
        private readonly SignInManager<AppUser> _signInManager;

        public LogoutCommandHandler(SignInManager<AppUser> signInManager)
        {
            _signInManager = signInManager;
        }

        public async Task<Result<AppUser>> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            await _signInManager.SignOutAsync();
            return Result<AppUser>.Success();
        }
    }
}
