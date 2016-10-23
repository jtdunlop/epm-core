namespace Dbsoft.Epm.Web.Controllers.Maintenance
{
	public class ConfigurationModel
	{
		public bool ConfigurationValid { get; set; }
		public decimal MinimumMarkup { get; set; }
		public decimal UndercutPercent { get; set; }
		public decimal OvercutPercent { get; set; }
		public decimal PurchaseBrokerFee { get; set; }
		public decimal SellBrokerFee { get; set; }
		public decimal SalesTax { get; set; }
		public int RegionID { get; set; }
		public int FactoryID { get; set; }
		public int MarketSellID { get; set; }
        public int MarketBuyID { get; set; }
		public int InboundContractThreshold { get; set; }
		public int BuyOrderRange { get; set; }
        public int? PosLocationID { get; set; }
        public bool PosEnabled { get { return PosLocationID.HasValue; } }
        public decimal FreightJumpCost { get; set; }
        public decimal FreightMaxCollateral { get; set; }
        public decimal FreightMaxVolume { get; set; }
        public decimal FreightPickupCost { get; set; }
	}
}