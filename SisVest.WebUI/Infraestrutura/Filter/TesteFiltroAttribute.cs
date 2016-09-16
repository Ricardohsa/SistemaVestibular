using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SisVest.WebUI.Infraestrutura.Filter
{
    public class TesteFiltroAttribute :FilterAttribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.HttpContext.Response.Write("Executando Filtro antes de iniciar a Action <br>");
            
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            filterContext.HttpContext.Response.Write("Executando Filtro após encerrar a Action <br>");
        }
    }
}