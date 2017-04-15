using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IPL.Entity;
using Data.IPLFUN;
using IPLFUN.Common;
using System.Web.Security;
using System.Web.Configuration;

namespace IPLFUN.Controllers
{
    public class UserController : BaseController
    {
        DBUser userDB = new DBUser();
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetUsers()
        {
            List<IPLBidder> userList = new List<IPLBidder>();
            userList = userDB.getUser(new sortPaging { pageNumber = 1, pageSize = 1000, sortColumn = "", sortDirection = "" });
            var vList = new
            {
                aaData = (from item in userList
                          select new
                          {
                              Name = item.firstName + " " + item.lastName,
                              EmailId = item.email,
                              Mobile = item.mobileNumber,
                              Points = item.points,
                              Action = "<a class='btn btn-info btn-sm' href='/IPL/BidHistory'>Bid History</a>",
                          }).ToArray(),
            };
            return Json(vList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreateUser()
        {
            IPLBidder user = new IPLBidder();
            return PartialView("_CreateUser", user);
        }

        [HttpPost]
        public ActionResult CreateUser(IPLBidder iplBidder)
         {
            iplBidder.password = GeneratePassword.CreateRandomPassword(8);
            iplBidder.roleID = (int)UserRoles.Bidder;
            int success = userDB.createUser(iplBidder, 1);
            if (success == 1)
                return Json(new { data = new Result { Status = ResultStatus.Success, Message = "User registered successfully." } }, JsonRequestBehavior.AllowGet);
            else
                return Json(new { data = new Result { Status = ResultStatus.Error, Message = "Error occurred while registering user." } }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CheckExistingUserName(string userName)
        {
            int success = -1;
            success = userDB.checkUsernameExists(userName);
            if (success == 1)
                return Json(true, JsonRequestBehavior.AllowGet);
            else
                return Json(false, JsonRequestBehavior.AllowGet);
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

            return RedirectToAction("Login", "Account");
        }
    }
}