using System;

namespace DBSoft.EPM.DAL.CodeFirst.Models
{
    public partial class ApplicationLog
    {
        public long ID { get; set; }
        public string Origin { get; set; }
        public string LogLevel { get; set; }
        public string Message { get; set; }
        public string Exception { get; set; }
        public string StackTrace { get; set; }
        public DateTime CreateDate { get; set; }
        public string Application { get; set; }
    }
}
