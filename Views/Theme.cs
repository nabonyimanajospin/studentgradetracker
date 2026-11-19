using System.Drawing;

namespace StudentGradeTracker.Views
{
    // Colours shared by all forms, so every screen looks the same.
    public static class Theme
    {
        public static readonly Color Primary   = Color.FromArgb(37, 99, 235);   // blue
        public static readonly Color PrimaryHv = Color.FromArgb(29, 78, 216);
        public static readonly Color Green     = Color.FromArgb(22, 163, 74);
        public static readonly Color GreenHv   = Color.FromArgb(15, 118, 55);
        public static readonly Color Amber     = Color.FromArgb(217, 119, 6);
        public static readonly Color AmberHv   = Color.FromArgb(180, 95, 4);
        public static readonly Color Red       = Color.FromArgb(220, 38, 38);
        public static readonly Color RedHv     = Color.FromArgb(185, 28, 28);
        public static readonly Color Bg        = Color.FromArgb(241, 245, 249); // light grey
        public static readonly Color Card      = Color.White;
        public static readonly Color Text      = Color.FromArgb(30, 41, 59);
        public static readonly Color Muted     = Color.FromArgb(100, 116, 139);
        public static readonly Color Input     = Color.FromArgb(248, 250, 252);
        public static readonly Color Error     = Red;
        public static readonly Color Success   = Green;
    }
}
