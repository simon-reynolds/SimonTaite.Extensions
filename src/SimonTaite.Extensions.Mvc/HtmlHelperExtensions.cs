using System;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;

namespace SimonTaite.Extensions.Mvc
{
    public static class HtmlHelperExtensions
    {
        public static string FieldIdFor<T, TResult>(this HtmlHelper<T> htmlHelper, Expression<Func<T, TResult>> expression)
        {
            if (htmlHelper == null)
            {
                throw new ArgumentNullException(nameof(htmlHelper), @"htmlHelper cannot be null");
            }
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression), @"expression cannot be null");
            }
            
            return htmlHelper.IdFor(expression);
        }
        
        public static string FieldNameFor<T, TResult>(this HtmlHelper<T> htmlHelper, Expression<Func<T, TResult>> expression)
        {
            if (htmlHelper == null)
            {
                throw new ArgumentNullException(nameof(htmlHelper), @"htmlHelper cannot be null");
            }
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression), @"expression cannot be null");
            }
            
            return htmlHelper.NameFor(expression);
        }
    }
}