using System;
using System.Collections.Generic;
using StudentGradeTracker.Models;

namespace StudentGradeTracker.Controllers
{
    // CONTROLLER for FR3 - Grade Entry (FR4 is calculated by the Student model).
    public static class GradeController
    {
        public static readonly string[] Subjects =
        {
            "Mathematics", "Science", "English", "Kinyarwanda", "French", "History"
        };

        public static List<Grade> GetGrades(string studentId)
        {
            return Database.GetGrades(studentId);
        }

        // Validates and saves a mark (0-100). An existing mark for the subject is replaced.
        // Returns null on success, or an error message.
        public static string SaveMark(string studentId, string subject, string markText)
        {
            if (string.IsNullOrEmpty(studentId))
                return "Please choose a student. Register students first if the list is empty.";

            if (string.IsNullOrEmpty(subject))
                return "Please choose a subject.";

            int mark;
            if (!int.TryParse((markText ?? "").Trim(), out mark))
                return "The mark must be a whole number.";

            if (mark < Grade.MinMark || mark > Grade.MaxMark)
                return "The mark must be between " + Grade.MinMark + " and " + Grade.MaxMark + ".";

            try
            {
                Database.SaveMark(studentId, subject, mark);
                return null;
            }
            catch (Exception ex)
            {
                return "Could not save the mark: " + ex.Message;
            }
        }
    }
}
