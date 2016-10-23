namespace Dbsoft.Epm.Web.Controllers.Production.PostSellOrders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using Infrastructure.Html;
    using DBSoft.EPM.DAL.DTOs;
    using DBSoft.EPM.DAL.Interfaces;
    using DBSoft.EPM.UI;

    public class PostSellOrdersModel
	{
		public PostSellOrdersModel(IMarketRestockService restockService, string token)
		{
			Detail = Mapper.Map<IEnumerable<MarketRestockDTO>, IEnumerable<PostSellOrdersItemModel>>(restockService.List(token)
                .OrderBy(o => o.ItemName));
			Table.Columns.Add(new DataColumnDefinition<PostSellOrdersItemModel>(f => f.ItemName));
			Table.Columns.Add(new DataColumnDefinition<PostSellOrdersItemModel>(f => f.NewPrice));
			Table.Columns.Add(new DataColumnDefinition<PostSellOrdersItemModel>(f => f.MarketPrice));
			Table.Columns.Add(new DataColumnDefinition<PostSellOrdersItemModel>(f => f.Markup));
		}

        private static string GetClass(object rec)
        {
            var model = rec as PostSellOrdersItemModel;
            if ( model != null && model.Timestamp < DateTime.UtcNow.AddHours(-4) )
            {
                return "class=\"error\"";
            }
            return "";
        }

        public IEnumerable<PostSellOrdersItemModel> Detail { get; private set; }
        public readonly TableDefinition Table = new TableDefinition
        {
            GetClass = GetClass
        };
	}
}
