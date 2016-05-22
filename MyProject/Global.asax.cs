using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using MyProject.App_Start;
using MyProject.DAL;

namespace MyProject
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            GlobalConfiguration.Configuration.EnsureInitialized(); 

            //RegisterRoutes(RouteTable.Routes);

            //need this for it to created the data and tables
            //using (var pContext = new ProductContext())
            //{
            //    pContext.Database.Initialize(true);
            //}

            //using (var uContext = new IdentityContext())
            //{
            //    uContext.Database.Initialize(true);
            //}

            //using (var dContext = new DatabaseMasterContext())
            //{
            //    dContext.Database.Initialize(true);
            //}
        }
    }
}
