namespace RU.Uci.OAMarket.Domain.Tests.Import.Resources
{
    using System.IO;
    using System.Reflection;

    public static class ResourceReader
    {
        public static string GetContentsOfResource(string filename)
        {
            using (var streamReader = new StreamReader(GetResourceStream(filename)))
            {
                return streamReader.ReadToEnd();
            }
        }

        private static Stream GetResourceStream(string filename)
        {
            var fullPath = string.Format("{0}.{1}", typeof(ResourceReader).Namespace, filename);

            return Assembly.GetExecutingAssembly().GetManifestResourceStream(fullPath);
        }
    }
}