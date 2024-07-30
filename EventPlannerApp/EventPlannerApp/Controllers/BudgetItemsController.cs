using EventPlannerApp.Data;
using EventPlannerApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventPlannerApp.Controllers
{
    [Authorize]
    public class BudgetItemsController : Controller
    {
        private readonly EventPlannerContext db;
        public BudgetItemsController(EventPlannerContext context)
        {
            db = context;
        }

        //Метод для отображения списка бюджетных элементов
        public IActionResult Index(int eventId)
        {
            var budgetItems = db.BudgetItems.Where(b => b.EventId == eventId).ToList();
            ViewBag.EventId = eventId;
            return View(budgetItems);
        }

        //Метод для отображения формы добавления бюджетных элементов
        public IActionResult Create(int eventId)
        {
            ViewBag.EventId = eventId;
            return View();
        }

        //Метод создания бюджетных элементов и сохранение в базу данных
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BudgetItem budgetItem)
        {
            db.Add(budgetItem);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new {eventId = budgetItem.EventId});
        }

        //Метод для отображения формы редактирования
        public async Task<IActionResult> Edit (int? id)
        {
            if(id == null)
            {
                return NotFound(); //Отправляем 404, если элемент не найден
            }

            var budgetItem = await db.BudgetItems.FindAsync(id);
            if(budgetItem == null)
            {
                return NotFound();
            }
            return View(budgetItem);
        }

        //Метод редактирования бюджетных элементов
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit (int id, BudgetItem budgetItem)
        {
            if(id != budgetItem.Id)
            {
                return NotFound();
            }

            db.Update(budgetItem);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { eventId = budgetItem.EventId});
        } 

        //Метод для удаления бюджетных элементов
        public async Task<IActionResult> Delete (int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var budgetItem = await db.BudgetItems.FindAsync(id);
            if(budgetItem == null)
            {
                return NotFound();
            }

            db.BudgetItems.Remove(budgetItem);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { eventId = budgetItem.EventId});
        }

    }
}
