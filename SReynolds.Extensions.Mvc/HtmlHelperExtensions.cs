using System;
using System.Linq.Expressions;
using Microsoft.AspNet.Mvc.ViewFeatures;

namespace SReynolds.Extensions.Mvc
{
    public static class HtmlHelperExtensions
    {
        public static string FieldIdFor<T, TResult>(this HtmlHelper<T> htmlHelper, Expression<Func<T, TResult>> expression)
        {
            return htmlHelper.GenerateIdFromName(htmlHelper.FieldNameFor(expression));
        }
        
        public static string FieldNameFor<T, TResult>(this HtmlHelper<T> htmlHelper, Expression<Func<T, TResult>> expression)
        {
            return htmlHelper.ViewData.TemplateInfo.GetFullHtmlFieldName(ExpressionHelper.GetExpressionText(expression));
        }
    }
}