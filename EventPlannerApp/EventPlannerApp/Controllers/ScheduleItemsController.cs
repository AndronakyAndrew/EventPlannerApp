using EventPlannerApp.Data;
using EventPlannerApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EventPlannerApp.Controllers
{
    [Authorize]
    public class ScheduleItemsController : Controller
    {
        private readonly EventPlannerContext db;
        public ScheduleItemsController(EventPlannerContext context)
        {
            db = context;
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
            // Преобразование DateTime в UTC
            if (scheduleItem.Time.Kind == DateTimeKind.Unspecified)
            {
                scheduleItem.Time = DateTime.SpecifyKind(scheduleItem.Time, DateTimeKind.Utc);
            }
            else if (scheduleItem.Time.Kind == DateTimeKind.Local)
            {
                scheduleItem.Time = scheduleItem.Time.ToUniversalTime();
            }

            db.Add(scheduleItem);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new {eventId = scheduleItem.EventId});
        }

        //Метод для отображения формы редактирования расписания
        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var scheduleItem = await db.ScheduleItems.FindAsync(id);
            if(scheduleItem == null)
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
            if(id != scheduleItem.Id)
            {
                return NotFound();
            }

            db.Update(scheduleItem);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new {eventId = scheduleItem.EventId});
        }

        //Метод для удаления расписания
        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var scheduleItem = await db.ScheduleItems.FindAsync(id);
            if(scheduleItem == null)
            {
                return NotFound();
            }

            db.ScheduleItems.Remove(scheduleItem);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new {eventId = scheduleItem.EventId});
        }
    }
}
