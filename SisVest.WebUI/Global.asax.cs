using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using SisVest.WebUI.Infraestrutura;
using SisVest.WebUI.Infraestrutura.FilterProvider;

namespace SisVest.WebUI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            //Passa a utilziar o NinjectDependecyResolver
            DependencyResolver.SetResolver(new NinjectDependecyResolver());

            FilterProviders.Providers.Clear();
            FilterProviders.Providers.Add(new FilterProviderCustom());

            //Injeta controle de dependencia somente nos Controles
            //ControllerBuilder.Current.SetControllerFactory(new NinjectControllerFactory());


        }

    }
}
