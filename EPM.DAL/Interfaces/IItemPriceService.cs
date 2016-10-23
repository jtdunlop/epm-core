using System.Collections.Generic;
using DBSoft.EPM.DAL.DTOs;

namespace DBSoft.EPM.DAL.Interfaces
{
    public interface IItemPriceService
    {
        List<ItemPriceDTO> ListBuildable(string token);
    }
}