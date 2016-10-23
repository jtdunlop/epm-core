using System.Collections.Generic;
using DBSoft.EPM.DAL.DTOs;
using DBSoft.EPM.DAL.Requests;

namespace DBSoft.EPM.DAL.Interfaces
{
    public interface IMaterialPurchaseService
    {
        /// <summary>
        /// A list of materials that need to be purchased based on sales and inventory levels
        /// </summary>
        IEnumerable<MaterialPurchaseDto> List(MaterialPurchaseRequest request);
    }
}