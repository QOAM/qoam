﻿namespace QOAM.Website
{
    using System.Web.Optimization;

    using QOAM.Website.Helpers;

    public static class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            var orderer = new AsIncludedOrderer();
            BundleTable.EnableOptimizations = false;

            bundles.Add(new ScriptBundle("~/scripts/libraries") { Orderer = orderer }.Include("~/Scripts/jquery-{version}.js", "~/Scripts/bootstrap.js", "~/Scripts/modernizr-{version}.js", "~/Scripts/knockout-{version}.js", "~/Scripts/knockout.mapping-latest.js", "~/Scripts/knockout.isdirty.js", "~/Scripts/spin.js"));
            bundles.Add(new ScriptBundle("~/scripts/form") { Orderer = orderer }
                .Include("~/Scripts/cldr.js", "~/Scripts/cldr/supplemental.js", "~/Scripts/cldr/event.js", "~/Scripts/cldr/unresolved.js")
                .Include("~/Scripts/globalize.js", "~/Scripts/globalize/number.js", "~/Scripts/globalize/plural.js", "~/Scripts/globalize/currency.js", "~/Scripts/globalize/date.js", "~/Scripts/globalize/message.js", "~/Scripts/globalize/relative-time.js", "~/Scripts/globalize/unit.js")
                .Include("~/Scripts/jquery.validate.js", "~/Scripts/jquery.validate.unobtrusive.js", "~/Scripts/jquery.unobtrusive-ajax.js", "~/Scripts/jquery.validate.globalize.js"));
            bundles.Add(new ScriptBundle("~/scripts/application") { Orderer = orderer }.Include("~/Scripts/Helpers/*.js", "~/Scripts/Controllers/*.js"));
            bundles.Add(new ScriptBundle("~/scripts/fancybox") { Orderer = orderer }.Include("~/Scripts/jquery.ipicture.min.js", "~/Scripts/jquery.fancybox.js"));
            bundles.Add(new ScriptBundle("~/scripts/typeahead").Include("~/Scripts/typeahead.bundle.js"));
            bundles.Add(new ScriptBundle("~/scripts/chosen").Include("~/Scripts/chosen.jquery.js"));
            bundles.Add(new ScriptBundle("~/scripts/slider").Include("~/Scripts/bootstrap-slider.js"));
            bundles.Add(new ScriptBundle("~/scripts/datatables").Include("~/Scripts/dataTables.js"));

            bundles.Add(new StyleBundle("~/styles/application") { Orderer = orderer }.Include("~/Content/bootstrap.css", "~/Content/Site.css", "~/Content/slider.css"));
            bundles.Add(new StyleBundle("~/styles/fancybox").Include("~/Content/iPicture.css", "~/Content/jquery.fancybox.css"));
            bundles.Add(new StyleBundle("~/styles/typeahead").Include("~/Content/typeahead.js-bootstrap.css"));
            bundles.Add(new StyleBundle("~/styles/chosen").Include("~/Content/chosen/chosen.css"));
            bundles.Add(new StyleBundle("~/styles/datatables") { Orderer = orderer }.Include("~/Content/dataTables.css", "~/Content/dataTables.custom.css"));
        }
    }
}