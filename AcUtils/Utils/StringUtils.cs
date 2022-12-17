using System.Buffers.Text;
using System.Globalization;

namespace AcUtils.Utils;

public static class StringUtils
{

    public static string ToTitleCase(this string title)
    {
        return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(title.ToLower()); 
    }
    
    // WARNING: Not tested as a cryptographic function (and probably not safe)
    public static string GetRandomBase64String(int length)
    {
        string ret = "";

        while (ret.Length < length)
        {
            Guid g = Guid.NewGuid();
            string guidString = Convert.ToBase64String(g.ToByteArray());
            guidString = guidString.Replace("=","").Replace("+","");

            ret += guidString;
        }

        return ret.Substring(0, length);
    }
}