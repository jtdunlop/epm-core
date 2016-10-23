namespace DBSoft.EVEAPI.Entities.MarketOrder
{
	using System;

	public enum OrderState
	{
		Active = 0,
// ReSharper disable UnusedMember.Global
		Inactive = 2
// ReSharper restore UnusedMember.Global
	}
	public enum OrderType
	{
		Buy,
		Sell
	}
	public class MarketOrder
	{
		public long ID;
		public int CharacterID;
		// ReSharper disable NotAccessedField.Global
		public int StationID;
		public int VolumeEntered;
		public int VolumeRemaining;
		public int MinimumVolume;
		public OrderState OrderState;
		public int ItemID;
		public short Range;
		public int AccountKey;
		public int Duration;
		public decimal Escrow;
		public decimal Price;
		public OrderType OrderType;
		public DateTime WhenIssued;
	}
}
