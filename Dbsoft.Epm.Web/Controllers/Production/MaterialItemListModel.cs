namespace Dbsoft.Epm.Web.Controllers.Production
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using DBSoft.EPM.DAL.DTOs;
    using DBSoft.EPM.DAL.Interfaces;
    using DBSoft.EPM.DAL.Services.Materials;

    public class MaterialItemListItem
	{
		public int ItemID { get; set; }
		public string ItemName { get; set; }
		public decimal? BounceFactor { get; set; }
        public DateTime? LastModified { get; set; }
	}

	public class MaterialItemListModel
	{
		public MaterialItemListModel(IMaterialItemService service, string token, string returnUrl)
		{
			Detail = Mapper.Map<IEnumerable<MaterialItemDto>, IEnumerable<MaterialItemListItem>>(service.ListBuildable(token)).OrderBy(f => f.ItemName);
			ReturnUrl = returnUrl;
		}

		public IEnumerable<MaterialItemListItem> Detail { get; set; }
		public string ReturnUrl{get;set;}
	}
}