using EventPlannerApp.Data;
using EventPlannerApp.Models;
using EventPlannerApp.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace EventPlannerApp.Services
{
    public class EventService
    {
        private readonly EventRepository eventRep;

        public EventService(EventRepository repository)
        {
            eventRep = repository;
        }

        //Логика добавления мероприятия
        public async Task CreateEvent(Event events)
        {

            // Преобразование DateTime в UTC
            if (events.Date.Kind == DateTimeKind.Unspecified)
            {
                events.Date = DateTime.SpecifyKind(events.Date, DateTimeKind.Utc);
            }
            else if (events.Date.Kind == DateTimeKind.Local)
            {
                events.Date = events.Date.ToUniversalTime();
            }
            
            await eventRep.CreateEvent(events);
        }

        //Логика изменения мероприятия
        public async Task EditEvent(Event events)
        {
            // Преобразование DateTime в UTC, если это необходимо
            if (events.Date.Kind == DateTimeKind.Unspecified)
            {
                events.Date = DateTime.SpecifyKind(events.Date, DateTimeKind.Utc);
            }
            else if (events.Date.Kind == DateTimeKind.Local)
            {
                events.Date = events.Date.ToUniversalTime();
            }
             await eventRep.EditEvent(events);
        }

        //Логика для удаления мероприятия
        public async Task DeleteEvent(int id, Event events)
        {
            if(id == 0 || events == null)
            {
                throw new Exception("Мероприятие не найдено!");
            }
            await eventRep.DeleteEvent(events);
        }
    }
}
