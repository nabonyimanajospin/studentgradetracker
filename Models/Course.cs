using System.Collections.Generic;

namespace StudentGradeTracker.Models
{
    // MODEL: the fixed list of courses and the subjects taught in each one.
    // To add or change a course, edit this list only.
    public static class Course
    {
        static readonly Dictionary<string, string[]> subjectsByCourse = new Dictionary<string, string[]>
        {
            { "DotNet Programming",   new[] { "C# Programming", "Windows Forms", "Databases (SQL)", "Mathematics", "English" } },
            { "Web Development",      new[] { "HTML & CSS", "JavaScript", "PHP", "Databases (SQL)", "English" } },
            { "Networking",           new[] { "Computer Networks", "Network Security", "Linux", "Mathematics", "English" } },
            { "Software Engineering", new[] { "Programming Fundamentals", "Data Structures", "Software Design", "Databases (SQL)", "Mathematics" } },
            { "General Studies",      new[] { "Mathematics", "Science", "English", "Kinyarwanda", "French", "History" } },
        };

        public static string[] GetAll()
        {
            return new List<string>(subjectsByCourse.Keys).ToArray();
        }

        public static bool Exists(string course)
        {
            return course != null && subjectsByCourse.ContainsKey(course);
        }

        // Returns the subjects of a course (empty if the course is not in the list)
        public static string[] GetSubjects(string course)
        {
            return Exists(course) ? subjectsByCourse[course] : new string[0];
        }
    }
}
