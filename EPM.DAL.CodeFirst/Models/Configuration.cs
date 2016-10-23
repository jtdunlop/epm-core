using System;
using System.Collections.Generic;

namespace DBSoft.EPM.DAL.CodeFirst.Models
{
    public partial class Configuration
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public Nullable<int> UserID { get; set; }
        public virtual User User { get; set; }
    }
}
