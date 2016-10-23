namespace DBSoft.UI.PresentationRules
{
	using System.ComponentModel;
	using System.ComponentModel.DataAnnotations;
	using System;

	public static class MetadataExtensions
	{
/*
		/// <summary>
		/// DisplayFormat for class properties which may be decorated
		/// </summary>
		/// <param name="instance"></param>
		/// <param name="property"></param>
		/// <returns></returns>
		public static string DisplayFormat(this object instance, string property)
		{
			return DisplayFormat(instance.GetType(), property);
		}
*/

		public static string DisplayFormat(Type t, string property)
		{
			var result = ExplicitDisplayFormat(t, property);
			if ( result == "" )
			{
				result = ImplicitDisplayFormat(ReflectionType(t, property));
			}
			return result;
		}
		/// <summary>
		/// DisplayFormat for standalone items (string, int, decimal, etc)
		/// </summary>
		/// <param name="instance"></param>
		/// <returns></returns>
		public static string DisplayFormat(this object instance)
		{
			return ImplicitDisplayFormat(instance.GetType());
		}
		private static string ExplicitDisplayFormat(Type t, string property)
		{
			var pdc = TypeDescriptor.GetProperties(t);
			var pd = pdc[property];
			var attr = pd.Attributes[typeof(DisplayFormatAttribute)];
			return attr != null ? ((DisplayFormatAttribute) attr).DataFormatString : "";
		}
		public static Type ReflectionType(Type t, string property)
		{
			Type result = null;
			var fi = t.GetProperty(property);
			if ( fi != null )
			{
				result = fi.PropertyType.IsGenericType ? Nullable.GetUnderlyingType(fi.PropertyType) : fi.PropertyType;
			}
			return result;
		}
		private static string ImplicitDisplayFormat(Type t)
		{
			if ( t.IsFloat() )
			{
				return "{0:#,0.00}";
			}
			if (t.IsInteger())
			{
				return "{0:#,0}";
			}
			if (t == typeof(DateTime))
			{
				return "{0:d}";
			}
			return "{0}";
		}
	}
	public class DefaultMetadata
	{
		[DisplayName("Profit")]
// ReSharper disable UnusedMember.Global
		public object GPAmt { get; set; }
		[DisplayName("GP%")]
		public object GPPct { get; set; }
		[DisplayName("Gross")]
		public object GrossAmount { get; set; }
		[DisplayName("Item")]
		public object ItemName { get; set; }
		[DisplayName("New Price")]
		public object NewPrice { get; set; }
		[DisplayName("My Price")]
		public object MyPrice { get; set; }
		[DisplayName("Quantity")]
		public object PurchaseQuantity { get; set; }
		[DisplayName("Bounce Factor")]
		public object BounceFactor { get; set; }
		[DisplayName("Minimum Stock")]
		public object MinimumStock { get; set; }
	}
}
