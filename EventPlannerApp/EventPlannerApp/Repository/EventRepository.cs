using EventPlannerApp.Data;
using EventPlannerApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EventPlannerApp.Repository
{
    public class EventRepository
    {
        private readonly EventPlannerContext db;

        public EventRepository(EventPlannerContext context)
        {
            db = context;
        }    

        //Метод для добавления мероприятия
        public async Task CreateEvent(Event events)
        {
            await db.Events.AddAsync(events);
            await db.SaveChangesAsync();
        }

        //Метод для изменения мероприятия
        public async Task EditEvent(Event events)
        {
            db.Update(events);
            await db.SaveChangesAsync();
        }
    }
}
