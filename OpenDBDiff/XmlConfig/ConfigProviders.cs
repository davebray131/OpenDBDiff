using System.Collections;
using System.Xml;

namespace OpenDBDiff.XmlConfig;

public class ConfigProviders
{
    private static Hashtable providers = null;

    public static ConfigProvider GetProvider(string key)
    {
        XmlNodeList nodes;
        if (providers == null)
        {
            var xmldom = new XmlDocument();
            xmldom.Load("OpenDBDiffConfig.xml");
            nodes = xmldom.SelectNodes("OpenDBDiff/Providers/Provider");
            providers = [];
            for (var index = 0; index < nodes.Count; index++)
            {
                var provider = new ConfigProvider
                {
                    Description = nodes[index].Attributes.GetNamedItem("description").Value,
                    Key = nodes[index].Attributes.GetNamedItem("key").Value,
                    Library = nodes[index].Attributes.GetNamedItem("library").Value
                };
                providers.Add(key, provider);
            }
        }
        return (ConfigProvider)providers[key];
    }
}
