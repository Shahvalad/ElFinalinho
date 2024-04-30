
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace Projecto.Application.Features.AdminDashboardUser.Commands.Login
{
    public record LoginCommand(string Username, string Password) : IRequest<Result<AppUser>>;
    public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<AppUser>>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserClaimsPrincipalFactory<AppUser> _userClaimsPrincipalFactory;

        public LoginCommandHandler(UserManager<AppUser> userManager, IHttpContextAccessor httpContextAccessor, IUserClaimsPrincipalFactory<AppUser> userClaimsPrincipalFactory)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
        }

        public async Task<Result<AppUser>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(request.Username);
            if (user == null)
            {
                return Result<AppUser>.Failure(new[] { "Invalid username or password" });
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!isPasswordValid)
            {
                return Result<AppUser>.Failure(new[] { "Invalid username or password" });
            }

            var principal = await _userClaimsPrincipalFactory.CreateAsync(user);
            await _httpContextAccessor.HttpContext.SignInAsync("AdminAuth", principal);
            return Result<AppUser>.Success(user);
        }
    }

}
