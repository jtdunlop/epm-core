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
    
    public partial class BlueprintInstance
    {
        public long ID { get; set; }
        public long AssetID { get; set; }
        public int MaterialEfficiency { get; set; }
        public int ProductionEfficiency { get; set; }
        public bool DeletedFlag { get; set; }
        public long BlueprintID { get; set; }
    
        public virtual Asset Asset { get; set; }
        public virtual Blueprint Blueprint { get; set; }
    }
}