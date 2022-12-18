using System.IO;
using System.Linq;
using AcUtils.ContentManagers;
using AcUtils.DataTypes;
using Newtonsoft.Json;
using Xunit;

namespace AcUtils.Tests;

public class TrackTests
{
    [Theory]
    [InlineData("TestData/Tracks/ks_nordschleife")]
    [InlineData("TestData/Tracks/imola")]
    public void TestLoadingTrackInfo(string path)
    {
        var track = new Track(path);
        var serialized = JsonConvert.SerializeObject(track, Formatting.Indented);
        
        var expected = File.ReadAllText(path + ".result");
        Assert.Equal(expected, serialized);
    }

    [Theory]
    [InlineData("TestData/Tracks")]
    public async void TestTrackDiscovery(string path)
    {
        var expected = Directory.GetDirectories(path).Length - 1;
        var tracks = TracksManager.GetAllTracks(path).ToList();
        
        Assert.Equal(expected, tracks.Count);
    }
}