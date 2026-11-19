# Student Grade Tracker

A Windows Forms desktop application (C#, .NET 8) with a PostgreSQL database.
Teachers register, log in, add students, enter marks, and see each student's
average and Passed/Failed result.

## How to run

1. **Create the database.** In pgAdmin, create a database named `grade_tracker_db`
   (or run the first line of [database.sql](database.sql)).
2. **Check the connection.** [Models/Database.cs](Models/Database.cs) connects to
   `localhost:5432` as user `postgres` with password `jospin123`.
   Change the connection string if your PostgreSQL setup is different.
3. **Run.** Open `StudentGradeTracker.slnx` in Visual Studio 2022
   (".NET desktop development" workload) and press **F5**.
   The Npgsql package downloads automatically and the tables are created on first start.
4. **Register an account** on the login screen, then log in. There is no default login.

## Project structure (MVC)

| Layer | Folder | Files | Role |
|---|---|---|---|
| Model | `Models/` | `Student.cs`, `Grade.cs`, `Database.cs` | Data classes, average + Passed/Failed, all SQL |
| Controller | `Controllers/` | `AuthController.cs`, `StudentController.cs`, `GradeController.cs` | Validate input, then call the Model |
| View | `Views/` | `LoginForm`, `RegisterForm`, `DashboardForm`, `AddStudentForm`, `GradeEntryForm`, `ViewStudentsForm`, `EditStudentForm` | The forms the user sees. They only call Controllers |

Flow: **View** (button click) → **Controller** (checks input) → **Model** (PostgreSQL) → result back to the View.

## Functional requirements

| ID | Requirement | Where |
|---|---|---|
| FR1 | User Access: register with username + password; only registered users can log in; wrong credentials show "Wrong username or password." | `LoginForm`, `RegisterForm` → `AuthController` |
| FR2 | Student Registration: Name, Student ID and Course/Class are required; Student ID must be unique | `AddStudentForm` → `StudentController.AddStudent` |
| FR3 | Grade Entry: choose a student and subject, mark 0–100 only; a new mark for the same subject replaces the old one | `GradeEntryForm` → `GradeController.SaveMark` |
| FR4 | Average and Pass/Fail: average of all marks; "Passed" if average ≥ 50, otherwise "Failed" | `Student.GetAverage()` / `GetStatus()`, shown in `GradeEntryForm` and `ViewStudentsForm` |
| FR5 | Record Management (CRUD): view all students with marks and result, search by name or ID, edit name/course, delete after confirmation | `ViewStudentsForm`, `EditStudentForm` → `StudentController` |

## Database (`grade_tracker_db`)

| Table | Columns |
|---|---|
| `users` | `id`, `username` (unique), `password` |
| `students` | `student_id` (primary key), `name`, `course` |
| `grades` | `id`, `student_id` (foreign key, `ON DELETE CASCADE`), `subject`, `mark` (0–100), unique (`student_id`, `subject`) |

> Note: passwords are stored as plain text to keep the project simple.
> A real system would hash them (for example with BCrypt).
