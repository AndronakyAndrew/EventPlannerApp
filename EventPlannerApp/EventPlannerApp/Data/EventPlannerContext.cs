using EventPlannerApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EventPlannerApp.Data
{
    public class EventPlannerContext : IdentityDbContext<ApplicationUser>
    {
        public EventPlannerContext(DbContextOptions<EventPlannerContext> options) : base(options)
        {
        }
        public DbSet<Event> Events { get; set; }
        public DbSet<Guest> Guests { get; set; }
        public DbSet<ScheduleItem> ScheduleItems { get; set; }
        public DbSet<BudgetItem> BudgetItems { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //Дополнительные настройки модели
            builder.Entity<ApplicationUser>()
                .HasMany(e => e.OrganizedEvents)
                .WithOne(e => e.Organizer)
                .HasForeignKey(e => e.OrganizerId);
        }
    }
}
