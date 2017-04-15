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
    public class DBUser : DALBase
    {
        public int createUser(IPLBidder bidder, int createBy, bool isActive = true)
        {
            int success = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {

                    connection.Open();
                    SqlCommand command = new SqlCommand("USP_CreateUser", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@firstName", bidder.firstName));
                    command.Parameters.Add(new SqlParameter("@lastName", bidder.lastName));
                    command.Parameters.Add(new SqlParameter("@mobileNumber", bidder.mobileNumber));
                    command.Parameters.Add(new SqlParameter("@email", bidder.email));
                    command.Parameters.Add(new SqlParameter("@points", bidder.points));
                    command.Parameters.Add(new SqlParameter("@roleID", bidder.roleID));
                    command.Parameters.Add(new SqlParameter("@isActive", isActive));
                    command.Parameters.Add(new SqlParameter("@ADMINID", createBy));
                    command.Parameters.Add(new SqlParameter("@userName", bidder.userName));
                    command.Parameters.Add(new SqlParameter("@password", bidder.password));
                    success = (int)command.ExecuteScalar();

                }
                catch (Exception ex)
                {

                }
            }
            return success;
        }

        public List<IPLBidder> getUser(sortPaging sortPage)
        {
            List<IPLBidder> users = new List<IPLBidder>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataReader reader;
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("USP_GetUser", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@PageNo", sortPage.pageNumber));
                    command.Parameters.Add(new SqlParameter("@PageSize", sortPage.pageSize));
                    command.Parameters.Add(new SqlParameter("@SortColumn", sortPage.sortColumn));
                    command.Parameters.Add(new SqlParameter("@SortOrder", sortPage.sortDirection));
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        IPLBidder bidder = new IPLBidder();
                        bidder.firstName = reader["firstName"].ToString();
                        bidder.lastName = reader["lastName"].ToString();
                        bidder.userName = reader["userName"].ToString();
                        bidder.mobileNumber = reader["MobileNumber"].ToString();
                        bidder.email = reader["EmailId"].ToString();
                        bidder.points = Convert.ToInt32(reader["Point"].ToString());
                        bidder.userId = Convert.ToInt32(reader["Id"]);
                        users.Add(bidder);
                    }

                }
                catch (Exception ex)
                {

                }
                return users;
            }
        }

        public IPLUser authenticateUser(string userName, string password)
        {
            
            IPLUser user = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataReader reader;
                try
                {

                    connection.Open();
                    SqlCommand command = new SqlCommand("USP_AuthenticateUser", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@UserName", userName));
                    command.Parameters.Add(new SqlParameter("@Password", password));              
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        user= new IPLUser();
                        user.roleId = Convert.ToInt32(reader["roleId"].ToString());
                        user.userId = Convert.ToInt32(reader["ID"].ToString());
                        user.Name = reader["Name"].ToString();
                    }
                }
                catch (Exception ex)
                {

                }
            }
            return user;

        }

        public int checkUsernameExists(string userName)
        {
            int success = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {

                    connection.Open();
                    SqlCommand command = new SqlCommand("USP_CheckUserNameExists", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@userName", userName));                    
                    success = (int)command.ExecuteScalar();

                }
                catch (Exception ex)
                {

                }
            }
            return success;
        }
    }
}
