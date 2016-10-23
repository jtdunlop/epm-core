namespace DBSoft.EPM.DAL.Interfaces
{
    using System.Collections.Generic;
    using DTOs;
    using Requests;

    public interface ILocationService
    {
        IEnumerable<StationDTO> ListStations(StationListRequest request);
    }
}