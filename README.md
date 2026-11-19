# Student Grade Tracker

A Windows Forms desktop application (C#, .NET 8) with a PostgreSQL database.
Teachers register, log in, add students, enter marks, and see each student's
average and Passed/Failed result.

## How to run

**Requirements:** Windows, PostgreSQL (any recent version) running on `localhost:5432`,
and Visual Studio 2022 with the ".NET desktop development" workload (or the .NET 8 SDK).

1. **Set your PostgreSQL password.** Open [Models/Database.cs](Models/Database.cs) and check the
   `connectionString`: it uses user `postgres` with password `jospin123`.
   Change `Username`/`Password` to match your own PostgreSQL installation.
2. **Run.** Open `StudentGradeTracker.sln` in Visual Studio and press **F5**
   (or run `dotnet run` in this folder).
   The Npgsql package downloads automatically (internet needed the first time).
   On first start the app **creates the database `grade_tracker_db` and its tables by itself** —
   nothing has to be created by hand. ([database.sql](database.sql) shows the same tables, for reference.)
3. **Register an account** on the login screen, then log in. There is no default login.

## Project structure (MVC)

| Layer | Folder | Files | Role |
|---|---|---|---|
| Model | `Models/` | `Student.cs`, `Grade.cs`, `Course.cs`, `PasswordHasher.cs`, `Database.cs` | Data classes, average + Passed/Failed, the course list, password hashing, all SQL |
| Controller | `Controllers/` | `AuthController.cs`, `StudentController.cs`, `GradeController.cs` | Validate input, then call the Model |
| View | `Views/` | `LoginForm`, `RegisterForm`, `DashboardForm`, `AddStudentForm`, `GradeEntryForm`, `ViewStudentsForm`, `EditStudentForm` | The forms the user sees. They only call Controllers |

Flow: **View** (button click) → **Controller** (checks input) → **Model** (PostgreSQL) → result back to the View.

## Functional requirements

| ID | Requirement | Where |
|---|---|---|
| FR1 | User Access: register with username + password; only registered users can log in; wrong credentials show "Wrong username or password." | `LoginForm`, `RegisterForm` → `AuthController` |
| FR2 | Student Registration: Name, Student ID and Course/Class are required; Student ID must be unique; the course is chosen from a fixed list | `AddStudentForm` → `StudentController.AddStudent` |
| FR3 | Grade Entry: choose a student, then one of the subjects of their course, mark 0–100 only; a new mark for the same subject replaces the old one. A table shows every subject of the course with its mark or "–" | `GradeEntryForm` → `GradeController.SaveMark` |
| FR4 | Average and Pass/Fail: average of all marks; "Passed" if average ≥ 50, otherwise "Failed" | `Student.GetAverage()` / `GetStatus()`, shown in `GradeEntryForm`, `ViewStudentsForm` (green/red rows) and the Dashboard counts |
| FR5 | Record Management (CRUD): view all students with marks and result, search by name or ID, edit name/course, delete after confirmation | `ViewStudentsForm`, `EditStudentForm` → `StudentController` |

## Courses and subjects

Courses are fixed, and each course has its own subjects. Edit [Models/Course.cs](Models/Course.cs) to change them.

| Course | Subjects |
|---|---|
| DotNet Programming | C# Programming, Windows Forms, Databases (SQL), Mathematics, English |
| Web Development | HTML & CSS, JavaScript, PHP, Databases (SQL), English |
| Networking | Computer Networks, Network Security, Linux, Mathematics, English |
| Software Engineering | Programming Fundamentals, Data Structures, Software Design, Databases (SQL), Mathematics |
| General Studies | Mathematics, Science, English, Kinyarwanda, French, History |

Changing a student's course deletes their old marks (after a confirmation), because the subjects are different.

## Database (`grade_tracker_db`)

| Table | Columns |
|---|---|
| `users` | `id`, `username` (unique), `password` |
| `students` | `student_id` (primary key), `name`, `course` |
| `grades` | `id`, `student_id` (foreign key, `ON DELETE CASCADE`), `subject`, `mark` (0–100), unique (`student_id`, `subject`) |

## Security and robustness

- Passwords are never stored as plain text: they are saved as salted PBKDF2-SHA256 hashes
  ([Models/PasswordHasher.cs](Models/PasswordHasher.cs)).
- All SQL uses parameters (`@name`), so typed text can never change a query (no SQL injection).
- Input is checked twice: in the Controllers (empty fields, lengths, mark range, course and subject lists)
  and in the database (primary keys, unique constraints, `CHECK (mark between 0 and 100)`).
- If PostgreSQL stops while the app is open, the app shows a clear message instead of crashing.
