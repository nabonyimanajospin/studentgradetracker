using System;
using System.Collections.Generic;
using Npgsql;   // the PostgreSQL package

namespace StudentGradeTracker.Models
{
    // MODEL (data access): the ONLY place that talks to PostgreSQL.
    // Controllers call this class; Views never call it directly.
    //
    // Tables used:
    //   users     (id, username, password)
    //   students  (student_id, name, course)
    //   grades    (id, student_id, subject, mark)
    //
    // Passwords are stored as salted hashes (see PasswordHasher), never as plain text.
    public static class Database
    {
        // Connection string for the local grade-tracker database.
        static string connectionString =
            "Host=localhost;Port=5432;Username=postgres;Password=jospin123;Database=grade_tracker_db";

        // Returns a new (un-opened) connection object.
        public static NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(connectionString);
        }

        // Creates the database (grade_tracker_db) if it does not exist yet,
        // so the app works on a new computer with only PostgreSQL installed.
        static void CreateDatabaseIfMissing()
        {
            var builder = new NpgsqlConnectionStringBuilder(connectionString);
            string dbName = builder.Database;
            builder.Database = "postgres";   // the default database that always exists

            using (NpgsqlConnection conn = new NpgsqlConnection(builder.ConnectionString))
            {
                conn.Open();
                using (NpgsqlCommand check = new NpgsqlCommand("SELECT 1 FROM pg_database WHERE datname = @name", conn))
                {
                    check.Parameters.AddWithValue("@name", dbName);
                    if (check.ExecuteScalar() != null) return;
                }

                // A database name cannot be a parameter; it comes from our own connection string above.
                using (NpgsqlCommand create = new NpgsqlCommand("CREATE DATABASE \"" + dbName + "\"", conn))
                    create.ExecuteNonQuery();
            }
        }

        // Tries to open a connection (creating the database first if needed).
        // Returns false and the error text if it fails.
        public static bool TestConnection(out string error)
        {
            try
            {
                CreateDatabaseIfMissing();
                using (NpgsqlConnection conn = GetConnection())
                {
                    conn.Open();
                    error = "";
                    return true;
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        // Creates the three tables if they do not exist yet.
        public static void CreateTables()
        {
            string sql =
                "CREATE TABLE IF NOT EXISTS users (" +
                "  id        SERIAL PRIMARY KEY," +
                "  username  VARCHAR(50)  UNIQUE NOT NULL," +
                "  password  VARCHAR(100) NOT NULL," +
                "  full_name VARCHAR(100) NOT NULL DEFAULT '');" +
                // databases made by an older version of the app get the new column too
                "ALTER TABLE users ADD COLUMN IF NOT EXISTS full_name VARCHAR(100) NOT NULL DEFAULT '';" +

                "CREATE TABLE IF NOT EXISTS students (" +
                "  student_id VARCHAR(30)  PRIMARY KEY," +
                "  name       VARCHAR(100) NOT NULL," +
                "  course     VARCHAR(100) NOT NULL);" +

                // ON DELETE CASCADE: deleting a student also deletes all their grades
                "CREATE TABLE IF NOT EXISTS grades (" +
                "  id         SERIAL PRIMARY KEY," +
                "  student_id VARCHAR(30) NOT NULL REFERENCES students(student_id) ON DELETE CASCADE," +
                "  subject    VARCHAR(50) NOT NULL," +
                "  mark       INT         NOT NULL CHECK (mark >= 0 AND mark <= 100)," +
                "  UNIQUE (student_id, subject));";

            using (NpgsqlConnection conn = GetConnection())
            {
                conn.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand(sql, conn))
                    cmd.ExecuteNonQuery();
            }
        }

        // =====================================================
        //  USERS  (FR1 – Login and Registration)
        // =====================================================

        // Returns true if the username is already registered (case-insensitive).
        public static bool UserExists(string username)
        {
            using (NpgsqlConnection conn = GetConnection())
            {
                conn.Open();
                string sql = "SELECT COUNT(*) FROM users WHERE LOWER(username) = LOWER(@u)";
                using (NpgsqlCommand cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@u", username);
                    return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                }
            }
        }

        // Saves a new staff account (passwordHash comes from PasswordHasher.Hash).
        public static void AddUser(string fullName, string username, string passwordHash)
        {
            using (NpgsqlConnection conn = GetConnection())
            {
                conn.Open();
                string sql = "INSERT INTO users (full_name, username, password) VALUES (@n, @u, @p)";
                using (NpgsqlCommand cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@n", fullName);
                    cmd.Parameters.AddWithValue("@u", username);
                    cmd.Parameters.AddWithValue("@p", passwordHash);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Returns the full name of a user ("" if none was saved).
        public static string GetFullName(string username)
        {
            using (NpgsqlConnection conn = GetConnection())
            {
                conn.Open();
                string sql = "SELECT full_name FROM users WHERE LOWER(username) = LOWER(@u)";
                using (NpgsqlCommand cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@u", username);
                    return (cmd.ExecuteScalar() as string) ?? "";
                }
            }
        }

        // Returns the stored password hash of a user, or null if the username does not exist.
        // The username check is case-insensitive.
        public static string GetPasswordHash(string username)
        {
            using (NpgsqlConnection conn = GetConnection())
            {
                conn.Open();
                string sql = "SELECT password FROM users WHERE LOWER(username) = LOWER(@u)";
                using (NpgsqlCommand cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@u", username);
                    return cmd.ExecuteScalar() as string;
                }
            }
        }

        // Replaces the stored password hash of a user.
        public static void SetPasswordHash(string username, string passwordHash)
        {
            using (NpgsqlConnection conn = GetConnection())
            {
                conn.Open();
                string sql = "UPDATE users SET password = @p WHERE LOWER(username) = LOWER(@u)";
                using (NpgsqlCommand cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@u", username);
                    cmd.Parameters.AddWithValue("@p", passwordHash);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // =====================================================
        //  STUDENTS  (FR2 and FR5 – CRUD)
        // =====================================================

        // Returns true if a student with this ID already exists (case-insensitive).
        public static bool StudentExists(string id)
        {
            using (NpgsqlConnection conn = GetConnection())
            {
                conn.Open();
                string sql = "SELECT COUNT(*) FROM students WHERE LOWER(student_id) = LOWER(@id)";
                using (NpgsqlCommand cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                }
            }
        }

        // Inserts a new student.
        public static void AddStudent(string id, string name, string course)
        {
            using (NpgsqlConnection conn = GetConnection())
            {
                conn.Open();
                string sql = "INSERT INTO students (student_id, name, course) VALUES (@id, @name, @course)";
                using (NpgsqlCommand cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id",     id);
                    cmd.Parameters.AddWithValue("@name",   name);
                    cmd.Parameters.AddWithValue("@course", course);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Returns all students (with their grades) whose name or ID contains searchText.
        // If searchText is empty, every student is returned.
        // One JOIN query loads the students and their grades together.
        public static List<Student> GetStudents(string searchText)
        {
            var dict = new Dictionary<string, Student>(
                StringComparer.OrdinalIgnoreCase);
            var order = new List<string>();   // preserves alphabetical order

            using (NpgsqlConnection conn = GetConnection())
            {
                conn.Open();

                // One query brings back students + their grades in one round-trip.
                string sql =
                    "SELECT s.student_id, s.name, s.course, g.subject, g.mark " +
                    "FROM students s " +
                    "LEFT JOIN grades g ON g.student_id = s.student_id " +
                    "WHERE POSITION(LOWER(@search) IN LOWER(s.name)) > 0 " +
                    "   OR POSITION(LOWER(@search) IN LOWER(s.student_id)) > 0 " +
                    "ORDER BY s.name, g.subject";

                using (NpgsqlCommand cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@search", searchText);

                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string sid = reader.GetString(0);

                            if (!dict.ContainsKey(sid))
                            {
                                var s = new Student
                                {
                                    Id     = sid,
                                    Name   = reader.GetString(1),
                                    Course = reader.GetString(2)
                                };
                                dict[sid] = s;
                                order.Add(sid);
                            }

                            // LEFT JOIN: subject column is null when student has no grades yet
                            if (!reader.IsDBNull(3))
                            {
                                dict[sid].Grades.Add(new Grade
                                {
                                    Subject = reader.GetString(3),
                                    Mark    = reader.GetInt32(4)
                                });
                            }
                        }
                    }
                }
            }

            var list = new List<Student>();
            foreach (string sid in order)
                list.Add(dict[sid]);

            return list;
        }

        // Updates the name and course of a student (ID cannot change).
        public static void UpdateStudent(string id, string name, string course)
        {
            using (NpgsqlConnection conn = GetConnection())
            {
                conn.Open();
                string sql = "UPDATE students SET name = @name, course = @course WHERE student_id = @id";
                using (NpgsqlCommand cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@name",   name);
                    cmd.Parameters.AddWithValue("@course", course);
                    cmd.Parameters.AddWithValue("@id",     id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Deletes a student; their grades are removed automatically (ON DELETE CASCADE).
        public static void DeleteStudent(string id)
        {
            using (NpgsqlConnection conn = GetConnection())
            {
                conn.Open();
                string sql = "DELETE FROM students WHERE student_id = @id";
                using (NpgsqlCommand cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // =====================================================
        //  GRADES  (FR3 – Grade Entry)
        // =====================================================

        // Returns all grades for one student, ordered by subject name.
        public static List<Grade> GetGrades(string studentId)
        {
            var list = new List<Grade>();

            using (NpgsqlConnection conn = GetConnection())
            {
                conn.Open();
                string sql = "SELECT subject, mark FROM grades WHERE student_id = @id ORDER BY subject";
                using (NpgsqlCommand cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", studentId);
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                            list.Add(new Grade
                            {
                                Subject = reader.GetString(0),
                                Mark    = reader.GetInt32(1)
                            });
                    }
                }
            }
            return list;
        }

        // Inserts or updates a grade for one subject.
        // Removes all marks of one student (used when the student's course changes).
        public static void DeleteGrades(string studentId)
        {
            using (NpgsqlConnection conn = GetConnection())
            {
                conn.Open();
                string sql = "DELETE FROM grades WHERE student_id = @id";
                using (NpgsqlCommand cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", studentId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // FR3: a mark for a subject that already has one REPLACES it
        // (PostgreSQL INSERT ... ON CONFLICT DO UPDATE).
        public static void SaveMark(string studentId, string subject, int mark)
        {
            using (NpgsqlConnection conn = GetConnection())
            {
                conn.Open();
                string sql =
                    "INSERT INTO grades (student_id, subject, mark) VALUES (@id, @subject, @mark) " +
                    "ON CONFLICT (student_id, subject) DO UPDATE SET mark = EXCLUDED.mark";

                using (NpgsqlCommand cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id",      studentId);
                    cmd.Parameters.AddWithValue("@subject", subject);
                    cmd.Parameters.AddWithValue("@mark",    mark);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
