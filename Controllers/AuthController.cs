using System;
using StudentGradeTracker.Models;

namespace StudentGradeTracker.Controllers
{
    // CONTROLLER for FR1 - User Access (Login and Registration).
    // Every method returns null on success, or an error message for the View to show.
    public static class AuthController
    {
        public const int MinUsernameLength = 3;
        public const int MinPasswordLength = 4;

        public static string Register(string username, string password, string confirm)
        {
            username = (username ?? "").Trim();

            if (username == "" || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirm))
                return "Please fill in all fields.";

            if (username.Length < MinUsernameLength)
                return "Username must be at least " + MinUsernameLength + " characters.";

            if (password.Length < MinPasswordLength)
                return "Password must be at least " + MinPasswordLength + " characters.";

            if (password != confirm)
                return "The two passwords do not match.";

            try
            {
                if (Database.UserExists(username))
                    return "That username is already taken.";

                Database.AddUser(username, password);
                return null;
            }
            catch (Exception ex)
            {
                return "Could not create the account: " + ex.Message;
            }
        }

        public static string Login(string username, string password)
        {
            username = (username ?? "").Trim();

            if (username == "" || string.IsNullOrEmpty(password))
                return "Please enter your username and password.";

            try
            {
                if (!Database.CheckLogin(username, password))
                    return "Wrong username or password.";

                return null;
            }
            catch (Exception ex)
            {
                return "Could not log in: " + ex.Message;
            }
        }
    }
}
