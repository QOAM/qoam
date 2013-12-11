namespace RU.Uci.OAMarket.Data.Helpers
{
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Objects;
    using System.Text.RegularExpressions;

    public static class ContextExtensions
    {
        public static string GetTableName<T>(this DbContext context) where T : class
        {
            return ((IObjectContextAdapter)context).ObjectContext.GetTableName<T>();
        }

        private static string GetTableName<T>(this ObjectContext context) where T : class
        {
            var sql = context.CreateObjectSet<T>().ToTraceString();
            var regex = new Regex("FROM (?<table>.*) AS");
            var match = regex.Match(sql);

            return match.Groups["table"].Value;
        }
    }
}