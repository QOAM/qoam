namespace QOAM.Website.Helpers
{
    using System.Collections.Generic;
    using System.Web.Optimization;

    public class AsIncludedOrderer : IBundleOrderer
    {
        public IEnumerable<BundleFile> OrderFiles(BundleContext context, IEnumerable<BundleFile> files)
        {
            return files;
        }
    }
}