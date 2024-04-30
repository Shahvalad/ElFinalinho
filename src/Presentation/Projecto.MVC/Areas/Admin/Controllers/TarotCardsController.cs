using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Projecto.Application.Dtos.TarotCardDtos;
using Projecto.Application.Features.TarotCards.Commands.Create;
using Projecto.Application.Features.TarotCards.Commands.Update;
using Projecto.Application.Features.TarotCards.Queries.GetAll;
using Projecto.Application.Features.TarotCards.Queries.GetById;

namespace Projecto.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "AdminOrModeratorPolicy")]
    public class TarotCardsController : Controller
    {
        private readonly ISender _sender;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _environment;
        public TarotCardsController(ISender sender, IMapper mapper, IWebHostEnvironment environment)
        {
            _sender = sender;
            _mapper = mapper;
            _environment = environment;
        }

        public async Task<IActionResult> Index()
        {
            var tarotCards = await _sender.Send(new GetAllTarotCardsQuery());
            return View(tarotCards);
        }

        public async Task<IActionResult> Details(int id)
        {
            var tarotCard = await _sender.Send(new GetTarotCardByIdQuery(id));
            return View(tarotCard);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateTarotCardDto tarotCardDto)
        {
            if (!ModelState.IsValid) return View(tarotCardDto);
            var command = new CreateTarotCardCommand(tarotCardDto);
            await _sender.Send(command);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var tarotCard = await _sender.Send(new GetTarotCardByIdQuery(id));
            var tarotCardDto = new UpdateTarotCardDto(tarotCard.Name, tarotCard.Description, null, tarotCard.DropRate, tarotCard.Rarity);
            return View(tarotCardDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateTarotCardDto tarotCardDto)
        {
            if (!ModelState.IsValid) return View(tarotCardDto);
            var command = new UpdateTarotCardCommand(tarotCardDto, id);
            await _sender.Send(command);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var tarotCard = await _sender.Send(new GetTarotCardByIdQuery(id));
            return View(tarotCard);
        }
       

    }
}
