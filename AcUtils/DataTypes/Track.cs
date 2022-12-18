using AcUtils.Utils;
using Newtonsoft.Json;

namespace AcUtils.DataTypes;

public class Track
{
    public struct Config
    {
        public string Name;
        public string Description;
        public List<string> Tags;
        public List<string> Geotags;
        public string Country;
        public string City;
        public string Length;

        public float ActualLength => float.Parse(Length.Replace(".", ""));
        
        public int Width;
        public int PitBoxes;
        /// <summary>
        /// Run direction
        /// </summary>
        public string Run;
        
        public string NameId;
        public string PreviewPath;
        public string OutlinePath;
        public List<string> Backgrounds;

        public void LoadConfigData(string configPath)
        {
            NameId = new DirectoryInfo(configPath).Name;
            if (NameId == "ui")
                NameId = "";
            
            var prevPath = Path.Combine(configPath, "preview.png");
            if (File.Exists(prevPath))
                PreviewPath = prevPath;

            var outPath = Path.Combine(configPath, "outline.png");
            if (File.Exists(outPath))
                OutlinePath = outPath;

            Backgrounds = new List<string>();
            // Order by name
            var bgFiles = Directory.GetFiles(configPath, "bgr*").OrderBy(x => x);
            Backgrounds.AddRange(bgFiles);
        }

        public override string ToString()
        {
            return Name;
        }
    }
    
    public string NameId;
    public List<Config> Configs;

    public override string ToString()
    {
        return NameId.Replace("ks_", "").Replace('_', ' ').ToTitleCase();
    }

    public static bool IsTrackDirectory(string path)
    {
        if (Directory.GetFiles(path, "*.kn5").Length == 0)
            return false;
        
        if(!Directory.Exists(Path.Combine(path, "ui")))
            return false;

        return true;
    }

    bool LoadConfig(string dir)
    {
        try
        {
            var fileContents = File.ReadAllText(Path.Combine(dir, "ui_track.json"));
            var deserialized = JsonConvert.DeserializeObject<Config>(fileContents);
            deserialized.LoadConfigData(dir);

            Configs.Add(deserialized);

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }

    public Track(string path)
    {
        NameId = new DirectoryInfo(path).Name;
        
        Configs = new List<Config>();
        var configsPath = Path.Combine(path, "ui");
        foreach (var dir in Directory.GetDirectories(configsPath)) 
            LoadConfig(dir);

        // One config track
        if (Configs.Count == 0)
            LoadConfig(configsPath);
    }
}