using LawFirmsHelper.Extentions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddServices(builder.Configuration);

var app = builder.Build();

app.UseEndpointExtensions();
app.UseMiddlewares();

app.Run();