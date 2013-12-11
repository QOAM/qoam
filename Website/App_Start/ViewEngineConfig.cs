namespace RU.Uci.OAMarket.Website.App_Start
{
    using System.Web.Mvc;

    public static class ViewEngineConfig
    {
        public static void RegisterViewEngines(ViewEngineCollection engines)
        {
            engines.Clear();
            engines.Add(new RazorViewEngine());
        }
    }
}