namespace QOAM.Website.Helpers
{
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
    }
}