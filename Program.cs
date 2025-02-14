global using dotnet_rpg.Models;
global using Dtos.Character;
global using Microsoft.EntityFrameworkCore;
global using AutoMapper;

using dotnet_rpg.Services.CharacterService;
using dotnet_rpg.Data;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<DataContext>(options =>
options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ICharacterService, CharacterService>();
builder.Services.AddAutoMapper(typeof(Program).Assembly);



var app = builder.Build();
app.UseCors(policy => policy.AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials()
                            .WithOrigins("http://localhost:4200"));

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
