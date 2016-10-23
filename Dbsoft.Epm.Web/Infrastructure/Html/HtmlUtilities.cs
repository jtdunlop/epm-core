using Microsoft.AspNetCore.Routing;

namespace Dbsoft.Epm.Web.Infrastructure.Html
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using DBSoft.EPM.UI;
    using DBSoft.EPM.UI.PresentationRules;

    public static class HtmlUtilities
    {
        public static LinkDefinition CreateLinkDefinition<T>(object rec, Expression<Func<object>> property, string action, string controller, object routeValues,
            IDictionary<string, object>  htmlAttributes = null) where T : class
        {
            var prop = GetPropertyName(property);
            return CreateLinkDefinition<T>(rec, prop, action, controller, new RouteValueDictionary(routeValues), htmlAttributes);
        }

        public static LinkDefinition CreateLinkDefinition<T>(object rec, string property, string action, string controller, RouteValueDictionary rvd = null,
            IDictionary<string, object> htmlAttributes = null) where T : class
        {
            var data = ColumnRules<T>.GetFormattedString(rec as T, property);
            //var result = HtmlHelper.GenerateLink(HttpContext.Current.Request.RequestContext,
            //	RouteTable.Routes, data, "Default", action, controller,
            //	rvd, htmlAttributes);
            return new LinkDefinition
            {
                Link = null,
                Alignment = ColumnRules<T>.Alignment(property)
            };
        }
        private static string GetPropertyName<T>(Expression<Func<T>> propertyLambda)
        {
            var me = propertyLambda.Body as MemberExpression;

            if (me == null)
            {
                throw new ArgumentException("You must pass a lambda of the form: '() => Class.Property' or '() => object.Property'");
            }

            return me.Member.Name;
        }
    }
}