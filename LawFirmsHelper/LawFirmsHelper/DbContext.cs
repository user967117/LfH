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
    
    public DbSet<Subscription> Subscriptions { get; set; }
    
    public DbSet<Currency> Currencies { get; set; }
    
    public DbSet<Plan> Plans { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Firm>().HasOne(f => f.Subscription).WithOne(s => s.Firm)
            .HasForeignKey<Subscription>(s => s.FirmId);
        
        modelBuilder.Entity<Subscription>().HasIndex(s => s.FirmId).IsUnique();
    }
}
