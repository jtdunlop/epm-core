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
    
    public partial class BuildableItem
    {
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public int ProductionTime { get; set; }
        public Nullable<int> ProductivityModifier { get; set; }
        public Nullable<int> MinimumStock { get; set; }
        public int BuildQuantity { get; set; }
        public Nullable<int> MaterialEfficiency { get; set; }
        public Nullable<int> ProductionEfficiency { get; set; }
    }
}