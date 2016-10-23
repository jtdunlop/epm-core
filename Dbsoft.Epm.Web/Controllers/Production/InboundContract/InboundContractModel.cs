namespace Dbsoft.Epm.Web.Controllers.Production.InboundContract
{
    using System.Collections.Generic;
    using AutoMapper;
    using DBSoft.EPM.DAL.Services.Contracts;
    using DBSoft.EPM.UI;
    using Infrastructure.Html;

    public class InboundContractModel
	{
		private readonly TableDefinition _tableDef;

		public InboundContractModel(IContractService service, string token)
		{
			_detail = Mapper.Map<IEnumerable<InboundContractDTO>, IEnumerable<InboundContractItemModel>>(service.ListInboundContracts(token));
			_tableDef = new TableDefinition();
			_tableDef.Columns.Add(new DataColumnDefinition<InboundContractItemModel>(f => f.StationName));
			_tableDef.Columns.Add(new DataColumnDefinition<InboundContractItemModel>(f => f.Value));
		}

		public string TableHtml
		{
			get
			{
				return TableHtmlGenerator.Generate(_tableDef, _detail);
			}
		}

		private readonly IEnumerable<InboundContractItemModel> _detail;
	}
}