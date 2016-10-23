using System;
using System.Collections.Generic;

namespace DBSoft.EPM.DAL.CodeFirst.Models
{
    public partial class Group
    {
        public Group()
        {
            this.Items = new List<Item>();
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public Nullable<int> CategoryID { get; set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<Item> Items { get; set; }
    }
}
