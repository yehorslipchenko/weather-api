using WeatherForecast.BL.Services;
using WeatherForecast.BL.Services.Contracts;
using WeatherForecast.BL.Services.OpenWeatherAPI;
using WeatherForecast.BL.Services.Sinoptic;
using WeatherForecast.BL.Services.WeatherAPI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();

builder.Services.AddHttpClient<WeatherAPIService>();
builder.Services.AddHttpClient<OpenWeatherAPIService>();
builder.Services.AddHttpClient<SinopticService>();

builder.Services.AddTransient<IWeatherAPIService, WeatherAPIService>();
builder.Services.AddTransient<IOpenWeatherAPIService, OpenWeatherAPIService>();
builder.Services.AddTransient<ISinopticService, SinopticService>();

var app = builder.Build();

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
