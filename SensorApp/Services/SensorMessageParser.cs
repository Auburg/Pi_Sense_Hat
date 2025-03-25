using Azure.Messaging.ServiceBus;
using System.Text;
using System.Text.Json;

namespace SensorApp.Services;

public interface ISensorMessageParser
{  
    string Parse(string message);
}

public class SensorMessageParser(ILogger<SensorMessageParser> logger) : ISensorMessageParser
{    
    public string Parse(string message)
    {        
        // Parse the JSON message body
        JsonDocument jsonDocument = JsonDocument.Parse(message);
        JsonElement dataElement = jsonDocument.RootElement.GetProperty("data");
        string? base64EncodedBody = dataElement.GetProperty("body").GetString();

        if (string.IsNullOrEmpty(base64EncodedBody))
        {
            throw new Exception("Message body is empty");
        }

        string decodedMessageBody = DecodeBase64(base64EncodedBody);
        logger.LogInformation("Decoded Message Body: {body}", decodedMessageBody);

        return decodedMessageBody;
    }

    private static string DecodeBase64(string base64EncodedData)
    {
        byte[] base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
        return Encoding.UTF8.GetString(base64EncodedBytes);
    }    
}
