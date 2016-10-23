namespace Dbsoft.Epm.Web.Controllers.Production.UpdateSellOrders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using Infrastructure.Html;
    using DBSoft.EPM.DAL.CodeFirst.Models;
    using DBSoft.EPM.DAL.DTOs;
    using DBSoft.EPM.DAL.Interfaces;
    using DBSoft.EPM.DAL.Requests;
    using DBSoft.EPM.UI;

    public class UpdateSellOrderModel
	{
		public UpdateSellOrderModel(IMarketRepriceService service, string token)
		{
			Detail = Mapper.Map<IEnumerable<MarketRepriceDTO>, IEnumerable<UpdateSellOrderItemModel>>(service.List(new MarketRepriceRequest 
			{ 
				Token = token,
				OrderType = OrderType.Sell 
			})
				.Where(f => f.ListedPrice != f.NewPrice)
				.OrderBy(f => f.ItemName));

			_tableDef.Columns.Add(new DataColumnDefinition<UpdateSellOrderItemModel>(f => f.ItemName));
			_tableDef.Columns.Add(new DataColumnDefinition<UpdateSellOrderItemModel>(f => f.NewPrice));
			_tableDef.Columns.Add(new DataColumnDefinition<UpdateSellOrderItemModel>(f => f.MarketPrice));
			_tableDef.Columns.Add(new DataColumnDefinition<UpdateSellOrderItemModel>(f => f.ListedPrice));
			_tableDef.Columns.Add(new DataColumnDefinition<UpdateSellOrderItemModel>(f => f.Markup));
		}

	    private static string GetClass(object rec)
	    {
	        var model = rec as UpdateSellOrderItemModel;
            if ( model != null && model.Timestamp < DateTime.UtcNow.AddHours(-4))
            {
                return "class='error'";
            }
            return "";
	    }

	    public string TableHtml
		{
			get
			{
				return TableHtmlGenerator.Generate(_tableDef, Detail);
			}
		}
        private readonly TableDefinition _tableDef = new TableDefinition
        {
            GetClass = GetClass
        };
		private IEnumerable<UpdateSellOrderItemModel> Detail {get;set;}

		public static string HelpUrl
		{
			get { return "https://dbsoft.atlassian.net/wiki/display/EPM/Update+Sell+Orders"; }
		}
	}
}