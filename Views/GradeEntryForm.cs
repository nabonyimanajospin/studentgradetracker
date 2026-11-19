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
        Panel   pnlHeader       = new Panel();
        Label   lblTitle        = new Label();
        Label   lblStudent      = new Label();
        ComboBox cmbStudent     = new ComboBox();
        Button  btnRefresh      = new Button();
        Panel   pnlStudentInfo  = new Panel();
        Label   lblInfoText     = new Label();
        Label   lblSubject      = new Label();
        ComboBox cmbSubject     = new ComboBox();
        Label   lblMark         = new Label();
        TextBox txtMark         = new TextBox();
        Button  btnSave         = new Button();
        Panel   pnlResult       = new Panel();
        Label   lblResultText   = new Label();
        Button  btnBack         = new Button();

        List<Student> students;

        // selectStudentId: optional, pre-selects a student (used right after registering one)
        public GradeEntryForm(string selectStudentId = null)
        {
            InitializeComponent();

            this.Text            = "Enter Grades";
            this.ClientSize      = new Size(460, 580);
            this.StartPosition   = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox     = false;
            this.BackColor       = Theme.Bg;
            this.Font            = new Font("Segoe UI", 9.5f);

            // ── header ────────────────────────────────────────────────────
            pnlHeader.Size      = new Size(460, 70);
            pnlHeader.Location  = new Point(0, 0);
            pnlHeader.BackColor = Theme.Primary;

            lblTitle.Text      = "✏  Enter / Update Grades";
            lblTitle.Font      = new Font("Segoe UI", 13, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.AutoSize  = false;
            lblTitle.Size      = new Size(420, 40);
            lblTitle.Location  = new Point(20, 15);
            lblTitle.TextAlign = ContentAlignment.MiddleLeft;
            pnlHeader.Controls.Add(lblTitle);

            // ── student picker ────────────────────────────────────────────
            lblStudent.Text      = "Select Student";
            lblStudent.Font      = new Font("Segoe UI", 9f, FontStyle.Bold);
            lblStudent.ForeColor = Theme.Text;
            lblStudent.AutoSize  = true;
            lblStudent.Location  = new Point(30, 90);

            cmbStudent.Location       = new Point(30, 110);
            cmbStudent.Size           = new Size(330, 28);
            cmbStudent.DropDownStyle  = ComboBoxStyle.DropDownList;
            cmbStudent.Font           = new Font("Segoe UI", 10f);
            cmbStudent.BackColor      = Theme.Input;
            cmbStudent.SelectedIndexChanged += cmbStudent_SelectedIndexChanged;

            btnRefresh.Text     = "⟳";
            btnRefresh.Size     = new Size(40, 28);
            btnRefresh.Location = new Point(368, 110);
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.BackColor = Color.FromArgb(226, 232, 240);
            btnRefresh.ForeColor = Theme.Text;
            btnRefresh.Font      = new Font("Segoe UI", 11f);
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.Cursor    = Cursors.Hand;
            btnRefresh.Click    += (s, e) => LoadStudents();
            new ToolTip().SetToolTip(btnRefresh, "Refresh student list");

            // ── student info panel ────────────────────────────────────────
            pnlStudentInfo.Size      = new Size(400, 60);
            pnlStudentInfo.Location  = new Point(30, 148);
            pnlStudentInfo.BackColor = Color.FromArgb(239, 246, 255);
            pnlStudentInfo.BorderStyle = BorderStyle.FixedSingle;
            pnlStudentInfo.Visible   = false;

            lblInfoText.Text      = "";
            lblInfoText.Font      = new Font("Segoe UI", 9f);
            lblInfoText.ForeColor = Theme.Primary;
            lblInfoText.AutoSize  = false;
            lblInfoText.Size      = new Size(390, 52);
            lblInfoText.Location  = new Point(5, 4);
            lblInfoText.TextAlign = ContentAlignment.MiddleLeft;
            pnlStudentInfo.Controls.Add(lblInfoText);

            // ── subject picker ────────────────────────────────────────────
            lblSubject.Text      = "Subject";
            lblSubject.Font      = new Font("Segoe UI", 9f, FontStyle.Bold);
            lblSubject.ForeColor = Theme.Text;
            lblSubject.AutoSize  = true;
            lblSubject.Location  = new Point(30, 225);

            cmbSubject.Location      = new Point(30, 245);
            cmbSubject.Size          = new Size(400, 28);
            cmbSubject.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSubject.Font          = new Font("Segoe UI", 10f);
            cmbSubject.BackColor     = Theme.Input;
            cmbSubject.Items.AddRange(GradeController.Subjects);

            // ── mark ──────────────────────────────────────────────────────
            lblMark.Text      = "Mark (0 – 100)";
            lblMark.Font      = new Font("Segoe UI", 9f, FontStyle.Bold);
            lblMark.ForeColor = Theme.Text;
            lblMark.AutoSize  = true;
            lblMark.Location  = new Point(30, 290);

            txtMark.Location    = new Point(30, 310);
            txtMark.Size        = new Size(400, 30);
            txtMark.Font        = new Font("Segoe UI", 10f);
            txtMark.BackColor   = Theme.Input;
            txtMark.BorderStyle = BorderStyle.FixedSingle;
            txtMark.ForeColor   = Theme.Text;
            txtMark.PlaceholderText = "Enter a number between 0 and 100";

            // ── save button ───────────────────────────────────────────────
            btnSave.Text     = "💾  Save Mark";
            btnSave.Size     = new Size(400, 40);
            btnSave.Location = new Point(30, 358);
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.BackColor = Theme.Primary;
            btnSave.ForeColor = Color.White;
            btnSave.Font      = new Font("Segoe UI", 10f, FontStyle.Bold);
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Cursor    = Cursors.Hand;
            btnSave.MouseEnter += (s, e) => ((Button)s).BackColor = Theme.PrimaryHv;
            btnSave.MouseLeave += (s, e) => ((Button)s).BackColor = Theme.Primary;
            btnSave.Click      += btnSave_Click;

            // ── result panel ──────────────────────────────────────────────
            pnlResult.Size      = new Size(400, 70);
            pnlResult.Location  = new Point(30, 410);
            pnlResult.BackColor = Color.White;
            pnlResult.BorderStyle = BorderStyle.FixedSingle;
            pnlResult.Visible   = false;

            lblResultText.Text      = "";
            lblResultText.Font      = new Font("Segoe UI", 10f, FontStyle.Bold);
            lblResultText.AutoSize  = false;
            lblResultText.Size      = new Size(390, 62);
            lblResultText.Location  = new Point(5, 4);
            lblResultText.TextAlign = ContentAlignment.MiddleCenter;
            pnlResult.Controls.Add(lblResultText);

            // ── back button ───────────────────────────────────────────────
            btnBack.Text     = "← Back to Dashboard";
            btnBack.Size     = new Size(400, 36);
            btnBack.Location = new Point(30, 530);
            btnBack.FlatStyle = FlatStyle.Flat;
            btnBack.BackColor = Color.White;
            btnBack.ForeColor = Theme.Primary;
            btnBack.Font      = new Font("Segoe UI", 9.5f);
            btnBack.FlatAppearance.BorderColor = Theme.Primary;
            btnBack.FlatAppearance.BorderSize  = 1;
            btnBack.Cursor    = Cursors.Hand;
            btnBack.MouseEnter += (s, e) => ((Button)s).BackColor = Color.FromArgb(239,246,255);
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
            this.Controls.Add(pnlResult);
            this.Controls.Add(btnBack);

            LoadStudents();

            int index = students.FindIndex(st => st.Id == selectStudentId);
            if (index >= 0)
            {
                cmbStudent.SelectedIndex = index;
                this.ActiveControl = cmbSubject;
            }
        }

        void LoadStudents()
        {
            students = StudentController.GetStudents("");
            cmbStudent.Items.Clear();
            foreach (Student s in students)
                cmbStudent.Items.Add(s.Id + "  –  " + s.Name + "  (" + s.Course + ")");

            if (cmbStudent.Items.Count == 0)
                cmbStudent.Items.Add("(No students registered yet)");
        }

        void cmbStudent_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbStudent.SelectedIndex < 0 || cmbStudent.SelectedIndex >= students.Count)
            {
                pnlStudentInfo.Visible = false;
                pnlResult.Visible      = false;
                return;
            }

            Student s = students[cmbStudent.SelectedIndex];
            s.Grades = GradeController.GetGrades(s.Id);

            lblInfoText.Text =
                "ID: " + s.Id + "   |   Name: " + s.Name + "   |   Course / Class: " + s.Course;
            pnlStudentInfo.Visible = true;

            ShowResult(s);
        }

        void btnSave_Click(object sender, EventArgs e)
        {
            bool hasStudent = cmbStudent.SelectedIndex >= 0 && cmbStudent.SelectedIndex < students.Count;
            Student s = hasStudent ? students[cmbStudent.SelectedIndex] : null;
            string subject = cmbSubject.SelectedIndex == -1 ? null : cmbSubject.Text;

            string error = GradeController.SaveMark(s?.Id, subject, txtMark.Text);
            if (error != null)
            { ShowMsg("⚠  " + error); return; }

            s.Grades = GradeController.GetGrades(s.Id);
            txtMark.Clear();
            ShowResult(s);
        }

        // Shows the coloured result panel after a save or student selection
        void ShowResult(Student s)
        {
            if (s.Grades.Count == 0)
            {
                pnlResult.BackColor    = Color.FromArgb(248, 250, 252);
                lblResultText.ForeColor = Theme.Muted;
                lblResultText.Text      = "No marks entered yet for " + s.Name + ".";
            }
            else
            {
                bool passed = s.HasPassed();
                pnlResult.BackColor    = passed
                    ? Color.FromArgb(240, 253, 244)   // green-50
                    : Color.FromArgb(254, 242, 242);  // red-50
                lblResultText.ForeColor = passed ? Theme.Green : Theme.Red;
                lblResultText.Text      =
                    s.Name + "  |  Average: " + s.GetAverage().ToString("0.0") +
                    "\n" + (passed ? "✔  " : "✖  ") + s.GetStatus().ToUpper();
            }
            pnlResult.Visible = true;
        }

        void ShowMsg(string msg)
        {
            MessageBox.Show(msg, "Grade Entry", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
