using EventPlannerApp.Data;
using EventPlannerApp.Models;
using Microsoft.Extensions.Logging;
using System.Data.Entity.Infrastructure;

namespace EventPlannerApp.Repository
{
    public class BudgetItemsRepository
    {
        private readonly EventPlannerContext db;

        public BudgetItemsRepository(EventPlannerContext context)
        {
            db = context;
        }

        //Метод для добавления бюджета
        public async Task CreateBudgetAsync(BudgetItem budget)
        {
            db.Add(budget);
            await db.SaveChangesAsync();
        }

        //Метод для редактирования бюджета
        public async Task EditBudgetAsync(BudgetItem budget)
        {
            try
            {
                db.Update(budget);
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

        }

        //Метод для удаления бюджета
        public async Task DeleteBudgetAsync(BudgetItem budget)
        {
            db.BudgetItems.Remove(budget);
            await db.SaveChangesAsync();
        }
    }
}
