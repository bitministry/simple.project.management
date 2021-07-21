using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Glen.Domain.Entities;
using Microsoft.Ajax.Utilities;

namespace Glen.MVC.Helpers
{

    public class GlenAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var login = HttpContext.Current.Session["Login"] as Employee;

            if (login?.GetType().Name == "EmployeeProxy")
            {
                login = null;
                HttpContext.Current.Session["Login"] = null;
            }

            if ( login == null )
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary
                {
                    {"action", "LogIn"},
                    {"controller", "Employee"},
                    {"returnUrl", filterContext.HttpContext.Request.Url}
                });
                return; 
            }
            if (! Roles.IsNullOrWhiteSpace() )
                if (! Roles.Contains(login.Position.ToString()))
                    throw new InvalidOperationException("Not authorized!");

//            base.OnAuthorization(filterContext);
        }
    }
}