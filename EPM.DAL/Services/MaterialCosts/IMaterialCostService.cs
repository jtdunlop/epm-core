namespace DBSoft.EPM.DAL.Services.MaterialCosts
{
    using System.Collections.Generic;
    using DTOs;
    using Requests;

    public interface IMaterialCostService
    {
        List<MaterialCostDTO> List(MaterialCostRequest request);
        IEnumerable<MaterialCostVarianceDto> ListVariances(string token);
    }
}