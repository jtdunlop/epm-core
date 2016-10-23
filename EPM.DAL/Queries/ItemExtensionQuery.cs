namespace DBSoft.EPM.DAL.Queries
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Linq.Expressions;
    using CodeFirst.Models;

    public interface IItemExtensionQuery
    {
        IQueryable<ItemExtension> GetQuery();
        DataQuery<ItemExtension> Specify(Expression<Func<ItemExtension, bool>> specification);
        List<ItemExtension> ToList();
    }

    public class ItemExtensionQuery : DataQuery<ItemExtension>, IItemExtensionQuery
    {
        public ItemExtensionQuery(DbContext context, int userId)
            : base(context)
        {
            Specify(f => f.UserID == userId);
            Specify(f => f.MinimumStock > 0);
        }
    }
}