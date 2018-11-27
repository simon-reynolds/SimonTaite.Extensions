using System;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;

namespace SimonTaite.Extensions.Mvc
{
    public static class HtmlHelperExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="htmlHelper">The HTML helper instance that this method extends.</param>
        /// <param name="expression">An expression that identifies the object that contains the ID.</param>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <returns>The HTML ID attribute value for the object that is represented by the expression.</returns>
        [Obsolete("Use HtmlHelper's own IdFor extension method. This is provided for backwards compatability only")]
        public static string FieldIdFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
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
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="htmlHelper">The HTML helper instance that this method extends.</param>
        /// <param name="expression">An expression that identifies the object that contains the name.</param>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <returns>The full HTML field name for the object that is represented by the expression.</returns>
        [Obsolete("Use HtmlHelper's own NameFor extension method. This is provided for backwards compatability only")]
        public static string FieldNameFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
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