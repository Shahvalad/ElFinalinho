using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;

namespace Projecto.MVC
{
    public class CustomUserIdProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            return connection.User?.Identity?.Name;
        }
    }
}
