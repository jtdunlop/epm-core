namespace DBSoft.EPM.DAL.CodeFirst.Models
{
	public partial class MarketImport2
    {
        public int ID { get; set; }
        public System.DateTime TimeStamp { get; set; }
        public long StationID { get; set; }
        public int SolarSystemID { get; set; }
        public int RegionID { get; set; }
        public decimal Price { get; set; }
        public int ItemID { get; set; }
        public OrderType OrderType { get; set; }
        public short Jumps { get; set; }
        public short Range { get; set; }
        public int UserID { get; set; }

        public virtual Item Item { get; set; }
        public virtual User User { get; set; }
    }

	public partial class MarketImport
	{
		public long ID { get; set; }
		public System.DateTime TimeStamp { get; set; }
		public int StationID { get; set; }
		public int SolarSystemID { get; set; }
		public int RegionID { get; set; }
		public decimal Price { get; set; }
		public int ItemID { get; set; }
		public OrderType OrderType { get; set; }
		public short Jumps { get; set; }
		public short Range { get; set; }
		public int UserID { get; set; }
		public virtual Item Item { get; set; }
		public virtual User User { get; set; }
	}
}
