using AcUtils.Game;
using AcUtils.Utils;
using Xunit;

namespace AcUtils.Tests;

public class UnitTest1
{
    [Fact]
    public void TestGameStarterExecutableNotFound()
    {
        var result = GameStarter.StartGameProcess("doesntexists.txt");
        Assert.Throws<ResultException>(() => result.Unwrap());
    }
}