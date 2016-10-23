namespace Dbsoft.Epm.Web.Controllers.Production.UpdateSellOrders
{
    using System;
    using System.ComponentModel;
    using JetBrains.Annotations;

    [UsedImplicitly]
    public class UpdateSellOrderItemModel
    {
        public string ItemName { get; set; }
        public decimal NewPrice { get; set; }
        [DisplayName("Market Price")]
        public decimal MarketPrice { get; set; }
        [DisplayName("My Price")]
        public decimal ListedPrice { get; set; }
        public decimal Markup { get; set; }
        public DateTime Timestamp { get; set; }
    }
}