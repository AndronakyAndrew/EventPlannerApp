using EventPlannerApp.Data;
using EventPlannerApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventPlannerApp.Controllers
{
    [Authorize]
    public class EventsController : Controller
    {
        private readonly EventPlannerContext db;
        private readonly UserManager<ApplicationUser> userManager;

        public EventsController(EventPlannerContext context, UserManager<ApplicationUser> _userManager)
        {
            db = context;
            userManager = _userManager;
        }

        //Метод для отображения списка мероприятий для текущего пользователя
        public async Task<IActionResult> Index()
        {
            var user = await userManager.GetUserAsync(User);
            var events = db.Events.Where(e => e.OrganizerId == user.Id).ToList();
            return View(events);
        }

        //Метод для отображения страницы создания мероприятия
        public IActionResult Create()
        {
            return View();
        }

        //Метод для добавления нового мероприятия
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Event @event)
        {
            var user = await userManager.GetUserAsync(User);
            @event.OrganizerId = user.Id;

            // Преобразование DateTime в UTC
            if (@event.Date.Kind == DateTimeKind.Unspecified)
            {
                @event.Date = DateTime.SpecifyKind(@event.Date, DateTimeKind.Utc);
            }
            else if (@event.Date.Kind == DateTimeKind.Local)
            {
                @event.Date = @event.Date.ToUniversalTime();
            }

            db.Add(@event);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //Метод для отображения страницы изменения мероприятия
        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null)
            {
                return NotFound(); //Отправляем 404, если мероприятие не было найдено
            }

            var @event = await db.Events.FindAsync(id);
            if(@event == null || @event.OrganizerId != userManager.GetUserId(User))
            {
                return NotFound(); //Отправляем 404 
            }
            return View(@event);
        }

        //Метод для изменения мероприятия
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Event @event)
        {
            var user = await userManager.GetUserAsync(User);
            @event.OrganizerId = user.Id;

            if (id != @event.Id)
            {
                return NotFound(); //Отправляем 404
            }

            // Преобразование DateTime в UTC, если это необходимо
            if (@event.Date.Kind == DateTimeKind.Unspecified)
            {
                @event.Date = DateTime.SpecifyKind(@event.Date, DateTimeKind.Utc);
            }
            else if (@event.Date.Kind == DateTimeKind.Local)
            {
                @event.Date = @event.Date.ToUniversalTime();
            }

            try
            {
                db.Update(@event);
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!db.Events.Any(e => e.Id == @event.Id))
                {
                    return NotFound(); //Отправляем 404
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }

        //Метод для отображения полной информации о мероприятии
        public async Task<IActionResult> Details(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var user = await userManager.GetUserAsync(User);
            var @event = await db.Events
                .Include(e => e.BudgetItems)
                .Include(e => e.Guests)
                .Include(e => e.ScheduleItems)
                .FirstOrDefaultAsync(e => e.Id == id && e.OrganizerId == user.Id);

            if(@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }
    }
}
