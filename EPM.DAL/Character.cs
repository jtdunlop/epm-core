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
    
    public partial class Character
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public Nullable<int> AccountID { get; set; }
        public int CorporationID { get; set; }
        public int EveApiID { get; set; }
    
        public virtual Account Account { get; set; }
        public virtual Corporation Corporation { get; set; }
    }
}
