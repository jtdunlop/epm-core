namespace DBSoft.EVE.SDE
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using YamlDotNet.RepresentationModel;

    public class BlueprintService
    {
        private readonly IYamlProvider _provider;

        public BlueprintService(IYamlProvider provider)
        {
            _provider = provider;
        }

        public IEnumerable<ManifestDTO> GetManifests()
        {
#if false
683:
    activities:
        copying:
            time: 4800
        manufacturing:
            materials:
            -   quantity: 22222
                typeID: 34
            -   quantity: 8000
                typeID: 35
            -   quantity: 2444
                typeID: 36
            -   quantity: 500
                typeID: 37
            -   quantity: 2
                typeID: 38
            -   quantity: 4
                typeID: 39
            products:
            -   quantity: 1
                typeID: 582
            skills:
            -   level: 1
                typeID: 3380
            time: 6000
        research_material:
            time: 2100
        research_time:
            time: 2100
    blueprintTypeID: 683
    maxProductionLimit: 30
#endif
            using (var stream = _provider.GetYamlStream())
            {
                if (stream == null) return null;
                using (var reader = new StreamReader(stream))
                {
                    var yaml = new YamlStream();
                    yaml.Load(reader);
                    var mapping = (YamlMappingNode)yaml.Documents[0].RootNode;
                    var result = new List<ManifestDTO>();
                    foreach (var blueprint in mapping.Children)
                    {
                        var manufacturing = blueprint.Value
                            .GetNode("activities")
                            .GetNode("manufacturing");

                        if (manufacturing == null) continue;
                        var materials = manufacturing.GetNode("materials");
                        if (materials == null) continue;

                        var products = manufacturing.GetNode("products");
                        var product = products.GetSequence().First();

                        var add = new ManifestDTO();
                        add.BlueprintID = int.Parse(blueprint.GetKey());
                        add.ItemID = int.Parse(product.GetNode("typeID").GetValue());
                        add.Quantity = int.Parse(product.GetNode("quantity").GetValue());
                        add.ProductionTime = int.Parse(manufacturing.GetNode("time").GetValue());

                        add.Materials = new List<MaterialDTO>();
                        foreach (var dto in materials
                            .GetSequence().Select(material => new MaterialDTO
                            {
                                ItemID = int.Parse(material.GetNode("typeID").GetValue()),
                                Quantity =
                                    int.Parse(material.GetNode("quantity").GetValue())
                            }))
                        {
                            add.Materials.Add(dto);
                        }
                        result.Add(add);
                    }
                    return result;
                }
            }

        }
    }

    public class ManifestDTO
    {
        public ManifestDTO()
        {
            Materials = new List<MaterialDTO>();
        }

        public int BlueprintID { get; set; }
        public int ItemID { get; set; }
        public int Quantity { get; set; }
        public int ProductionTime { get; set; }
        public List<MaterialDTO> Materials { get; set; }
    }

    public class MaterialDTO
    {
        public int ItemID { get; set; }
        public int Quantity { get; set; }
    }
}
