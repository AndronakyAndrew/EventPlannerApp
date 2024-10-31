using EventPlannerApp.Data;
using EventPlannerApp.Models;
using Microsoft.AspNetCore.Identity;

namespace EventPlannerApp.Repository
{
    public class EventRepository
    {
        private readonly EventPlannerContext db;
        private readonly UserManager<ApplicationUser> userManage;

        public EventRepository(EventPlannerContext context, UserManager<ApplicationUser> manager)
        {
            db = context;
            userManage = manager;
        }    

        //Метод для добавления мероприятия
        public async Task CreateEvent(Event events)
        {
            db.Events.Add(events);
            await db.SaveChangesAsync();
        }

        //Метод для изменения мероприятия
        public async Task EditEvent(Event events)
        {
            db.Update(events);
            await db.SaveChangesAsync();
        }

        //Метод для удаления мероприятия
        public async Task DeleteEvent(Event events)
        {
            db.Remove(events);
            await db.SaveChangesAsync();
        }
    }
}
