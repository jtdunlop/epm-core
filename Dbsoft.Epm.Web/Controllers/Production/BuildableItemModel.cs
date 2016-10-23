namespace Dbsoft.Epm.Web.Controllers.Production
{
    using System.ComponentModel;

    public class BuildableItemModel
	{
		public BuildableItemModel()
		{
			ReturnUrl = "Index";
		}
		public int ItemId { get; set; }
		[DisplayName("Minimum Stock")]
		public int? MinimumStock { get; set; }
        public decimal? PerJobAdditionalCost { get; set; }
		public string ReturnUrl { get; set; }
	}
}