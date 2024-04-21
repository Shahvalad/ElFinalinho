using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Projecto.Application.Dtos.MessageDtos;

namespace Projecto.Application.Features.Messages.Queries.GetMessages
{
    public record GetMessagesQuery(string senderUsername, string recipientUsername) : IRequest<List<MessageDto>>;

    public class GetMessagesQueryHandler : IRequestHandler<GetMessagesQuery, List<MessageDto>>
    {
        private readonly IDataContext _context;
        private readonly UserManager<AppUser> _userManager;

        public GetMessagesQueryHandler(IDataContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<List<MessageDto>> Handle(GetMessagesQuery request, CancellationToken cancellationToken)
        {
            var recipient = await _userManager.Users.Include(u => u.ProfilePicture)
                .FirstOrDefaultAsync(u => u.UserName == request.recipientUsername, cancellationToken);

            var messages = await _context.Messages
                .Where(m => (m.SenderUsername == request.senderUsername && m.RecipientUsername == request.recipientUsername)
                            || (m.SenderUsername == request.recipientUsername && m.RecipientUsername == request.senderUsername)).ToListAsync();

            messages.Where(m => m.SenderUsername == request.recipientUsername).ToList().ForEach(m => m.IsRead = true);
            
            var messageDtos = messages.Select(m => new MessageDto
            {
                Text = m.Text,
                IsSent = m.SenderUsername == request.senderUsername,
                UserProfilePictureName = recipient.ProfilePicture.FileName,
                IsRead = m.IsRead
            }).ToList();
            await _context.SaveChangesAsync(cancellationToken);
                
            return messageDtos;
        }
    }
}
