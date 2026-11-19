namespace StudentGradeTracker.Models
{
    // MODEL: one mark in one subject, e.g. Mathematics = 80 (FR3)
    public class Grade
    {
        public const int MinMark = 0;
        public const int MaxMark = 100;

        public string Subject { get; set; }
        public int    Mark    { get; set; }
    }
}
