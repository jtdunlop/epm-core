namespace Dbsoft.Epm.Web.Controllers.Production.OutboundContract
{
    using System.Linq;
    using Infrastructure.Html;
    using DBSoft.EPM.DAL.DTOs;
    using DBSoft.EPM.DAL.Services.Contracts;
    using DBSoft.EPM.UI;

    public class OutboundContractModel
    {
		private readonly IOrderedEnumerable<OutboundContractDTO> _detail;
		private readonly TableDefinition _tableDef;

		public OutboundContractModel(IContractService service, string token)
		{
			_detail = service.ListOutboundContracts(token)
				.OrderBy(f => f.ItemName);
			_tableDef = new TableDefinition();
			_tableDef.Columns.Add(new DataColumnDefinition<AssetByItemDTO>(f => f.ItemName));

		}

		public string TableHtml => TableHtmlGenerator.Generate(_tableDef, _detail);
    }
}
