namespace DBSoft.EPM.Logic
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
	}
}
