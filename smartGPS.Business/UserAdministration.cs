using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using smartGPS.Business.ExternalServices;
using smartGPS.Persistance;
using smartGPS.Persistance.Users;

namespace smartGPS.Business
{
    public class UserAdministration
    {
        #region SignIn/Signup/SignOut

        // return 1 if success, 2 if username is taken, 0 if error on database
        public static int signUp(String username, String password, String name, String surname, Boolean isExternal, String facebookId, String twitterId)
        {

            // check if user already exists
            if (UsersDAO.alreadyExists(username))
                return (int)ErrorHandler.SignUpErrors.UsernameAlreadyTaken;
            else
            {
                try
                {
                    String id = Guid.NewGuid().ToString();
                    // add new user
                    if (isExternal)
                    {
                        UsersDAO.addNew(id, username, "external", name, surname, facebookId, twitterId);
                    }
                    else
                    {
                        UsersDAO.addNew(id, username, Utilities.encryptPassword(password), name, surname, null , null);
                    }
                    FormsAuthentication.SetAuthCookie(id, false);
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

        public static int signInExternalUser(String username)
        {
            users model = UsersDAO.getByUsername(username);
            if (model == null)
                return (int)ErrorHandler.SignInErrors.Failed;
            else
            {
                try
                {
                    // sign in user using forms authentication
                    UsersDAO.updateDateLastLogin(model);
                    FormsAuthentication.SetAuthCookie(model.Id, true);
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

        public static Boolean checkIfUserExists(String username)
        {
            return UsersDAO.alreadyExists(username);
        }

        #endregion


        #region Users

        public static users getUserByUserId(String userId)
        {
            return UsersDAO.getById(userId);
        }

        public static void updateUserLocation(double latitude, double longitude, String userId)
        {
            if (latitude > -90 && latitude < 90 && longitude > -90 && longitude < 90)
                UsersDAO.updateLastLocation(latitude, longitude, userId);
        }

        #endregion

        # region Profile

        public static profile getProfileByUserId(String userId)
        {
            profile model = UsersDAO.getProfileByUserId(userId);
            return model;
        }

        #endregion

    }
}