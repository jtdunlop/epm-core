namespace Dbsoft.Epm.Web.Controllers.Production
{
    using JetBrains.Annotations;

    [UsedImplicitly]
    public class ProductionQueueItemModel
	{
		public int ItemId { get; set; }
		public string ItemName { get; set; }
		public long Quantity { get; set; }
		public decimal HourlyProfit { get; set; }
		public int? MinimumStock { get; set; }
		public decimal? Markup { get; set; }
        public decimal? MinimumMarkup { get; set; }
        public decimal? ProfitFactor { get; set; }
        public decimal FreightCost { get; set; }
        public int AvailableBlueprints { get; set; }
	}
}
