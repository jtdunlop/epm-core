using System;
using System.Collections.Generic;

namespace DBSoft.EPM.DAL.CodeFirst.Models
{
    public partial class IpAddressBlacklist
    {
        public int ID { get; set; }
        public string IpAddress { get; set; }
        public int Count { get; set; }
    }
}
