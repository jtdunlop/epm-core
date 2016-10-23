namespace DBSoft.EPM.DAL.DTOs
{
	using System;
	using BODA;

	public class ItemInventoryDTO
	{
		public int ItemID { get; set; }
		public string ItemName { get; set; }
		public decimal MarketPrice { get; set; }
		public decimal NewPrice
		{
			get
			{
				if ((decimal)InventoryQuantity * 2 < DesiredQuantity)
				{
					var size = Math.Max(Math.Floor(Math.Log10((double)MarketPrice)), 1);
					var add = (decimal)Math.Pow(10, size - 3);
					return MarketPrice.SignificantFigures(4) + add;
				}
				else
				{
					var size = Math.Max(Math.Floor(Math.Log10((double)RangePrice)), 1);
					var add = (decimal)Math.Pow(10, size - 3);
					return RangePrice.SignificantFigures(4) + add;
				}
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
				if (InventoryQuantity > DesiredQuantity)
				{
					return 0;
				}
				return (long) (DesiredQuantity / 4M).SignificantFigures(2);
			}
		}
	}
}
