namespace DBSoft.EPM.UI
{
    using System;
    using System.Linq.Expressions;
    using PresentationRules;

    public interface IColumnDefinition
	{
		string Caption { get; }
		string Format { get; }
		string Field { get; }
		GetStringDelegate GetStyle { get; }
		ColumnAlignment Alignment { get; }
		IFooterDefinition Footer { get; }
	}
	public interface IDataColumnDefinition : IColumnDefinition
	{
	}
	public interface ILinkColumnDefinition : IColumnDefinition
	{
		GetLinkDelegate GetLink { get; }
	}
	public interface IFooterDefinition
	{
		string Field { get; }
		string Format { get; }
		ColumnAlignment Alignment { get; }
	}

	public class LinkDefinition
	{
		public string Link { get; set; }
		public ColumnAlignment Alignment { get; set; }
	}
	public delegate LinkDefinition GetLinkDelegate(object rec);
	public delegate string GetStringDelegate(object rec);

	public class LinkColumnDefinition<TEntity> : ColumnDefinition<TEntity>, ILinkColumnDefinition
	{
		public LinkColumnDefinition(Expression<Func<TEntity, object>> expr, GetLinkDelegate getlink = null, 
			string caption = null, IFooterDefinition footer = null)
			: base(expr, caption, footer)
		{
			GetLink = getlink;
		}
		public GetLinkDelegate GetLink { get; set; }
	}
	public class ColumnDefinition<TEntity> : IColumnDefinition
	{
		protected ColumnDefinition(Expression<Func<TEntity, object>> expr, string caption = null,
			IFooterDefinition footer = null, GetStringDelegate style = null, GetStringDelegate @class = null)
		{
			if ( expr != null )
			{
				Field = ExpressionHelper.GetMemberName(expr.Body);
			}
			if ( Field != null )
			{
				Caption = caption ?? ColumnRules<TEntity>.Caption(Field);
				Format = ColumnRules<TEntity>.Format(Field);
				Alignment = ColumnRules<TEntity>.Alignment(Field);
			}
			GetStyle = style;
            GetClass = @class;
			Footer = footer;
		}

	    public GetStringDelegate GetClass { get; set; }

	    public string Caption { get; set; }
		public string Field { get; protected set; }
		public string Format { get; set; }
		public ColumnAlignment Alignment { get; set; }
		public IFooterDefinition Footer { get; set; }
		public GetStringDelegate GetStyle { get; set; }
	}
	public class DataColumnDefinition<TEntity> : ColumnDefinition<TEntity>, IDataColumnDefinition
	{
		// Formats: float
		public DataColumnDefinition(Expression<Func<TEntity, object>> expr, string caption = null, 
            IFooterDefinition footer = null)
			: base(expr, caption, footer)
		{
		}
	}
	public class FooterDefinition<TEntity> : IFooterDefinition
	{
		public FooterDefinition(Expression<Func<TEntity, object>> expr)
		{
			Field = ExpressionHelper.GetMemberName(expr.Body);
			Format = ColumnRules<TEntity>.Format(Field);
			Alignment = ColumnRules<TEntity>.Alignment(Field);
		}
		public string Field { get; private set; }
		public string Format { get; private set; }
		public ColumnAlignment Alignment { get; private set; }
	}
}