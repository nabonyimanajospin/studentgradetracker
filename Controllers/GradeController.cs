using System;
using System.Collections.Generic;
using StudentGradeTracker.Models;

namespace StudentGradeTracker.Controllers
{
    // CONTROLLER for FR3 - Grade Entry (FR4 is calculated by the Student model).
    public static class GradeController
    {
        // Subjects of the student's course
        public static string[] GetSubjects(Student student)
        {
            return student == null ? new string[0] : Course.GetSubjects(student.Course);
        }

        public static List<Grade> GetGrades(string studentId)
        {
            return Database.GetGrades(studentId);
        }

        // Validates and saves a mark (0-100). An existing mark for the subject is replaced.
        // Returns null on success, or an error message.
        public static string SaveMark(Student student, string subject, string markText)
        {
            if (student == null)
                return "Please choose a student. Register students first if the list is empty.";

            if (string.IsNullOrEmpty(subject))
                return "Please choose a subject.";

            if (Array.IndexOf(GetSubjects(student), subject) < 0)
                return "\"" + subject + "\" is not a subject of " + student.Course + ".";

            int mark;
            if (!int.TryParse((markText ?? "").Trim(), out mark))
                return "The mark must be a whole number.";

            if (mark < Grade.MinMark || mark > Grade.MaxMark)
                return "The mark must be between " + Grade.MinMark + " and " + Grade.MaxMark + ".";

            try
            {
                Database.SaveMark(student.Id, subject, mark);
                return null;
            }
            catch (Exception ex)
            {
                return "Could not save the mark: " + ex.Message;
            }
        }
    }
}
