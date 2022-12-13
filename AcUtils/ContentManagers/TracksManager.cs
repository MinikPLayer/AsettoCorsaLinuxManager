namespace AcUtils.ContentManagers;

public static class TracksManager
{

    public static List<Track> GetAllTracks(string path)
    {
        var tracks = new List<Track>();
        var dirs = Directory.GetDirectories(path);
        foreach (var dir in dirs)
        {
            if (!Track.IsTrackDirectory(dir))
                continue;
            
            tracks.Add(new Track(dir));
        }

        return tracks;
    }
}