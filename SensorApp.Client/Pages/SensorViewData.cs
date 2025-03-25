using SensorApp.Core;

namespace SensorApp.Client.Pages;

public class SensorViewData(SensorData sensorData)
{
    private const int Precision = 3;

    public double Temp
    {
        get
        {
            return Math.Round(sensorData.Temp, Precision);
        }
    }

    public double Pressure
    {
        get
        {
            return Math.Round(sensorData.Pressure, Precision);
        }
    }

    public double Mag
    {
        get
        {
            return Math.Round(sensorData.Mag, Precision);
        }
    }   

    public double Humidity
    {
        get
        {
            return Math.Round(sensorData.Humidity, Precision);
        }
    }

    public Orientation Orientation { get; set; } = new Orientation(sensorData.Orientation);
    public Gyro Gyro { get; set; } = new Gyro(sensorData.Gyro);
    public Accel Accel { get; set; } = new Accel(sensorData.Accel);
}

public class Orientation
{
    private const int Precision = 3;

    public Orientation(Core.Orientation orientation)
    {
        Roll = Math.Round(orientation.Roll, Precision);
        Pitch = Math.Round(orientation.Pitch, Precision);
        Yaw = Math.Round(orientation.Yaw, Precision);
    }

    public double Roll { get; set; }
    public double Pitch { get; set; }
    public double Yaw { get; set; }
}

public class Gyro
{
    private const int Precision = 3;

    public Gyro(Core.Gyro gyro)
    {
        Roll = Math.Round(gyro.Roll, Precision);
        Pitch = Math.Round(gyro.Pitch, Precision);
        Yaw = Math.Round(gyro.Yaw, Precision);
    }

    public double Roll { get; set; }
    public double Pitch { get; set; }
    public double Yaw { get; set; }
}

public class Accel
{
    private const int Precision = 3;

    public Accel(Core.Accel accel)
    {
        Roll = Math.Round(accel.Roll, Precision);
        Pitch = Math.Round(accel.Pitch, Precision);
        Yaw = Math.Round(accel.Yaw, Precision);
    }

    public double Roll { get; set; }
    public double Pitch { get; set; }
    public double Yaw { get; set; }
}
