namespace DBSoft.EPM.DAL.Exceptions
{
	public class ReferentialIntegrityException : DalException
	{
		public ReferentialIntegrityException(string message) : base(message)
		{}
	}
}
