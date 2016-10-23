using System;
using System.Collections.Generic;

namespace DBSoft.EPM.DAL.CodeFirst.Models
{
    public partial class SchemaVersion
    {
        public int Id { get; set; }
        public string ScriptName { get; set; }
        public System.DateTime Applied { get; set; }
    }
}
