
using Microsoft.AspNetCore.SignalR;
using SensorApp.Hubs;
using SensorApp.Services;

namespace SensorApp.HostedService;

public class SensorHostedService(ISensorMessageService sensorMessageService, 
    IHubContext<SensorHub, INotificationClient> hubContext,
    ILogger<SensorHostedService> logger) : BackgroundService
{  

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        sensorMessageService.MessageHandler += async (sender, args) =>
        {
            logger.LogInformation("Received message: {Message}", args.Message);
            await hubContext.Clients.All.ReceiveNotification(args.Message);
        };

        await sensorMessageService.StartProcessingAsync();
    }
}
