using System;
using System.Collections.Generic;

namespace DBSoft.EPM.DAL.CodeFirst.Models
{
    public partial class AccountBalance
    {
        public int ID { get; set; }
        public int AccountID { get; set; }
        public int AccountKey { get; set; }
        public decimal Balance { get; set; }
        public virtual Account Account { get; set; }
    }
}
