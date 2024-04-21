using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecto.Application.Features.Messages.Queries.GetUnreadMessageCount
{
    public record GetUnreadMessageCountQuery(string recipientName) : IRequest<int>;
    public class GetUnreadMessageCountQueryHandler : IRequestHandler<GetUnreadMessageCountQuery, int>
    {
        private readonly IDataContext _context;
        private readonly UserManager<AppUser> _userManager;

        public GetUnreadMessageCountQueryHandler(IDataContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<int> Handle(GetUnreadMessageCountQuery request, CancellationToken cancellationToken)
        {
            var unreadMessageCount = await _context.Messages
                .Where(m => m.RecipientUsername == request.recipientName && !m.IsRead)
                .CountAsync(cancellationToken);
            return unreadMessageCount;
        }
    }
}
