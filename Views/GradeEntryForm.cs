using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using StudentGradeTracker.Controllers;
using StudentGradeTracker.Models;

namespace StudentGradeTracker.Views
{
    // FR3 - Grade Entry  and  FR4 - Average and Passed/Failed
    public partial class GradeEntryForm : Form
    {
        // left column: choose student, subject and mark
        Panel    pnlHeader      = new Panel();
        Label    lblTitle       = new Label();
        Label    lblStudent     = new Label();
        ComboBox cmbStudent     = new ComboBox();
        Button   btnRefresh     = new Button();
        Panel    pnlStudentInfo = new Panel();
        Label    lblInfoText    = new Label();
        Label    lblSubject     = new Label();
        ComboBox cmbSubject     = new ComboBox();
        Label    lblMark        = new Label();
        TextBox  txtMark        = new TextBox();
        Button   btnSave        = new Button();
        Label    lblHint        = new Label();

        // right column: the student's marks and result
        Label    lblMarksTitle  = new Label();
        ListView listMarks      = new ListView();
        Panel    pnlResult      = new Panel();
        Label    lblResultText  = new Label();

        Button   btnBack        = new Button();

        List<Student> students = new List<Student>();

        // selectStudentId: optional, pre-selects a student (used right after registering one)
        public GradeEntryForm(string selectStudentId = null)
        {
            InitializeComponent();

            this.Text            = "Enter Grades";
            this.ClientSize      = new Size(800, 556);
            this.StartPosition   = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox     = false;
            this.BackColor       = Theme.Bg;
            this.Font            = new Font("Segoe UI", 9.5f);

            // ── header ────────────────────────────────────────────────────
            pnlHeader.Size      = new Size(800, 70);
            pnlHeader.Location  = new Point(0, 0);
            pnlHeader.BackColor = Theme.Primary;

            lblTitle.Text      = "✏  Enter / Update Grades";
            lblTitle.Font      = new Font("Segoe UI", 13, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.AutoSize  = false;
            lblTitle.Size      = new Size(760, 40);
            lblTitle.Location  = new Point(20, 15);
            lblTitle.TextAlign = ContentAlignment.MiddleLeft;
            pnlHeader.Controls.Add(lblTitle);

            // ── student picker ────────────────────────────────────────────
            MakeLabel(lblStudent, "1. Select Student", 30, 90);

            cmbStudent.Location      = new Point(30, 110);
            cmbStudent.Size          = new Size(292, 28);
            cmbStudent.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbStudent.Font          = new Font("Segoe UI", 10f);
            cmbStudent.BackColor     = Theme.Input;
            cmbStudent.SelectedIndexChanged += cmbStudent_SelectedIndexChanged;
            Theme.BlockMouseWheel(cmbStudent);

            btnRefresh.Text      = "⟳";
            btnRefresh.Size      = new Size(42, 28);
            btnRefresh.Location  = new Point(328, 110);
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.BackColor = Color.FromArgb(226, 232, 240);
            btnRefresh.ForeColor = Theme.Text;
            btnRefresh.Font      = new Font("Segoe UI", 11f);
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.Cursor    = Cursors.Hand;
            btnRefresh.Click    += (s, e) => LoadStudents();
            new ToolTip().SetToolTip(btnRefresh, "Refresh student list");

            // ── student info panel ────────────────────────────────────────
            pnlStudentInfo.Size        = new Size(340, 62);
            pnlStudentInfo.Location    = new Point(30, 146);
            pnlStudentInfo.BackColor   = Color.FromArgb(239, 246, 255);
            pnlStudentInfo.BorderStyle = BorderStyle.FixedSingle;
            pnlStudentInfo.Visible     = false;

            lblInfoText.Font      = new Font("Segoe UI", 9f);
            lblInfoText.ForeColor = Theme.Primary;
            lblInfoText.AutoSize  = false;
            lblInfoText.Size      = new Size(330, 56);
            lblInfoText.Location  = new Point(5, 2);
            lblInfoText.TextAlign = ContentAlignment.MiddleLeft;
            lblInfoText.UseMnemonic = false;
            pnlStudentInfo.Controls.Add(lblInfoText);

            // ── subject picker ────────────────────────────────────────────
            MakeLabel(lblSubject, "2. Subject (from the student's course)", 30, 222);

            cmbSubject.Location      = new Point(30, 242);
            cmbSubject.Size          = new Size(340, 28);
            cmbSubject.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSubject.Font          = new Font("Segoe UI", 10f);
            cmbSubject.BackColor     = Theme.Input;
            cmbSubject.SelectedIndexChanged += cmbSubject_SelectedIndexChanged;
            Theme.BlockMouseWheel(cmbSubject);

            // ── mark ──────────────────────────────────────────────────────
            MakeLabel(lblMark, "3. Mark (0 – 100)", 30, 285);

            txtMark.Location        = new Point(30, 305);
            txtMark.Size            = new Size(340, 30);
            txtMark.Font            = new Font("Segoe UI", 10f);
            txtMark.BackColor       = Theme.Input;
            txtMark.BorderStyle     = BorderStyle.FixedSingle;
            txtMark.ForeColor       = Theme.Text;
            txtMark.MaxLength       = 3;
            txtMark.PlaceholderText = "Enter a whole number between 0 and 100";

            // ── save button ───────────────────────────────────────────────
            btnSave.Text      = "💾  Save Mark";
            btnSave.Size      = new Size(340, 40);
            btnSave.Location  = new Point(30, 350);
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.BackColor = Theme.Primary;
            btnSave.ForeColor = Color.White;
            btnSave.Font      = new Font("Segoe UI", 10f, FontStyle.Bold);
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Cursor    = Cursors.Hand;
            btnSave.MouseEnter += (s, e) => ((Button)s).BackColor = Theme.PrimaryHv;
            btnSave.MouseLeave += (s, e) => ((Button)s).BackColor = Theme.Primary;
            btnSave.Click      += btnSave_Click;

            lblHint.Text      = "If the subject already has a mark, it is shown above.\nSaving a new mark replaces it.";
            lblHint.Font      = new Font("Segoe UI", 8.5f);
            lblHint.ForeColor = Theme.Muted;
            lblHint.AutoSize  = false;
            lblHint.Size      = new Size(340, 36);
            lblHint.Location  = new Point(30, 398);

            // ── marks list (right column) ─────────────────────────────────
            MakeLabel(lblMarksTitle, "Marks for this student", 400, 90);

            listMarks.Location      = new Point(400, 110);
            listMarks.Size          = new Size(370, 250);
            listMarks.View          = View.Details;
            listMarks.FullRowSelect = true;
            listMarks.GridLines     = true;
            listMarks.MultiSelect   = false;
            listMarks.HideSelection = false;
            listMarks.Font          = new Font("Segoe UI", 9.5f);
            listMarks.Columns.Add("Subject", 260);
            listMarks.Columns.Add("Mark", 90);
            listMarks.SelectedIndexChanged += listMarks_SelectedIndexChanged;

            // ── result panel ──────────────────────────────────────────────
            pnlResult.Size        = new Size(370, 82);
            pnlResult.Location    = new Point(400, 372);
            pnlResult.BackColor   = Color.White;
            pnlResult.BorderStyle = BorderStyle.FixedSingle;

            lblResultText.Font      = new Font("Segoe UI", 10f, FontStyle.Bold);
            lblResultText.AutoSize  = false;
            lblResultText.Size      = new Size(360, 74);
            lblResultText.Location  = new Point(4, 3);
            lblResultText.TextAlign = ContentAlignment.MiddleCenter;
            pnlResult.Controls.Add(lblResultText);

            // ── back button ───────────────────────────────────────────────
            btnBack.Text      = "← Back to Dashboard";
            btnBack.Size      = new Size(740, 36);
            btnBack.Location  = new Point(30, 500);
            btnBack.FlatStyle = FlatStyle.Flat;
            btnBack.BackColor = Color.White;
            btnBack.ForeColor = Theme.Primary;
            btnBack.Font      = new Font("Segoe UI", 9.5f);
            btnBack.FlatAppearance.BorderColor = Theme.Primary;
            btnBack.FlatAppearance.BorderSize  = 1;
            btnBack.Cursor    = Cursors.Hand;
            btnBack.MouseEnter += (s, e) => ((Button)s).BackColor = Color.FromArgb(239, 246, 255);
            btnBack.MouseLeave += (s, e) => ((Button)s).BackColor = Color.White;
            btnBack.Click      += (s, e) => this.Close();

            this.Controls.Add(pnlHeader);
            this.Controls.Add(lblStudent);
            this.Controls.Add(cmbStudent);
            this.Controls.Add(btnRefresh);
            this.Controls.Add(pnlStudentInfo);
            this.Controls.Add(lblSubject);
            this.Controls.Add(cmbSubject);
            this.Controls.Add(lblMark);
            this.Controls.Add(txtMark);
            this.Controls.Add(btnSave);
            this.Controls.Add(lblHint);
            this.Controls.Add(lblMarksTitle);
            this.Controls.Add(listMarks);
            this.Controls.Add(pnlResult);
            this.Controls.Add(btnBack);

            this.AcceptButton = btnSave;

            LoadStudents(selectStudentId);
            if (SelectedStudent() != null)
                this.ActiveControl = cmbSubject;
        }

        // Fills the student dropdown. Keeps (or sets) the selected student if possible.
        void LoadStudents(string selectId = null)
        {
            if (selectId == null && SelectedStudent() != null)
                selectId = SelectedStudent().Id;

            students = StudentController.GetStudents("");
            cmbStudent.Items.Clear();
            foreach (Student s in students)
                cmbStudent.Items.Add(s.Id + "  –  " + s.Name + "  (" + s.Course + ")");

            if (students.Count == 0)
                cmbStudent.Items.Add("(No students registered yet)");

            int index = students.FindIndex(st => st.Id == selectId);
            if (index >= 0)
                cmbStudent.SelectedIndex = index;
            else
                ShowNoStudent();
        }

        // The selected student, or null if none is selected
        Student SelectedStudent()
        {
            bool valid = cmbStudent.SelectedIndex >= 0 && cmbStudent.SelectedIndex < students.Count;
            return valid ? students[cmbStudent.SelectedIndex] : null;
        }

        void cmbStudent_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbSubject.Items.Clear();
            txtMark.Clear();

            Student s = SelectedStudent();
            if (s == null) { ShowNoStudent(); return; }

            s.Grades = GradeController.GetGrades(s.Id);

            string[] subjects = GradeController.GetSubjects(s);
            cmbSubject.Items.AddRange(subjects);

            lblInfoText.Text = "ID: " + s.Id + "\nName: " + s.Name + "\nCourse: " + s.Course;
            if (subjects.Length == 0)
                lblInfoText.Text = "This student's course has no subjects.\nEdit the student and choose a course from the list.";
            pnlStudentInfo.Visible = true;

            ShowMarks(s);
        }

        // Shows the current mark (if any) so the user knows they are replacing it
        void cmbSubject_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtMark.Clear();
            Student s = SelectedStudent();
            if (s == null) return;

            foreach (Grade g in s.Grades)
                if (g.Subject == cmbSubject.Text)
                    txtMark.Text = g.Mark.ToString();

            // keep the marks list in step with the chosen subject
            foreach (ListViewItem item in listMarks.Items)
                item.Selected = item.Text == cmbSubject.Text;
        }

        // Clicking a subject in the marks list chooses it for editing
        void listMarks_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listMarks.SelectedItems.Count == 0) return;

            string subject = listMarks.SelectedItems[0].Text;
            if (cmbSubject.Items.Contains(subject) && cmbSubject.Text != subject)
            {
                cmbSubject.SelectedItem = subject;
                txtMark.Focus();
                txtMark.SelectAll();
            }
        }

        void btnSave_Click(object sender, EventArgs e)
        {
            Student s = SelectedStudent();
            string subject = cmbSubject.SelectedIndex == -1 ? null : cmbSubject.Text;

            string error = GradeController.SaveMark(s, subject, txtMark.Text);
            if (error != null)
            { ShowMsg("⚠  " + error); return; }

            s.Grades = GradeController.GetGrades(s.Id);
            ShowMarks(s);
            cmbSubject_SelectedIndexChanged(null, EventArgs.Empty);   // re-select the saved row
        }

        void ShowNoStudent()
        {
            pnlStudentInfo.Visible = false;
            listMarks.Items.Clear();
            pnlResult.BackColor     = Color.White;
            lblResultText.ForeColor = Theme.Muted;
            lblResultText.Text      = students.Count == 0
                ? "No students registered yet."
                : "Select a student to see their marks.";
        }

        // FR4: fills the marks list and shows the average and Passed/Failed
        void ShowMarks(Student s)
        {
            listMarks.Items.Clear();

            int marked = 0;
            string[] subjects = GradeController.GetSubjects(s);
            foreach (string subject in subjects)
            {
                Grade grade = s.Grades.Find(g => g.Subject == subject);
                var item = new ListViewItem(subject);
                if (grade != null)
                {
                    item.SubItems.Add(grade.Mark.ToString());
                    marked++;
                }
                else
                {
                    item.SubItems.Add("–");
                    item.ForeColor = Theme.Muted;
                }
                listMarks.Items.Add(item);
            }

            if (s.Grades.Count == 0)
            {
                pnlResult.BackColor     = Color.FromArgb(248, 250, 252);
                lblResultText.ForeColor = Theme.Muted;
                lblResultText.Text      = "No marks entered yet for " + s.Name + ".";
                return;
            }

            bool passed = s.HasPassed();
            pnlResult.BackColor     = passed ? Color.FromArgb(240, 253, 244) : Color.FromArgb(254, 242, 242);
            lblResultText.ForeColor = passed ? Theme.Green : Theme.Red;
            lblResultText.Text =
                "Average: " + s.GetAverage().ToString("0.0") +
                "   (" + marked + " of " + subjects.Length + " subjects marked)\n" +
                (passed ? "✔  " : "✖  ") + s.GetStatus().ToUpper() +
                "   (pass mark " + Student.PassMark + ")";
        }

        void MakeLabel(Label lbl, string text, int x, int y)
        {
            lbl.Text      = text;
            lbl.Font      = new Font("Segoe UI", 9f, FontStyle.Bold);
            lbl.ForeColor = Theme.Text;
            lbl.AutoSize  = true;
            lbl.Location  = new Point(x, y);
        }

        void ShowMsg(string msg)
        {
            MessageBox.Show(msg, "Grade Entry", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
