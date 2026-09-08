using LawFirmsHelper.Extentions;

var builder = WebApplication.CreateBuilder(args);

builder.AddServices();

var app = builder.Build();

await app.SeedDatabaseAsync();

app.UseSwagger();
app.UseSwaggerUI();

app.UseEndpointExtensions();
app.UseMiddlewares();

app.Run();