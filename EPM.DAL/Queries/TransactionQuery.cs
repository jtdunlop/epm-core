namespace DBSoft.EPM.DAL.Queries
{
	using System.Data.Entity;
	using CodeFirst.Models;

	public class TransactionQuery : DataQuery<Transaction>
	{
		public TransactionQuery(DbContext context)
			: base(context)
		{
		}

	    public TransactionQuery SpecifyVisible()
	    {
	        Specify(f => f.VisibleFlag);
	        return this;
	    }
	}
}
