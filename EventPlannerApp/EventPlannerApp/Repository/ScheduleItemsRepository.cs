using EventPlannerApp.Data;
using EventPlannerApp.Models;

namespace EventPlannerApp.Repository
{
    public class ScheduleItemsRepository
    {
        private readonly EventPlannerContext db;

        public ScheduleItemsRepository(EventPlannerContext context)
        {
            db = context;
        }

        //Метод для создания расписания и добавления его в базу данных
        public async Task CreateScheduleAsync(ScheduleItem schedule)
        {
            db.Add(schedule);
            await db.SaveChangesAsync();
        }

        //Метод для изменения расписания в базе данных
        public async Task EditScheduleAsync(ScheduleItem schedule)
        {
            db.Update(schedule);
            await db.SaveChangesAsync();
        }

        //Метод для удаления расписания из базы данных
        public async Task DeleteScheduleAsync(ScheduleItem schedule)
        {
            db.ScheduleItems.Remove(schedule);
            await db.SaveChangesAsync();
        }
    }
}
