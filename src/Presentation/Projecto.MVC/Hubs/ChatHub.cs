using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Projecto.Application.Common.Interfaces;
using Projecto.Application.Features.Friends.Queries.GetAll;
using Projecto.Application.Features.Messages.Queries.GetUnreadMessageCount;

namespace Projecto.MVC.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IDataContext _context;
        private readonly ISender _sender;
        private readonly UserManager<AppUser> _userManager;
        public ChatHub(IDataContext context, UserManager<AppUser> userManager, ISender sender)
        {
            _context = context;
            _userManager = userManager;
            _sender = sender;
        }

        public async Task SendMessage(string senderUsername, string recipientUsername, string text)
        {
            try
            {
                var message = new Message
                {
                    SenderUsername = senderUsername,
                    RecipientUsername = recipientUsername,
                    Text = text,
                    Timestamp = DateTime.UtcNow,
                    IsRead = false
                };

                await _context.Messages.AddAsync(message);
                await _context.SaveChangesAsync(cancellationToken: CancellationToken.None);

                var unreadMessageCount = await _context.Messages
                    .Where(m => m.RecipientUsername == recipientUsername && !m.IsRead)
                    .CountAsync();

                await Clients.User(recipientUsername).SendAsync("ReceiveMessage", senderUsername, text, unreadMessageCount);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public async Task GetUnreadMessageCounts(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            var friends = await _sender.Send(new GetAllFriendsQuery(user.Id));
            var unreadMessageCounts = new Dictionary<string, int>();

            foreach (var friend in friends.Data)
            {
                var count = await _sender.Send(new GetUnreadMessageCountQuery(friend.Username));
                unreadMessageCounts[friend.Username] = count;
            }

            await Clients.User(user.Id).SendAsync("ReceiveUnreadMessageCounts", unreadMessageCounts);
        }

        




    }
}
