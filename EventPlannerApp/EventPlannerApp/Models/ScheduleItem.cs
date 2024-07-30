namespace EventPlannerApp.Models
{
    public class ScheduleItem
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime Time { get; set; } = DateTime.UtcNow;
        public int EventId { get; set; }
        public Event Event { get; set; }
    }
}
