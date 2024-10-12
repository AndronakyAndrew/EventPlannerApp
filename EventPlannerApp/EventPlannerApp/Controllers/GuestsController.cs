using EventPlannerApp.Data;
using EventPlannerApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventPlannerApp.Controllers
{
    [Authorize]
    public class GuestsController : Controller
    {
        private readonly EventPlannerContext db;
        public GuestsController(EventPlannerContext context)
        {
            db = context;
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
            db.Add(guest);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { eventId = guest.EventId });
        }

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


        //Метод для отображения формы редактирования гостя
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit (int id, Guest guest)
        {
            if(id != guest.Id)
            {
                return NotFound();
            }

            db.Update(guest);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new {eventId = guest.EventId});
        }

        //Метод для удаления пользователя
        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var guest = await db.Guests.FindAsync(id);
            if(guest == null)
            {
                return NotFound();
            }

            db.Guests.Remove(guest);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { eventId = guest.Id});
        }

    }
}
