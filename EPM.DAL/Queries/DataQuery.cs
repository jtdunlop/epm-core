namespace DBSoft.EPM.DAL.Queries
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Linq.Expressions;

    public interface IDataQuery<T> where T : class
    {
        IQueryable<T> GetQuery();
        DataQuery<T> Specify(Expression<Func<T, bool>> specification);
        List<T> ToList();
    }

    public class DataQuery<T> : IDataQuery<T> where T : class
	{
		protected readonly DbContext Context;
		private IQueryable<T> _query;

		public DataQuery(DbContext context)
		{
			Context = context;
			_query = Context.Set<T>();
		}
		public virtual IQueryable<T> GetQuery()
		{
			return _query;
		}
		public DataQuery<T> Specify(Expression<Func<T, bool>> specification)
		{
			_query = _query.Where(specification);
			return this;
		}

	    public List<T> ToList()
	    {
	        return _query.ToList();
	    }
	}
}
