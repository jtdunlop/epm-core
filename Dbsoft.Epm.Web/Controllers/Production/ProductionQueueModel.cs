namespace Dbsoft.Epm.Web.Controllers.Production
{
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using DBSoft.EPM.DAL.DTOs;
    using DBSoft.EPM.DAL.Interfaces;

    public class ProductionQueueModel
    {
		public ProductionQueueModel(IProductionQueueService service, string token)
		{
            Detail = Mapper.Map<IEnumerable<ProductionQueueDto>, IEnumerable<ProductionQueueItemModel>>(service
				.List(token)
				.OrderByDescending(f => f.ProfitFactor)
                .ThenByDescending(f => f.HourlyProfit));
		}

		public static string HelpUrl => "https://dbsoft.atlassian.net/wiki/display/EPM/Build+Items";
        public IEnumerable<ProductionQueueItemModel> Detail { get; private set; }
	}
}
