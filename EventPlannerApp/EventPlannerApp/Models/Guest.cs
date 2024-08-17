using System.ComponentModel.DataAnnotations;

namespace EventPlannerApp.Models
{
    public class Guest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int EventId { get; set; }
        public Event Event { get; set; }
    }
}
