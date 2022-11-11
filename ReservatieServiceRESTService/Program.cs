using ReservatieServiceBL.Interfaces;
using ReservatieServiceBL.Managers;
using ReservatieServiceDL;
using ReservatieServiceDL.Repositories;

var builder = WebApplication.CreateBuilder(args);
var connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=ReservatieService;Integrated Security=True";

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSingleton<IGebruikerRepository>(r => new GebruikerRepository(connectionString));
builder.Services.AddSingleton<ILocatieRepository>(r => new LocatieRepository(connectionString));
builder.Services.AddSingleton<GebruikerManager>();
builder.Services.AddSingleton<LocatieManager>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
