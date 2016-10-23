namespace DBSoft.EPM.DAL.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using DTOs;
    using EVEAPI.Crest.MarketOrder;

    public interface IUniverseService
    {
        Task<IEnumerable<RegionDTO>> ListRegions();
        List<SolarSystemDTO> ListSolarSystems();
        StationDTO GetStation(long stationId);
        SolarSystemDTO GetStationSolarSystem(long stationId);
        List<MarketAdjustedPriceDTO> GetAdjustedMarketPrices();
        IEnumerable<StationDTO> ListStations();
        void AddCitadel(Citadel citadel);
    }
}