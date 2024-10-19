using EventPlannerApp.Data;
using EventPlannerApp.Models;
using EventPlannerApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventPlannerApp.Controllers
{
    [Authorize]
    public class ScheduleItemsController : Controller
    {
        private readonly EventPlannerContext db;
        private readonly ScheduleItemsService service;
        public ScheduleItemsController(EventPlannerContext context, ScheduleItemsService scheduleItemsService)
        {
            db = context;
            service = scheduleItemsService;
        }

        //Метод для отображения списка элементов расписания
        public IActionResult Index(int eventId)
        {
            var scheduleItems = db.ScheduleItems.Where(s => s.EventId == eventId).ToList();
            ViewBag.EventId = eventId;
            return View(scheduleItems);
        }

        //Метод для отображения формы создания расписания 
        public IActionResult Create(int eventId)
        {
            ViewBag.EventId = eventId;
            return View();
        }

        //Метод для создания элементов расписания
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create (ScheduleItem scheduleItem)
        {
            await service.CreateSchedule(scheduleItem);
            return RedirectToAction("Index","Events", new {eventId = scheduleItem.EventId});
        }

        //Метод для отображения формы редактирования расписания
        public async Task<IActionResult> Edit(int? id)
        {
            var scheduleItem = await db.ScheduleItems.FindAsync(id);

            if (id == null && scheduleItem == null)
            {
                return NotFound();
            }
            return View(scheduleItem);
        }

        //Метод для редактирования расписания
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ScheduleItem scheduleItem)
        {
            await service.EditSchedule(id, scheduleItem);
            return RedirectToAction("Index","Events", new {eventId = scheduleItem.EventId});
        }

        //Метод для удаления расписания
        public async Task<IActionResult> Delete(int id, ScheduleItem schedule)
        {
            await service.DeleteSchedule(id, schedule);
            return RedirectToAction("Index","Events", new {eventId = schedule.EventId});
        }
    }
}
