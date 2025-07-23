using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;
using System.Xml;

namespace XWA.Core.Converters;

public static class XmlConverter
{
    public static string Beautify(string xml)
    {
        StringBuilder sb = new();

        XmlDocument doc = new();
        try
        {
            doc.LoadXml(xml);

            using (MemoryStream ms = new())
            {
                XmlWriterSettings settings = new()
                {
                    Encoding = Encoding.Unicode,
                    Indent = true,
                    IndentChars = "  ",
                    NewLineChars = "\r\n",
                    NewLineHandling = NewLineHandling.Replace
                };
                using (XmlWriter writer = XmlWriter.Create(ms, settings))
                {
                    doc.Save(writer);
                }
                sb.Append(Encoding.Unicode.GetString(ms.ToArray()));
            }

            Debug.WriteLine($"\n{sb}\n");
        }
        catch (XmlException ex)
        {
            Debug.WriteLine(ex);
        }

        return sb.ToString();
    }

    public static string Serialize(string xml)
    {
        StringBuilder sb = new();

        XmlDocument doc = new();
        doc.LoadXml(xml);

        sb.Append(JsonConvert.SerializeXmlNode(doc, Newtonsoft.Json.Formatting.None));

        Debug.WriteLine($"\n{JsonConverter.Beautify(sb.ToString())}\n");

        return sb.ToString();
    }

    public static string Validate(string xml)
    {
        XmlDocument doc = new();
        doc.LoadXml(xml);

        using (MemoryStream ms = new())
        {
            XmlWriterSettings settings = new()
            {
                Encoding = Encoding.Unicode,
                Indent = true,
                IndentChars = "  ",
                NewLineChars = "\r\n",
                NewLineHandling = NewLineHandling.Replace
            };
            using XmlWriter writer = XmlWriter.Create(ms, settings);
            doc.Save(writer);
        }

        return doc.OuterXml;
    }
}
