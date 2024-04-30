using Microsoft.AspNetCore.Mvc;
using Projecto.Application.Features.Payments.Queries.GetForChart;

namespace Projecto.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "AdminOrModeratorPolicy")]
    public class DashboardsController : Controller
    {
        private readonly ISender _sender;

        public DashboardsController(ISender sender)
        {
            _sender = sender;
        }

        public async Task<IActionResult> Index()
        {
            var payments = await _sender.Send(new GetPaymentsForChartQuery());
            return View(payments);
        }
    }
}