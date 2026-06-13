using LawFirmsHelper.Extentions;

var builder = WebApplication.CreateBuilder(args);

builder.AddServices();

var app = builder.Build();

app.UseEndpointExtensions();
app.UseMiddlewares();

app.Run();