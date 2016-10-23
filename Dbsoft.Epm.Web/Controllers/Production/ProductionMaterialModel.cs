namespace Dbsoft.Epm.Web.Controllers.Production
{
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using DBSoft.EPM.DAL.DTOs;
    using DBSoft.EPM.DAL.Interfaces;
    using JetBrains.Annotations;

    [UsedImplicitly]
    public class ProductionMaterialItemModel
	{
		public int ItemId { get; set; }
		public string ItemName { get; set; }
		public long Needed { get; set; }
		public long Available { get; set; }
        public decimal BounceFactor { get; set; }
	}

	public class ProductionMaterialModel 
	{
		public ProductionMaterialModel(IProductionMaterialService service, string token, string returnUrl)
		{
			Detail = Mapper.Map<IEnumerable<ProductionMaterialDto>, IEnumerable<ProductionMaterialItemModel>>(service.List(token)
				.OrderBy(f => f.ItemName));
			ReturnUrl = returnUrl;
		}

	    public IEnumerable<ProductionMaterialItemModel> Detail { get; private set; }

		public string HelpUrl => "https://dbsoft.atlassian.net/wiki/display/EPM/Material";

	    private string ReturnUrl { get; set; }
	}
}