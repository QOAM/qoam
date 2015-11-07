namespace QOAM.Website
{
    using System.Web.Mvc;

    using QOAM.Website.Helpers;

    public static class ModelBinderConfig
    {
        public static void RegisterModelBinders(ModelBinderDictionary binders)
        {
            binders.Add(typeof(decimal), new DecimalModelBinder());
            binders.Add(typeof(decimal?), new DecimalModelBinder());
        }
    }
}