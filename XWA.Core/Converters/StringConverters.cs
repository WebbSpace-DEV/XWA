using System.Text;
using XWA.Core.Constants;

namespace XWA.Core.Converters;

public static class StringConverters
{
    public static String FixCommas(String s)
    {
        return s.Replace(Global.CsvDelimiter, ' ');
    }

    public static String FixNewLines(String s)
    {
        return s.Replace(System.Environment.NewLine, Global.NewLine);
    }

    public static String FixFileName(string s)
    {
        StringBuilder sb = new(s);
        foreach (char c in System.IO.Path.GetInvalidFileNameChars())
        {
            sb.Replace(c.ToString(), String.Empty);
        }
        return sb.ToString();
    }
}
