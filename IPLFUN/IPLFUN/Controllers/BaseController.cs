using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using IPL.Constant;

namespace IPLFUN.Controllers
{
    public class BaseController : Controller, IActionFilter
    {
        void IActionFilter.OnActionExecuted(ActionExecutedContext filterContext)
        {
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];

            if (authCookie != null)
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
                string name = ticket.Name;
                string userData = ticket.UserData;
                string roleID = userData.Split(':')[0];
                if (roleID != Constant.adminRoleId)
                {
                    filterContext.Result = new RedirectResult("/Account/Login");
                }
            }
            else
            {
                filterContext.Result = new RedirectResult("/Account/Login");
            }
                    
        }
    }
}