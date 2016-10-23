using System;
using System.Collections.Generic;

namespace DBSoft.EPM.DAL.CodeFirst.Models
{
    public partial class EveApiStatus
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public System.DateTime CacheExpiry { get; set; }
        public int UserID { get; set; }
        public string Result { get; set; }
        public virtual User User { get; set; }
    }
}
