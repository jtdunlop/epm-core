using System;
using System.Collections.Generic;
using DBSoft.EPM.DAL.DTOs;

namespace DBSoft.EPM.DAL.Interfaces
{
    public interface IEveApiStatusService
    {
        IEnumerable<EveApiStatusDTO> List(string token);
        void UpdateStatus(string token, string name, string result, DateTime? cachedUntil);
    }
}