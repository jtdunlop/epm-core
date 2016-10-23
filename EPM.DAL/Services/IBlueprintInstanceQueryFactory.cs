namespace DBSoft.EPM.DAL.Services
{
    using CodeFirst.Models;
    using Queries;

    public interface IBlueprintInstanceQueryFactory
    {
        IBlueprintInstanceQuery Create(EPMContext context, int userId);
    }
}