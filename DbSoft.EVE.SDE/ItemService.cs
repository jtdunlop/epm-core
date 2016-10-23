namespace DBSoft.EVE.SDE
{
    using System.Collections.Generic;
    using System.IO;
    using YamlDotNet.RepresentationModel;

    public interface IYamlProvider
    {
        Stream GetYamlStream();
    }

    public class ItemService
    {
        private readonly IYamlProvider _provider;

        public ItemService(IYamlProvider provider)
        {
            _provider = provider;

        }

        public IEnumerable<ItemDTO> GetItems()
        {
            using (var stream = _provider.GetYamlStream())
            {
                if (stream == null) return null;
                using (var reader = new StreamReader(stream))
                {
                    var yaml = new YamlStream();
                    yaml.Load(reader);
                    var root = (YamlMappingNode)yaml.Documents[0].RootNode;
                    var result = new List<ItemDTO>();
                    foreach (var item in root)
                    {
                        var volume = item.Value.GetNodeValue("volume");
                        if (string.IsNullOrWhiteSpace(volume)) continue;
                        var add = new ItemDTO
                        {
                            ItemID = int.Parse(item.GetKey()),
                            GroupID = int.Parse(item.Value.GetNodeValue("groupID")),
                            ItemName = GetItemName(item),
                            PortionSize = int.Parse(item.Value.GetNodeValue("portionSize")),
                            IsPublished = bool.Parse(item.Value.GetNodeValue("published")),
                            Volume = decimal.Parse(volume)
                        };
                        result.Add(add);
                    }
                    return result;
                }
            }
        }

        private static string GetItemName(KeyValuePair<YamlNode, YamlNode> item)
        {
            var name = item.Value.GetNode("name");
            return name == null ? "" : name.GetNodeValue("en");
        }
    }

    public static class DecimalExtensions
    {
        public static decimal? ParseNullableDecimal(this string s)
        {
            return s == null ? default(decimal?) : decimal.Parse(s);
        }
    }
}
