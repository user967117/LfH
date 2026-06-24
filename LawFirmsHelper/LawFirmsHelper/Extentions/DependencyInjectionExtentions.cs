using System.Text;
using LawFirmsHelper.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
namespace LawFirmsHelper.Extentions;

public static class DependencyInjectionExtentions
{
    public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder)
    {
       builder.Services.AddEndpointsApiExplorer();
       builder.Services.AddSwaggerGen(); 
       builder.Services.AddScoped<IJwtService, JwtService>();
       builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
       builder.Services.AddAuthorization();
       
       builder.Services.AddDbContext<AppDbContext>(options =>
           options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
       
       builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
       {
           options.Password.RequireDigit = true;
           options.Password.RequiredLength = 6;
           options.Password.RequireNonAlphanumeric = false;
           options.Password.RequireUppercase = false;
           options.Password.RequireLowercase = false;       
       }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

       builder.Services.AddAuthentication(options =>
       {
           options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
           options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
           options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
       }).AddJwtBearer(o =>
       {
           o.TokenValidationParameters = new TokenValidationParameters
           {    
                ValidIssuer = builder.Configuration["JwtOptions:Issuer"], 
                ValidAudience = builder.Configuration["JwtOptions:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtOptions:Key"]!)),
                ValidateIssuer = true,      
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
           };
       });
       
       builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)));
       
       builder.Services.AddScoped<IJwtService, JwtService>();
        return builder;
    }
}