using SendGrid.Helpers.Mail;
using SendGrid;
namespace Projecto.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly SendGridClient _client;

        public EmailService(string apiKey)
        {
            _client = new SendGridClient(apiKey);
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var from = new EmailAddress("shahvaladayvazov@gmail.com", "Your Hollowed Keys");
            var toEmail = new EmailAddress(to);
            var msg = MailHelper.CreateSingleEmail(from, toEmail, subject, null, body);
            var response = await _client.SendEmailAsync(msg);
        }

        public async Task SendReceiptEmailAsync(string to, string ticketNumber, List<CartItem> cartItems)
        {
            string emailContent = GenerateEmailContent(DateTime.Now.ToString(), to, ticketNumber, cartItems);
            await SendEmailAsync(to, "Your Receipt", emailContent);
        }
        public async Task SendGameKeysEmailAsync(string to, List<GameKey> gameKeys)
        {
            string emailContent = GenerateEmailContentForGameKeysTemplate(gameKeys);
            await SendEmailAsync(to, "Your Keys", emailContent);
        }
        public string GenerateEmailContent(string date, string email, string ticketNumber, List<CartItem> cartItems)
        {
            StringBuilder sb = new StringBuilder();

            var groupedItems = cartItems.GroupBy(x => x.Game.Name)
                            .Select(g => new CartItem
                            {
                                Game = new Game { Name = g.Key, Price = g.First().Game.Price },
                                Quantity = g.Sum(item => item.Quantity)
                            });

            string templatePath = "wwwroot/Email/ReceiptTemplate.html";
            string template = System.IO.File.ReadAllText(templatePath);

            template = template.Replace("{Date}", date);
            template = template.Replace("janedoe@example.com", email);
            template = template.Replace("123456789", ticketNumber);
            template = template.Replace("{TotalAmount}", cartItems.Sum(x => x.Game.Price * x.Quantity).ToString());

            foreach (var item in groupedItems)
            {
                sb.Append("<table width=\"173\" style=\"width:173px; border-spacing:0; border-collapse:collapse; margin:0px 0px 0px 0px;\" cellpadding=\"0\" cellspacing=\"0\" align=\"left\" border=\"0\" bgcolor=\"\" class=\"column column-0\">");
                sb.Append("<tbody><tr><td style=\"padding:0px;margin:0px;border-spacing:0;\"><table class=\"module\" role=\"module\" data-type=\"text\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"table-layout: fixed;\" data-muid=\"64573b96-209a-4822-93ec-5c5c732af15c.2\" data-mc-module-version=\"2019-10-22\">");
                sb.Append("<tbody><tr><td style=\"padding:15px 0px 15px 0px; line-height:22px; text-align:inherit;\" height=\"100%\" valign=\"top\" bgcolor=\"\" role=\"module-content\"><div><div style=\"font-family: inherit; text-align: center\"><span style=\"color: #80817f; font-size: 12px\">" + item.Game.Name + "</span></div><div></div></div></td></tr></tbody></table></td></tr></tbody></table>");
                sb.Append("<table width=\"173\" style=\"width:173px; border-spacing:0; border-collapse:collapse; margin:0px 0px 0px 0px;\" cellpadding=\"0\" cellspacing=\"0\" align=\"left\" border=\"0\" bgcolor=\"\" class=\"column column-1\"><tbody><tr><td style=\"padding:0px;margin:0px;border-spacing:0;\"><table class=\"module\" role=\"module\" data-type=\"text\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"table-layout: fixed;\" data-muid=\"64573b96-209a-4822-93ec-5c5c732af15c.1\" data-mc-module-version=\"2019-10-22\">");
                sb.Append("<tbody><tr><td style=\"padding:15px 0px 15px 0px; line-height:22px; text-align:inherit;\" height=\"100%\" valign=\"top\" bgcolor=\"\" role=\"module-content\"><div><div style=\"font-family: inherit; text-align: center\"><span style=\"color: #80817f; font-size: 12px\"><strong>" + item.Quantity + "</strong></span></div><div></div></div></td></tr></tbody></table></td></tr></tbody></table>");
                sb.Append("<table width=\"173\" style=\"width:173px; border-spacing:0; border-collapse:collapse; margin:0px 0px 0px 0px;\" cellpadding=\"0\" cellspacing=\"0\" align=\"left\" border=\"0\" bgcolor=\"\" class=\"column column-2\"><tbody><tr><td style=\"padding:0px;margin:0px;border-spacing:0;\"><table class=\"module\" role=\"module\" data-type=\"text\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"table-layout: fixed;\" data-muid=\"64573b96-209a-4822-93ec-5c5c732af15c.1.1\" data-mc-module-version=\"2019-10-22\">");
                sb.Append("<tbody><tr><td style=\"padding:15px 0px 15px 0px; line-height:22px; text-align:inherit;\" height=\"100%\" valign=\"top\" bgcolor=\"\" role=\"module-content\"><div><div style=\"font-family: inherit; text-align: center\"><span style=\"color: #80817f; font-size: 12px\"><strong>" + item.Game.Price + "</strong></span></div><div></div></div></td></tr></tbody></table></td></tr></tbody></table>");
            }

            template = template.Replace("{Items}", sb.ToString());
            return template;
        }
        public string GenerateEmailContentForGameKeysTemplate(List<GameKey> GameKeys)
        {
            StringBuilder sb = new StringBuilder();
            string templatePath = "wwwroot/Email/GameKeysTemplate.html";
            string template = System.IO.File.ReadAllText(templatePath);
            foreach (var gameKey in GameKeys)
            {
                sb.Append("<tr>");
                sb.Append("<td style=\"padding:0px; margin:0px; border-spacing:0;\">");
                sb.Append("<table class=\"module\" role=\"module\" data-type=\"text\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"table-layout: fixed;\" data-muid=\"313cf894-3cfb-43ee-ba10-f71c82f35abf.1\" data-mc-module-version=\"2019-10-22\">");
                sb.Append("<tbody>");
                sb.Append("<tr>");
                sb.Append("<td style=\"padding:0px 0px 0px 0px; line-height:22px; text-align:inherit;\" height=\"100%\" valign=\"top\" bgcolor=\"\" role=\"module-content\">");
                sb.Append("<div>");
                sb.Append("<div style=\"font-family: inherit; text-align: center\">");
                sb.Append("<span style=\"color: #ffffff;\"> Game: " + gameKey.Game.Name + " Key: " + gameKey.Value + "</span>");
                sb.Append("</div>");
                sb.Append("</div>");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("</tbody>");
                sb.Append("</table>");
                sb.Append("</td>");
                sb.Append("</tr>");
            }

            template = template.Replace("{GameKeys}", sb.ToString());
            return template;
        }

        
    }
}
