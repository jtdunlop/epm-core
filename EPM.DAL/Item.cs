//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EPM.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class Item
    {
        public Item()
        {
            this.Assets = new HashSet<Asset>();
            this.Blueprints = new HashSet<Blueprint>();
            this.Blueprints1 = new HashSet<Blueprint>();
            this.Manifests = new HashSet<Manifest>();
            this.Manifests1 = new HashSet<Manifest>();
            this.MarketImports = new HashSet<MarketImport>();
            this.MarketOrders = new HashSet<MarketOrder>();
            this.ProductionJobs = new HashSet<ProductionJob>();
            this.Transactions = new HashSet<Transaction>();
        }
    
        public int ID { get; set; }
        public string Name { get; set; }
        public int GroupID { get; set; }
        public Nullable<int> MinimumStock { get; set; }
        public int QuantityMultiplier { get; set; }
        public Nullable<decimal> BounceFactor { get; set; }
    
        public virtual ICollection<Asset> Assets { get; set; }
        public virtual ICollection<Blueprint> Blueprints { get; set; }
        public virtual ICollection<Blueprint> Blueprints1 { get; set; }
        public virtual Group Group { get; set; }
        public virtual ICollection<Manifest> Manifests { get; set; }
        public virtual ICollection<Manifest> Manifests1 { get; set; }
        public virtual ICollection<MarketImport> MarketImports { get; set; }
        public virtual ICollection<MarketOrder> MarketOrders { get; set; }
        public virtual ICollection<ProductionJob> ProductionJobs { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
