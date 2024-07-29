using Microsoft.AspNetCore.Identity;

namespace EventPlannerApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        //Связь с событиями, которые пользователь организует
        public List<Event> OrganizedEvents { get; set; }
    }
}
