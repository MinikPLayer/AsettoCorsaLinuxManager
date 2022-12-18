using AcUtils.DataTypes;

namespace AcUtils.ContentManagers;

public class CarsManager
{
    public static IEnumerable<Car> GetAllCars(string path)
    {
        var dirs = Directory.GetDirectories(path);
        foreach (var dir in dirs)
        {
            if (!Car.IsCarDirectory(dir))
                continue;

            yield return Car.LoadFromFile(dir);
        }
        
    }
}