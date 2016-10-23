namespace DBSoft.EPM.DAL.DTOs
{
	public class ConfigurationSettingsDTO
	{
		public decimal MinimumMarkup { get; set; }
		public decimal UndercutPercent { get; set; }
		public decimal OvercutPercent { get; set; }
		public decimal PurchaseBrokerFee { get; set; }
		public decimal SellBrokerFee { get; set; }
		public decimal SalesTax { get; set; }
		public int FactoryLocation { get; set; }
		public int MarketSellLocation { get; set; }
        public int MarketBuyLocation { get; set; }
        public int? PosLocation { get; set; }
		public int InboundContractThreshold { get; set; }
		public int BuyOrderRange { get; set; }
	    public string EveOnlineCharacter { get; set; }
	    public decimal FreightJumpCost { get; set; }
	    public decimal FreightMaxCollateral { get; set; }
	    public decimal FreightMaxVolume { get; set; }
	    public decimal FreightPickupCost { get; set; }
	}
}
