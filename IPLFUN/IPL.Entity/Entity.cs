using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPL.Entity
{
    public class IPLBidder
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public int points { get; set; }
        public string mobileNumber { get; set; }
        public int roleID { get; set; }
        public string userName { get; set; }
        public string password { get; set; }

        public int userId { get; set; }
    }

    public class Schedule
    {
        public string matchDate { get; set; }
        public string TeamA { get; set; }
        public string TeamB { get; set; }
        public int Id { get; set; }
    }

    public class sortPaging
    {
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public string sortColumn { get; set; }
        public string sortDirection { get; set; }
    }


    public class BidQuestion
    {
        public int bidId { get; set; }
        public string bidQuestion { get; set; }
        public string Team { get; set; }
        public string TeamA { get; set; }
        public string TeamB { get; set; }
        public int MasterId { get; set; }
        public bool AlreadySubmitted { get; set; }
        public bool? IsBidActive { get; set; }
        public bool? IsBidResult { get; set; }
        public int? BidPoint { get; set; }

    }

    public class ActiveQuestions
    {
        public int MatchId { get; set; }
        public int BidId { get; set; }
        public bool IsActive { get; set; }
        public int BidPoint { get; set; }
    }

    public class IPLUser
    {
        public int roleId { get; set; }
        public int userId { get; set; }
    }

    public class LoginViewModel
    {

        public string Username { get; set; }


        public string Password { get; set; }

    }
}
