namespace DBSoft.EPM.Logic
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using DAL.DTOs;
    using DAL.Interfaces;

    public interface IImportManager
    {
        Task StartImport(string token);
        IEnumerable<EveApiStatusDTO> List(string token);
    }

    public class ImportManager : IImportManager
    {
        private readonly IEveApiImporter _importer;
        private readonly IEveApiStatusService _status;
        private readonly Dictionary<string, IEveApiImporter> _instances; 

        public ImportManager(IEveApiImporter importer, IEveApiStatusService status)
        {
            _importer = importer;
            _status = status;
            _instances = new Dictionary<string, IEveApiImporter>();
        }

        public async Task StartImport(string token)
        {
            if (_instances.ContainsKey(token)) return;
            _instances[token] = _importer;
            await _importer.Start(token);
            _instances.Remove(token);
        }

        public IEnumerable<EveApiStatusDTO> List(string token)
        {
            var result = new List<EveApiStatusDTO>();
            if ( _instances.ContainsKey(token) )
            {
                result.AddRange(_instances[token].List(token));
            }
            var more = _status.List(token);
            result.AddRange(more.Where(f => result.All(g => g.ApiName != f.ApiName)));
            return result;
        }
    }
}
