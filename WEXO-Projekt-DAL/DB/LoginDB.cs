using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using WEXO_Projekt_DAL.Model;
using BCrypt;
using Microsoft.Extensions.Configuration;

namespace WEXO_Projekt_DAL.DB
{
    /**
    * This class handles user authentication by validating login credentials against the database.
    * Implements ILoginDB interface.
    */
    public class LoginDB : ILoginDB
    {

        private readonly string _connectionString;
        private string _loginSQL = "select username, password from Users where username = @username";

        public LoginDB(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DBConnectionString");
        }

        /**
        * Attempts to log in a user by verifying the provided credentials.
        * returns true if login is successful; otherwise, throws an exception.
        * exception thrown if the user is not found or the password does not match.
        */
        public bool Login(LoginUser user)
        {
            bool res = false;
            using IDbConnection connection = new SqlConnection(_connectionString);
            connection.Open();
            //Set empty Organizer to return to ensure no leak on data
            try
            {
                LoginUser loginUserFromDB = connection.QueryFirstOrDefault<LoginUser>(_loginSQL, new {username = user.username});

                if (loginUserFromDB == null || loginUserFromDB.password == null)
                {
                    throw new Exception($"Not correct password for {user.username}");
                }

                // Verify the provided password using BCrypt
                if (!BCrypt.Net.BCrypt.Verify(user.password, loginUserFromDB.password))
                {
                    throw new Exception($"Not correct password for: {user.username}");
                }

                res = true;
                return res;

            }
            catch (Exception e)
            {
                throw new Exception($"There was a problem connecting to database using user {user.username}", e);
            } finally
            {
                connection.Close();
            }
            
        }
    }
    }
