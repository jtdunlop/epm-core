namespace DBSoft.UI.PresentationRules
{
	using System;

	public static class TypeExtensions
	{
		static public bool IsNumeric(this Type type)
		{
			return type.IsFloat() || type.IsInteger();
		}
		static public bool IsFloat(this Type type)
		{
			return type == typeof(double) || type == typeof(decimal);
		}
		static public bool IsInteger(this Type type)
		{
			return type == typeof(int) || type == typeof(long);
		}
	}
}
