using Microsoft.AspNetCore.SignalR;

namespace Notification.Api.Hubs;

public class NotificationHub : Hub
{
    // Kullanıcı bağlandığında console'a log atıyorum (Takip için)
    public override async Task OnConnectedAsync()
    {
        Console.WriteLine($"==> Connection Established: {Context.ConnectionId} on Instance: {Environment.MachineName}");

        await base.OnConnectedAsync();
    }
}