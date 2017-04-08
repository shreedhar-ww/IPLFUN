using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IPL.Entity;
using Data.IPLFUN;
using IPLFUN.Common;
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
                return Json(new { data = new Result { Status = ResultStatus.Success, Message = "User registered successfully." }},JsonRequestBehavior.AllowGet);                
            else
                return Json(new { data = new Result { Status = ResultStatus.Error, Message = "Error occurred while registering user." } }, JsonRequestBehavior.AllowGet);            
        }
    }
}