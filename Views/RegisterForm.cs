using System;
using System.Drawing;
using System.Windows.Forms;
using StudentGradeTracker.Controllers;

namespace StudentGradeTracker.Views
{
    // FR1 - User Access: create a new account
    public partial class RegisterForm : Form
    {
        Panel   pnlCard     = new Panel();
        Label   lblTitle    = new Label();
        Label   lblSubtitle = new Label();
        Label   lblUsername = new Label();
        Label   lblPassword = new Label();
        Label   lblConfirm  = new Label();
        TextBox txtUsername = new TextBox();
        TextBox txtPassword = new TextBox();
        TextBox txtConfirm  = new TextBox();
        Button  btnRegister = new Button();
        Button  btnBack     = new Button();
        Label   lblStatus   = new Label();

        public RegisterForm()
        {
            InitializeComponent();

            this.Text            = "Student Grade Tracker – Register";
            this.ClientSize      = new Size(420, 560);
            this.StartPosition   = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox     = false;
            this.BackColor       = Theme.Bg;
            this.Font            = new Font("Segoe UI", 9.5f);

            pnlCard.Size      = new Size(360, 490);
            pnlCard.Location  = new Point(30, 35);
            pnlCard.BackColor = Color.White;
            pnlCard.Paint    += (s, e) =>
            {
                var p = (Panel)s;
                using (var pen = new Pen(Color.FromArgb(20, 0, 0, 0), 1))
                    e.Graphics.DrawRectangle(pen, new Rectangle(0, 0, p.Width-1, p.Height-1));
            };

            lblTitle.Text      = "Create Account";
            lblTitle.Font      = new Font("Segoe UI", 15, FontStyle.Bold);
            lblTitle.ForeColor = Theme.Primary;
            lblTitle.AutoSize  = false;
            lblTitle.Size      = new Size(320, 32);
            lblTitle.Location  = new Point(20, 25);
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;

            lblSubtitle.Text      = "Fill in the details below to register";
            lblSubtitle.Font      = new Font("Segoe UI", 9f);
            lblSubtitle.ForeColor = Theme.Muted;
            lblSubtitle.AutoSize  = false;
            lblSubtitle.Size      = new Size(320, 20);
            lblSubtitle.Location  = new Point(20, 59);
            lblSubtitle.TextAlign = ContentAlignment.MiddleCenter;

            // username
            lblUsername.Text      = "Username";
            lblUsername.ForeColor = Theme.Text;
            lblUsername.Font      = new Font("Segoe UI", 9f, FontStyle.Bold);
            lblUsername.AutoSize  = true;
            lblUsername.Location  = new Point(30, 100);
            StyleTB(txtUsername);
            txtUsername.Location        = new Point(30, 120);
            txtUsername.PlaceholderText = "At least 3 characters";

            // password
            lblPassword.Text      = "Password";
            lblPassword.ForeColor = Theme.Text;
            lblPassword.Font      = new Font("Segoe UI", 9f, FontStyle.Bold);
            lblPassword.AutoSize  = true;
            lblPassword.Location  = new Point(30, 168);
            StyleTB(txtPassword);
            txtPassword.Location        = new Point(30, 188);
            txtPassword.PasswordChar    = '●';
            txtPassword.PlaceholderText = "At least 4 characters";

            // confirm
            lblConfirm.Text      = "Confirm Password";
            lblConfirm.ForeColor = Theme.Text;
            lblConfirm.Font      = new Font("Segoe UI", 9f, FontStyle.Bold);
            lblConfirm.AutoSize  = true;
            lblConfirm.Location  = new Point(30, 236);
            StyleTB(txtConfirm);
            txtConfirm.Location      = new Point(30, 256);
            txtConfirm.PasswordChar  = '●';
            txtConfirm.PlaceholderText = "Repeat your password";

            // status
            lblStatus.Text      = "";
            lblStatus.ForeColor = Theme.Error;
            lblStatus.Font      = new Font("Segoe UI", 9f);
            lblStatus.AutoSize  = false;
            lblStatus.Size      = new Size(300, 20);
            lblStatus.Location  = new Point(30, 300);

            // register button
            StylePrimary(btnRegister);
            btnRegister.Text     = "Register";
            btnRegister.Location = new Point(30, 328);
            btnRegister.Click   += btnRegister_Click;

            // back button
            StyleSecondary(btnBack);
            btnBack.Text     = "← Back to Login";
            btnBack.Location = new Point(30, 378);
            btnBack.Click   += (s, e) => this.Close();

            pnlCard.Controls.Add(lblTitle);
            pnlCard.Controls.Add(lblSubtitle);
            pnlCard.Controls.Add(lblUsername);
            pnlCard.Controls.Add(txtUsername);
            pnlCard.Controls.Add(lblPassword);
            pnlCard.Controls.Add(txtPassword);
            pnlCard.Controls.Add(lblConfirm);
            pnlCard.Controls.Add(txtConfirm);
            pnlCard.Controls.Add(lblStatus);
            pnlCard.Controls.Add(btnRegister);
            pnlCard.Controls.Add(btnBack);
            this.Controls.Add(pnlCard);

            this.AcceptButton = btnRegister;
        }

        void btnRegister_Click(object sender, EventArgs e)
        {
            lblStatus.ForeColor = Theme.Error;
            lblStatus.Text      = "";

            string error = AuthController.Register(txtUsername.Text, txtPassword.Text, txtConfirm.Text);
            if (error != null)
            { lblStatus.Text = "⚠  " + error; return; }

            MessageBox.Show("Account created! You can now log in.", "Registered",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }

        void StyleTB(TextBox tb)
        {
            tb.Size        = new Size(300, 30);
            tb.Font        = new Font("Segoe UI", 10f);
            tb.BackColor   = Theme.Input;
            tb.BorderStyle = BorderStyle.FixedSingle;
            tb.ForeColor   = Theme.Text;
        }

        void StylePrimary(Button b)
        {
            b.Size      = new Size(300, 38);
            b.FlatStyle = FlatStyle.Flat;
            b.BackColor = Theme.Primary;
            b.ForeColor = Color.White;
            b.Font      = new Font("Segoe UI", 10f, FontStyle.Bold);
            b.FlatAppearance.BorderSize = 0;
            b.Cursor    = Cursors.Hand;
            b.MouseEnter += (s, e) => ((Button)s).BackColor = Theme.PrimaryHv;
            b.MouseLeave += (s, e) => ((Button)s).BackColor = Theme.Primary;
        }

        void StyleSecondary(Button b)
        {
            b.Size      = new Size(300, 36);
            b.FlatStyle = FlatStyle.Flat;
            b.BackColor = Color.White;
            b.ForeColor = Theme.Primary;
            b.Font      = new Font("Segoe UI", 9.5f);
            b.FlatAppearance.BorderColor = Theme.Primary;
            b.FlatAppearance.BorderSize  = 1;
            b.Cursor    = Cursors.Hand;
            b.MouseEnter += (s, e) => ((Button)s).BackColor = Color.FromArgb(239,246,255);
            b.MouseLeave += (s, e) => ((Button)s).BackColor = Color.White;
        }
    }
}
