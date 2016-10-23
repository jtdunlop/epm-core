namespace DBSoft.EVE.SDE
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using YamlDotNet.RepresentationModel;

    public class GroupService
    {
        private readonly YamlProvider _provider;

        public GroupService(YamlProvider provider)
        {
            _provider = provider;
        }

        public IEnumerable<GroupDTO> GetGroups()
        {
            using (var stream = _provider.GetYamlStream())
            {
                if (stream == null) return null;
                using (var reader = new StreamReader(stream))
                {
                    var yaml = new YamlStream();
                    yaml.Load(reader);
                    var root = (YamlMappingNode)yaml.Documents[0].RootNode;
                    return root.Select(group => new GroupDTO
                    {
                        GroupID = int.Parse(group.GetKey()),
                        CategoryID = int.Parse(group.GetNodes().Single(f => f.GetKey() == "categoryID").GetValue()),
                        GroupName = group.GetNodes().Single(f => f.GetKey() == "name").GetNodes().Single(f => f.GetKey() == "en").GetValue(),
                        IsPublished = bool.Parse(group.GetNodes().Single(f => f.GetKey() == "published").GetValue())
                    }).ToList();
                }
            }
        }
    }
}