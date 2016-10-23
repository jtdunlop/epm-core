namespace DBSoft.EPM.DAL.Services
{
    using CodeFirst.Models;
    using JetBrains.Annotations;
    using Queries;

    [UsedImplicitly]
    public class ItemExtensionQueryFactory : IItemExtensionQueryFactory
    {
        public IItemExtensionQuery Create(EPMContext context, int userId)
        {
            return new ItemExtensionQuery(context, userId);
        }    
    }
}