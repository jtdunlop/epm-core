namespace DBSoft.EPM.DAL.DTOs
{
	using System;
	using BODA;

	public class OrderPriceDTO
	{
		public decimal OvercutPercent { get; set; }
		public int ItemID { get; set; }
		public string ItemName { get; set; }
		public decimal MarketPrice { get; set; }
		public decimal NewPrice
		{
			get
			{
				if (FactoryQuantity < FactoryRequired)
				{
					return (Math.Max(MarketPrice, RangePrice) * (1 + OvercutPercent / 100)).SignificantFigures(4);
				}
				return (decimal)InventoryQuantity * 2 < DesiredQuantity ? 
					(Math.Max(MarketPrice, RangePrice) * (1 + OvercutPercent/ 100)).SignificantFigures(4) : 
					(RangePrice * (1 + OvercutPercent / 100)).SignificantFigures(4);
			}
		}
		public decimal RangePrice { get; set; }
		public long UsageQuantity { get; set; }
		public long DesiredQuantity
		{
			get
			{
				var bounce = BounceFactor ?? 1;
				return (long) (UsageQuantity * 2 * bounce);
			}
		}
		public long InventoryQuantity { get; set; }
		public decimal? BounceFactor { get; set; }
		public decimal Percentage
		{
			get
			{
				return (decimal)InventoryQuantity * 100 / DesiredQuantity;
			}
		}
		public long PurchaseQuantity
		{
			get
			{
				var bounce = BounceFactor ?? 1;
				if (InventoryQuantity > DesiredQuantity * bounce)
				{
					return 0;
				}
				return (long)(UsageQuantity * bounce / 2).SignificantFigures(2);
			}
		}
		public long FactoryQuantity { get; set; }
		public long FactoryRequired { get; set; }
	}
}
