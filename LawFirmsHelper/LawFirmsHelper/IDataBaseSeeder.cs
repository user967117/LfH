using LawFirmsHelper.Models;
using Microsoft.EntityFrameworkCore;

namespace LawFirmsHelper;

public interface IDatabaseSeeder
{
    Task SeedAsync();
}

public class DatabaseSeeder : IDatabaseSeeder
{
    private readonly AppDbContext _context;

    public DatabaseSeeder(AppDbContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        if (!await _context.SubscriptionPlans.AnyAsync())
        {
            var plans = new List<SubscriptionPlan>
            {
                new SubscriptionPlan 
                { 
                    Name = "Free", 
                    Price = 0m, 
                    MaxLeads = 10 
                },
                new SubscriptionPlan 
                { 
                    Name = "Pro", 
                    Price = 49.99m, 
                    MaxLeads = 100 
                },
                new SubscriptionPlan 
                { 
                    Name = "Enterprise", 
                    Price = 199.99m, 
                    MaxLeads = 9999 
                }
            };

            await _context.SubscriptionPlans.AddRangeAsync(plans);
            await _context.SaveChangesAsync();
        }
    }
}