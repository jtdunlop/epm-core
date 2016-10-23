namespace DBSoft.EPM.DAL.Services
{
    using CodeFirst.Models;
    using JetBrains.Annotations;
    using Queries;

    [UsedImplicitly]
    public class BlueprintInstanceQueryFactory : IBlueprintInstanceQueryFactory
    {
        public IBlueprintInstanceQuery Create(EPMContext context, int userId)
        {
            return new BlueprintInstanceQuery(context, userId);
        }
    }
}