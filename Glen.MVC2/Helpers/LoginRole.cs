using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Glen.Domain.Entities;

namespace Glen.MVC.Helpers
{
    public class LoginRole
    {
        public static bool Is(params PositionEnum[] positions)
        {
            var login = HttpContext.Current.Session["Login"] as Employee;

            return login != null && (login.Position == PositionEnum.Boss || positions.Any(pos => login.Position == pos));
        }

        public static bool Exclusive (params PositionEnum[] positions)
        {
            var login = HttpContext.Current.Session["Login"] as Employee;
            return login != null && positions.Any(pos => login.Position == pos);
        }
    }
}