using EventPlannerApp.Models;
using EventPlannerApp.Repository;

namespace EventPlannerApp.Services
{
    public class ScheduleItemsService
    {
        private readonly ScheduleItemsRepository rep;

        public ScheduleItemsService(ScheduleItemsRepository scheduleRepository)
        {
            rep = scheduleRepository;
        }

        //Логика создания нового расписания
        public async Task CreateSchedule(ScheduleItem schedule)
        {
            // Преобразование DateTime в UTC
            if (schedule.Time.Kind == DateTimeKind.Unspecified)
            {
                schedule.Time = DateTime.SpecifyKind(schedule.Time, DateTimeKind.Utc);
            }
            else if (schedule.Time.Kind == DateTimeKind.Local)
            {
                schedule.Time = schedule.Time.ToUniversalTime();
            }
            await rep.CreateScheduleAsync(schedule);
        }

        //Логика редактирования расписания
        public async Task EditSchedule(int id, ScheduleItem schedule)
        {
            if (id != schedule.Id && schedule == null)
            {
                throw new Exception("Мерроприятие не найдено!");
            }

            // Преобразование DateTime в UTC
            if (schedule.Time.Kind == DateTimeKind.Unspecified)
            {
                schedule.Time = DateTime.SpecifyKind(schedule.Time, DateTimeKind.Utc);
            }
            else if (schedule.Time.Kind == DateTimeKind.Local)
            {
                schedule.Time = schedule.Time.ToUniversalTime();
            }

            await rep.EditScheduleAsync(schedule);
        }

        //Логика для удаления расписания
        public async Task DeleteSchedule(int id, ScheduleItem schedule)
        {
            if(id != schedule.Id && schedule == null)
            {
                throw new Exception("Мероприятие не было найдено для удаления!");
            }
            await rep.DeleteScheduleAsync(schedule);
        }
    }
}
