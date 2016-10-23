namespace DBSoft.EPM.DAL.DTOs
{
	using System;
	using JetBrains.Annotations;

    public class MaterialPurchaseDto
	{
		public decimal OvercutPercent { get; set; }
		public int ItemId { get; set; }
		public string ItemName { get; set; }
		public decimal MarketPrice { get; set; }

	    [UsedImplicitly]
	    public decimal NewPrice
		{
			get
			{
                var hubPrice = Math.Max(MarketPrice, RangePrice);
                var price = FactoryQuantity < FactoryRequired || (decimal)InventoryQuantity * 2 < DesiredQuantity ?
                    hubPrice : RangePrice;

				if (OvercutPercent > 0)
				{
                    var newPrice = (price * (1 + OvercutPercent / 100)).SignificantFigures(4);
				    if (Percentage >= 50) return newPrice;
                    // When under 50% overcut the greater of overcut% and 20% of the difference to the sell price
				    var bouncePrice = ((SellPrice - hubPrice) * .2M + price).SignificantFigures(4);
				    return Math.Max(newPrice, bouncePrice);
				}
			    // Minimum increment is .01 the minimum size we want is 1 (1 - 3 = -2 -> .01)
				var size = Math.Max(Math.Floor(Math.Log10((double)price)), 1);
				// The 3 subtracted from size is the number of significant figures less 1
				var add = (decimal)Math.Pow(10, size - 3);
				return price.SignificantFigures(4) + add;
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

        public decimal Percentage => DesiredQuantity == 0 ? 0 : (decimal)InventoryQuantity * 100 / DesiredQuantity;

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
		public long FactoryQuantity { get; set; }
		public long FactoryRequired { get; set; }
	    public decimal SellPrice { get; set; }
	}
}
