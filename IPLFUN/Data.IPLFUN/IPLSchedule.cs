using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IPL.Entity;

namespace Data.IPLFUN
{
    public class IPLSchedule : DALBase
    {
        public List<Schedule> getSchedule(sortPaging sortPage)
        {
            List<Schedule> iplSchedule = new List<Schedule>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataReader reader;
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("USP_GetSchedule", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@PageNo", sortPage.pageNumber));
                    command.Parameters.Add(new SqlParameter("@PageSize", sortPage.pageSize));
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Schedule schedule = new Schedule();
                        schedule.TeamA = reader["teamAname"].ToString();
                        schedule.TeamB = reader["teamBname"].ToString();
                        schedule.matchDate = reader["matchDate"].ToString();
                        schedule.Id = Convert.ToInt32(reader["Id"]);
                        iplSchedule.Add(schedule);
                    }

                }
                catch (Exception ex)
                {

                }
                return iplSchedule;
            }
        }

        public List<BidQuestion> getBidQuestion(int matchId)
        {

            List<BidQuestion> bidQuestions = new List<BidQuestion>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataReader reader;
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("USP_GetBidQuestion", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@MatchId", matchId));
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        BidQuestion schedule = new BidQuestion();
                        schedule.bidQuestion = reader["Question"].ToString();
                        schedule.bidId = Convert.ToInt32(reader["bidId"]);
                        schedule.Team = reader["Team"].ToString();
                        if (reader["BidPoints"] == DBNull.Value) { schedule.BidPoint = null; }
                        else
                            schedule.BidPoint = Convert.ToInt32(reader["BidPoints"]);
                        schedule.IsBidActive = Convert.ToBoolean(reader["IsActive"]);
                        bidQuestions.Add(schedule);
                    }

                }
                catch (Exception ex)
                {

                }
                return bidQuestions;
            }
        }

        public int setQuestion(DataTable datatable, int createBy, int matchId)
        {
            List<Schedule> iplSchedule = new List<Schedule>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                int success = -1;
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("USP_SetBidQuestionResult", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@CreatedBy", createBy));
                    command.Parameters.Add(new SqlParameter("@MatchId", matchId));
                    SqlParameter parameter = new SqlParameter();
                    //The parameter for the SP must be of SqlDbType.Structured 
                    parameter.ParameterName = "@bidMatchID";
                    parameter.SqlDbType = System.Data.SqlDbType.Structured;
                    parameter.Value = datatable;
                    command.Parameters.Add(parameter);
                    success = (int)command.ExecuteScalar();

                }
                catch (Exception ex)
                {

                }
                return success;
            }
        }

        // Called when set Question result
        public int updateBidStatusAndCalculatePoint(int bidId, int matchId, bool isBidActive, int updatedBy)
        {
            int success = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataReader reader;
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("USP_UpdateBidStatus", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@BidId", bidId));
                    command.Parameters.Add(new SqlParameter("@MatchId", matchId));
                    command.Parameters.Add(new SqlParameter("@BidResult", isBidActive));
                    command.Parameters.Add(new SqlParameter("@UpdatedBy", updatedBy));
                    success = (int)command.ExecuteScalar();

                }
                catch (Exception ex)
                {

                }
                return success;
            }
        }

        public List<BidQuestion> getActiveBidQuestion(int matchId)
        {

            List<BidQuestion> bidQuestions = new List<BidQuestion>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataReader reader;
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("USP_GetActiveBidQuestionsByMatch", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@MatchId", matchId));
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        BidQuestion schedule = new BidQuestion();
                        schedule.bidQuestion = reader["Question"].ToString();
                        schedule.bidId = Convert.ToInt32(reader["bidId"]);
                        schedule.Team = reader["Team"].ToString();
                        if (reader["IsBidActive"] == DBNull.Value)
                        {
                            schedule.IsBidActive = null;
                        }
                        else
                        {
                            schedule.IsBidActive = Convert.ToBoolean(reader["IsBidActive"]);
                        }
                        if (reader["BidResult"] == DBNull.Value)
                        {
                            schedule.IsBidResult = null;
                        }
                        else
                        {
                            schedule.IsBidResult = Convert.ToBoolean(reader["BidResult"]);
                        }
                        bidQuestions.Add(schedule);
                    }

                }
                catch (Exception ex)
                {

                }
                return bidQuestions;
            }
        }

        public int activateBidQuestion(bool value, int bidId, int matchId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                int success = -1;
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("USP_ActivateBidQuestion", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@Value", value));
                    command.Parameters.Add(new SqlParameter("@BidId", bidId));
                    command.Parameters.Add(new SqlParameter("@MatchId", matchId));
                    success = (int)command.ExecuteScalar();

                }
                catch (Exception ex)
                {

                }
                return success;
            }
        }

        public List<BidQuestion> getBidQuestionForBidder(int userId)
        {

            List<BidQuestion> bidQuestions = new List<BidQuestion>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataReader reader;
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("USP_GetBidQuestionsForBidder", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@UserId", userId));
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        BidQuestion schedule = new BidQuestion();
                        schedule.bidId = Convert.ToInt32(reader["bidId"]);
                        schedule.Team = reader["Team"].ToString();
                        schedule.TeamA = reader["TeamA"].ToString();
                        schedule.TeamB = reader["TeamB"].ToString();
                        schedule.InFavour = reader["InFavour"].ToString();
                        schedule.Against = reader["Against"].ToString();
                        schedule.bidPoints = reader["bidPoints"].ToString();

                        if (schedule.Team == "A")
                        {
                            schedule.bidQuestion = schedule.TeamA + " : " + reader["Question"].ToString() + "?";
                        }
                        if (schedule.Team == "B")
                        {
                            schedule.bidQuestion = schedule.TeamB + " : " + reader["Question"].ToString() + "?";
                        }
                        schedule.MasterId = Convert.ToInt32(reader["Id"]);
                        schedule.AlreadySubmitted = Convert.ToBoolean(reader["AlreadySubmitted"]);
                        schedule.IsBidActive = Convert.ToBoolean(reader["IsBidActive"]);
                        bidQuestions.Add(schedule);
                    }

                }
                catch (Exception ex)
                {

                }
                return bidQuestions;
            }
        }

        public BidResult submitBidResultByBidder(bool value, int bidId, int userId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataReader reader;
                int success = -1;
                List<BidHistory> history = null;
                BidResult bidResult = new BidResult();
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("USP_UpdateBidResultByBidder", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@Value", value));
                    command.Parameters.Add(new SqlParameter("@BidId", bidId));
                    command.Parameters.Add(new SqlParameter("@UserId", userId));
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        bidResult.IsSucceed = Convert.ToInt32(reader["IsSucceed"]);
                        bidResult.InFavour = Convert.ToInt32(reader["InFavour"]);
                        bidResult.Against = Convert.ToInt32(reader["Against"]);
                        if (bidResult.IsSucceed == 1)
                        {
                            history = new List<BidHistory>();
                            if (reader.NextResult())
                            {
                                while (reader.Read())
                                {
                                    BidHistory bidHistory = new BidHistory();
                                    string teamA = reader["TeamA"].ToString();
                                    string teamB = reader["TeamB"].ToString();

                                    bidHistory.Match = teamA + " VS " + teamB;
                                    if (reader["Team"].ToString() == "A")
                                    {
                                        bidHistory.Question = teamA + " : " + reader["Question"].ToString();
                                    }
                                    else
                                    {
                                        bidHistory.Question = teamB + " : " + reader["Question"].ToString();
                                    }
                                    bidHistory.Points = reader["BidPoints"].ToString();
                                    bool answer = Convert.ToBoolean(reader["UserBid"].ToString());
                                    if (answer) bidHistory.Answer = "Yes";
                                    else bidHistory.Answer = "No";
                                    history.Add(bidHistory);
                                }
                            }
                        }
                    }
                    bidResult.BidHistory = history;
                }
                catch (Exception ex)
                {

                }
                return bidResult;
            }
        }


        public List<BidHistory> getBidderHistory(int userId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataReader reader;
                List<BidHistory> history = new List<BidHistory>();
                int success = -1;
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("USP_GETUSERBIDHISTORY", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@UserId", userId));
                    reader = command.ExecuteReader();
                    decimal pointswin = 0;
                    decimal pointsloose = 0;
                    decimal pointsdraw = 0;
                    while (reader.Read())
                    {
                        BidHistory bhistory = new BidHistory();

                        string teamA = reader["TeamA"].ToString();
                        string teamB = reader["TeamB"].ToString();

                        bhistory.Match = teamA + " VS " + teamB;

                        if (reader["Team"].ToString() == "A")
                        {
                            bhistory.Question = teamA + " : " + reader["Question"].ToString();
                        }
                        else
                        {
                            bhistory.Question = teamB + " : " + reader["Question"].ToString();
                        }

                        bhistory.MatchDate = reader["MatchDate"].ToString();

                        pointsloose = Convert.ToDecimal(reader["PointsLoose"]);
                        pointswin = Convert.ToDecimal(reader["PointsWin"]);

                        if (reader["PointDraw"] != DBNull.Value)
                            pointsdraw = Convert.ToDecimal(reader["PointDraw"]);

                        if (pointsloose != 0)
                            bhistory.Points = pointsloose.ToString("0.00");
                        else if (pointswin != 0)
                            bhistory.Points = "+" + pointswin.ToString("0.00");
                        else if (pointsdraw != 0)
                            bhistory.Points = pointsdraw.ToString("0.00");
                        history.Add(bhistory);
                    }
                }
                catch (Exception ex)
                {

                }
                return history;
            }
        }
    }
}
