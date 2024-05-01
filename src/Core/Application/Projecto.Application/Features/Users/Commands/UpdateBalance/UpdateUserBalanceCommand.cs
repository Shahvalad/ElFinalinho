using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Application.Features.Users.Commands.UpdateBalance
{
    public record UpdateUserBalanceCommand(string UserId, decimal NewBalance) : IRequest;
    public class UpdateUserBalanceCommandHandler : IRequestHandler<UpdateUserBalanceCommand>
    {
        private readonly UserManager<AppUser> _userManager;
        public UpdateUserBalanceCommandHandler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task Handle(UpdateUserBalanceCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if(user == null)
                throw new UserNotFoundException("No user with such id!");
            user.Balance = request.NewBalance;
            await _userManager.UpdateAsync(user);
        }
    }
}
