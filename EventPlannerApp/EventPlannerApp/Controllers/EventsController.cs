using EventPlannerApp.Data;
using EventPlannerApp.Models;
using EventPlannerApp.Services;
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
        private readonly EventService eventService;

        public EventsController(EventPlannerContext context, UserManager<ApplicationUser> _userManager, EventService _eventService)
        {
            db = context;
            userManager = _userManager;
            eventService = _eventService;
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
            if (ModelState.IsValid)
            {
                var user = await userManager.GetUserAsync(User);
                @event.OrganizerId = user.Id;

                await eventService.CreateEvent(@event);
                return RedirectToAction(nameof(Index));                
            }
            else
            {
                return View(@event);
            }
        }

        //Метод для отображения страницы изменения мероприятия
        public async Task<IActionResult> Edit(int? id)
        {
            var events = await db.Events.FindAsync(id);

            if (id == null && events == null || events?.OrganizerId != userManager.GetUserId(User))
            {
                return NotFound(); //Отправляем 404, если мероприятие не было найдено
            }
            return View(events);
        }

        //Метод для изменения мероприятия
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Event events)
        {
            if(ModelState.IsValid)
            {
                var user = await userManager.GetUserAsync(User);
                events.OrganizerId = user.Id;
                if(user.Id != events.OrganizerId)
                {
                    return Forbid();
                }
                else
                    await eventService.EditEvent(events);
            }
            else
            {
                return View(events);
            }
            return RedirectToAction(nameof(Index));
        }

        //Метод для удаления мероприятия
        public async Task<IActionResult> DeleteEvent(int id, Event events)
        {
            await eventService.DeleteEvent(id, events);
            return RedirectToAction(nameof(Index));
        }

        //Метод для отображения полной информации о мероприятии
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var user = await userManager.GetUserAsync(User);
            var @event = await db.Events
                .Include(e => e.BudgetItems)
                .Include(e => e.Guests)
                .Include(e => e.ScheduleItems)
                .FirstOrDefaultAsync(e => e.Id == id && e.OrganizerId == user.Id);

            if (id == 0 && @event == null)
            {
                return NotFound();
            }
            return View(@event);
        }

        //Метод для отображения списка мероприятий для текущего пользователя
        public async Task<IActionResult> Index()
        {
            var user = await userManager.GetUserAsync(User);
            if(user == null)
            {
                return NotFound("Пользователь не найден");
            }
            var events = await db.Events
                .Where(e => e.OrganizerId == user.Id)
                .ToListAsync();
            return View(events);
        }

        //Метод поиска мероприятий
        [HttpPost]
        public async  Task<IActionResult> Search(string query)
        {
            var user =  await userManager.GetUserAsync(User);
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest("Строка поиска не должна быть пустой");
            }

            //Поиск совпадений в базе данных
            var result = db.Events
                .Where(e => e.OrganizerId == user.Id && (e.Name.Contains(query) || e.Description.Contains(query)))
                .ToList();

            if(result.Count == 0)
            {
                return NotFound("Мероприятия не найдены");
            }
            return View("Index", result);
        }
    }
}
