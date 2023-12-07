using System.Text.Json;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.AddRedis("redis");

builder.AddDefaultHealthChecks();
builder.ConfigureOpenTelemetry();

builder.Services.AddSingleton((option) =>
    ConnectionMultiplexer.Connect(
        builder.Configuration.GetConnectionString("redis")
    ).GetDatabase() 
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", (IDatabase db) =>
{
    var value = db.StringGet("default");
    WeatherForecast[] forecast;
    if (value.HasValue) {
        forecast = JsonSerializer.Deserialize<WeatherForecast[]>(value.ToString())??[];
    }
    else {
        forecast =  Enumerable.Range(1, 5).Select(index =>
            new WeatherForecast
            (
                DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                Random.Shared.Next(-20, 55),
                summaries[Random.Shared.Next(summaries.Length)]
            ))
            .ToArray();
        db.StringSet("default", JsonSerializer.Serialize(forecast));
    }
    return ;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();

app.MapDefaultEndpoints();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
