namespace Dbsoft.Epm.Web.Controllers.Production
{
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using DBSoft.EPM.DAL.DTOs;
    using DBSoft.EPM.DAL.Interfaces;

    public class BuildableItemListItemModel
	{
		public int ItemID { get; set; }
		public string ItemName { get; set; }
		public int? MinimumStock { get; set; }
        public decimal? PerJobAdditionalCost { get; set; }
	}

	public class BuildableItemListModel
	{
		public BuildableItemListModel(IItemService service, string token, string returnUrl)
		{
            Detail = Mapper.Map<IEnumerable<BuildableItemDTO>, IEnumerable<BuildableItemListItemModel>>(service.ListMaintainable(token).OrderBy(f => f.ItemName));
			ReturnUrl = returnUrl;
		}
		public IEnumerable<BuildableItemListItemModel> Detail { get; set; }
		public string ReturnUrl{get;set;}
	}
}