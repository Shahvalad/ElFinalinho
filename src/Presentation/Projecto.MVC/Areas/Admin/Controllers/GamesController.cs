
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Projecto.Application.Dtos.GameDtos;
using Projecto.Application.Features.Games.Commands.Create;
using Projecto.Application.Features.Games.Queries.GetAll;
using Projecto.Domain.Models;

namespace Projecto.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class GamesController : Controller
    {
        private readonly ISender _sender;

        public GamesController(ISender sender)
        {
            _sender = sender;
        }

        public async Task<IActionResult> Index()
        {
            var games = await _sender.Send(new GetGamesQuery());
            return View(games);
        }

        public async Task<IActionResult> Create()
        {
            ViewData["DeveloperId"] = new SelectList(await _sender.Send(new GetDevelopersQuery()), "Id", "Name");
            ViewData["PublisherId"] = new SelectList(await _sender.Send(new GetPublishersQuery()), "Id", "Name");
            ViewBag.Genres = new SelectList(await _sender.Send(new GetGenresQuery()), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateGameDto gameDto)
        {
            if (!ModelState.IsValid)
            {
                ViewData["DeveloperId"] = new SelectList(await _sender.Send(new GetDevelopersQuery()), "Id", "Name");
                ViewData["PublisherId"] = new SelectList(await _sender.Send(new GetPublishersQuery()), "Id", "Name");
                ViewBag.Genres = new SelectList(await _sender.Send(new GetGenresQuery()), "Id", "Name");
                return View(gameDto);
            }
            var command = new CreateGameCommand(gameDto);
            await _sender.Send(command);
            return RedirectToAction("Index");
        }


    }
}
