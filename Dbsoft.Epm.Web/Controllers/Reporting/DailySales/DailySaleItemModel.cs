namespace Dbsoft.Epm.Web.Controllers.Reporting.DailySales
{
    using System;
    using JetBrains.Annotations;

    [UsedImplicitly]
    public class DailySaleItemModel
	{
		public DateTime DateTime { get; set; }
		public decimal GrossAmount { get; set; }
		public decimal GpAmt { get; set; }
		public decimal GpPct { get; set; }
	}
}