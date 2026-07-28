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
        if (!await _context.Currencies.AnyAsync())
        {
            var currencies = new List<Currency>
            {
                new Currency{Code = "EUR", Symbol = "$"},
                new Currency{Code = "USD", Symbol = "$"},
            };
            await _context.Currencies.AddRangeAsync(currencies);
            await _context.SaveChangesAsync();
        }
        
        if (!await _context.Plans.AnyAsync())
        {
            var usd = _context.Currencies.FirstOrDefault(c => c.Code == "USD");
            
            var plans = new List<Plan>
            {
                new Plan {Name = "Free", Price = 0m, MaxLeads = 249, CurrencyId = usd.Id},
                new Plan {Name = "Pro", Price = 49.99m, MaxLeads = 999, CurrencyId = usd.Id},
                new Plan {Name = "Enterprice", Price = 199.99m, MaxLeads = 9999, CurrencyId = usd.Id},
            };
            await _context.Plans.AddRangeAsync(plans);
            await _context.SaveChangesAsync();
        }
    }
}