using System;
using System.Windows.Forms;
using StudentGradeTracker.Models;
using StudentGradeTracker.Views;

namespace StudentGradeTracker
{
    // The program starts here.
    // It checks the database connection, then opens the Login form.
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Any unexpected error (e.g. PostgreSQL stops while the app is open)
            // shows a clear message instead of crashing the app.
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += (s, e) =>
                MessageBox.Show(
                    "Something went wrong:\n\n" + e.Exception.Message +
                    "\n\nCheck that PostgreSQL is running, then try again.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            // Step 1: can we connect to PostgreSQL?
            string error;
            if (Database.TestConnection(out error) == false)
            {
                MessageBox.Show(
                    "Cannot connect to PostgreSQL.\n\n" + error +
                    "\n\nCheck that PostgreSQL is running on localhost:5432, and that the\n" +
                    "username and password in Models\\Database.cs (connectionString)\n" +
                    "match your PostgreSQL installation.",
                    "Database error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;     // stop the program
            }

            // Step 2: create the tables if they do not exist yet
            Database.CreateTables();

            // Step 3: open the first form
            Application.Run(new LoginForm());
        }
    }
}
