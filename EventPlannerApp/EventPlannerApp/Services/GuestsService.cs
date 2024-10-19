using EventPlannerApp.Models;
using EventPlannerApp.Repository;

namespace EventPlannerApp.Services
{
    public class GuestsService
    {
        private readonly GuestsRepository guestsRep;

        public GuestsService(GuestsRepository repository)
        {
            guestsRep = repository;
        }

        //Логика метода для добавления гостя
        public async Task CreateGuest(Guest guest)
        {
            if(guest == null)
            {
                throw new ArgumentNullException(nameof(guest));
            }
            await guestsRep.CreateGuestAsync(guest);
        }

        //Логика метода редактирования данных гостей
        public async Task EditGuest(int id,Guest guest)
        {
            if (id != guest.Id && guest == null)
            {
                throw new ArgumentNullException(nameof(guest));
            }
            await guestsRep.EditGuestAsync(guest);
        }

        //Логика метода удаления гостя из списка
        public async Task DeleteGuest(int id, Guest guest)
        {
            if(id != guest.Id && guest == null)
            {
                throw new ArgumentNullException(nameof(guest));
            }
            await guestsRep.DeleteGuestAsync(guest);
        }
    }
}
