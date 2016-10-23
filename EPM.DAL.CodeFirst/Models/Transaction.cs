using System;

namespace DBSoft.EPM.DAL.CodeFirst.Models
{
    using System.Collections.Generic;

    public enum TransactionType : short
	{
		Buy,
		Sell
	}
    public partial class Transaction
    {
        public int ID { get; set; }
        public DateTime DateTime { get; set; }
        public int Quantity { get; set; }
        public int ItemID { get; set; }
        public decimal Price { get; set; }
        public TransactionType TransactionType { get; set; }
        public int EveCharacterID { get; set; }
        public int? CorporationID { get; set; }
        public decimal? Cost { get; set; }
        public long EveTransactionID { get; set; }
        public int UserID { get; set; }
        public int? AccountID { get; set; }
		public bool VisibleFlag { get; set; }
        public virtual User User { get; set; }
		public virtual Item Item { get; set; }
        public ICollection<HiredTeamCost> HiredTeamCosts { get; set; }
    }
}
