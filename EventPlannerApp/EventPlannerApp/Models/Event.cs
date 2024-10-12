using System.ComponentModel.DataAnnotations;

namespace EventPlannerApp.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public string Location { get; set; }
        public string Description { get; set; }
        
        public string? OrganizerId { get; set; }

        public List<Guest> Guests { get; set; }
        public List<ScheduleItem> ScheduleItems { get; set; }
        public List<BudgetItem> BudgetItems { get; set; }

        public ApplicationUser? Organizer { get; set; }

        public Event()
        {
            Guests = new List<Guest>();
            ScheduleItems = new List<ScheduleItem>();
            BudgetItems = new List<BudgetItem>();
        }
    }
}
