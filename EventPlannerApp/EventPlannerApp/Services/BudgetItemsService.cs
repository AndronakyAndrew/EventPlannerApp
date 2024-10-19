using EventPlannerApp.Models;
using EventPlannerApp.Repository;

namespace EventPlannerApp.Services
{
    public class BudgetItemsService
    {
        private readonly BudgetItemsRepository rep;

        public BudgetItemsService(BudgetItemsRepository repository)
        {
            rep = repository;
        }

        //Логика добавления бюджета
        public async Task CreateBudget(BudgetItem budget)
        {
            await rep.CreateBudgetAsync(budget);
        }

        //Логика редактирования бюджета
        public async Task EditBudget(BudgetItem budget)
        {
            await rep.EditBudgetAsync(budget);
        }

        //Логика удаления бюджета
        public async Task DeleteBudget(int id, BudgetItem budget)
        {
            await rep.DeleteBudgetAsync(id, budget);
        }
    }
}
