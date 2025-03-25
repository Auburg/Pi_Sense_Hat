using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Components;

namespace SensorApp.Services;

public class MessageEventArgs : EventArgs
{    public string Message { get; internal set; } = "";
}

public interface ISensorMessageService
{
    EventHandler<MessageEventArgs>? MessageHandler { get; set; }
    Task StartProcessingAsync();
    Task StopProcessingAsync();
}

public class SensorMessageService : ISensorMessageService, IDisposable
{
    private readonly ServiceBusClient _client;
    private readonly ServiceBusProcessor _processor;
    private readonly ISensorMessageParser? _parser = null;
    private readonly ILogger<SensorMessageService> _logger;
    private bool _disposed = false;

    private EventHandler<MessageEventArgs>? OnMessageHandler;

    EventHandler<MessageEventArgs>? ISensorMessageService.MessageHandler { get => OnMessageHandler; set => OnMessageHandler = value; }

    public SensorMessageService(IConfiguration configuration, ISensorMessageParser parser, ILogger<SensorMessageService> logger)
    {
        _logger = logger;
        _parser = parser;
        var sbConn = configuration.GetConnectionString("ServiceBus");
        var sbQueue = configuration["ServiceBusQueue"];

        _client = new ServiceBusClient(sbConn);
        _processor = _client.CreateProcessor(sbQueue, new ServiceBusProcessorOptions());

        _processor.ProcessMessageAsync += MessageHandler;
        _processor.ProcessErrorAsync += ErrorHandler;       
    }

    public async Task StartProcessingAsync()
    {
        if(_processor.IsProcessing)
        {
            return;
        }

        await _processor.StartProcessingAsync();
    }

    public async Task StopProcessingAsync()
    {
        await _processor.StopProcessingAsync();
    }

    private async Task MessageHandler(ProcessMessageEventArgs args)
    {
        try
        { 
            var message = _parser?.Parse(args.Message.Body.ToString());

            if (message != null)
            {
                await Dispatcher.CreateDefault().InvokeAsync(() => OnMessageHandler?.Invoke(this, new MessageEventArgs { Message = message }));
            }
            else
            {
                _logger.LogWarning("Failed to parse message");
            }

           // OnMessageHandler?.Invoke(this, new MessageEventArgs { Message = message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to decode message body");
        }
        finally
        {
            // Complete the message
            await args.CompleteMessageAsync(args.Message);
        }
    }    

    private Task ErrorHandler(ProcessErrorEventArgs args)
    {
        // Handle the error
        Console.WriteLine($"Error processing message: {args.Exception.Message}");
        return Task.CompletedTask;
    }

    protected virtual async void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                await StopProcessingAsync();
                await _processor.DisposeAsync();
                await _client.DisposeAsync();
            }
            _disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }    
}