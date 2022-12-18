using System.IO;
using System.Linq;
using AcUtils.ContentManagers;
using AcUtils.DataTypes;
using Xunit;

namespace AcUtils.Tests;

public class CarTests
{
    [Theory]
    [InlineData("TestData/Cars/abarth500")]
    public void TestCarLoading(string path)
    {
        var car = Car.LoadFromFile(path);
        var str = car.Serialize();

        var expected = File.ReadAllText(path + ".result");
        Assert.Equal(expected, str);
    }
}