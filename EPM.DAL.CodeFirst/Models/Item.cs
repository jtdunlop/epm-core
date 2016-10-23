namespace DBSoft.EPM.DAL.CodeFirst.Models
{
	using System.Collections.Generic;

	public class Item
	{
		public Item()
		{
			Assets = new List<Asset>();
			BuildBlueprints = new List<Blueprint>();
			Blueprints = new List<Blueprint>();
			ItemExtensions = new List<ItemExtension>();
			Manifests = new List<Manifest>();
			Manifests1 = new List<Manifest>();
			MarketImports = new List<MarketImport2>();
			MarketOrders = new List<MarketOrder>();
			ProductionJobs = new List<ProductionJob>();
            MarketPrices = new List<MarketPrice>();
		}

		public int ID { get; set; }
		public string Name { get; set; }
		public int GroupID { get; set; }
		public int QuantityMultiplier { get; set; }
        public int? ItemMetaGroup { get; set; }
        public bool IsPublished { get; set; }
        public decimal Volume { get; set; }
		public virtual ICollection<Asset> Assets { get; set; }
		public virtual ICollection<Blueprint> BuildBlueprints { get; set; }
		public virtual ICollection<Blueprint> Blueprints { get; set; }
		public virtual Group Group { get; set; }
		public virtual ICollection<ItemExtension> ItemExtensions { get; set; }
		public virtual ICollection<Manifest> Manifests { get; set; }
		public virtual ICollection<Manifest> Manifests1 { get; set; }
		public virtual ICollection<MarketImport2> MarketImports { get; set; }
		public virtual ICollection<MarketOrder> MarketOrders { get; set; }
		public virtual ICollection<ProductionJob> ProductionJobs { get; set; }
		public virtual ICollection<Transaction> Transactions { get; set; }
	    public virtual ICollection<MarketPrice> MarketPrices { get; set; }
	}
}
