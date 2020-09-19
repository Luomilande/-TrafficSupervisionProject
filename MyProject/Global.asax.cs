using MyProject.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MyProject
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        protected void Application_Error(object sender, EventArgs e)
        {
              var httpContext = ((MvcApplication)sender).Context;
             var currentController = "";
              var currentAction = "";
              var currentRouterData = RouteTable.Routes.GetRouteData(new HttpContextWrapper(httpContext));

            if (currentRouterData != null)
             {
                 if (currentRouterData.Values["controller"] != null && !string.IsNullOrEmpty(currentRouterData.Values["controller"].ToString()))
                {
                     currentController = currentRouterData.Values["controller"].ToString();
                 }
 
                if (currentRouterData.Values["action"] != null && !string.IsNullOrEmpty(currentRouterData.Values["action"].ToString()))
                {
                     currentAction = currentRouterData.Values["action"].ToString();
                 }
            }
 
            var ex = Server.GetLastError();
             //record error log here.
 
            var controller = new ErrorController();
            var routeData = new RouteData();
             var action = "Error";

            if (ex is HttpException)
            {
                var httpEx = ex as HttpException;
 
                switch (httpEx.GetHttpCode())
                {
                    case  404:
                        action = "Not Found";
                        Response.Redirect("/Error/Error");
                        break;
               }
 
         
 
                routeData.Values["controller"] = "Error";
                 routeData.Values["action"] = action;
             }
         }
    }
}
