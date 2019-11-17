using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using InvoiceAppDemo.Infrastructure;

namespace InvoiceAppDemo
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //MEF Related Code
            //Create catalogs
            AggregateCatalog catalog = new AggregateCatalog();
            DirectoryCatalog dirCatalog = new DirectoryCatalog("bin");
            catalog.Catalogs.Add(dirCatalog);

            //Create the composition container
            CompositionContainer composition = new CompositionContainer(catalog);

            //Below code for MVC controllers
            IControllerFactory mefControllerFactory = new
                MEFControllerFactory(composition);
            ControllerBuilder.Current.SetControllerFactory(mefControllerFactory);
        }
    }
}
