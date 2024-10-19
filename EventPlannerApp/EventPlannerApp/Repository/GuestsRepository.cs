using EventPlannerApp.Data;
using EventPlannerApp.Models;

namespace EventPlannerApp.Repository
{
    public class GuestsRepository
    {
        private readonly EventPlannerContext db;

        public GuestsRepository(EventPlannerContext context)
        {
            db = context;
        }

        //Метод добавления гостей в базу данных
        public async Task CreateGuestAsync(Guest guest)
        {
            db.Add(guest);
            await db.SaveChangesAsync();
        }

        //Метод редактирования данных гостей в базе данныъ
        public async Task EditGuestAsync(Guest guest)
        {
            db.Update(guest);
            await db.SaveChangesAsync();
        }

        //метод для удаления гостей из базы данных
        public async Task DeleteGuestAsync(Guest guest)
        {
            db.Guests.Remove(guest);
            await db.SaveChangesAsync();
        }
    }
}
