namespace DBSoft.EPM.DAL.Services.Market
{
    using System.Collections.Generic;
    using DTOs;
    using Requests;

    public interface IMarketPriceService
	{
		List<MarketPriceDTO> List(MarketPriceRequest request);
	}
}