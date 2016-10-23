namespace Dbsoft.Epm.Web.Controllers.Production.PostBuyOrders
{
    using JetBrains.Annotations;

    [UsedImplicitly]
    public class PostBuyOrderItemModel
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public decimal MarketPrice { get; set; }
        public long UsageQuantity { get; set; }
        public long InventoryQuantity { get; set; }
        public decimal Percentage { get; set; }
        public decimal RangePrice { get; set; }
        public decimal NewPrice { get; set; }
        public long PurchaseQuantity { get; set; }
        public long FactoryQuantity { get; set; }
        public long FactoryRequired { get; set; }
    }
}