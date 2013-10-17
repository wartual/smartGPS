using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using smartGPS.Persistence;
using smartGPS.Persistence.Users;

namespace smartGPS.BusinessLogic
{
    public class UserAdministration
    {
        #region SignIn/Signup/SignOut

        // return 1 if success, 2 if username is taken, 0 if error on database
        public static int signUp(String username, String password, String name, String surname)
        {

            // check if user already exists
            if (UsersDAO.alreadyExists(username))
                return (int)ErrorHandler.SignUpErrors.UsernameAlreadyTaken;
            else
            {
                try
                {
                    // add new user
                    UsersDAO.addNew(username, Utilities.encryptPassword(password), name, surname);
                    return (int)ErrorHandler.SignUpErrors.Success; ;
                }
                catch (Exception e)
                {
                    Console.Write(e.ToString());
                    return (int)ErrorHandler.SignUpErrors.Database;
                }
            }
        }

        public static int signIn(String username, String password, Boolean rememberMe)
        {
            users model = UsersDAO.getByUsernameAndPassword(username, Utilities.encryptPassword(password));
            if (model == null)
                return (int)ErrorHandler.SignInErrors.Failed;
            else
            {
                try
                {
                    // sign in user using forms authentication
                    UsersDAO.updateDateLastLogin(model);
                    FormsAuthentication.SetAuthCookie(model.Id, rememberMe);
                    return (int)ErrorHandler.SignInErrors.Success;
                 }
                catch (Exception e)
                {
                    Console.Write(e.ToString());
                    return (int)ErrorHandler.SignInErrors.Database;
               }
            }
        }

        public static void signOut()
        {
            // sign out using forms authentication
            FormsAuthentication.SignOut();
        }

        #endregion

        # region Profile

        public static profile getProfile(String userId)
        {
            return UsersDAO.getProfileByUserId(userId);
        }

        #endregion
    }
}