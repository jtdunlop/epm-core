namespace DBSoft.EPM.DAL.Services
{
    using CodeFirst.Models;
    using Queries;

    public interface IItemExtensionQueryFactory
    {
        IItemExtensionQuery Create(EPMContext context, int userId);
    }
}