namespace DBSoft.EPM.DAL.Services.Contracts
{
    using System.Collections.Generic;

    public interface IContractService
    {
        IEnumerable<OutboundContractDTO> ListOutboundContracts(string token);
        IEnumerable<InboundContractDTO> ListInboundContracts(string token);
    }
}