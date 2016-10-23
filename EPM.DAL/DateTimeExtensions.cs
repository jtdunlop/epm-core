namespace DBSoft.EPM.DAL
{
	using System;

	public static class DateTimeExtensions
	{
		static public DateTime StartOfTheDay(this DateTime dt)
		{
			return dt.Date;
		}

		static public DateTime EndOfTheDay(this DateTime dt)
		{
			return dt.Date.AddDays(1).AddTicks(-1);
		}

		static public DateTime StartOfTheMonth(this DateTime dt)
		{
			return new DateTime(dt.Year, dt.Month, 1);
		}

		static public DateTime EndOfTheMonth(this DateTime dt)
		{
			return dt.StartOfTheMonth().AddMonths(1).AddTicks(-1);
		}
	}
}
