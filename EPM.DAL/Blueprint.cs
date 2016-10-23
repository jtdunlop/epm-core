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
    
    public partial class Blueprint
    {
        public Blueprint()
        {
            this.BlueprintInstances = new HashSet<BlueprintInstance>();
        }
    
        public long ID { get; set; }
        public int ItemID { get; set; }
        public int ProductionTime { get; set; }
        public int WasteFactor { get; set; }
        public Nullable<int> BuildItemID { get; set; }
        public Nullable<int> ProductivityModifier { get; set; }
    
        public virtual Item BuildItem { get; set; }
        public virtual Item Item { get; set; }
        public virtual ICollection<BlueprintInstance> BlueprintInstances { get; set; }
    }
}
