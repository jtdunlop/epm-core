using System;
using System.Collections.Generic;

namespace DBSoft.EPM.DAL.CodeFirst.Models
{
    public partial class Category
    {
        public Category()
        {
            this.Groups = new List<Group>();
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Group> Groups { get; set; }
    }
}
