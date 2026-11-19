using System;
using StudentGradeTracker.Models;

namespace StudentGradeTracker.Controllers
{
    // CONTROLLER for FR1 - User Access (Login and Registration).
    // Accounts are for staff (teachers and administrators) who use the system, not for students.
    // Every method returns null on success, or an error message for the View to show.
    public static class AuthController
    {
        public const int MinUsernameLength = 3;
        public const int MaxUsernameLength = 50;
        public const int MinPasswordLength = 4;
        public const int MaxPasswordLength = 50;
        public const int MaxFullNameLength = 100;

        public static string Register(string fullName, string username, string password, string confirm)
        {
            fullName = (fullName ?? "").Trim();
            username = (username ?? "").Trim();

            if (fullName == "" || username == "" || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirm))
                return "Please fill in all fields.";

            if (fullName.Length > MaxFullNameLength)
                return "Full name can have at most " + MaxFullNameLength + " characters.";

            if (username.Length < MinUsernameLength || username.Length > MaxUsernameLength)
                return "Username must be " + MinUsernameLength + " to " + MaxUsernameLength + " characters.";

            if (!IsValidUsername(username))
                return "Username can only use letters, numbers, dots (.) and underscores (_), with no spaces.";

            if (password.Length < MinPasswordLength || password.Length > MaxPasswordLength)
                return "Password must be " + MinPasswordLength + " to " + MaxPasswordLength + " characters.";

            if (password != confirm)
                return "The two passwords do not match.";

            try
            {
                if (Database.UserExists(username))
                    return "That username is already taken.";

                Database.AddUser(fullName, username, PasswordHasher.Hash(password));
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
                string stored = Database.GetPasswordHash(username);
                if (stored == null)
                    return "Wrong username or password.";

                if (PasswordHasher.IsHashed(stored))
                    return PasswordHasher.Verify(password, stored) ? null : "Wrong username or password.";

                // Account created before passwords were hashed: check the old way, then upgrade it.
                if (stored != password)
                    return "Wrong username or password.";

                Database.SetPasswordHash(username, PasswordHasher.Hash(password));
                return null;
            }
            catch (Exception ex)
            {
                return "Could not log in: " + ex.Message;
            }
        }

        // The name shown on the Dashboard: the full name, or the username for older accounts
        public static string GetDisplayName(string username)
        {
            string fullName = Database.GetFullName(username);
            return fullName != "" ? fullName : username;
        }

        static bool IsValidUsername(string username)
        {
            foreach (char c in username)
                if (!char.IsLetterOrDigit(c) && c != '.' && c != '_')
                    return false;
            return true;
        }
    }
}
