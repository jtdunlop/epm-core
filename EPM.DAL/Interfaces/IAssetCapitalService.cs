namespace DBSoft.EPM.DAL.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using DTOs;

    public interface IAssetCapitalService
	{
		IEnumerable<MaterialCapitalDTO> ListMaterialCapital(string token);
		IEnumerable<ItemCapitalDTO> ListItemCapital(string token);
	}
}