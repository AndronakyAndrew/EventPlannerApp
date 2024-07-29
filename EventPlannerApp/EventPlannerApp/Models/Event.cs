namespace EventPlannerApp.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }

        public List<Guest> Guests { get; set; }
        public List<ScheduleItem> ScheduleItems { get; set; }
        public List<BudgetItem> BudgetItems { get; set; }

        public string OrganizerId { get; set; }
        public ApplicationUser Organizer {  get; set; }
    }
}
