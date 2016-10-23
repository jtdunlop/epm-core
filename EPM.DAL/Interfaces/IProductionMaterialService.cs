namespace DBSoft.EPM.DAL.Interfaces
{
    using System.Collections.Generic;
    using DTOs;

    public interface IProductionMaterialService
    {
        IEnumerable<ProductionMaterialDto> List(string token);
    }
}