namespace Projecto.Application.Features.Users.Commands.ConfirmEmail
{
    public record UserConfirmEmailCommand(string UserId, string Token) : IRequest<Result<AppUser>>;
    public class UserConfirmEmailCommandHandler : IRequestHandler<UserConfirmEmailCommand, Result<AppUser>>
    {
        private readonly UserManager<AppUser> _userManager;

        public UserConfirmEmailCommandHandler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Result<AppUser>> Handle(UserConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
            {
                return Result<AppUser>.Failure(new[] { "User not found." });
            }

            var identityResult = await _userManager.ConfirmEmailAsync(user, request.Token);
            if (identityResult.Succeeded)
            {
                return Result<AppUser>.Success();
            }

            return Result<AppUser>.Failure(identityResult.Errors.Select(e => e.Description));
        }
    }
}
