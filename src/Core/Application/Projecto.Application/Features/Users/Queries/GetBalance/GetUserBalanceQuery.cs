using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Application.Features.Users.Queries.GetBalance
{
    public record GetUserBalanceQuery(string UserId) : IRequest<decimal>;
    public class GetUserBalanceQueryHandler : IRequestHandler<GetUserBalanceQuery, decimal>
    {
        private readonly UserManager<AppUser> _userManager;

        public GetUserBalanceQueryHandler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<decimal> Handle(GetUserBalanceQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if(user == null)
                throw new UserNotFoundException("No user with such id!");
            return user.Balance;
        }
    }
}
