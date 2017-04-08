using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Data.IPLFUN;
using IPL.Entity;

namespace IPLFUN.Controllers
{
    public class IPLController : Controller
    {
        IPLSchedule iplSchedule = new IPLSchedule();
        // GET: IPL
        public ActionResult Schedule()
        {
            return View();
        }

        public ActionResult GetSchedule()
        {
            List<Schedule> matches = new List<Schedule>();
            matches = iplSchedule.getSchedule(new sortPaging { pageNumber = 1, pageSize = 1000, sortColumn = "", sortDirection = "" });
            var vList = new
            {
                aaData = (from item in matches
                          select new
                          {
                              Day = string.Format("{0:g}", Convert.ToDateTime(item.matchDate)),
                              TeamA = item.TeamA,
                              TeamB = item.TeamB,
                              SetQuestions = "<a class='btn btn-info btn-sm' href='javascipt:void(0);' onclick='fn_setMatchDetails(" + item.Id.ToString() + ",\"" + item.TeamA.ToString() + "\"" + ",\"" + item.TeamB.ToString() + "\")'>Set Questions</a>",
                              SetAnswers = "<a class='btn btn-info btn-sm' href='javascipt:void(0);' onclick='fn_setMatchDetailsForAnswers(" + item.Id.ToString() + ",\"" + item.TeamA.ToString() + "\"" + ",\"" + item.TeamB.ToString() + "\")'>Set Answers</a>",
                          }).ToArray(),
            };
            return Json(vList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SetTempDataForMatchDetails(int matchId, string teamA, string teamB)
        {
            Schedule match = new Schedule();
            match.Id = matchId;
            match.TeamA = teamA;
            match.TeamB = teamB;
            TempData["MatchDetails"] = match;
            return null;
        }

        public ActionResult SetBidQuestions()
        {
            Schedule match = new Schedule();
            match = TempData["MatchDetails"] as Schedule;
            List<BidQuestion> bidQuestions = new List<BidQuestion>();
            bidQuestions = iplSchedule.getBidQuestion(match.Id);
            ViewBag.MatchDetails = match;
            TempData.Keep();
            return View(bidQuestions);
        }

        public ActionResult SaveBidQuestions(List<ActiveQuestions> questions, int matchId)
        {
            DataTable dataTable = new DataTable("Questions");
            //we create column names as per the type in DB 
            dataTable.Columns.Add("bidId", typeof(int));
            dataTable.Columns.Add("bidPoint", typeof(int));
            //and fill in some values 
            foreach (ActiveQuestions question in questions)
            {
                dataTable.Rows.Add(question.BidId, question.BidPoint);
            }
            int success = iplSchedule.setQuestion(dataTable, 1, matchId);

            if (success == 1)
                return Json(new { data = new Result { Status = ResultStatus.Success, Message = "Questions set successfully." } }, JsonRequestBehavior.AllowGet);
            else
                return Json(new { data = new Result { Status = ResultStatus.Error, Message = "Error occurred while adding questions." } }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SetBidAnswers()
        {
            Schedule match = new Schedule();
            match = TempData["MatchDetails"] as Schedule;
            ViewBag.MatchDetails = match;
            List<BidQuestion> bidQuestions = new List<BidQuestion>();
            bidQuestions = iplSchedule.getActiveBidQuestion(match.Id);
            TempData.Keep();
            return View(bidQuestions);
        }

        public ActionResult ActivateBidQuestion(bool val, int bidId,int matchId)
        {
            int success = iplSchedule.activateBidQuestion(val, bidId, matchId);
            if (success == 1)
                return Json(new { data = new Result { Status = ResultStatus.Success, Message = "Question status updated successfully." } }, JsonRequestBehavior.AllowGet);
            else
                return Json(new { data = new Result { Status = ResultStatus.Error, Message = "Error occurred while updating success." } }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SubmitBidResult(bool val, int bidId, int matchId)
        {
            int success = iplSchedule.updateBidStatusAndCalculatePoint(bidId, matchId,val, getUserID());
            if (success == 1)
                return Json(new { data = new Result { Status = ResultStatus.Success, Message = "Result updated successfully." } }, JsonRequestBehavior.AllowGet);
            else
                return Json(new { data = new Result { Status = ResultStatus.Error, Message = "Error occurred while updating result." } }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult BidderIndex()
        {
            List<BidQuestion> bidQuestions = new List<BidQuestion>();
            bidQuestions = iplSchedule.getBidQuestionForBidder(1058);
            return View(bidQuestions);
        }

        public ActionResult SubmitBidResultByBidder(bool val, int masterId)
        {
            int success = iplSchedule.submitBidResultByBidder(val, masterId, getUserID());
            if (success == 1)
                return Json(new { data = new Result { Status = ResultStatus.Success, Message = "Your pole submitted successfully." } }, JsonRequestBehavior.AllowGet);
            else
                return Json(new { data = new Result { Status = ResultStatus.Error, Message = "Error occurred while submitting pole." } }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult BidHistory()
        {
            return View();
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