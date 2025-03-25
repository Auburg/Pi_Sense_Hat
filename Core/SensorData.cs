using System.Text.Json;

namespace SensorApp.Core;

public class LowerCaseNamingPolicy : JsonNamingPolicy
{
    public override string ConvertName(string name)
    {
        return name.ToLower();
    }
}

public class SensorData
{
    private static readonly JsonSerializerOptions SerializerOptions = new JsonSerializerOptions
    {
        PropertyNamingPolicy = new LowerCaseNamingPolicy(),
        PropertyNameCaseInsensitive = true
    };

    public float Temp { get;  set; }
    public float Mag { get;  set; }
    public float Pressure { get; set; }
    public float Humidity { get; set; }
    public Orientation Orientation { get; set; } = new Orientation();
    public Gyro Gyro { get; set; } = new Gyro();
    public Accel Accel { get; set; } = new Accel();

    public static SensorData? FromString(string data)
    {
        return JsonSerializer.Deserialize<SensorData>(data, SerializerOptions);
    }
}

public class Orientation
{
    public float Roll { get; set; }
    public float Pitch { get; set; }
    public float Yaw { get; set; }
}

public class Gyro
{
    public float Roll { get; set; }
    public float Pitch { get; set; }
    public float Yaw { get; set; }
}

public class Accel
{
    public float Roll { get; set; }
    public float Pitch { get; set; }
    public float Yaw { get; set; }
}

