namespace DBSoft.EPM.DAL.Services.Contracts
{
    using Aspects;
    using DbSoft.Cache.Aspect.Attributes;
    using DTOs;
    using System.Collections.Generic;
    using System.Linq;
    using Enums;
    using Interfaces;
    using Requests;

    public class ContractService : IContractService
    {
        private readonly IAssetService _assets;
        private readonly IConfigurationService _config;
        private readonly IMarketRestockService _restock;
        private readonly IAssetValueService _assetValues;
        private readonly IItemService _items;

        public ContractService(IAssetService assets, IConfigurationService config, IMarketRestockService restock, 
            IAssetValueService assetValues, IItemService items)
        {
            _assets = assets;
            _config = config;
            _restock = restock;
            _assetValues = assetValues;
            _items = items;
        }

        [Trace,Cache.Cacheable]
        public IEnumerable<OutboundContractDTO> ListOutboundContracts(string token)
        {
            var buildable = _items.ListBuildable(token);
            var restock = _restock.List(token);
            var request = new AssetServiceRequest
            {
                Token = token,
                StationID = _config.GetSetting<int>(token, ConfigurationType.FactoryLocation)
            };
            var here = from asset in _assets.ListByItem(request)
                    join b in buildable on asset.ItemID equals b.ItemID
                    select new AssetByItemDTO
                    {
                        ItemID = asset.ItemID,
                        ItemName = asset.ItemName,
                        Quantity = asset.Quantity
                    };
            var there = _assets.ListByItem(new AssetServiceRequest
            {
                Token = token,
                StationID = _config.GetSetting<int>(token, ConfigurationType.MarketSellLocation)
            }).Where(f => restock.All(g => g.ItemID != f.ItemID)).ToList();

            var result = here.Where(f => there.All(t => t.ItemID != f.ItemID))
                .Select(s => new OutboundContractDTO
                {
                    ItemID = s.ItemID,
                    ItemName = s.ItemName
                });
            return result;
        }

        [Trace,Cache.Cacheable]
        public IEnumerable<InboundContractDTO> ListInboundContracts(string token)
        {
   			var threshold = _config.GetSetting<int>(token, ConfigurationType.InboundContractThreshold);

            var values = _assetValues
                .ListMaterialsByStation(new AssetValueRequest { Token = token })
                .Where(f => f.Value > threshold)
                .Where(f => f.StationID != _config.GetSetting<int>(token, ConfigurationType.FactoryLocation))
                .OrderBy(f => f.StationName);
            return values.Select(f => new InboundContractDTO
            {
                StationName = f.StationName,
                Value = f.Value
            });
        }
    }
}
