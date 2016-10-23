 namespace DBSoft.EPM.DAL.Commands
{
	using System;
	using System.Data.Entity;
	using System.Data.Entity.Infrastructure;
	using System.Linq;
	using System.Linq.Expressions;

    public sealed class DataCommand : IDisposable 
	{
		private readonly DbContext _context;

		public DataCommand(DbContext context) 
		{
			_context = context;
		}

		public TGet Get<TGet>(object pk) where TGet : class
		{
			var expr = MakeExpression<TGet>(pk);
			var result = _context.Set<TGet>().Where(expr).Single();
			return result;
		}

		public TGet Get<TGet>(Expression<Func<TGet, bool>> expr) where TGet: class, new()
		{
            var tracking = _context.Set<TGet>().Local.SingleOrDefault(expr.Compile());
            if ( tracking != null )
            {
                return tracking;
            }
			var v = _context.Set<TGet>().Where(expr).SingleOrDefault();
            return v ?? _context.Set<TGet>().Add(new TGet());
		}

		public void Remove<TRemove>(object pk) where TRemove : class
		{
			_context.Set<TRemove>().Remove(Get<TRemove>(pk));
		}

		private string GetIdentityColumn<T>() where T : class
		{
			var objectSet = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<T>();
			var result = objectSet.EntitySet.ElementType.KeyMembers.Select(k => k.Name).First();
			return result;
		}
		private Expression<Func<TEntity, bool>> MakeExpression<TEntity>(object pk) where TEntity: class
		{
			var id = GetIdentityColumn<TEntity>();
			var member = typeof(TEntity).GetProperty(id);
			var param = Expression.Parameter(typeof(TEntity), "f");
			var left = Expression.MakeMemberAccess(param, member);
			var right = Expression.Constant(pk);
			var body = Expression.Equal(left, right);
			var lambda = Expression.Lambda<Func<TEntity, bool>>(body, param);
			return lambda;
		}

		public void Dispose()
		{
			_context.SaveChangesWithErrors();
		}
	}
}
