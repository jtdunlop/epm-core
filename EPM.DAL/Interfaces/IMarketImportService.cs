namespace DBSoft.EPM.DAL.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Requests;

    public interface IMarketImportService
    {
        Task SaveMarketImports(string token, DateTime utcNow, List<SaveMarketImportRequest> requests);
    }
}