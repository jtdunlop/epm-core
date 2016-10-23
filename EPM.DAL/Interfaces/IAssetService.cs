namespace DBSoft.EPM.DAL.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using DTOs;
    using Requests;

    public interface IAssetService
    {
        IEnumerable<AssetByItemAndStationDTO> ListByItemAndStation(AssetServiceRequest request);
        List<AssetByItemDTO> ListByItem(AssetServiceRequest request);
        Task UpdateAssets(List<SaveAssetRequest> requests);
    }
}