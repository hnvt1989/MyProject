using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Linq.Expressions;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace MyProject.Helper
{
    public static partial class Helper
    {

        /// <summary>
        /// Create a TextBox with the data-binding set up for Knockout.
        /// </summary>
        /// <typeparam name="TModel">Model</typeparam>
        /// <typeparam name="TProperty">Property</typeparam>
        /// <param name="helper">helper</param>
        /// <param name="expression">lambda expression</param>
        /// <param name="htmlAttributes">HTML attributes</param>
        /// <returns></returns>
        public static MvcHtmlString KTextBoxFor<TModel, TProperty>(
            this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TProperty>> expression,
            object htmlAttributes = null)
        {
            var attribs = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            var valueString = "value: model." + helper.NameFor(expression);

            object o;

            if (attribs.TryGetValue("data-bind", out o))
            {
                attribs["data-bind"] = attribs["data-bind"] + ", " + valueString;
            }
            else
            {
                attribs.Add("data-bind", valueString);
            }

            return helper.TextBoxFor(expression, attribs);
        }
    }
}