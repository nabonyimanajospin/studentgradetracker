using System;
using System.Drawing;
using System.Windows.Forms;
using StudentGradeTracker.Controllers;

namespace StudentGradeTracker.Views
{
    // FR1 - User Access: the user logs in here, or goes to the Register form
    public partial class LoginForm : Form
    {
        // ── background panel ──────────────────────────────────────────────
        Panel pnlCard = new Panel();

        // ── controls ──────────────────────────────────────────────────────
        Label  lblAppName  = new Label();
        Label  lblSubtitle = new Label();
        Label  lblUsername = new Label();
        Label  lblPassword = new Label();
        TextBox txtUsername = new TextBox();
        TextBox txtPassword = new TextBox();
        Button  btnLogin    = new Button();
        Button  btnRegister = new Button();
        Label  lblStatus   = new Label();     // shows inline error messages

        public LoginForm()
        {
            InitializeComponent();

            // ── form ──────────────────────────────────────────────────────
            this.Text            = "Student Grade Tracker – Login";
            this.ClientSize      = new Size(420, 520);
            this.StartPosition   = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox     = false;
            this.BackColor       = Theme.Bg;
            this.Font            = new Font("Segoe UI", 9.5f);

            // ── card panel ────────────────────────────────────────────────
            pnlCard.Size      = new Size(360, 440);
            pnlCard.Location  = new Point(30, 40);
            pnlCard.BackColor = Theme.Card;
            pnlCard.Paint    += Panel_Paint_RoundedShadow;

            // ── app name ──────────────────────────────────────────────────
            lblAppName.Text      = "📚 Student Grade Tracker";
            lblAppName.Font      = new Font("Segoe UI", 15, FontStyle.Bold);
            lblAppName.ForeColor = Theme.Primary;
            lblAppName.AutoSize  = false;
            lblAppName.Size      = new Size(320, 32);
            lblAppName.Location  = new Point(20, 28);
            lblAppName.TextAlign = ContentAlignment.MiddleCenter;

            lblSubtitle.Text      = "Staff sign in (teachers and administrators)";
            lblSubtitle.Font      = new Font("Segoe UI", 9f);
            lblSubtitle.ForeColor = Theme.Muted;
            lblSubtitle.AutoSize  = false;
            lblSubtitle.Size      = new Size(320, 20);
            lblSubtitle.Location  = new Point(20, 62);
            lblSubtitle.TextAlign = ContentAlignment.MiddleCenter;

            // ── username ──────────────────────────────────────────────────
            lblUsername.Text      = "Username";
            lblUsername.ForeColor = Theme.Text;
            lblUsername.Font      = new Font("Segoe UI", 9f, FontStyle.Bold);
            lblUsername.AutoSize  = true;
            lblUsername.Location  = new Point(30, 105);

            StyleTextBox(txtUsername);
            txtUsername.Location    = new Point(30, 125);
            txtUsername.PlaceholderText = "Enter your username";

            // ── password ──────────────────────────────────────────────────
            lblPassword.Text      = "Password";
            lblPassword.ForeColor = Theme.Text;
            lblPassword.Font      = new Font("Segoe UI", 9f, FontStyle.Bold);
            lblPassword.AutoSize  = true;
            lblPassword.Location  = new Point(30, 175);

            StyleTextBox(txtPassword);
            txtPassword.Location      = new Point(30, 195);
            txtPassword.PasswordChar  = '●';
            txtPassword.PlaceholderText = "Enter your password";

            // ── status label ──────────────────────────────────────────────
            lblStatus.Text      = "";
            lblStatus.ForeColor = Theme.Error;
            lblStatus.Font      = new Font("Segoe UI", 9f);
            lblStatus.AutoSize  = false;
            lblStatus.Size      = new Size(300, 20);
            lblStatus.Location  = new Point(30, 243);

            // ── login button ──────────────────────────────────────────────
            StylePrimaryButton(btnLogin);
            btnLogin.Text     = "Login";
            btnLogin.Location = new Point(30, 270);
            btnLogin.Click   += btnLogin_Click;

            // ── register link-style button ────────────────────────────────
            StyleSecondaryButton(btnRegister);
            btnRegister.Text     = "New teacher? Create a staff account";
            btnRegister.Location = new Point(30, 320);
            btnRegister.Click   += btnRegister_Click;

            // ── add to card, card to form ──────────────────────────────────
            pnlCard.Controls.Add(lblAppName);
            pnlCard.Controls.Add(lblSubtitle);
            pnlCard.Controls.Add(lblUsername);
            pnlCard.Controls.Add(txtUsername);
            pnlCard.Controls.Add(lblPassword);
            pnlCard.Controls.Add(txtPassword);
            pnlCard.Controls.Add(lblStatus);
            pnlCard.Controls.Add(btnLogin);
            pnlCard.Controls.Add(btnRegister);
            this.Controls.Add(pnlCard);

            txtUsername.MaxLength = AuthController.MaxUsernameLength;
            txtPassword.MaxLength = AuthController.MaxPasswordLength;

            this.AcceptButton = btnLogin;
        }

        // ── event handlers ────────────────────────────────────────────────
        void btnLogin_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "";
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            string error = AuthController.Login(username, password);
            if (error != null)
            {
                lblStatus.Text = "⚠  " + error;
                txtPassword.Clear();
                txtPassword.Focus();
                return;
            }

            DashboardForm dashboard = new DashboardForm(AuthController.GetDisplayName(username));
            this.Hide();
            dashboard.ShowDialog();
            lblStatus.Text = "";
            txtPassword.Clear();
            this.Show();
        }

        void btnRegister_Click(object sender, EventArgs e)
        {
            RegisterForm reg = new RegisterForm();
            this.Hide();
            reg.ShowDialog();
            this.Show();

            if (reg.RegisteredUsername != null)
            {
                txtUsername.Text = reg.RegisteredUsername;
                txtPassword.Clear();
                lblStatus.Text = "";
                txtPassword.Focus();
            }
        }

        // ── shared style helpers ──────────────────────────────────────────
        void StyleTextBox(TextBox tb)
        {
            tb.Size        = new Size(300, 30);
            tb.Font        = new Font("Segoe UI", 10f);
            tb.BackColor   = Theme.Input;
            tb.BorderStyle = BorderStyle.FixedSingle;
            tb.ForeColor   = Theme.Text;
        }

        void StylePrimaryButton(Button b)
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

        void StyleSecondaryButton(Button b)
        {
            b.Size      = new Size(300, 36);
            b.FlatStyle = FlatStyle.Flat;
            b.BackColor = Color.White;
            b.ForeColor = Theme.Primary;
            b.Font      = new Font("Segoe UI", 9.5f);
            b.FlatAppearance.BorderColor = Theme.Primary;
            b.FlatAppearance.BorderSize  = 1;
            b.Cursor    = Cursors.Hand;
            b.MouseEnter += (s, e) => { ((Button)s).BackColor = Color.FromArgb(239,246,255); };
            b.MouseLeave += (s, e) => { ((Button)s).BackColor = Color.White; };
        }

        // draws a subtle bottom-right shadow around the card panel
        void Panel_Paint_RoundedShadow(object sender, PaintEventArgs e)
        {
            var p = (Panel)sender;
            using (var pen = new Pen(Color.FromArgb(20, 0, 0, 0), 1))
            {
                e.Graphics.DrawRectangle(pen,
                    new Rectangle(0, 0, p.Width - 1, p.Height - 1));
            }
        }
    }
}
