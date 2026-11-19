using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using StudentGradeTracker.Controllers;
using StudentGradeTracker.Models;

namespace StudentGradeTracker.Views
{
    // FR5 - Record Management (CRUD): Read, Update, Delete
    public partial class ViewStudentsForm : Form
    {
        Panel    pnlHeader   = new Panel();
        Label    lblTitle    = new Label();
        Label    lblSearch   = new Label();
        TextBox  txtSearch   = new TextBox();
        Button   btnSearch   = new Button();
        Button   btnShowAll  = new Button();
        ListView listStudents = new ListView();
        Label    lblCount    = new Label();
        Button   btnEdit     = new Button();
        Button   btnDelete   = new Button();
        Button   btnBack     = new Button();

        public ViewStudentsForm()
        {
            InitializeComponent();

            this.Text            = "View Students";
            this.ClientSize      = new Size(900, 580);
            this.StartPosition   = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox     = false;
            this.BackColor       = Theme.Bg;
            this.Font            = new Font("Segoe UI", 9.5f);

            // ── header ────────────────────────────────────────────────────
            pnlHeader.Size      = new Size(900, 70);
            pnlHeader.Location  = new Point(0, 0);
            pnlHeader.BackColor = Theme.Amber;

            lblTitle.Text      = "🔍  View, Search & Manage Student Records";
            lblTitle.Font      = new Font("Segoe UI", 13, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.AutoSize  = false;
            lblTitle.Size      = new Size(860, 40);
            lblTitle.Location  = new Point(20, 15);
            lblTitle.TextAlign = ContentAlignment.MiddleLeft;
            lblTitle.UseMnemonic = false;   // show "&" as a normal character
            pnlHeader.Controls.Add(lblTitle);

            // ── search bar ────────────────────────────────────────────────
            lblSearch.Text      = "Search (name or ID):";
            lblSearch.Font      = new Font("Segoe UI", 9f, FontStyle.Bold);
            lblSearch.ForeColor = Theme.Text;
            lblSearch.AutoSize  = true;
            lblSearch.Location  = new Point(20, 85);

            txtSearch.Location        = new Point(170, 82);
            txtSearch.Size            = new Size(220, 28);
            txtSearch.Font            = new Font("Segoe UI", 10f);
            txtSearch.BackColor       = Color.White;
            txtSearch.BorderStyle     = BorderStyle.FixedSingle;
            txtSearch.PlaceholderText = "Type to search…";
            txtSearch.KeyDown        += (s, e) => { if (e.KeyCode == Keys.Enter) ShowStudents(txtSearch.Text.Trim()); };

            btnSearch.Text     = "Search";
            btnSearch.Location = new Point(400, 80);
            btnSearch.Size     = new Size(90, 30);
            btnSearch.FlatStyle = FlatStyle.Flat;
            btnSearch.BackColor = Theme.Primary;
            btnSearch.ForeColor = Color.White;
            btnSearch.Font      = new Font("Segoe UI", 9f, FontStyle.Bold);
            btnSearch.FlatAppearance.BorderSize = 0;
            btnSearch.Cursor    = Cursors.Hand;
            btnSearch.MouseEnter += (s, e) => ((Button)s).BackColor = Theme.PrimaryHv;
            btnSearch.MouseLeave += (s, e) => ((Button)s).BackColor = Theme.Primary;
            btnSearch.Click      += (s, e) => ShowStudents(txtSearch.Text.Trim());

            btnShowAll.Text     = "Show All";
            btnShowAll.Location = new Point(500, 80);
            btnShowAll.Size     = new Size(90, 30);
            btnShowAll.FlatStyle = FlatStyle.Flat;
            btnShowAll.BackColor = Color.White;
            btnShowAll.ForeColor = Theme.Primary;
            btnShowAll.Font      = new Font("Segoe UI", 9f);
            btnShowAll.FlatAppearance.BorderColor = Theme.Primary;
            btnShowAll.FlatAppearance.BorderSize  = 1;
            btnShowAll.Cursor    = Cursors.Hand;
            btnShowAll.MouseEnter += (s, e) => ((Button)s).BackColor = Color.FromArgb(239,246,255);
            btnShowAll.MouseLeave += (s, e) => ((Button)s).BackColor = Color.White;
            btnShowAll.Click      += (s, e) => { txtSearch.Clear(); ShowStudents(""); };

            // ── list view ─────────────────────────────────────────────────
            listStudents.Location     = new Point(20, 120);
            listStudents.Size         = new Size(860, 370);
            listStudents.View         = View.Details;
            listStudents.FullRowSelect= true;
            listStudents.GridLines    = true;
            listStudents.MultiSelect  = false;
            listStudents.Font         = new Font("Segoe UI", 9.5f);
            listStudents.Columns.Add("Student ID",    100);
            listStudents.Columns.Add("Name",          160);
            listStudents.Columns.Add("Course / Class",165);
            listStudents.Columns.Add("Marks",         235);
            listStudents.Columns.Add("Average",        75);
            listStudents.Columns.Add("Result",         90);
            listStudents.DoubleClick += btnEdit_Click;   // double-click a row to edit it

            // ── count label ───────────────────────────────────────────────
            lblCount.Text      = "";
            lblCount.Font      = new Font("Segoe UI", 8.5f);
            lblCount.ForeColor = Theme.Muted;
            lblCount.AutoSize  = true;
            lblCount.Location  = new Point(20, 497);

            // ── action buttons ────────────────────────────────────────────
            btnEdit.Text     = "✏  Edit Selected";
            btnEdit.Size     = new Size(160, 36);
            btnEdit.Location = new Point(20, 530);
            StyleActionBtn(btnEdit, Theme.Amber, Theme.AmberHv);
            btnEdit.Click   += btnEdit_Click;

            btnDelete.Text     = "🗑  Delete Selected";
            btnDelete.Size     = new Size(160, 36);
            btnDelete.Location = new Point(192, 530);
            StyleActionBtn(btnDelete, Theme.Red, Theme.RedHv);
            btnDelete.Click   += btnDelete_Click;

            btnBack.Text     = "← Back to Dashboard";
            btnBack.Size     = new Size(190, 36);
            btnBack.Location = new Point(690, 530);
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
            this.Controls.Add(lblSearch);
            this.Controls.Add(txtSearch);
            this.Controls.Add(btnSearch);
            this.Controls.Add(btnShowAll);
            this.Controls.Add(listStudents);
            this.Controls.Add(lblCount);
            this.Controls.Add(btnEdit);
            this.Controls.Add(btnDelete);
            this.Controls.Add(btnBack);

            ShowStudents("");
        }

        // READ: fills the list from the database
        void ShowStudents(string searchText)
        {
            listStudents.Items.Clear();
            List<Student> students = StudentController.GetStudents(searchText);

            foreach (Student s in students)
            {
                string marksText = "";
                foreach (Grade g in s.Grades)
                {
                    if (marksText != "") marksText += ", ";
                    marksText += g.Subject + ": " + g.Mark;
                }
                if (marksText == "") marksText = "No marks entered";

                string avgText = s.Grades.Count > 0
                    ? s.GetAverage().ToString("0.0")
                    : "–";

                ListViewItem item = new ListViewItem(s.Id);
                item.Tag = s;   // remember which student this row is
                item.SubItems.Add(s.Name);
                item.SubItems.Add(s.Course);
                item.SubItems.Add(marksText);
                item.SubItems.Add(avgText);
                item.SubItems.Add(s.GetStatus());

                // colour-code rows: green = Passed, red = Failed, default = no marks
                if (s.Grades.Count > 0)
                {
                    bool passed = s.HasPassed();
                    item.BackColor = passed
                        ? Color.FromArgb(240, 253, 244)   // green-50
                        : Color.FromArgb(254, 242, 242);  // red-50
                    item.ForeColor = passed ? Theme.Green : Theme.Red;
                }

                listStudents.Items.Add(item);
            }

            lblCount.Text = students.Count == 0
                ? "No students found."
                : students.Count + " student" + (students.Count == 1 ? "" : "s") + " found.";
        }

        // UPDATE: open Edit form for selected student
        void btnEdit_Click(object sender, EventArgs e)
        {
            if (listStudents.SelectedItems.Count == 0)
            { MessageBox.Show("Please select a student to edit.", "No selection", MessageBoxButtons.OK, MessageBoxIcon.Information); return; }

            Student s = (Student)listStudents.SelectedItems[0].Tag;

            var form = new EditStudentForm(s);
            this.Hide();
            form.ShowDialog();
            this.Show();
            ShowStudents(txtSearch.Text.Trim());
        }

        // DELETE: remove selected student with confirmation
        void btnDelete_Click(object sender, EventArgs e)
        {
            if (listStudents.SelectedItems.Count == 0)
            { MessageBox.Show("Please select a student to delete.", "No selection", MessageBoxButtons.OK, MessageBoxIcon.Information); return; }

            Student s = (Student)listStudents.SelectedItems[0].Tag;

            DialogResult ans = MessageBox.Show(
                "Are you sure you want to delete:\n\n" +
                "  Name:  " + s.Name + "\n" +
                "  ID:    " + s.Id   + "\n\n" +
                "All their grades will also be deleted. This cannot be undone.",
                "Confirm Delete",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (ans != DialogResult.Yes) return;

            string error = StudentController.DeleteStudent(s.Id);
            if (error != null)
            {
                MessageBox.Show(error, "Delete Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            ShowStudents(txtSearch.Text.Trim());
        }

        void StyleActionBtn(Button b, Color bg, Color hover)
        {
            b.FlatStyle = FlatStyle.Flat;
            b.BackColor = bg;
            b.ForeColor = Color.White;
            b.Font      = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            b.FlatAppearance.BorderSize = 0;
            b.Cursor    = Cursors.Hand;
            b.MouseEnter += (s, e) => ((Button)s).BackColor = hover;
            b.MouseLeave += (s, e) => ((Button)s).BackColor = bg;
        }
    }
}
