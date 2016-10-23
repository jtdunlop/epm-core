namespace DBSoft.EPM.DAL.Services.Materials
{
    using System.Collections.Generic;
    using DTOs;

    public interface IMaterialItemService
	{
		List<MaterialItemDto> ListBuildable(string token);
	}
}