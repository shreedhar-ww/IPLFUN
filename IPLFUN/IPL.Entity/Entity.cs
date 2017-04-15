using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace IPL.Entity
{ 
    public class IPLBidder
    {
        [Required(ErrorMessage ="First Name is required.")]
        public string firstName { get; set; }
        [Required(ErrorMessage = "Last Name is required.")]
        public string lastName { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string email { get; set; }
        [Required(ErrorMessage = "Points required.")]
        public int? points { get; set; }
        [RegularExpression(@"^([0]|\+91[\-\s]?)?[789]\d{9}$", ErrorMessage = "Invalid Mobile Number.")]
        [Required(ErrorMessage = "Mobile Number is required.")]
        public string mobileNumber { get; set; }
        public int roleID { get; set; }
        [Required(ErrorMessage = "UserName is required.")]
        [Remote("CheckExistingUserName", "User", ErrorMessage = "UserName already exists.")]
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

        public string InFavour { get; set; }
        public string Against { get; set; }
        public string bidPoints { get; set; }

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
        public string Name { get; set; }
    }

    public class LoginViewModel
    {
        [Required(ErrorMessage ="Username is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

    }

    public class BidHistory
    {
        public string Match { get; set; }

        public string MatchDate { get; set; }

        public string Points{ get; set; }

        public string Question { get; set; }
        public string Answer { get; set; }
        //public string PointLoose { get; set; }

    }

    public class BidResult
    {
        public int IsSucceed { get; set; }
        public int InFavour { get; set; }
        public int Against { get; set; }

        public List<BidHistory> BidHistory { get; set; }
    }

}
