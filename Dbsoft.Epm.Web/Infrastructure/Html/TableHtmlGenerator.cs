using Microsoft.AspNetCore.Mvc.Rendering;

namespace Dbsoft.Epm.Web.Infrastructure.Html
{
    using System.Collections.Generic;
    using System.Linq;
    using DBSoft.EPM.UI;
    using DBSoft.EPM.UI.PresentationRules;

    public class TableModel
	{
		public TableDefinition TableDef { get; set; }
		public IEnumerable<object> Detail { get; set; }
		public object Totals { get; set; }
	}

	public static class TableHtmlGenerator
	{
		static public string Generate(TableDefinition tableDef, IEnumerable<object> det, object totals = null)
		{
			var header = GetHeader(tableDef);
			var detail = GetDetail(tableDef, det);
			var footer = GetFooter(tableDef, totals);
			var html = string.Format("<table class=\"table table-striped table-bordered table-condensed\">{0}{1}{2}</table>", header, detail, footer);
			return html;
		}
		static private string GetHeader(TableDefinition tableDef)
		{
			if (!tableDef.DisplayHeader)
			{
				return "";
			}
			var header = new TagBuilder("tr");
			foreach ( var def in tableDef.Columns )
			{
				var title = new TagBuilder("th");
				title.MergeAttribute("style", GetAlignment(def.Alignment));
				//title.InnerHtml = def.Caption;
				//header.InnerHtml += title.ToString();
			}
			return header.ToString();
		}
		static private string GetStyle(TableDefinition def, object rec)
		{
		    return def.GetStyle != null ? def.GetStyle.Invoke(rec) : "";
		}

        private static string GetClass(TableDefinition def, object rec)
        {
            return def.GetClass != null ? def.GetClass.Invoke(rec) : "";
        }

        static private string GetStyle(IColumnDefinition def, object rec)
		{
			if ( def.GetStyle != null )
			{
				return def.GetStyle.Invoke(rec);
			}
			return "";
		}
		static private string GetCell(IColumnDefinition def, object rec)
		{
			var td = new TagBuilder("td");
			var styles = new List<string>();
			var style = GetStyle(def, rec);
			if (!string.IsNullOrEmpty(style))
			{
				styles.Add(style);
			}

			var linkdef = def as ILinkColumnDefinition;
			if ( linkdef != null )
			{
				LinkDefinition link = linkdef.GetLink.Invoke(rec);
				if ( link != null )
				{
					styles.Add(GetAlignment(link.Alignment));
					// td.InnerHtml = link.Link;
				}
			}

			var datadef = def as IDataColumnDefinition;
			if ( datadef != null )
			{
				styles.Add(GetAlignment(def.Alignment));
				//td.InnerHtml = string.Format(datadef.Format, 
				//	rec.GetType().GetProperty(datadef.Field).GetValue(rec, null));
			}

			td.MergeAttribute("style", string.Join(";", styles.ToArray()));
			return td.ToString();
		}
		static private string GetDetail(TableDefinition tableDef, IEnumerable<object> det)
		{
			return (from rec in det 
                    let cells = tableDef.Columns.Aggregate("", (current, def) => current + GetCell(def, rec)) 
                    let style = GetStyle(tableDef, rec) 
                    let @class = GetClass(tableDef, rec)
                    select string.Format("<tr {1} {2}>{0}</tr>", cells, style, @class))
                    .Aggregate("", (current1, line) => current1 + line);
		}

	    static private string GetFooter(TableDefinition tableDef, object totals)
		{
			var footer = "";
	        if (totals == null) return footer;
	        var footercells = "";
	        foreach ( var def in tableDef.Columns )
	        {
	            if ( def.Footer != null )
	            {
	                var v = totals.GetType().GetProperty(def.Footer.Field).GetValue(totals, null);
	                var s = string.Format(def.Footer.Format, v);
	                if ( s == "" )
	                {
	                    s = v == null ? "" : v.ToString();
	                }
                    var th = new TagBuilder("th");
                    th.AddCssClass(def.Footer.Alignment == ColumnAlignment.Right ? "display-numeric" : "display-text");
                    // th.SetInnerText(s);

                    var footercell = th.ToString();
	                footercells += footercell;
	            }
	            else
	            {
	                footercells += "<th/>";
	            }
	        }
	        footer = string.Format("<tr>{0}</tr>", footercells);
	        return footer;
		}
		private static string GetAlignment(ColumnAlignment ca)
		{
			var result = string.Format("text-align: {0}", ca == ColumnAlignment.Right ? "right" : "left");
			return result;
		}
	}
}