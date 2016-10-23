namespace DBSoft.EPM.DAL.Services
{
    using Aspects;
    using DbSoft.Cache.Aspect.Attributes;
    using DTOs;
	using Interfaces;
	using System.Collections.Generic;
	using System.Linq;
    using JetBrains.Annotations;
    using Materials;

    [UsedImplicitly]
    public class MaterialRequirementService : IMaterialRequirementService
    {
		private readonly IItemService _items;
        private readonly IBuildMaterialService _materials;

        public MaterialRequirementService(IItemService items, IBuildMaterialService materials)
		{
		    _items = items;
            _materials = materials;
		}

        [Trace,Cache.Cacheable]
		public IEnumerable<MaterialRequirementDto> ListBuildable(string token)
		{
			var materials = _materials.ListBuildable(token);
            var items = _items.ListBuildable(token);
			var result = BuildResult(materials, items);
			return result.ToList();
		}

        private static IEnumerable<MaterialRequirementDto> BuildResult(IEnumerable<BuildMaterialDto> materials,
            IEnumerable<BuildableItemDTO> items)
        {
            var result = from m in materials
                join item in items on m.ItemId equals item.ItemID
                select new MaterialRequirementDto
                {
                    ItemId = item.ItemID,
                    ItemName = item.ItemName,
                    MaterialId = m.MaterialId,
                    MaterialName = m.MaterialName,
                    BaseQuantity = m.Quantity,
                    AdditionalQuantity = m.AdditionalQuantity,
                    MaterialEfficiency = item.MaterialEfficiency,
                    BounceFactor = m.BounceFactor
                };
            return result;
        }
    }
}
