
using Microsoft.AspNetCore.SignalR;

namespace SensorApp.Hubs;


public class SensorHub : Hub<INotificationClient>
{
    //public async Task SendMessage(string user, string message)
    //{
    //    await Clients.All.SendAsync("ReceiveMessage", user, message);
    //}

    public override async Task OnConnectedAsync()
    {

        await base.OnConnectedAsync();
    }
}

public interface  INotificationClient
{
    Task ReceiveNotification(string message);
}
