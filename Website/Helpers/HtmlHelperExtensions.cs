namespace RU.Uci.OAMarket.Website.Helpers
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;

    using PagedList.Mvc;

    using HtmlHelper = System.Web.Mvc.HtmlHelper;

    public static class HtmlHelperExtensions
    {
        private static readonly PagedListRenderOptions DefaultPagedListRenderOptions = new PagedListRenderOptions
                                                                                           {
                                                                                               Display = PagedListDisplayMode.IfNeeded,
                                                                                               UlElementClasses = new[] { "pagination" }
                                                                                           };
        
        public static PagedListRenderOptions PagedListRenderOptions(this HtmlHelper helper)
        {
            return DefaultPagedListRenderOptions;
        }

        public static MvcHtmlString EnumDropDownListFor<TModel, TEnum>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression)
        {
            return htmlHelper.EnumDropDownListFor(expression, null);
        }

        public static MvcHtmlString EnumDropDownListFor<TModel, TEnum>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression, object htmlAttributes)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            var enumType = GetNonNullableModelType(metadata);
            var values = Enum.GetValues(enumType).Cast<TEnum>();

            var items =
                values.Select(value => new SelectListItem
                                           {
                                               Text = value.GetName(),
                                               Value = value.ToString(),
                                               Selected = value.Equals(metadata.Model)
                                           });

            return htmlHelper.DropDownListFor(expression, items, htmlAttributes);
        }

        private static Type GetNonNullableModelType(ModelMetadata modelMetadata)
        {
            var realModelType = modelMetadata.ModelType;
            var underlyingType = Nullable.GetUnderlyingType(realModelType);

            return underlyingType ?? realModelType;
        }
    }
}