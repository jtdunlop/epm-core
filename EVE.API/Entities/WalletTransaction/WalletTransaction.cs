namespace DBSoft.EVEAPI.Entities.WalletTransaction
{
	using System;

	public class WalletTransaction : IComparable
	{
		public long ID { get; set; }
		public DateTime DateTime { get; set; }
		public int Quantity { get; set; }
		public int TypeID { get; set; }
		public Decimal Price { get; set; }
		public string Type { get; set; }
		public int CompareTo(object o)
		{
			return DateTime.CompareTo(((WalletTransaction) o).DateTime);
		}
	}
}