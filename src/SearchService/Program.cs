using MongoDB.Driver;
using MongoDB.Entities;
using SearchService.Data;
using SearchService.Models;
using SearchService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHttpClient<AuctoinServiceHttpClient>();

var app = builder.Build();


app.UseAuthorization();

app.MapControllers();

try
{
    await DBInitializer.InitDB(app);
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
}


app.Run();
