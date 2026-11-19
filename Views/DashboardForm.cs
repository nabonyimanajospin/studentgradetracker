using System;
using System.Drawing;
using System.Windows.Forms;

namespace StudentGradeTracker.Views
{
    // The main menu. From here the user opens the other forms.
    public partial class DashboardForm : Form
    {
        public DashboardForm(string username)
        {
            InitializeComponent();

            this.Text            = "Student Grade Tracker – Dashboard";
            this.ClientSize      = new Size(420, 520);
            this.StartPosition   = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox     = false;
            this.BackColor       = Theme.Bg;
            this.Font            = new Font("Segoe UI", 9.5f);

            // ── header panel ──────────────────────────────────────────────
            var pnlHeader = new Panel
            {
                Size      = new Size(420, 100),
                Location  = new Point(0, 0),
                BackColor = Theme.Primary
            };

            var lblApp = new Label
            {
                Text      = "📚 Student Grade Tracker",
                Font      = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize  = false,
                Size      = new Size(380, 36),
                Location  = new Point(20, 18),
                TextAlign = ContentAlignment.MiddleLeft
            };

            var lblWelcome = new Label
            {
                Text      = "Welcome back, " + username + " 👋",
                Font      = new Font("Segoe UI", 10f),
                ForeColor = Color.FromArgb(219, 234, 254),   // blue-100
                AutoSize  = false,
                Size      = new Size(380, 24),
                Location  = new Point(20, 60),
                TextAlign = ContentAlignment.MiddleLeft
            };

            pnlHeader.Controls.Add(lblApp);
            pnlHeader.Controls.Add(lblWelcome);

            // ── section label ─────────────────────────────────────────────
            var lblNav = new Label
            {
                Text      = "NAVIGATION",
                Font      = new Font("Segoe UI", 8f, FontStyle.Bold),
                ForeColor = Theme.Muted,
                AutoSize  = true,
                Location  = new Point(30, 120)
            };

            // ── nav buttons ───────────────────────────────────────────────
            var btnAddStudent = MakeNavButton(
                "👤  Register New Student",
                "Add a new student record",
                Theme.Green, Theme.GreenHv, 160);

            var btnGrades = MakeNavButton(
                "✏  Enter / Update Grades",
                "Record or change marks for a student",
                Theme.Primary, Theme.PrimaryHv, 230);

            var btnView = MakeNavButton(
                "🔍  View, Search & Manage Records",
                "Browse, edit or delete student records",
                Theme.Amber, Theme.AmberHv, 300);

            // ── divider ───────────────────────────────────────────────────
            var divider = new Panel
            {
                Size      = new Size(360, 1),
                Location  = new Point(30, 378),
                BackColor = Color.FromArgb(203, 213, 225)
            };

            // ── logout button ─────────────────────────────────────────────
            var btnLogout = new Button
            {
                Text     = "🔒  Logout",
                Size     = new Size(360, 38),
                Location = new Point(30, 393),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.White,
                ForeColor = Theme.Red,
                Font      = new Font("Segoe UI", 10f),
                Cursor    = Cursors.Hand
            };
            btnLogout.FlatAppearance.BorderColor = Theme.Red;
            btnLogout.FlatAppearance.BorderSize  = 1;
            btnLogout.MouseEnter += (s, e) => { btnLogout.BackColor = Color.FromArgb(254,242,242); };
            btnLogout.MouseLeave += (s, e) => { btnLogout.BackColor = Color.White; };
            btnLogout.Click      += (s, e) => this.Close();

            // ── footer ────────────────────────────────────────────────────
            var lblFooter = new Label
            {
                Text      = "Student Grade Tracker  •  " + DateTime.Now.ToString("dddd, dd MMMM yyyy"),
                Font      = new Font("Segoe UI", 8f),
                ForeColor = Theme.Muted,
                AutoSize  = false,
                Size      = new Size(360, 20),
                Location  = new Point(30, 462),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // ── wire up nav buttons ───────────────────────────────────────
            btnAddStudent.Click += (s, e) =>
            {
                var form = new AddStudentForm();
                this.Hide();
                form.ShowDialog();
                this.Show();
            };
            btnGrades.Click += (s, e) =>
            {
                var form = new GradeEntryForm();
                this.Hide();
                form.ShowDialog();
                this.Show();
            };
            btnView.Click += (s, e) =>
            {
                var form = new ViewStudentsForm();
                this.Hide();
                form.ShowDialog();
                this.Show();
            };

            // ── add everything ────────────────────────────────────────────
            this.Controls.Add(pnlHeader);
            this.Controls.Add(lblNav);
            this.Controls.Add(btnAddStudent);
            this.Controls.Add(btnGrades);
            this.Controls.Add(btnView);
            this.Controls.Add(divider);
            this.Controls.Add(btnLogout);
            this.Controls.Add(lblFooter);
        }

        // Creates a tall nav button with a title line and a subtitle line
        Button MakeNavButton(string title, string subtitle, Color bg, Color hover, int y)
        {
            var b = new Button
            {
                Text      = title + "\n" + subtitle,
                Size      = new Size(360, 55),
                Location  = new Point(30, y),
                FlatStyle = FlatStyle.Flat,
                BackColor = bg,
                ForeColor = Color.White,
                Font      = new Font("Segoe UI", 10f, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding   = new Padding(10, 0, 0, 0),
                Cursor    = Cursors.Hand
            };
            b.FlatAppearance.BorderSize = 0;
            b.MouseEnter += (s, e) => ((Button)s).BackColor = hover;
            b.MouseLeave += (s, e) => ((Button)s).BackColor = bg;
            return b;
        }
    }
}
