using Microsoft.EntityFrameworkCore;
using ReservatieServiceBL.Interfaces;
using ReservatieServiceBL.Managers;
using ReservatieServiceDL.Repositories;
using ReservatieServiceGebruikerRESTService;
using ReservatieServiceGebruikerRESTService.MapperInterface;
using ReservatieServiceGebruikerRESTService.Wrapper;
using System.Configuration;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();

// Add services to the container.
string connectionString = "Data Source=.\\SQLEXPRESS; Initial Catalog = ReservatieService; Integrated Security = True; TrustServerCertificate = True;";
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});
builder.Services.AddSingleton<IGebruikerRepository>(r => new GebruikerRepository(connectionString));
builder.Services.AddSingleton<ILocatieRepository>(r => new LocatieRepository(connectionString));
builder.Services.AddSingleton<IRestaurantRepository>(r => new RestaurantRepository(connectionString));
builder.Services.AddSingleton<IReservatieRepository>(r => new ReservatieRepository(connectionString));
builder.Services.AddSingleton<IMapFromDomain>(r => new MapFromDomainWrapper());
builder.Services.AddSingleton<IMapToDomain>(r => new MapToDomainWrapper());
builder.Services.AddSingleton<GebruikerManager>();
builder.Services.AddSingleton<LocatieManager>();
builder.Services.AddSingleton<RestaurantManager>();
builder.Services.AddSingleton<ReservatieManager>();
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
app.UseLogger();

app.MapControllers();

app.Run();
