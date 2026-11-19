-- Run this in pgAdmin (Query Tool) if you want to create the database by hand.
-- Step 1: run this line on its own while connected to the default "postgres" database:
CREATE DATABASE grade_tracker_db;

-- Step 2: connect to grade_tracker_db, then run the rest.
-- (The application also creates these tables automatically on start, so Step 2 is optional.)

CREATE TABLE IF NOT EXISTS users (
    id SERIAL PRIMARY KEY,
    username VARCHAR(50) UNIQUE NOT NULL,
    password VARCHAR(100) NOT NULL
);

CREATE TABLE IF NOT EXISTS students (
    student_id VARCHAR(30) PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    course VARCHAR(100) NOT NULL
);

CREATE TABLE IF NOT EXISTS grades (
    id SERIAL PRIMARY KEY,
    student_id VARCHAR(30) NOT NULL REFERENCES students(student_id) ON DELETE CASCADE,
    subject VARCHAR(50) NOT NULL,
    mark INT NOT NULL CHECK (mark >= 0 AND mark <= 100),
    UNIQUE (student_id, subject)
);
