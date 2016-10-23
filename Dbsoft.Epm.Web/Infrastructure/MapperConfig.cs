using DBSoft.EPM.DAL.Services.Market;
using DBSoft.EPM.DAL.Services.Transactions;
using DBSoft.EVEAPI.Crest.MarketOrder;

namespace Dbsoft.Epm.Web.Infrastructure
{
    using AutoMapper;
    using Controllers.Maintenance;
    using Controllers.Production;
    using Controllers.Production.InboundContract;
    using Controllers.Production.PostBuyOrders;
    using Controllers.Production.PostSellOrders;
    using Controllers.Production.UpdateBuyOrders;
    using Controllers.Production.UpdateSellOrders;
    using Controllers.Reporting.DailySales;
    using DBSoft.EPM.DAL.CodeFirst.Models;
    using DBSoft.EPM.DAL.DTOs;
    using DBSoft.EPM.DAL.Requests;
    using DBSoft.EPM.DAL.Services.AccountApi;
    using DBSoft.EPM.DAL.Services.Contracts;
    using DBSoft.EVEAPI.Entities.MarketOrder;
    using MarketOrder = DBSoft.EVEAPI.Entities.MarketOrder.MarketOrder;

    public static class MapperConfig
    {
        public static void ConfigureMappings()
        {
            Mapper.Initialize(cfg => {
                cfg.CreateMap<Station, StationDTO>()
                    .ForMember(m => m.StationID, opt => opt.MapFrom(m => m.ID))
                    .ForMember(m => m.StationName, opt => opt.MapFrom(m => m.Name))
                    .ForMember(m => m.StationTax, opt => opt.MapFrom(m => m.Tax));

                cfg.CreateMap<MarketPrice, MarketAdjustedPriceDTO>();
                cfg.CreateMap<SolarSystem, SolarSystemDTO>()
                    .ForMember(m => m.SolarSystemID, opt => opt.MapFrom(m => m.ID))
                    .ForMember(m => m.SolarSystemName, opt => opt.MapFrom(m => m.Name))
                    ;

                cfg.CreateMap<UserDTO, ApplicationUser>();
                cfg.CreateMap<User, UserDTO>()
                    .ForMember(m => m.UserID, map => map.MapFrom(m => m.ID))
                    .ForMember(m => m.UserName, map => map.MapFrom(m => m.EveOnlineCharacter));

                cfg.CreateMap<MaterialItemDto, MaterialItemModel>();

                cfg.CreateMap<ProductionMaterialDto, ProductionMaterialItemModel>()
                    .ForMember(m => m.Available, opt => opt.MapFrom(m => m.Inventory))
                    .ForMember(m => m.Needed, opt => opt.MapFrom(m => m.Required));

                cfg.CreateMap<MarketOrder, SaveMarketOrderRequest>()
                    .ForMember(m => m.OrderID, opt => opt.MapFrom(m => m.ID))
                    .ForMember(m => m.OriginalQuantity, opt => opt.MapFrom(m => m.VolumeEntered))
                    .ForMember(m => m.RemainingQuantity, opt => opt.MapFrom(m => m.VolumeRemaining))
                    .ForMember(m => m.MinimumQuantity, opt => opt.MapFrom(m => m.MinimumVolume))
                    .ForMember(m => m.OrderStatus,
                        opt =>
                            opt.ResolveUsing(
                                f => f.OrderState == OrderState.Active ? OrderStatus.Active : OrderStatus.Inactive))
                    .ForMember(m => m.EveCharacterID, opt => opt.MapFrom(m => m.CharacterID))
                    .ForMember(m => m.Token, opt => opt.Ignore())
                    ;

                cfg.CreateMap<ProductionQueueDto, ProductionQueueItemModel>();

                cfg.CreateMap<InboundContractDTO, InboundContractItemModel>();

                cfg.CreateMap<MarketRestockDTO, PostSellOrdersItemModel>();

                cfg.CreateMap<MarketRepriceDTO, UpdateSellOrderItemModel>();

                cfg.CreateMap<MaterialPurchaseDto, PostBuyOrderItemModel>();

                cfg.CreateMap<MaterialPurchaseDto, UpdateBuyOrderItemModel>();

                cfg.CreateMap<ItemTransactionByDateDto, DailySaleItemModel>();
                cfg.CreateMap<ItemTransactionByMonthDto, DailySaleItemModel>();

                cfg.CreateMap<Item, ItemDTO>()
                    .ForMember(f => f.ItemID, opt => opt.MapFrom(m => m.ID))
                    .ForMember(f => f.ItemName, opt => opt.MapFrom(m => m.Name))
                    ;

                cfg.CreateMap<ConfigurationModel, ConfigurationSettingsDTO>()
                    .ForMember(m => m.FactoryLocation, opt => opt.MapFrom(m => m.FactoryID))
                    .ForMember(m => m.MarketSellLocation, opt => opt.MapFrom(m => m.MarketSellID))
                    .ForMember(m => m.MarketBuyLocation, opt => opt.MapFrom(m => m.MarketBuyID))
                    .ForMember(m => m.PosLocation, opt => opt.MapFrom(m => m.PosLocationID));

                cfg.CreateMap<ConfigurationSettingsDTO, ConfigurationModel>()
                    .ForMember(m => m.FactoryID, opt => opt.MapFrom(m => m.FactoryLocation))
                    .ForMember(m => m.MarketSellID, opt => opt.MapFrom(m => m.MarketSellLocation))
                    .ForMember(m => m.MarketBuyID, opt => opt.MapFrom(m => m.MarketBuyLocation))
                    .ForMember(m => m.PosLocationID, opt => opt.MapFrom(m => m.PosLocation));

                cfg.CreateMap<Item, ItemDTO>()
                    .ForMember(f => f.ItemID, opt => opt.MapFrom(m => m.ID))
                    .ForMember(f => f.ItemName, opt => opt.MapFrom(m => m.Name))
                    ;
                cfg.CreateMap<AccountDTO, AccountModel>().ReverseMap();

                cfg.CreateMap<SaveMarketOrderRequest, DBSoft.EPM.DAL.CodeFirst.Models.MarketOrder>()
                    .ForMember(m => m.EveMarketOrderID, opt => opt.MapFrom(m => m.OrderID));

                cfg.CreateMap<SaveMarketImportRequest, MarketImport2>()
                    .ForMember(m => m.OrderType, opt => opt.MapFrom(m => m.OrderType));
                
                cfg.CreateMap<SaveProductionJobRequest, ProductionJob>()
                    .ForMember(m => m.ID, opt => opt.MapFrom(m => m.ProductionJobID));

                cfg.CreateMap<SaveTransactionRequest, Transaction>()
                    .ForMember(m => m.EveTransactionID, opt => opt.MapFrom(f => f.EveTransactionID));

                cfg.CreateMap<SaveAccountRequest, Account>()
                    .ForMember(m => m.ID, opt => opt.MapFrom(m => m.AccountID))
                    .ForMember(m => m.Name, opt => opt.MapFrom(m => m.AccountName))
                    .ForMember(m => m.ApiKeyType, opt => opt.MapFrom(m => m.ApiKeyType))
                    .ForMember(m => m.Corporations, opt => opt.Ignore())
                    .ForMember(m => m.Characters, opt => opt.Ignore());

                cfg.CreateMap<SaveAssetRequest, Asset>()
                    .ForMember(m => m.ID, opt => opt.MapFrom(m => m.AssetID))
                    .ForMember(m => m.StationID,
                        opt => opt.ResolveUsing(f => f.LocationID >= 60000000 && f.LocationID < 70000000 ? f.LocationID : (int?)null))
                    .ForMember(m => m.SolarSystemID,
                        opt => opt.ResolveUsing(f => f.LocationID >= 30000000 && f.LocationID < 40000000 ? f.LocationID : (int?)null))
                    .ForMember(m => m.Quantity, opt => opt.MapFrom(m => (int)m.Quantity));

                cfg.CreateMap<MarketSummaryDTO, MarketSummaryItem>();

                cfg.CreateMap<BuildableItemDTO, BuildableItemListItemModel>();

                cfg.CreateMap<MaterialItemDto, MaterialItemListItem>();
            });
        }
    }
}
