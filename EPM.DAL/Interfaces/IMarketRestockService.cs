using System.Collections.Generic;
using DBSoft.EPM.DAL.DTOs;

namespace DBSoft.EPM.DAL.Interfaces
{
    public interface IMarketRestockService
    {
        IEnumerable<MarketRestockDTO> List(string token);
    }
}