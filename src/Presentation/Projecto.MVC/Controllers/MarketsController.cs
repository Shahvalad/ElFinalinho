using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Projecto.Application.Common.Interfaces;
using Projecto.Application.Common.Options;
using Projecto.Application.Features.Listings.Commands.Cancel;
using Projecto.Application.Features.Listings.Commands.Create;
using Projecto.Application.Features.Listings.Queries.GetUsersPendingListings;
using Projecto.Application.Features.MarketItems.Queries.GetAll;
using Projecto.Application.Features.MarketItems.Queries.GetById;
using Projecto.Application.Features.TarotCards.Queries.GetTarotCardsOfUser;
using Projecto.Domain.Enums;
using System.Security.Claims;

namespace Projecto.MVC.Controllers
{
    [Authorize]
    public class MarketsController : Controller
    {
        private readonly ISender _sender;
        private readonly IDataContext _context;
        private readonly IPaymentService _paymentService;
        private readonly UserManager<AppUser> _userManager;
        private readonly string _domain;

        public MarketsController(ISender sender, IDataContext context, IPaymentService paymentService, IOptions<WebsiteOptions> websiteOptions, UserManager<AppUser> userManager)
        {
            _sender = sender;
            _context = context;
            _paymentService = paymentService;
            _domain = websiteOptions.Value.Domain;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var marketItems = await _sender.Send(new GetAllMarketItemsQuery());
            var userPendingListings = await _sender.Send(new GetUsersPendingListingsQuery(GetUserId()));
            MarketVM marketVM = new MarketVM
            {
                MarketItems = marketItems,
                UsersPendingListings = userPendingListings
            };
            return View(marketVM);
        }
        public async Task<IActionResult> ViewYourItems()
        {
            var result = await _sender.Send(new GetTarotCardsOfUserQuery(GetUserId()));
            if (result.Succeeded)
            {
                return View(result.Data);
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> SellYourItem(int? id)
        {
            var marketItem = await _sender.Send(new GetMarketItemByIdQuery(id));
            var sellYourItemVM = new SellYourItemVM
            {
                MarketItem = marketItem,
                AverageSellingPrice = marketItem.Listings.Count > 0 ? marketItem.Listings.Average(l => l.Price) : 0,
                HighestSellingPrice = marketItem.Listings.Count > 0 ? marketItem.Listings.Max(l => l.Price) : 0,
                LowestSellingPrice = marketItem.Listings.Count > 0 ? marketItem.Listings.Min(l => l.Price) : 0
            };
            return View(sellYourItemVM);
        }

        [HttpPost]
        public async Task<IActionResult> SellYourItem(CreateListingCommand command)
        {
            if (!ModelState.IsValid)
            {
                var marketItem = await _sender.Send(new GetMarketItemByIdQuery(command.CardId));
                return View(marketItem);
            }

            var result = await _sender.Send(command);

            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error);
            }

            var failedMarketItem = await _sender.Send(new GetMarketItemByIdQuery(command.CardId));
            return View(failedMarketItem);
        }

        public async Task<IActionResult> ViewMarketItem(int id)
        {
            var marketItem = await _sender.Send(new GetMarketItemByIdQuery(id));
            return View(marketItem);
        }

        public IActionResult OrderConfirmation()
        {
            var service = new SessionService();

            Session session = service.Get(TempData["Session"].ToString());

            if (session.PaymentStatus == "paid")
            {
                return RedirectToAction(nameof(OrderConfirmed));
            }
            return RedirectToAction(nameof(OrderCancelled));
        }
        public async Task<IActionResult> BuyItem(int id)
        {
            var listing = await _context.Listings
                .Include(l => l.MarketItem)
                    .ThenInclude(l => l.TarotCard)
                .Include(l => l.User)
                .FirstOrDefaultAsync(l => l.Id == id);
            if (listing == null)
            {
                return NotFound();
            }

            var successUrl = _domain + "Markets/OrderConfirmation";
            var cancelUrl = _domain + "Markets/OrderCancelled";

            var session = await _paymentService.CreateStripeSessionForListing(listing, successUrl, cancelUrl);
            TempData["Session"] = session.Id;
            TempData["ListingId"] = id;

            Response.Headers.Add("location", session.Url);
            return new StatusCodeResult(303);
        }

        public async Task<IActionResult> OrderConfirmed()
        {
            var service = new SessionService();
            Session session = service.Get(TempData["Session"].ToString());

            if (session.PaymentStatus == "paid")
            {
                int id = (int)TempData["ListingId"];
                var sellersListing = await _context.Listings
                                                .Include(l => l.User)
                                                .Include(l => l.MarketItem)
                                                    .ThenInclude(l => l.TarotCard)
                                                .FirstOrDefaultAsync(li => li.Id == id);

                sellersListing.Status = ListingStatus.Sold;

                await _context.UserTarotCards.AddAsync(new UserTarotCard
                {
                    TarotCardId = sellersListing.MarketItem.TarotCard.Id,
                    UserId = GetUserId(),
                    IsDisplayedOnProfile = false
                });

                var seller = sellersListing.User;
                seller.Balance = seller.Balance + sellersListing.Price * 0.95m;
                try
                {
                    await _context.SaveChangesAsync(CancellationToken.None);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                return RedirectToAction(nameof(ViewYourItems));
            }
            return RedirectToAction(nameof(OrderCancelled));
        }
        public IActionResult OrderCancelled()
        {
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> CancelListing(int id)
        {
            await _sender.Send(new CancelListingCommand(id));
            return RedirectToAction(nameof(ViewYourItems));
        }

        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
        }
    }
}
