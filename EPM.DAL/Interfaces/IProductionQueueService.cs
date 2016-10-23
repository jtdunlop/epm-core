namespace DBSoft.EPM.DAL.Interfaces
{
    using System.Collections.Generic;
    using DTOs;

    public interface IProductionQueueService
    {
        IEnumerable<ProductionQueueDto> List(string token);
    }
}