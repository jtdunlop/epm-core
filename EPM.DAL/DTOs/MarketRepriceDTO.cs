namespace DBSoft.EPM.DAL.DTOs
{
	using System;

	public class MarketRepriceDTO
	{
		public int ItemID { get; set; }
		public string ItemName { get; set; }
		public decimal MarketPrice { get; set; }
		public decimal ListedPrice { get; set; }
		public decimal Cost { get; set; }
		public decimal? Variance { get; set; }
		public decimal MinimumMarkup { get; set; }
		public decimal UndercutPercent { get; set; }
		// ReSharper disable UnusedMember.Global
		public decimal Markup
		{
			get
			{
				return Cost == 0 ? 0 : (MarketPrice - Cost) / Cost * 100;
			}
		}
		public decimal NewPrice
		{
			get
			{
				var variance = Variance ?? 0M;
				var newPrice = MarketPrice - (UndercutPercent) / 100 * (MarketPrice - Cost);
				var minimum = Cost * (1 + (variance + MinimumMarkup) / 100);
				// No reason to undercut if doing so would go too low, or if already the best price
				if (ListedPrice > MarketPrice && newPrice < minimum || ListedPrice <= MarketPrice)
				{
					return Math.Max(ListedPrice, minimum).SignificantFigures(4);
				}
				return Math.Max(newPrice, minimum).SignificantFigures(4);
			}
		}

	    public DateTime Timestamp { get; set; }
	}
}
