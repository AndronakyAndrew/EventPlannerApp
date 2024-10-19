using EventPlannerApp.Data;
using EventPlannerApp.Models;
using EventPlannerApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventPlannerApp.Controllers
{
    public class BudgetItemsController : Controller
    {
        private readonly EventPlannerContext db;
        private readonly BudgetItemsService service;
        public BudgetItemsController(EventPlannerContext context, BudgetItemsService budgetItemsService)
        {
            db = context;
            service = budgetItemsService;
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

        //Метод добавления бюджета
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BudgetItem budgetItem)
        {
            if(budgetItem == null)
            {
                return BadRequest("Некорректные данные");
            }
            else
            {
                await service.CreateBudget(budgetItem);
            }
            return RedirectToAction("Details","Events", new {eventId = budgetItem.EventId});
        }

        //Метод для отображения формы редактирования
        [HttpGet]
        public async Task<IActionResult> Edit (int? id)
        {
            var budgetItems = await db.BudgetItems.FindAsync(id);
            if (id == null && budgetItems == null)
            {
                return NotFound(); //Отправляем 404, если элемент не найден
            }
            return View(budgetItems);
        }

        //Метод редактирования бюджетных элементов
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit (int id, BudgetItem budget)
        {
            if(id != budget.Id)
            {
                return NotFound();
            }
            else 
               await service.EditBudget(budget);
            return RedirectToAction("Index", "Events");
        }

        //Метод для удаления бюджетных элементов
        public async Task<IActionResult> Delete (int id, BudgetItem budget)
        {
            if (id == null && budget == null)
            {
                return NotFound();
            }
            else
                await service.DeleteBudget(id, budget);
            return RedirectToAction("Index", "Events");
        }

    }
}
