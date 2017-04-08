using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
 
using Data.IPLFUN;
using IPL.Entity;
using System.Web.Security;
using IPL.Constant;

namespace IPLFUN.Controllers
{
    public class AccountController : Controller
    {
        DBUser userDB = new DBUser();
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            IPLUser user = userDB.authenticateUser(model.Username, model.Password);
            if (user != null)
            {
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                  1,                                     // ticket version
                  model.Username,                              // authenticated username
                  DateTime.Now,                          // issueDate
                  DateTime.Now.AddMinutes(30),           // expiryDate
                  true,                          // true to persist across browser sessions
                  user.roleId.ToString()+":"+user.userId ,                              // can be used to store additional user data
                  FormsAuthentication.FormsCookiePath);  // the path for the cookie

                // Encrypt the ticket using the machine key
                string encryptedTicket = FormsAuthentication.Encrypt(ticket);

                // Add the cookie to the request to save it
                HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                cookie.HttpOnly = true;
                Response.Cookies.Add(cookie);
                if (user.roleId == Convert.ToInt32(Constant.userId))
                {
                    return RedirectToAction("Index", "IPLFun");
                }
                return RedirectToAction("Index", "Home");
            }
            else
                return RedirectToAction("Login", "Account");

        }

        public ActionResult Logout()
        {
            return RedirectToAction("Login", "Account");
        }
    }
}