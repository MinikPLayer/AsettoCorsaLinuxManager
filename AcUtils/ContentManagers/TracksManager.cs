namespace AcUtils.ContentManagers;

public static class TracksManager
{

    public static IEnumerable<Track> GetAllTracks(string path)
    {
        var dirs = Directory.GetDirectories(path);
        foreach (var dir in dirs)
        {
            if (!Track.IsTrackDirectory(dir))
                continue;
            
            yield return new Track(dir);
        }
        
    }
}