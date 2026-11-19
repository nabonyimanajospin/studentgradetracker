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

            // Step 1: can we connect to PostgreSQL?
            string error;
            if (Database.TestConnection(out error) == false)
            {
                MessageBox.Show(
                    "Cannot connect to the database 'grade_tracker_db'.\n\n" + error +
                    "\n\nCheck that PostgreSQL is running, the database exists,\n" +
                    "and the password in Database.cs is correct.",
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
