namespace DBSoft.EPM.DAL
{
	using System;

	public static class DecimalExtensions
	{
		public static decimal SignificantFigures(this decimal d, int digits)
		{
			if (d == 0)
			{
				return d;
			}
			var num = Convert.ToDouble(d);
			var size = Math.Floor(Math.Log10(Math.Abs(num))) + 1;
			var scale = digits - size;
			var result = Math.Floor(num * Math.Pow(10, scale)) * Math.Pow(10, -scale);
			return Convert.ToDecimal(result);
		}
	}
}
