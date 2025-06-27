using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEXO_Projekt_DAL.DB;
using WEXO_Projekt_DAL.Model;

namespace WEXO_Project_BLL.BusinessLogic
{
    /*
     * This class provides business logic for user login functionality.
     * It interacts with the data layer through the ILoginDB interface to handle the authentication process.
     * The class validates user credentials and returns a success or failure status based on the result.
     */
    public class LoginBusinessLogic : ILoginBusinessLogic
    {
        private readonly ILoginDB _loginDB;

        public LoginBusinessLogic(ILoginDB loginDB)
        {
            _loginDB = loginDB;
        }

        /*
         * Attempts to log in the user with the provided LoginUser object.
         * The method calls the Login method from the ILoginDB interface to check user credentials.
         * 
         * Returns true if the login credentials are valid; otherwise, returns false.
         * If an error occurs during the login attempt, false is returned.
         */
        public bool Login(LoginUser user)
        {
            try
            {
                // Calls the Login method in the data layer to validate the user's credentials.
                return _loginDB.Login(user);
            }
            catch (Exception)
            {
                // If any exception occurs during login, it silently returns false.

            }
            // Returns false if login failed or an error occurred.
            return false;
        }
    }
}
