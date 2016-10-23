namespace DBSoft.EPM.DAL.Services.Materials
{
    using System.Collections.Generic;
    using DTOs;

    public interface IBuildMaterialService
	{
		List<BuildMaterialDto> ListBuildable(string token);
        List<BuildMaterialDto> ListProducible(string token);
	}
}