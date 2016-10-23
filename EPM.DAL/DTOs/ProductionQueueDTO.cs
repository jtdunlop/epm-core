namespace DBSoft.EPM.DAL.DTOs
{
	using System;
	using JetBrains.Annotations;

    public class ProductionQueueDto
	{
		public int ItemId { get; set; }
		public string ItemName { get; set; }
		public long Quantity
		{
			get
			{
				var quantity = Math.Max(Sold - Inventory - Building, 0);
                if (!MinimumStock.HasValue || (!(Inventory < MinimumStock) && Inventory >= Sold)) return quantity;
                if (quantity == 0)
                {
                    // Trigger a build. 1 will be rounded up to MinimumStock by the client
                    quantity = 1;
                }
                return quantity;
			}
		}
		public long Sold { get; set; }
		public long Inventory { get; set; }
		public decimal HourlyProfit
		{
			get
			{
				if (Cost.HasValue && Price.HasValue && ProductionTime != 0 )
				{
					return (Price.Value - Cost.Value) / (ProductionTime * (decimal) (1 - .01 * TimeEfficiency) * .8M * .85M) * 60 * 60;
				}
				return 0;
			}
		}

	    [UsedImplicitly]
	    public decimal Markup
        {
            get
            {
                if (Cost.HasValue && Price.HasValue && Cost.Value != 0)
                {
                    return (Price.Value - Cost.Value) / Cost.Value * 100;
                }
                return 0;
            }
        }
        public decimal? MinimumMarkup { get; set; }
		public int? MinimumStock { get; set; }
		public int ProductionTime { get; set; }
		public decimal? Cost { get; set; }
		public decimal? Price { get; set; }
	    public int TimeEfficiency { get; set; }
	    public int Building { get; set; }
	    public int AvailableBlueprints { get; set; }
        public decimal? ProfitFactor { get; set; }
	    public decimal FreightCost { get; set; }
	}

}
