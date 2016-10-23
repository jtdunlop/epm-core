namespace Dbsoft.Epm.Web.Controllers.Production.InboundContract
{
    using System.ComponentModel.DataAnnotations;
    using JetBrains.Annotations;

    [UsedImplicitly]
    public class InboundContractItemModel
    {
        public string StationName { get; set; }
        [DisplayFormat(DataFormatString = "{0:#,###}")]
        public decimal Value { get; set; }
    }
}