using System;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using Microsoft.AspNet.Mvc.ViewFeatures;

namespace SimonTaite.Extensions.Mvc
{
    public static class HtmlHelperExtensions
    {
        public static string FieldIdFor<T, TResult>(this HtmlHelper<T> htmlHelper, Expression<Func<T, TResult>> expression)
        {
            Contract.Requires<ArgumentNullException>(htmlHelper != null);
            Contract.Requires<ArgumentNullException>(expression != null);
            
            return htmlHelper.GenerateIdFromName(htmlHelper.FieldNameFor(expression));
        }
        
        public static string FieldNameFor<T, TResult>(this HtmlHelper<T> htmlHelper, Expression<Func<T, TResult>> expression)
        {
            Contract.Requires<ArgumentNullException>(htmlHelper != null);
            Contract.Requires<ArgumentNullException>(expression != null);
            
            return htmlHelper.ViewData.TemplateInfo.GetFullHtmlFieldName(ExpressionHelper.GetExpressionText(expression));
        }
    }
}