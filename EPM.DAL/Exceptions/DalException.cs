using System;

namespace DBSoft.EPM.DAL.Exceptions
{
	public class DalException : Exception
	{
		protected DalException(string message) : base(message)
		{}
	}
}