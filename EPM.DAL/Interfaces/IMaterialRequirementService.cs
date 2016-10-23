namespace DBSoft.EPM.DAL.Interfaces
{
    using System.Collections.Generic;
    using DTOs;

    public interface IMaterialRequirementService
    {
        IEnumerable<MaterialRequirementDto> ListBuildable(string token);
    }
}