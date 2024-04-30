namespace Projecto.Application.Services.ConfirmationLinkService
{
    public interface IConfirmationLinkService
    {
        string GenerateLink(string userId, string token, string scheme);
    }
}
