namespace DBSoft.EPM.DAL.Requests
{
	using System;
	using CodeFirst.Models;

	public class SaveMarketOrderRequest
	{
		public string Token { get; set; }
		public long OrderID { get; set; }
		// ReSharper disable UnusedMember.Global
		public int ItemID { get; set; }
		public int StationID { get; set; }
		public int EveCharacterID { get; set; }
		public int OrderStatus { get; set; }
		public OrderType OrderType { get; set; }
		public int Duration { get; set; }
		public decimal Price;
		public decimal Escrow { get; set; }
		public DateTime WhenIssued { get; set; }
		public int Range;
		public int OriginalQuantity { get; set; }
		public int MinimumQuantity { get; set; }
		public int RemainingQuantity { get; set; }
		// ReSharper restore UnusedMember.Global
	}
}