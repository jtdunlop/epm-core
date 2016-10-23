namespace DBSoft.EPM.DAL
{
	using System;

	// For a class to be serializable its public setter/getters must both be defined and  public
	public class DateRange
	{
		public DateRange()
		{
			_startDate = DateTime.UtcNow.AddDays(-7);
			_endDate = DateTime.UtcNow.AddDays(-1);
		}
		public DateRange(DateTime startDate, DateTime endDate)
		{
			_startDate = startDate;
			_endDate = endDate;
		}
		private DateTime _startDate;
		private DateTime _endDate;

		public DateTime StartDate
		{
			get { return _startDate.StartOfTheDay(); }
			// ReSharper disable UnusedMember.Global
			set { _startDate = value; }
			// ReSharper restore UnusedMember.Global
		}

		public DateTime EndDate
		{
			get { return _endDate.EndOfTheDay(); }
// ReSharper disable UnusedMember.Global
			set { _endDate = value; }
// ReSharper restore UnusedMember.Global
		}
	}
}
