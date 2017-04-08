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
                        schedule.bidQuestion = reader["Question"].ToString();
                        schedule.bidId = Convert.ToInt32(reader["bidId"]);
                        schedule.Team = reader["Team"].ToString();
                        schedule.TeamA = reader["TeamA"].ToString();
                        schedule.TeamB = reader["TeamB"].ToString();
                        schedule.MasterId = Convert.ToInt32(reader["Id"]);
                        schedule.AlreadySubmitted = Convert.ToBoolean(reader["AlreadySubmitted"]);
                        bidQuestions.Add(schedule);
                    }

                }
                catch (Exception ex)
                {

                }
                return bidQuestions;
            }
        }

        public int submitBidResultByBidder(bool value, int bidId, int userId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                int success = -1;
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("USP_UpdateBidResultByBidder", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@Value", value));
                    command.Parameters.Add(new SqlParameter("@BidId", bidId));
                    command.Parameters.Add(new SqlParameter("@UserId", userId));
                    success = (int)command.ExecuteScalar();

                }
                catch (Exception ex)
                {

                }
                return success;
            }
        }
    }
}
