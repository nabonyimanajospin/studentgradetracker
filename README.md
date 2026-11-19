# Student Grade Tracker

**Course:** DotNet (.NET)
**Project type:** Windows Forms desktop application
**Language / Database:** C# (.NET 8) with PostgreSQL

## About the project

Student Grade Tracker helps teachers record and manage student grades in one place
instead of on paper. A teacher logs in, registers students, enters their marks, and
the system instantly shows each student's average and whether they **Passed** or **Failed**.

## Functional requirements

| ID | Requirement | What the system does |
|---|---|---|
| FR1 | User Access (Login and Registration) | A teacher or administrator creates a **staff account** (full name, username, password). Only registered staff can log in. Wrong details show *"Wrong username or password."* |
| FR2 | Student Registration | Add a student with **Name**, **Student ID** and **Course/Class**. No field can be empty and the Student ID must be unique. |
| FR3 | Grade Entry | Choose a student and a subject, then enter a mark from **0 to 100**. Marks outside this range are rejected. Entering a new mark for the same subject replaces the old one. |
| FR4 | Average and Pass/Fail | The system calculates the student's average automatically. **50 or more = Passed**, below 50 = Failed. |
| FR5 | Record Management (CRUD) | View all students with their marks and results, search by name or ID, edit a student's name or course, and delete a student after confirmation. |

## Main screens

- **Login / Create Staff Account:** teachers sign in or create their account
  (students do not log in; they are records added by teachers)
- **Dashboard:** menu, plus totals of students who passed, failed or have no marks yet
- **Register Student:** add a new student and choose their course
- **Enter Grades:** enter marks; see every subject, the average and the result
- **View Records:** list, search, edit and delete students

Each course has its own subjects (for example, *DotNet Programming*: C# Programming,
Windows Forms, Databases (SQL), Mathematics, English). The list is in `Models/Course.cs`.

## How to run

You need **PostgreSQL** and **Visual Studio 2022 or newer** (with ".NET desktop development").

1. Open `Models/Database.cs` and set your PostgreSQL password in the connection string
   (it is currently `jospin123`).
2. Open `StudentGradeTracker.sln` in Visual Studio and press **F5**.
3. Click **Create a staff account**, then log in. Add students with **Register New Student**.

The app creates the database `grade_tracker_db` and its tables automatically on first start.

## Project structure (MVC)

```
Models/        Student, Grade, Course, PasswordHasher, Database (all SQL)
Controllers/   AuthController, StudentController, GradeController (checks and rules)
Views/         The forms: Login, Register, Dashboard, Add Student,
               Enter Grades, View Students, Edit Student
```

The **Views** (forms) only talk to the **Controllers**, and only the **Models** talk to PostgreSQL.

## Database

| Table | Stores |
|---|---|
| `users` | staff accounts: full name, username, password (saved as a secure hash, never as plain text) |
| `students` | student ID, name and course |
| `grades` | each student's mark per subject (0–100) |
