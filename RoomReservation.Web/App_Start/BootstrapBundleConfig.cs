using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace RoomReservation.Web
{
    public class BootstrapBundleConfig
    {
        public static void RegisterBundles()
        {
            BundleTable.Bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/bootstrap*"));
            BundleTable.Bundles.Add(new StyleBundle("~/Content/bootstrap").Include(
                "~/Content/metro-bootstrap.css",
                "~/Content/bootstrap-responsive.css"
                ));
        }
    }
}