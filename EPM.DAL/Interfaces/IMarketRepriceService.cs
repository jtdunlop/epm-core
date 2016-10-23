using System.Collections.Generic;
using DBSoft.EPM.DAL.DTOs;
using DBSoft.EPM.DAL.Requests;

namespace DBSoft.EPM.DAL.Interfaces
{
    public interface IMarketRepriceService
    {
        IEnumerable<MarketRepriceDTO> List(MarketRepriceRequest request);
    }
}