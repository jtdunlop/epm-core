namespace DBSoft.EPM.Logic
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using DAL.DTOs;

    public interface IEveApiImporter
    {
        Task Start(string token);
        IEnumerable<EveApiStatusDTO> List(string token);
    }
}