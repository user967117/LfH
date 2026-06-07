using Microsoft.EntityFrameworkCore;

namespace LawFirmsHelper;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
}