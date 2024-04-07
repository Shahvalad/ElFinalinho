using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Projecto.Domain.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
namespace Projecto.Domain.Models
{
    public class AppUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Bio { get; set; }
        public DateTime MemberSince { get; set; }
        public decimal Balance { get;  set; } 
        public decimal TotalSpendings { get;  set; }
        
        // Navigation properties
        public List<UserGame> UserGames { get; set; } = new List<UserGame>();
        public List<UserFavouriteGame> UserFavouriteGames { get; set; } = new List<UserFavouriteGame>();
        public AppUserProfilePicture? ProfilePicture { get; set; }
    }
}
