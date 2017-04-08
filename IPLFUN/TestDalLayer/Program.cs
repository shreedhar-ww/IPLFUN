using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.IPLFUN;
using IPL.Entity;

namespace TestDalLayer
{
    class Program
    {
        static void Main(string[] args)
        {
            AuthenticateUser();
        }
        public static void createUser()
        {
            DBUser user = new DBUser();
            IPLBidder bidder = new IPLBidder();
            bidder.firstName = "";
            bidder.lastName = "";
            bidder.email = "";
            bidder.points = 100;
            bidder.roleID = 1;

            bidder.mobileNumber = "123123";
            bidder.userName = Guid.NewGuid().ToString();
            bidder.password = "";
            user.createUser(bidder, 1, true);
        }

        public static void getUser()
        {
            DBUser user = new DBUser();
            sortPaging sr = new sortPaging();
            sr.pageNumber = 1;
            sr.pageSize = 10;
            sr.sortColumn = "Firstname";
            sr.sortDirection = "asc";

            user.getUser(sr);
        }

        public static void getSchedule()
        {
            IPLSchedule user = new IPLSchedule();
            sortPaging sr = new sortPaging();
            sr.pageNumber = 1;
            sr.pageSize = 100;
            user.getSchedule(sr);

            user.getBidQuestion(1);
        }

        public static void setSchedule()
        {
            DataTable dataTable = new DataTable("SampleDataType");
            //we create column names as per the type in DB 
            dataTable.Columns.Add("bidId", typeof(int));
            dataTable.Columns.Add("matchId", typeof(int));
            dataTable.Columns.Add("bidPoint", typeof(int));
            //and fill in some values 
            dataTable.Rows.Add(2,10 ,10);
            dataTable.Rows.Add(2,10,4);
            dataTable.Rows.Add(2,10,6);

            //IPLSchedule user = new IPLSchedule();
            //user.setQuestion(dataTable,1);

        }

        public static void UpdateBidStatus()
        {
            IPLSchedule user = new IPLSchedule();
            user.updateBidStatusAndCalculatePoint(1, 10, false, 1);
        }

        public static void AuthenticateUser()
        {
            DBUser user = new DBUser();
            user.authenticateUser("tejals", "ASuS6-st");
        }
    }
}
