namespace DBSoft.EVE.SDE
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using YamlDotNet.RepresentationModel;

    public class CategoryService
    {
        private readonly YamlProvider _provider;

        public CategoryService(YamlProvider provider)
        {
            _provider = provider;
        }

        public IEnumerable<CategoryDTO> GetCategories()
        {

            using (var stream = _provider.GetYamlStream())
            {
                if (stream == null) return null;
                using (var reader = new StreamReader(stream))
                {
                    var yaml = new YamlStream();
                    yaml.Load(reader);
                    var root = (YamlMappingNode)yaml.Documents[0].RootNode;
                    return root.Select(category => new CategoryDTO
                    {
                        CategoryID = int.Parse(category.GetKey()),
                        CategoryName = category.GetNodes().Single(f => f.GetKey() == "name").GetNodes().Single(f => f.GetKey() == "en").GetValue(),
                        IsPublished = bool.Parse(category.GetNodes().Single(f => f.GetKey() == "published").GetValue())
                    }).ToList();
                }
            }
        }
    }

    public static class YamlExtensions
    {
        public static string GetKey(this KeyValuePair<YamlNode, YamlNode> node)
        {
            var result = ((YamlScalarNode)node.Key).Value;
            return result;
        }

        public static string GetValue(this KeyValuePair<YamlNode, YamlNode> node)
        {
            return ((YamlScalarNode)node.Value).Value;
        }

        public static string GetValue(this YamlNode node)
        {
            return ((YamlScalarNode)node).Value;
        }

        public static IEnumerable<KeyValuePair<YamlNode, YamlNode>> GetNodes(this KeyValuePair<YamlNode, YamlNode> node)
        {
            return ((YamlMappingNode)node.Value).Children;
        }

        public static KeyValuePair<YamlNode, YamlNode> GetNode(this KeyValuePair<YamlNode, YamlNode> node, 
            Func<KeyValuePair<YamlNode, YamlNode>, bool> func)
        {
            return ((YamlMappingNode)node.Value).Children.Single(func);
        }

        public static YamlNode GetNode(this YamlNode node, string value)
        {
            var c = ((YamlMappingNode)node).Children;
            
            return c.SingleOrDefault(f => f.GetKey() == value).Value;
        }

        public static string GetNodeValue(this YamlNode node, string value)
        {
            var result = node.GetNode(value);
            return result == null ? "" : result.GetValue();
        }

        public static IList<YamlNode> GetSequence(this YamlNode node)
        {
            return ((YamlSequenceNode)node).Children;
        }
    }
}