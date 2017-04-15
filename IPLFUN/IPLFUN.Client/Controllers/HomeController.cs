using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Security;
using Data.IPLFUN;
using IPL.Entity;

namespace IPLFUN.Client.Controllers
{
    public class HomeController : Controller
    {
        IPLSchedule iplSchedule = new IPLSchedule();
        DBUser userDB = new DBUser();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult History()
        {
            if (getUser() != null)
            {
                IPLUser user = getUser();
                ViewBag.user = user;
                List<BidHistory> history = new List<BidHistory>();
                history = iplSchedule.getBidderHistory(user.userId);
                return View(history);
            }
            else
                return RedirectToAction("Index", "Home");
        }

        public ActionResult Bid()
        {
            if (getUser() != null)
            {
                IPLUser user = getUser();
                ViewBag.user = user;
                List<BidQuestion> bidQuestions = new List<BidQuestion>();
                bidQuestions = iplSchedule.getBidQuestionForBidder(user.userId);
                return View(bidQuestions);
            }
            else
                return RedirectToAction("Index", "Home");
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
                  DateTime.Now.AddMonths(2),           // expiryDate
                  true,                          // true to persist across browser sessions
                  user.roleId.ToString() + ":" + user.userId + ":" + user.Name,                              // can be used to store additional user data
                  FormsAuthentication.FormsCookiePath);  // the path for the cookie

                // Encrypt the ticket using the machine key
                string encryptedTicket = FormsAuthentication.Encrypt(ticket);

                // Add the cookie to the request to save it
                HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                cookie.HttpOnly = true;
                Response.Cookies.Add(cookie);
                if (user.roleId == 3)
                    return Json(new { data = new Result { Status = ResultStatus.Success, Message = "Successfully authenticated." } }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { data = new Result { Status = ResultStatus.Error, Message = "Username or password is incorrect." } }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new { data = new Result { Status = ResultStatus.Error, Message = "Username or password is incorrect." } }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SubmitBidResultByBidder(bool val, int masterId)
        {
            BidResult success = iplSchedule.submitBidResultByBidder(val, masterId, getUser().userId);
            if (success.IsSucceed == 1)
                return Json(new { data = new Result { Status = ResultStatus.Success, Message = "Your bid submitted successfully." }, favour = success.InFavour, against = success.Against, history = success.BidHistory }, JsonRequestBehavior.AllowGet);
            else if(success.IsSucceed == -2)
                return Json(new { data = new Result { Status = ResultStatus.Warning, Message = "Sorry bid is not available." } }, JsonRequestBehavior.AllowGet);
            else
                return Json(new { data = new Result { Status = ResultStatus.Error, Message = "Error occurred while submitting pole." } }, JsonRequestBehavior.AllowGet);
        }

        private IPLUser getUser()
        {
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            string username = string.Empty;
            IPLUser user = null;
            if (authCookie != null)
            {
                user = new IPLUser();
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
                string name = ticket.Name;
                string userData = ticket.UserData;
                user.userId = Convert.ToInt32(userData.Split(':')[1]);
                user.Name = Convert.ToString(userData.Split(':')[2]);
            }
            return user;
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();

            // clear authentication cookie
            HttpCookie cookie1 = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            cookie1.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie1);

            // clear session cookie (not necessary for your current problem but i would recommend you do it anyway)
            SessionStateSection sessionStateSection = (SessionStateSection)WebConfigurationManager.GetSection("system.web/sessionState");
            HttpCookie cookie2 = new HttpCookie(sessionStateSection.CookieName, "");
            cookie2.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie2);

            return RedirectToAction("Index", "Home");
        }
    }
}