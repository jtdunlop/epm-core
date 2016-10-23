namespace Dbsoft.Epm.Web.Controllers.Production.UpdateBuyOrders
{
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using DBSoft.EPM.DAL.DTOs;
    using DBSoft.EPM.DAL.Interfaces;
    using DBSoft.EPM.DAL.Requests;
    using DBSoft.EPM.UI;
    using Infrastructure.Html;

    public class UpdateBuyOrderModel
    {
        private readonly TableDefinition _tableDef;
        private readonly IEnumerable<UpdateBuyOrderItemModel> _detail;

        public UpdateBuyOrderModel(IMaterialPurchaseService service, string token)
        {
            _detail = Mapper.Map<IEnumerable<MaterialPurchaseDto>, IEnumerable<UpdateBuyOrderItemModel>>(service.List(new MaterialPurchaseRequest
                {
                    Token = token,
                    IncludeMaterialsWithActiveOrders = true
                }))
                .Where(f => f.Percentage < 100)
                .OrderBy(f => f.ItemName);

            _tableDef = new TableDefinition
            {
                Columns = new List<IColumnDefinition>
                {
                    new DataColumnDefinition<UpdateBuyOrderItemModel>(f => f.ItemName),
                    new DataColumnDefinition<UpdateBuyOrderItemModel>(f => f.NewPrice),
                    new DataColumnDefinition<UpdateBuyOrderItemModel>(f => f.MarketPrice),
                    new DataColumnDefinition<UpdateBuyOrderItemModel>(f => f.RangePrice),
                    new DataColumnDefinition<UpdateBuyOrderItemModel>(f => f.UsageQuantity),
                    new DataColumnDefinition<UpdateBuyOrderItemModel>(f => f.InventoryQuantity),
                    new DataColumnDefinition<UpdateBuyOrderItemModel>(f => f.FactoryQuantity),
                    new DataColumnDefinition<UpdateBuyOrderItemModel>(f => f.FactoryRequired),
                    new DataColumnDefinition<UpdateBuyOrderItemModel>(f => f.Percentage),
                }
            };
        }

        public string TableHtml => TableHtmlGenerator.Generate(_tableDef, _detail);
    }
}