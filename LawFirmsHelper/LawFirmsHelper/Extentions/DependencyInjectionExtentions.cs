using LawFirmsHelper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LawFirmsHelper.Extentions;

public static class DependencyInjectionExtentions
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
       services.AddEndpointsApiExplorer();
       services.AddSwaggerGen(); 
       
       services.AddAuthorization();
       
       services.AddDbContext<AppDbContext>(options =>
           options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
       
       services.AddIdentity<IdentityUser, IdentityRole>(options =>
       {
           options.Password.RequireDigit = true;
           options.Password.RequiredLength = 6;
           options.Password.RequireNonAlphanumeric = false;
           options.Password.RequireUppercase = false;
           options.Password.RequireLowercase = false;       
       }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
       
        return services;
    }
}