namespace Dbsoft.Epm.Web.Infrastructure.Html
{
    using System.Collections.Generic;
    using DBSoft.EPM.UI;

    public class TableDefinition
	{
		public TableDefinition(bool displayHeader = true)
		{
			Columns = new List<IColumnDefinition>();
			DisplayHeader = displayHeader;
		}
		public GetStringDelegate GetStyle { get; set; }
        public GetStringDelegate GetClass { get; set; }
        public List<IColumnDefinition> Columns { get; set; }
		public bool DisplayHeader { get; private set; }
	}
}