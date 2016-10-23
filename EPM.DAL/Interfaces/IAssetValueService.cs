namespace DBSoft.EPM.DAL.Interfaces
{
    using System.Collections.Generic;
    using DTOs;
    using Requests;

    public interface IAssetValueService
	{
		IEnumerable<AssetValueByStationDTO> ListMaterialsByStation(AssetValueRequest request);
		List<AssetValueByStationAndItemDTO> ListByItemAndStation(AssetValueRequest request);
	}
}