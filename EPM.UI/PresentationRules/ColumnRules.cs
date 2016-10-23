namespace DBSoft.EPM.UI.PresentationRules
{
    using System;
    using System.ComponentModel;
    using System.Linq.Expressions;
    using System.Text.RegularExpressions;
    using DBSoft.UI.PresentationRules;
    using UI;

    public enum ColumnAlignment
	{
		Left, 
		Right
	}
	public static class ColumnRules<TEntity>
	{
		public static string GetFormattedString(TEntity instance, Expression<Func<TEntity, object>> expr)
		{
			var property = ExpressionHelper.GetMemberName(expr);
			return GetFormattedString(instance, property);
		}

		public static string GetFormattedString(TEntity instance, string property)
		{
			var val = instance.GetType().GetProperty(property).GetValue(instance, null);
			var format = MetadataExtensions.DisplayFormat(typeof(TEntity), property);
			return string.Format(format, val);
		}
		public static string DefaultCaption(string property)
		{
			var pdc = TypeDescriptor.GetProperties(typeof(DefaultMetadata));
			var pd = pdc[property];
			if ( pd != null )
			{
				var attr = pd.Attributes[typeof(DisplayNameAttribute)];
				if ( !attr.IsDefaultAttribute() )
				{
					return ((DisplayNameAttribute) attr).DisplayName;
				}
			}
			return ToFriendlyName(property);
		}

		public static string ToFriendlyName(string property)
		{
			return new Regex("([A-Z]+[a-z]+)").Replace(property, m => (m.Value) + " ");
		}

		public static string Caption(string property)
		{
			var pdc = TypeDescriptor.GetProperties(typeof(TEntity));
			var pd = pdc[property];
			var attr = pd.Attributes[typeof(DisplayNameAttribute)];
			if ( !attr.IsDefaultAttribute() )
			{
				return ((DisplayNameAttribute) attr).DisplayName;
			}
			return DefaultCaption(property);
		}
		public static string Format(string property)
		{
			return MetadataExtensions.DisplayFormat(typeof(TEntity), property);
		}
		public static ColumnAlignment Alignment(string property)
		{
			Type vt = MetadataExtensions.ReflectionType(typeof(TEntity), property);
			if ( vt.IsNumeric() )
			{
				return ColumnAlignment.Right;
			}
			return ColumnAlignment.Left;
		}
	}
}
