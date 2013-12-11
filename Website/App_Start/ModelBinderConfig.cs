namespace RU.Uci.OAMarket.Website.App_Start
{
    using System.Web.Mvc;

    using RU.Uci.OAMarket.Website.Helpers;

    public static class ModelBinderConfig
    {
        public static void RegisterModelBinders(ModelBinderDictionary binders)
        {
            binders.Add(typeof(decimal), new DecimalModelBinder());
            binders.Add(typeof(decimal?), new DecimalModelBinder());
        }
    }
}