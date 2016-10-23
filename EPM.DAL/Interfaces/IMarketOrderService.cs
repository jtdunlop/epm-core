namespace DBSoft.EPM.DAL.Interfaces
{
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using DTOs;
	using Requests;
	using Services.Market;

    public interface IMarketOrderService
	{
		List<MarketOrderDTO> List(MarketOrderRequest request);

		void SaveOrder(SaveMarketOrderRequest request);
		Task DeleteAll(string token);
	}
}