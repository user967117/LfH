using LawFirmsHelper.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LawFirmsHelper;

public class AppDbContext : IdentityDbContext<IdentityUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
    
    public DbSet<Firm> Firm { get; set; }
    
    public DbSet<Agent> Agents { get; set; }
    
    public DbSet<SubscriptionPlan> SubscriptionPlans { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<SubscriptionPlan>().HasData(
            new SubscriptionPlan
            {
                Id = 1,
                Name = "Free",
                Price = 0m,
                MaxLeads = 10,
            },
            new SubscriptionPlan
            {
                Id = 2,
                Name = "Pro",
                Price = 49.99m,
                MaxLeads = 100,
            },
            new SubscriptionPlan
            {
                Id = 3,
                Name = "Enterprise",
                Price = 199.99m,
                MaxLeads = 9999,
            });
    }
}
