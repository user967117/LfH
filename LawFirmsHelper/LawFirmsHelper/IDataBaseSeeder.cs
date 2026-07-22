using LawFirmsHelper.Models;
using LawFirmsHelper.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LawFirmsHelper;

public interface IDatabaseSeeder
{
    Task SeedAsync();
}

public class DatabaseSeeder : IDatabaseSeeder
{
    private readonly AppDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public DatabaseSeeder(AppDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task SeedAsync()
    {
        if (!await _context.Plans.AnyAsync())
        {
            var plans = new List<Plan>
            {
                new Plan {Name = "Free", Price = 0m, MaxLeads = 249},
                new Plan {Name = "Pro", Price = 49.99m, MaxLeads = 999},
                new Plan {Name = "Enterprice", Price = 199.99m, MaxLeads = 9999},
            };
            await _context.Plans.AddRangeAsync(plans);
            await _context.SaveChangesAsync();
        }
    }
}