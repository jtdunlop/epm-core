namespace DBSoft.EPM.DAL.Services
{
    using Aspects;
    using DbSoft.Cache.Aspect.Attributes;
    using DTOs;
	using Enums;
	using Interfaces;
	using Requests;
	using System.Collections.Generic;
	using System.Linq;
    using Materials;

    public class ProductionMaterialService : IProductionMaterialService
    {
        private readonly IConfigurationService _config;
        private readonly IAssetService _assets;
        private readonly IProductionQueueService _queue;
        private readonly IBuildMaterialService _materials;

        public ProductionMaterialService(IAssetService assets, IProductionQueueService queue, IBuildMaterialService materials,
            IConfigurationService config)
		{
            _config = config;
            _assets = assets;
            _queue = queue;
            _materials = materials;
		}

		[Trace,Cache.Cacheable(name: "BuildMaterials")]
		public IEnumerable<ProductionMaterialDto> List(string token)
		{
			var queue = _queue.List(token)
				.Where(f => f.Quantity > 0);
            var materials = _materials.ListBuildable(token);
            var pos = _config.GetSetting<int>(token, ConfigurationType.PosLocation);
			var assets = GetAssets(token, pos);

		    var result = from q in queue
						 join material in materials on q.ItemId equals material.ItemId
						 orderby material.MaterialId
						 group new {material, q} by new { MaterialID = material.MaterialId, material.MaterialName, material.BounceFactor } into grp
						 from asset in assets.Where(f => f.ItemID == grp.Key.MaterialID).DefaultIfEmpty()
						 select new ProductionMaterialDto
						 {
							 ItemId = grp.Key.MaterialID,
							 ItemName = grp.Key.MaterialName,
							 Inventory = asset?.Quantity ?? 0,
							 Required = grp.Sum(f => f.q.Quantity * (f.material.Quantity + f.material.AdditionalQuantity)),
                             BounceFactor = grp.Key.BounceFactor ?? 1
						 };
			return result.ToList();
		}

        private IEnumerable<AssetByItemDTO> GetAssets(string token, int pos)
        {
            var station = _config.GetSetting<int>(token, ConfigurationType.FactoryLocation);
            var assets = new List<AssetByItemDTO>();
            assets.AddRange(_assets
                .ListByItem(new AssetServiceRequest
                {
                    Token = token,
                    StationID = station
                }).ToList());

            if ( pos != 0 )
            {
                var request = new AssetServiceRequest
                {
                    Token = token,
                    SolarSystemID = pos
                };
                assets.AddRange(_assets.ListByItem(request));
            }
            var result = from asset in assets
                         group new { asset } by new { asset.ItemID, asset.ItemName } into grp
                         select new AssetByItemDTO
                         {
                             ItemID = grp.Key.ItemID,
                             ItemName = grp.Key.ItemName,
                             Quantity = grp.Sum(f => f.asset.Quantity)
                         };
            return result;
        }
    }
}
