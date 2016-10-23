namespace Dbsoft.Epm.Web.Controllers.Production.PostSellOrders
{
    using System;
    using System.ComponentModel;
    using JetBrains.Annotations;

    [UsedImplicitly]
    public class PostSellOrdersItemModel
    {
        public string ItemName { get; set; }
        public decimal NewPrice { get; set; }
        [DisplayName("Market Price")]
        public decimal MarketPrice { get; set; }
        public decimal Markup { get; set; }
        public DateTime Timestamp { get; set; }
    }
}