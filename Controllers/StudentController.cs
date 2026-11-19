using System;
using System.Collections.Generic;
using StudentGradeTracker.Models;

namespace StudentGradeTracker.Controllers
{
    // CONTROLLER for FR2 - Student Registration and FR5 - Record Management (CRUD).
    // Methods that change data return null on success, or an error message.
    public static class StudentController
    {
        // FR2 (Create): all fields required, Student ID must be unique
        public static string AddStudent(string id, string name, string course)
        {
            id     = (id     ?? "").Trim();
            name   = (name   ?? "").Trim();
            course = (course ?? "").Trim();

            if (name == "" || id == "" || course == "")
                return "Please fill in Name, Student ID and Course/Class.";

            try
            {
                if (Database.StudentExists(id))
                    return "A student with ID \"" + id + "\" already exists.";

                Database.AddStudent(id, name, course);
                return null;
            }
            catch (Exception ex)
            {
                return "Could not save the student: " + ex.Message;
            }
        }

        // FR5 (Read + Search): all students with marks, filtered by name or ID
        public static List<Student> GetStudents(string searchText)
        {
            return Database.GetStudents((searchText ?? "").Trim());
        }

        // FR5 (Update): change name or course; the ID stays the same
        public static string UpdateStudent(string id, string name, string course)
        {
            name   = (name   ?? "").Trim();
            course = (course ?? "").Trim();

            if (name == "" || course == "")
                return "Name and Course/Class cannot be empty.";

            try
            {
                Database.UpdateStudent(id, name, course);
                return null;
            }
            catch (Exception ex)
            {
                return "Could not update the student: " + ex.Message;
            }
        }

        // FR5 (Delete): the View asks for confirmation before calling this
        public static string DeleteStudent(string id)
        {
            try
            {
                Database.DeleteStudent(id);
                return null;
            }
            catch (Exception ex)
            {
                return "Could not delete the student: " + ex.Message;
            }
        }
    }
}
