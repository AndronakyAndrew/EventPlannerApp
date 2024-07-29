namespace EventPlannerApp.Models
{
    public class BudgetItem
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public int EventId { get; set; }
        public Event Event { get; set; }
    }
}
