namespace DBSoft.EPM.DAL.Services.ItemCosts
{
    using System.Collections.Generic;
    using DTOs;
    using MaterialCosts;

    public interface IItemCostService
    {
        List<ItemCostDTO> ListBuildable(ListBuildableRequest request);
        IEnumerable<ItemCostVarianceDto> ListVariances(string token);
        IEnumerable<MaterialCostVarianceByItemDto> ListVariancesByItem(string token);
    }
}