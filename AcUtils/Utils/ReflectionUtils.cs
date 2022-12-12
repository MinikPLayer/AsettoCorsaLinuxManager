using System.Reflection;

namespace AcUtils.Utils;

public static class ReflectionUtils
{
    public static IEnumerable<(object?, object)> GetAttributeFilteredProperties<T>(object? data)
    {
        if (data is null)
            return new List<(object?, object)>();

        var type = data.GetType();
        return type.GetProperties()
            .Where(x => Attribute.IsDefined(x, typeof(T)))
            .Select(x => (x.GetValue(data), x.GetCustomAttributes(typeof(T), false).FirstOrDefault()))!;
    }
}