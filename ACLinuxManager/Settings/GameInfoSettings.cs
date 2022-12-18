using System.IO;

namespace ACLinuxManager.Settings;

public static class GameInfoSettings
{
    public static string DocumentsPath =
        "/home/minik/.steam/steam/steamapps/compatdata/244210/pfx/drive_c/users/steamuser/Documents/Assetto Corsa/";
    
    public static string ContentPath = "/home/minik/.steam/steam/steamapps/common/assettocorsa/content/";
    public static string TrackContentPath => Path.Combine(ContentPath, "tracks");
    public static string CarsContentPath => Path.Combine(ContentPath, "cars");
}