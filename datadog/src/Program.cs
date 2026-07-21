var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", (ILogger<Program> logger) =>
{
    logger.LogInformation("Gerando previsao do tempo");

    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.MapGet("/slow", async (ILogger<Program> logger) =>
{
    var delayMs = Random.Shared.Next(200, 1500);
    logger.LogInformation("Simulando chamada lenta de {DelayMs}ms", delayMs);
    await Task.Delay(delayMs);
    return Results.Ok(new { delayMs });
})
.WithName("GetSlow");

app.MapGet("/error", (ILogger<Program> logger) =>
{
    logger.LogError("Simulando falha proposital para gerar erro no APM");
    return Results.Problem("Erro simulado para estudo de observabilidade", statusCode: 500);
})
.WithName("GetError");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
