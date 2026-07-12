using System.Text;
using LawFirmsHelper.Repositories; 
using LawFirmsHelper.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
namespace LawFirmsHelper.Extentions;
using Microsoft.OpenApi.Models;

public static class DependencyInjectionExtentions
{
    public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder)
    {
       builder.Services.AddEndpointsApiExplorer();
       builder.Services.AddSwaggerGen(options =>
       {
           options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
           {
               Name = "Authorization",
               Type = SecuritySchemeType.Http,
               Scheme = "Bearer",
               BearerFormat = "JWT",
               In = ParameterLocation.Header,
               Description = "Jwt token"
           });
           
           options.AddSecurityRequirement(new OpenApiSecurityRequirement
           {
               {
                   new OpenApiSecurityScheme
                   {
                       Reference = new OpenApiReference
                       {
                           Type = ReferenceType.SecurityScheme,
                           Id = "Bearer"
                       }
                   },
                   Array.Empty<string>()
               }
           });
       });
       builder.Services.AddScoped<IJwtService, JwtService>();
       builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
       builder.Services.AddAuthorization();
       
       builder.Services.AddHttpContextAccessor();
       builder.Services.AddScoped<IUserContextService, UserContextService>();
       
       builder.Services.AddScoped<IFirmService, FirmService>();
       builder.Services.AddScoped<IAgentService, AgentService>();
       
       builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
       
       builder.Services.AddDbContext<AppDbContext>(options =>
           options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
       
       builder.Services.AddScoped<ISubscriptionService, SubscriptionService>();
       
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