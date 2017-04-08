using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Data.IPLFUN;
using IPL.Entity;

namespace IPLFUN.Controllers
{
    public class IPLFunController : Controller
    {
        IPLSchedule iplSchedule = new IPLSchedule();
        public ActionResult Index()
        {
            List<BidQuestion> bidQuestions = new List<BidQuestion>();
            bidQuestions = iplSchedule.getBidQuestionForBidder(getUserID());
            return View(bidQuestions);
        }

        private int getUserID()
        {
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            int userid = -1;
            if (authCookie != null)
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
                string name = ticket.Name;
                string userData = ticket.UserData;
                userid = Convert.ToInt32(userData.Split(':')[1]);
            }
            return userid;
        }


    }
}