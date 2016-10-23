
namespace Dbsoft.Epm.Web.Controllers.Production.PostBuyOrders
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using AutoMapper;
    using DBSoft.EPM.DAL.DTOs;
    using DBSoft.EPM.DAL.Interfaces;
    using DBSoft.EPM.DAL.Requests;
    using DBSoft.EPM.UI;
    using Infrastructure.Html;

    public class PostBuyOrderModel
    {
        private readonly TableDefinition _tableDef;
        private readonly IEnumerable<PostBuyOrderItemModel> _detail;

        public PostBuyOrderModel(IMaterialPurchaseService service, string token, bool igb)
        {
            _detail = Mapper.Map<IEnumerable<MaterialPurchaseDto>, IEnumerable<PostBuyOrderItemModel>>(service.List(new MaterialPurchaseRequest
            {
                Token = token,
                IncludeMaterialsWithActiveOrders = false
            }))
                .Where(f => f.Percentage < 100)
                .OrderBy(f => f.Percentage);
            _tableDef = new TableDefinition
            {
                Columns = new List<IColumnDefinition>
				{
                    igb ? (IColumnDefinition) new LinkColumnDefinition<PostBuyOrderItemModel>(f => f.ItemName, CreateLink) : 
                        new DataColumnDefinition<PostBuyOrderItemModel>(f => f.ItemName), 
					new DataColumnDefinition<PostBuyOrderItemModel>(f => f.NewPrice),
					new DataColumnDefinition<PostBuyOrderItemModel>(f => f.MarketPrice),
					new DataColumnDefinition<PostBuyOrderItemModel>(f => f.RangePrice),
					new DataColumnDefinition<PostBuyOrderItemModel>(f => f.PurchaseQuantity),
					new DataColumnDefinition<PostBuyOrderItemModel>(f => f.UsageQuantity),
					new DataColumnDefinition<PostBuyOrderItemModel>(f => f.InventoryQuantity),
					new DataColumnDefinition<PostBuyOrderItemModel>(f => f.FactoryQuantity),
					new DataColumnDefinition<PostBuyOrderItemModel>(f => f.FactoryRequired),
					new DataColumnDefinition<PostBuyOrderItemModel>(f => f.Percentage)
				}
            };
        }

        private static LinkDefinition CreateLink(object rec)
        {
            var pmi = rec as PostBuyOrderItemModel;
            Debug.Assert(pmi != null, "dsd != null");

            var linkDef = new LinkDefinition
            {
                Link = string.Format("<a href=javascript:CCPEVE.buyType({1})>{0}</a>", pmi.ItemName, pmi.ItemId)
            };
            return linkDef;
        }

        public string TableHtml => TableHtmlGenerator.Generate(_tableDef, _detail);

        public static string HelpUrl => "https://dbsoft.atlassian.net/wiki/display/EPM/Post+Buy+Orders";
    }
}