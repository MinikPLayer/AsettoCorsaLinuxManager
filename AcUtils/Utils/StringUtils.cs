using System.Buffers.Text;

namespace AcUtils.Utils;

public static class StringUtils
{
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