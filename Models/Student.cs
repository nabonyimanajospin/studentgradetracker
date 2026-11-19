using System.Collections.Generic;

namespace StudentGradeTracker.Models
{
    // MODEL: one student with their marks (FR2, FR4)
    public class Student
    {
        public string      Id     { get; set; }
        public string      Name   { get; set; }
        public string      Course { get; set; }
        public List<Grade> Grades { get; set; } = new List<Grade>();

        // FR4: predefined pass mark of 50%
        public const int PassMark = 50;

        // Same sizes as the database columns
        public const int MaxIdLength   = 30;
        public const int MaxNameLength = 100;

        // FR4: average of all marks (0 when no marks have been entered)
        public double GetAverage()
        {
            if (Grades.Count == 0) return 0.0;

            double total = 0;
            foreach (Grade g in Grades)
                total += g.Mark;

            return total / Grades.Count;
        }

        public bool HasPassed()
        {
            return Grades.Count > 0 && GetAverage() >= PassMark;
        }

        // FR4: "Passed", "Failed", or "No marks"
        public string GetStatus()
        {
            if (Grades.Count == 0) return "No marks";
            return HasPassed() ? "Passed" : "Failed";
        }
    }
}
