using SensorApp.Core;

namespace SensorAppTests;

public class GivenADecodedSensorData
{
    [Fact]
    public void WhenTheStringIsParsedTheExpectedSensorDataIsCorrect()
    {
        var data = """
        {
            "temp": 30.37288475036621,
            "pressure": 1018.63818359375,
            "mag": 120,
            "humidity": 47.32919692993164,
            "orientation": {
                "roll": 1.39608298238779,
                "pitch": 353.4492406991216,
                "yaw": 353.1551400105369
            },
            "gyro": {
                "roll": 1.3969988680668064,
                "pitch": 353.4491348311895,
                "yaw": 353.1382946296095
            },
            "accel": {
                "roll": 1.3950869487077318,
                "pitch": 353.45853744035566,
                "yaw": 353.13830231357235
            }
        }
""";
        var sensorData = SensorData.FromString(data);

        Assert.NotNull(sensorData);

        Assert.True(CheckValues(30.37288475036621, sensorData.Temp));
        Assert.True(CheckValues(1018.63818359375, sensorData.Pressure));
        Assert.True(CheckValues(47.32919692993164, sensorData.Humidity));
        Assert.True(CheckValues(120, sensorData.Mag));

        Assert.True(CheckValues(1.39608298238779, sensorData.Orientation.Roll));
        Assert.True(CheckValues(353.4492406991216, sensorData.Orientation.Pitch));
        Assert.True(CheckValues(353.1551400105369, sensorData.Orientation.Yaw));

        Assert.True(CheckValues(1.3969988680668064, sensorData.Gyro.Roll));
        Assert.True(CheckValues(353.4491348311895, sensorData.Gyro.Pitch));
        Assert.True(CheckValues(353.1382946296095, sensorData.Gyro.Yaw));

        Assert.True(CheckValues(1.3950869487077318, sensorData.Accel.Roll));
        Assert.True(CheckValues(353.45853744035566, sensorData.Accel.Pitch));
        Assert.True(CheckValues(353.13830231357235, sensorData.Accel.Yaw));

        static bool CheckValues(double expected, float actual)
        {
            return Math.Round(expected, 3) == Math.Round(actual, 3);
        }
    }
}
