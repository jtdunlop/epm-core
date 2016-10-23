using System;
using System.Collections.Generic;

namespace DBSoft.EPM.DAL.CodeFirst.Models
{
    public partial class Character
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int AccountID { get; set; }
        public int CorporationID { get; set; }
        public int EveApiID { get; set; }
        public virtual Account Account { get; set; }
    }
}
