namespace DBSoft.EPM.DAL.Services
{
	using CacheAspect.Attributes;
	using DTOs;
	using Interfaces;
	using System.Collections.Generic;
	using System.Linq;

	public class MaterialUsageService : DataService 
	{
		private readonly BuildMaterialService _materials;
		private readonly BuildableItemService _buildable;

		public MaterialUsageService(IDbContextFactory factory) : base(factory)
		{
			_materials = new BuildMaterialService(factory);
			_buildable = new BuildableItemService(factory);
		}

		[Cache.Cacheable]
		public IEnumerable<MaterialUsageDTO> List(string token)
		{
			var materials = _materials.List(token);
			var buildable = _buildable.List(token);
			var result = from m in materials
						 join b in buildable on m.ItemID equals b.ItemID
						 select new MaterialUsageDTO
						 {
							 ItemID = b.ItemID,
							 ItemName = b.ItemName,
							 MaterialID = m.MaterialID,
							 MaterialName = m.MaterialName,
							 BaseQuantity = m.Quantity,
							 AdditionalQuantity = m.AdditionalQuantity,
							 MaterialEfficiency = b.MaterialEfficiency,
							 BounceFactor = m.BounceFactor
						 };
			return result.ToList();

		}
	}
}
