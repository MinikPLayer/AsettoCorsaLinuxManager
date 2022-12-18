using AcUtils.Utils;
using Newtonsoft.Json;

namespace AcUtils.DataTypes;

public class Car
{
    public class Skin
    {
        public string SkinName = "";
        public string DriverName = "";
        public string Country = "";
        public string Team = "";
        public string Number = "";
        public int Priority = 0;
        
        public string NameId;
        public string PreviewPath = "";
        public string LiveryPath = "";
        
        public static Skin LoadFromFile(string path)
        {
            var dataFile = Path.Combine(path, "ui_skin.json");
            if (!File.Exists(dataFile))
                throw new FileNotFoundException("Cannot find file \"" + dataFile + "\"");

            var contents = File.ReadAllText(dataFile);
            var s = JsonConvert.DeserializeObject<Skin>(contents);
            if (s == null)
                throw new InvalidDataException("Invalid data in file");
            
            s.NameId = new DirectoryInfo(path).Name;
            if (s.NameId == "ui")
                s.NameId = "";

            var prevPaths = new string[] { "preview.png", "preview.jpg" };
            foreach (var p in prevPaths)
            {
                var testPath = Path.Combine(path, p);
                if (File.Exists(testPath))
                {
                    s.PreviewPath = testPath;
                    break;
                }
            }


            var outPath = Path.Combine(path, "livery.png");
            if (File.Exists(outPath))
                s.LiveryPath = outPath;

            return s;
        }

        public override string ToString()
        {
            return string.IsNullOrEmpty(SkinName) ? NameId.Replace('_', ' ').ToTitleCase() : SkinName;
        }
    }
    
    public struct SpecsData
    {
        public string BHP;
        public string Torque;
        public string Weight;
        public string TopSpeed;
        public string Acceleration;
        public string PWRatio;
        public int Range;
    }
    
    public string NameId;
    public string Name;
    public string Brand;
    public string Description;
    public List<string> Tags;
    public string Class;
    public SpecsData Specs;
    public List<string[]> TorqueCurve = new();
    public List<string[]> PowerCurve = new();

    public List<Skin> Skins = new();
    
    private Car()
    {
    }

    public static bool IsCarDirectory(string path)
    {
        return Directory.Exists(Path.Combine(path, "ui")) && Directory.Exists(Path.Combine(path, "skins"));
    }
    
    public static Car LoadFromFile(string path)
    {
        var uiPath = Path.Combine(path, "ui", "ui_car.json");
        if (!File.Exists(uiPath))
            throw new FileNotFoundException("Cannot find file ui_car.json in \"" + uiPath + "\"");

        var contents = File.ReadAllText(uiPath);
        var car = JsonConvert.DeserializeObject<Car>(contents);
        if (car == null)
            throw new InvalidDataException("Cannot parse data to Car");
        
        car.NameId = new DirectoryInfo(path).Name;

        // Load skins
        var skinsPath = Path.Combine(path, "skins");
        var paths = Directory.GetDirectories(skinsPath);
        foreach (var p in paths)
            car.Skins.Add(Skin.LoadFromFile(p));
        
        return car;
    }

    public string Serialize()
    {
        return JsonConvert.SerializeObject(this, Formatting.Indented);
    }

    public override string ToString() => Name;
}