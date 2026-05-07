using Microsoft.AspNetCore.SignalR;
using Notification.Api.Hubs;
using Notification.Api.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecks();

// Redis Connection String
var redisConn = builder.Configuration.GetValue<string>("RedisConnection") ?? "localhost:6379";

// SignalR + Redis Backplane
builder.Services.AddSignalR()
    .AddStackExchangeRedis(redisConn, options =>
    {
        options.Configuration.ChannelPrefix = "RealtimeApp_Notify";
    });

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddCors(options => {
    options.AddDefaultPolicy(policy => {
        policy.AllowAnyHeader()
              .AllowAnyMethod()
              .SetIsOriginAllowed((host) => true)
              .AllowCredentials();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
else
{
    app.UseHttpsRedirection();
}

app.UseCors();
app.UseAuthorization();

app.MapControllers();

// Hub Mapping
app.MapHub<NotificationHub>("/notifications");
app.MapHealthChecks("/health");

// Test Endpoint
// Bu endpoint'e istek attığımızda, Redis üzerinden tüm instance'lara yayılacak.
app.MapPost("/api/notify", async (NotificationModel model, IHubContext<NotificationHub> hubContext) =>
{
    // Belirli bir kullanıcıya veya herkese gönderim testi
    if (model.UserId == "all")
    {
        await hubContext.Clients.All.SendAsync("ReceiveNotification", new
        {
            Content = model.Message,
            Instance = Environment.MachineName // Hangi sunucudan tetiklendiğini görelim
        });
    }
    else
    {
        await hubContext.Clients.Client(model.UserId).SendAsync("ReceiveNotification", new
        {
            Content = model.Message,
            Instance = Environment.MachineName // Hangi sunucudan tetiklendiğini görelim
        });
    }

    return Results.Ok(new { SentFrom = Environment.MachineName, Status = "Broadcasted to Redis" });
});

app.Run();
