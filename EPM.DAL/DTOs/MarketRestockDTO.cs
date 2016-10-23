namespace DBSoft.EPM.DAL.DTOs
{
	using System;

	public class MarketRestockDTO
	{
		public int ItemID { get; set; }
		public string ItemName { get; set; }
// ReSharper disable UnusedMember.Global
		public long Quantity { get; set; }
		public decimal? MarketPrice { get; set; }
		public decimal MinimumMarkup { get; set; }
		public decimal? UndercutPrice
		{
			get
			{
				if (ItemCost != null && MarketPrice != null)
				{
					return (MarketPrice.Value - .01M).SignificantFigures(4);
				}
				return null;
			}
		}
		public decimal? ItemCost { get; set; }
		public decimal? Variance { get; set; }
		public decimal? MinimumPrice
		{
			get
			{
			    if (Variance == null || ItemCost == null) return null;
			    var v = (1 + (Variance.Value + MinimumMarkup) / 100);
			    return v * ItemCost.Value;
			}
		}
		public decimal? NewPrice
		{
			get 
			{
				if (UndercutPrice != null && MinimumPrice != null)
				{
					return Math.Max(UndercutPrice.Value, MinimumPrice.Value).SignificantFigures(4);
				}
				return null;
			}
		}
		public decimal? Markup
		{
			get
			{
				return (MarketPrice / ItemCost - 1) * 100;
			}
		}

	    public DateTime Timestamp { get; set; }
	}
}
