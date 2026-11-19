using System;
using System.Drawing;
using System.Windows.Forms;
using StudentGradeTracker.Controllers;
using StudentGradeTracker.Models;

namespace StudentGradeTracker.Views
{
    // FR2 - Student Registration
    public partial class AddStudentForm : Form
    {
        Panel   pnlHeader  = new Panel();
        Label   lblTitle   = new Label();
        Label   lblName    = new Label();
        Label   lblId      = new Label();
        Label   lblCourse  = new Label();
        TextBox txtName    = new TextBox();
        TextBox txtId      = new TextBox();
        ComboBox cmbCourse = new ComboBox();
        Label   lblSubjects = new Label();
        Button  btnSave    = new Button();
        Button  btnBack    = new Button();
        Label   lblStatus  = new Label();

        public AddStudentForm()
        {
            InitializeComponent();

            this.Text            = "Register Student";
            this.ClientSize      = new Size(420, 520);
            this.StartPosition   = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox     = false;
            this.BackColor       = Theme.Bg;
            this.Font            = new Font("Segoe UI", 9.5f);

            // header
            pnlHeader.Size      = new Size(420, 70);
            pnlHeader.Location  = new Point(0, 0);
            pnlHeader.BackColor = Theme.Green;

            lblTitle.Text      = "👤  Register New Student";
            lblTitle.Font      = new Font("Segoe UI", 13, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.AutoSize  = false;
            lblTitle.Size      = new Size(380, 40);
            lblTitle.Location  = new Point(20, 15);
            lblTitle.TextAlign = ContentAlignment.MiddleLeft;
            pnlHeader.Controls.Add(lblTitle);

            // name
            MakeLabel(lblName, "Full Name", 100);
            MakeTB(txtName, 120, "e.g. Sifa Uwase");

            // student id
            MakeLabel(lblId, "Student ID", 170);
            MakeTB(txtId, 190, "e.g. STD-001");

            // course
            MakeLabel(lblCourse, "Course / Class", 240);
            cmbCourse.Size          = new Size(360, 30);
            cmbCourse.Location      = new Point(30, 260);
            cmbCourse.Font          = new Font("Segoe UI", 10f);
            cmbCourse.BackColor     = Theme.Input;
            cmbCourse.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCourse.Items.AddRange(Course.GetAll());
            cmbCourse.SelectedIndexChanged += (s, e) => ShowSubjects();
            Theme.BlockMouseWheel(cmbCourse);

            // subjects of the chosen course
            lblSubjects.Font      = new Font("Segoe UI", 8.5f);
            lblSubjects.ForeColor = Theme.Muted;
            lblSubjects.AutoSize  = false;
            lblSubjects.Size      = new Size(360, 34);
            lblSubjects.Location  = new Point(30, 292);
            lblSubjects.UseMnemonic = false;
            ShowSubjects();

            // status label
            lblStatus.Text      = "";
            lblStatus.ForeColor = Theme.Error;
            lblStatus.Font      = new Font("Segoe UI", 9f);
            lblStatus.AutoSize  = false;
            lblStatus.Size      = new Size(360, 20);
            lblStatus.Location  = new Point(30, 332);

            // save button
            btnSave.Text     = "💾  Save Student";
            btnSave.Size     = new Size(360, 40);
            btnSave.Location = new Point(30, 360);
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.BackColor = Theme.Green;
            btnSave.ForeColor = Color.White;
            btnSave.Font      = new Font("Segoe UI", 10f, FontStyle.Bold);
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Cursor    = Cursors.Hand;
            btnSave.MouseEnter += (s, e) => ((Button)s).BackColor = Theme.GreenHv;
            btnSave.MouseLeave += (s, e) => ((Button)s).BackColor = Theme.Green;
            btnSave.Click      += btnSave_Click;

            // back button
            btnBack.Text     = "← Back to Dashboard";
            btnBack.Size     = new Size(360, 36);
            btnBack.Location = new Point(30, 410);
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
            this.Controls.Add(lblName);
            this.Controls.Add(txtName);
            this.Controls.Add(lblId);
            this.Controls.Add(txtId);
            this.Controls.Add(lblCourse);
            this.Controls.Add(cmbCourse);
            this.Controls.Add(lblSubjects);
            this.Controls.Add(lblStatus);
            this.Controls.Add(btnSave);
            this.Controls.Add(btnBack);

            txtName.MaxLength = Student.MaxNameLength;
            txtId.MaxLength   = Student.MaxIdLength;

            this.AcceptButton = btnSave;
        }

        void btnSave_Click(object sender, EventArgs e)
        {
            lblStatus.ForeColor = Theme.Error;
            lblStatus.Text      = "";

            string name = txtName.Text.Trim();
            string id   = txtId.Text.Trim();

            string course = cmbCourse.SelectedIndex == -1 ? "" : cmbCourse.Text;

            string error = StudentController.AddStudent(id, name, course);
            if (error != null)
            { lblStatus.Text = "⚠  " + error; return; }

            // Ask whether to enter grades now
            DialogResult ans = MessageBox.Show(
                "✔  Student \"" + name + "\" (ID " + id + ") registered in " + course + ".\n\n" +
                "Subjects: " + string.Join(", ", Course.GetSubjects(course)) + "\n\n" +
                "Would you like to enter grades for them now?",
                "Student Registered",
                MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            txtName.Clear();
            txtId.Clear();
            cmbCourse.SelectedIndex = -1;
            txtName.Focus();
            lblStatus.Text = "";

            if (ans == DialogResult.Yes)
            {
                this.Hide();
                var gradeForm = new GradeEntryForm(id);
                gradeForm.ShowDialog();
                this.Show();
            }
        }

        void ShowSubjects()
        {
            lblSubjects.Text = cmbCourse.SelectedIndex == -1
                ? "Choose a course to see its subjects."
                : "Subjects: " + string.Join(", ", Course.GetSubjects(cmbCourse.Text));
        }

        void MakeLabel(Label lbl, string text, int y)
        {
            lbl.Text      = text;
            lbl.Font      = new Font("Segoe UI", 9f, FontStyle.Bold);
            lbl.ForeColor = Color.FromArgb(30, 41, 59);
            lbl.AutoSize  = true;
            lbl.Location  = new Point(30, y);
        }

        void MakeTB(TextBox tb, int y, string placeholder)
        {
            tb.Size             = new Size(360, 30);
            tb.Location         = new Point(30, y);
            tb.Font             = new Font("Segoe UI", 10f);
            tb.BackColor        = Theme.Input;
            tb.BorderStyle      = BorderStyle.FixedSingle;
            tb.ForeColor        = Color.FromArgb(30, 41, 59);
            tb.PlaceholderText  = placeholder;
        }
    }
}
