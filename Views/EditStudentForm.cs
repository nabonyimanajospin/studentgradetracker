using System;
using System.Drawing;
using System.Windows.Forms;
using StudentGradeTracker.Controllers;

namespace StudentGradeTracker.Views
{
    // UPDATE (part of FR5): change the name and course of a student
    public partial class EditStudentForm : Form
    {
        Panel   pnlHeader  = new Panel();
        Label   lblTitle   = new Label();
        Label   lblIdInfo  = new Label();
        Label   lblName    = new Label();
        Label   lblCourse  = new Label();
        TextBox txtId      = new TextBox();
        TextBox txtName    = new TextBox();
        TextBox txtCourse  = new TextBox();
        Button  btnUpdate  = new Button();
        Button  btnBack    = new Button();
        Label   lblStatus  = new Label();

        public EditStudentForm(string id, string name, string course)
        {
            InitializeComponent();

            this.Text            = "Edit Student";
            this.ClientSize      = new Size(420, 450);
            this.StartPosition   = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox     = false;
            this.BackColor       = Theme.Bg;
            this.Font            = new Font("Segoe UI", 9.5f);

            // header
            pnlHeader.Size      = new Size(420, 70);
            pnlHeader.Location  = new Point(0, 0);
            pnlHeader.BackColor = Theme.Amber;

            lblTitle.Text      = "✏  Edit Student Record";
            lblTitle.Font      = new Font("Segoe UI", 13, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.AutoSize  = false;
            lblTitle.Size      = new Size(380, 40);
            lblTitle.Location  = new Point(20, 15);
            lblTitle.TextAlign = ContentAlignment.MiddleLeft;
            pnlHeader.Controls.Add(lblTitle);

            // read-only ID info
            lblIdInfo.Text      = "Student ID (cannot be changed)";
            lblIdInfo.Font      = new Font("Segoe UI", 9f, FontStyle.Bold);
            lblIdInfo.ForeColor = Theme.Text;
            lblIdInfo.AutoSize  = true;
            lblIdInfo.Location  = new Point(30, 95);

            txtId.Text        = id;
            txtId.ReadOnly    = true;
            txtId.Size        = new Size(360, 30);
            txtId.Location    = new Point(30, 115);
            txtId.Font        = new Font("Segoe UI", 10f);
            txtId.BackColor   = Color.FromArgb(226, 232, 240);  // slate-200 (locked)
            txtId.ForeColor   = Color.FromArgb(100, 116, 139);
            txtId.BorderStyle = BorderStyle.FixedSingle;

            // name
            lblName.Text      = "Full Name";
            lblName.Font      = new Font("Segoe UI", 9f, FontStyle.Bold);
            lblName.ForeColor = Theme.Text;
            lblName.AutoSize  = true;
            lblName.Location  = new Point(30, 163);

            txtName.Text        = name;
            txtName.Size        = new Size(360, 30);
            txtName.Location    = new Point(30, 183);
            txtName.Font        = new Font("Segoe UI", 10f);
            txtName.BackColor   = Theme.Input;
            txtName.BorderStyle = BorderStyle.FixedSingle;
            txtName.ForeColor   = Theme.Text;

            // course
            lblCourse.Text      = "Course / Class";
            lblCourse.Font      = new Font("Segoe UI", 9f, FontStyle.Bold);
            lblCourse.ForeColor = Theme.Text;
            lblCourse.AutoSize  = true;
            lblCourse.Location  = new Point(30, 231);

            txtCourse.Text        = course;
            txtCourse.Size        = new Size(360, 30);
            txtCourse.Location    = new Point(30, 251);
            txtCourse.Font        = new Font("Segoe UI", 10f);
            txtCourse.BackColor   = Theme.Input;
            txtCourse.BorderStyle = BorderStyle.FixedSingle;
            txtCourse.ForeColor   = Theme.Text;

            // status
            lblStatus.Text      = "";
            lblStatus.ForeColor = Theme.Error;
            lblStatus.Font      = new Font("Segoe UI", 9f);
            lblStatus.AutoSize  = false;
            lblStatus.Size      = new Size(360, 20);
            lblStatus.Location  = new Point(30, 295);

            // update button
            btnUpdate.Text     = "✔  Update Student";
            btnUpdate.Size     = new Size(360, 40);
            btnUpdate.Location = new Point(30, 320);
            btnUpdate.FlatStyle = FlatStyle.Flat;
            btnUpdate.BackColor = Theme.Amber;
            btnUpdate.ForeColor = Color.White;
            btnUpdate.Font      = new Font("Segoe UI", 10f, FontStyle.Bold);
            btnUpdate.FlatAppearance.BorderSize = 0;
            btnUpdate.Cursor    = Cursors.Hand;
            btnUpdate.MouseEnter += (s, e) => ((Button)s).BackColor = Theme.AmberHv;
            btnUpdate.MouseLeave += (s, e) => ((Button)s).BackColor = Theme.Amber;
            btnUpdate.Click      += btnUpdate_Click;

            // cancel button
            btnBack.Text     = "✖  Cancel";
            btnBack.Size     = new Size(360, 36);
            btnBack.Location = new Point(30, 372);
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
            this.Controls.Add(lblIdInfo);
            this.Controls.Add(txtId);
            this.Controls.Add(lblName);
            this.Controls.Add(txtName);
            this.Controls.Add(lblCourse);
            this.Controls.Add(txtCourse);
            this.Controls.Add(lblStatus);
            this.Controls.Add(btnUpdate);
            this.Controls.Add(btnBack);

            this.AcceptButton = btnUpdate;
        }

        void btnUpdate_Click(object sender, EventArgs e)
        {
            lblStatus.ForeColor = Theme.Error;
            lblStatus.Text      = "";
            string error = StudentController.UpdateStudent(txtId.Text, txtName.Text, txtCourse.Text);
            if (error != null)
            { lblStatus.Text = "⚠  " + error; return; }

            MessageBox.Show("Student updated successfully.", "Updated",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }
    }
}
