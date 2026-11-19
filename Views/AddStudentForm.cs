using System;
using System.Drawing;
using System.Windows.Forms;
using StudentGradeTracker.Controllers;

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
        TextBox txtCourse  = new TextBox();
        Button  btnSave    = new Button();
        Button  btnBack    = new Button();
        Label   lblStatus  = new Label();

        public AddStudentForm()
        {
            InitializeComponent();

            this.Text            = "Register Student";
            this.ClientSize      = new Size(420, 490);
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
            MakeTB(txtCourse, 260, "e.g. S3A, Year 2");

            // status label
            lblStatus.Text      = "";
            lblStatus.ForeColor = Theme.Error;
            lblStatus.Font      = new Font("Segoe UI", 9f);
            lblStatus.AutoSize  = false;
            lblStatus.Size      = new Size(360, 20);
            lblStatus.Location  = new Point(30, 308);

            // save button
            btnSave.Text     = "💾  Save Student";
            btnSave.Size     = new Size(360, 40);
            btnSave.Location = new Point(30, 335);
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
            btnBack.Location = new Point(30, 385);
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
            this.Controls.Add(txtCourse);
            this.Controls.Add(lblStatus);
            this.Controls.Add(btnSave);
            this.Controls.Add(btnBack);

            this.AcceptButton = btnSave;
        }

        void btnSave_Click(object sender, EventArgs e)
        {
            lblStatus.ForeColor = Theme.Error;
            lblStatus.Text      = "";

            string name = txtName.Text.Trim();
            string id   = txtId.Text.Trim();

            string error = StudentController.AddStudent(id, name, txtCourse.Text);
            if (error != null)
            { lblStatus.Text = "⚠  " + error; return; }

            // Ask whether to enter grades now
            DialogResult ans = MessageBox.Show(
                "✔  Student \"" + name + "\" registered successfully!\n\n" +
                "Would you like to enter grades for them now?",
                "Student Registered",
                MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            txtName.Clear();
            txtId.Clear();
            txtCourse.Clear();
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
