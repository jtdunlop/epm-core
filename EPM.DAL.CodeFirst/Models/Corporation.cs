using System;
using System.Collections.Generic;

namespace DBSoft.EPM.DAL.CodeFirst.Models
{
    public partial class Corporation
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int EveApiID { get; set; }
        public Nullable<int> AccountID { get; set; }
		public virtual Account Account { get; set; }
 
    }
}
