namespace DBSoft.EPM.DAL.Services.Market
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IMarketResearchService
    {
        List<MarketResearchDTO> List(string token);
        Task Update(int stationId);
    }
}