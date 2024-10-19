using EventPlannerApp.Data;
using EventPlannerApp.Models;
using EventPlannerApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventPlannerApp.Controllers
{
    [Authorize]
    public class GuestsController : Controller
    {
        private readonly EventPlannerContext db;
        private readonly GuestsService guestService;
        public GuestsController(EventPlannerContext context, GuestsService service)
        {
            db = context;
            guestService = service;
        }

        //Метод для отображения списка гостей для мероприятия
        public IActionResult Index (int eventId)
        {
            var guests = db.Guests.Where(g => g.EventId == eventId).ToList();
            ViewBag.EventId = eventId;
            return View(guests);
        }

        //Метод для отображения формы добавления нового гостя
        public IActionResult Create (int eventId)
        {
            ViewBag.EventId = eventId;
            return View();
        }

        //Метод для добавления гостя
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create (Guest guest)
        {
            await guestService.CreateGuest(guest);
            return RedirectToAction("Index", "Events", new { eventId = guest.EventId });
        }

        //Метод для отображения формы редактирования гостя
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            var budgetItems = await db.Guests.FindAsync(id);
            if (id == null && budgetItems == null)
            {
                return NotFound(); //Отправляем 404, если элемент не найден
            }
            return View(budgetItems);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit (int id, Guest guest)
        {
            await guestService.EditGuest(id, guest);
            return RedirectToAction("Index","Events", new {eventId = guest.EventId});
        }

        //Метод для удаления пользователя
        public async Task<IActionResult> Delete(int id, Guest guest)
        {
            await guestService.DeleteGuest(id, guest);
            return RedirectToAction("Index","Events", new { eventId = guest.Id});
        }

    }
}
