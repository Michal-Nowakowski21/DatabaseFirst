using DatabaseFirst.Models;
using DatabaseFirst.Service;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddScoped<ITripsService, TripsService>();
builder.Services.AddDbContext<Database12Context>(options =>
    options.UseSqlServer("Server=localhost/SQLEXPRESS;Database=Database12;Trusted_Connection=True;Encrypt=False"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseAuthorization();

app.MapControllers();

app.Run();