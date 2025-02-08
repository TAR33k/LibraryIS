using IS_za_biblioteku.Forms;
using IS_za_biblioteku;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System;

namespace IS_za_biblioteku
{
    public partial class NewLogin : Form
    {
        private readonly Color PrimaryColor = Color.FromArgb(255, 140, 0);    // Orange
        private readonly Color SecondaryColor = Color.White;
        private readonly Color TextColor = Color.FromArgb(51, 51, 51);        // Dark Gray
        private readonly Font TitleFont;
        private readonly Font RegularFont;
        private readonly Font BoldFont;

        private TextBox tbKorisnickoIme;
        private TextBox tbLozinka;
        private Label lblPrijava;
        private Button btnPrijaviSe;
        private Label lblKorisnickoIme;
        private Label lblLozinka;
        private Label lblKorisnickoImeError;
        private Label lblLozinkaError;

        private readonly Dictionary<string, (string password, bool isLibrarian)> credentials = new Dictionary<string, (string, bool)>
    {
        { "bibliotekar1", ("bibliotekar1", true) },
        { "korisnik1", ("korisnik1", false) }
    };

        public NewLogin()
        {
            // Initialize fonts
            try
            {
                string fontPath = Path.Combine(Application.StartupPath, "Resources");
                var pfc = new PrivateFontCollection();

                pfc.AddFontFile(Path.Combine(fontPath, "Poppins-Regular.ttf"));
                pfc.AddFontFile(Path.Combine(fontPath, "Poppins-Bold.ttf"));

                TitleFont = new Font(pfc.Families[0], 24, FontStyle.Bold);
                RegularFont = new Font(pfc.Families[0], 12, FontStyle.Regular);
                BoldFont = new Font(pfc.Families[0], 12, FontStyle.Bold);
            }
            catch
            {
                TitleFont = new Font("Segoe UI", 24, FontStyle.Bold);
                RegularFont = new Font("Segoe UI", 12, FontStyle.Regular);
                BoldFont = new Font("Segoe UI", 12, FontStyle.Bold);
            }

            InitializeForm();
            CreateControls();
            StyleControls();

            this.Text = "Biblioteka";
            this.tbKorisnickoIme.Text = "korisnik1";
            this.tbLozinka.Text = "korisnik1";
        }

        private void InitializeForm()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.Size = new Size(400, 500);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = SecondaryColor;

            // Add shadow
            this.Paint += (s, e) =>
            {
                var graphics = e.Graphics;
                var shadowColor = Color.FromArgb(20, 0, 0, 0);
                graphics.FillRectangle(new SolidBrush(shadowColor),
                    new Rectangle(4, 4, this.Width - 4, this.Height - 4));
            };
        }

        private void CreateControls()
        {
            // Create a main container panel
            var mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = SecondaryColor,
                Padding = new Padding(0)
            };

            // Title
            lblPrijava = new Label
            {
                Text = "Prijava",
                Font = TitleFont,
                ForeColor = TextColor,
                TextAlign = ContentAlignment.MiddleCenter,
                AutoSize = true,
                BackColor = SecondaryColor
            };

            // Labels
            lblKorisnickoIme = new Label
            {
                Text = "Korisničko ime",
                Font = BoldFont,
                ForeColor = TextColor,
                AutoSize = true,
                BackColor = SecondaryColor
            };

            lblLozinka = new Label
            {
                Text = "Lozinka",
                Font = BoldFont,
                ForeColor = TextColor,
                AutoSize = true,
                BackColor = SecondaryColor
            };

            // TextBoxes
            tbKorisnickoIme = CreateStyledTextBox();
            tbLozinka = CreateStyledTextBox();
            tbLozinka.UseSystemPasswordChar = true;

            // Button
            btnPrijaviSe = new Button
            {
                Text = "Prijavi se",
                Size = new Size(300, 45),
                FlatStyle = FlatStyle.Flat,
                Font = BoldFont,
                BackColor = PrimaryColor,
                ForeColor = SecondaryColor,
                Cursor = Cursors.Hand
            };
            btnPrijaviSe.FlatAppearance.BorderSize = 0;
            btnPrijaviSe.Click += btnPrijaviSe_Click;

            // Add hover effects
            btnPrijaviSe.MouseEnter += (s, e) =>
                btnPrijaviSe.BackColor = Color.FromArgb(
                    (int)(PrimaryColor.R * 0.9),
                    (int)(PrimaryColor.G * 0.9),
                    (int)(PrimaryColor.B * 0.9)
                );

            btnPrijaviSe.MouseLeave += (s, e) =>
                btnPrijaviSe.BackColor = PrimaryColor;

            // Error labels
            lblKorisnickoImeError = new Label
            {
                Text = "",
                Font = RegularFont,
                ForeColor = Color.FromArgb(220, 38, 38), // Error red
                AutoSize = true,
                BackColor = SecondaryColor,
                Visible = false
            };

            lblLozinkaError = new Label
            {
                Text = "",
                Font = RegularFont,
                ForeColor = Color.FromArgb(220, 38, 38), // Error red
                AutoSize = true,
                BackColor = SecondaryColor,
                Visible = false
            };

            mainPanel.Controls.AddRange(new Control[] {
                lblPrijava,
                lblKorisnickoIme,
                lblKorisnickoImeError,
                lblLozinka,
                lblLozinkaError,
                tbKorisnickoIme.Parent,
                tbLozinka.Parent,
                btnPrijaviSe
            });

            // Add close button to main panel
            var closeButton = CreateCloseButton();
            mainPanel.Controls.Add(closeButton);

            // Add the main panel to the form
            this.Controls.Add(mainPanel);
        }

        private TextBox CreateStyledTextBox()
        {
            var container = new Panel
            {
                Size = new Size(300, 40),
                BackColor = Color.FromArgb(249, 250, 251)
            };

            var textBox = new TextBox
            {
                Size = new Size(280, 30),
                Location = new Point(10, 5),
                Font = RegularFont,
                BorderStyle = BorderStyle.None,
                BackColor = container.BackColor
            };

            var bottomBorder = new Panel
            {
                Size = new Size(300, 2),
                Location = new Point(0, 38),
                BackColor = Color.FromArgb(229, 231, 235)
            };

            textBox.Enter += (s, e) => bottomBorder.BackColor = PrimaryColor;
            textBox.Leave += (s, e) => bottomBorder.BackColor = Color.FromArgb(229, 231, 235);

            container.Controls.Add(textBox);
            container.Controls.Add(bottomBorder);

            return textBox;
        }

        private void StyleControls()
        {
            int centerX = (this.ClientSize.Width - 300) / 2;

            lblPrijava.Location = new Point(
                (this.ClientSize.Width - lblPrijava.Width) / 2,
                40
            );

            lblKorisnickoIme.Location = new Point(centerX, 120);
            lblKorisnickoImeError.Location = new Point(centerX, 190);
            tbKorisnickoIme.Parent.Location = new Point(centerX, 150);

            lblLozinka.Location = new Point(centerX, 220);
            lblLozinkaError.Location = new Point(centerX, 290);
            tbLozinka.Parent.Location = new Point(centerX, 250);

            btnPrijaviSe.Location = new Point(centerX, 350);
        }

        private Label CreateCloseButton()
        {
            var closeButton = new Label
            {
                Text = "×",
                Font = new Font("Arial", 16),
                ForeColor = TextColor,
                Size = new Size(30, 30),
                Location = new Point(this.Width - 40, 10),
                TextAlign = ContentAlignment.MiddleCenter,
                Cursor = Cursors.Hand,
                BackColor = SecondaryColor
            };

            closeButton.BringToFront();  // Add this line
            closeButton.Click += (s, e) => Application.Exit();
            closeButton.MouseEnter += (s, e) => closeButton.ForeColor = Color.Red;
            closeButton.MouseLeave += (s, e) => closeButton.ForeColor = TextColor;

            return closeButton;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left)
            {
                this.Capture = false;
                Message msg = Message.Create(this.Handle, 0xA1, new IntPtr(2), IntPtr.Zero);
                this.WndProc(ref msg);
            }
        }

        private void btnPrijaviSe_Click(object sender, EventArgs e)
        {
            ResetujVizualneEfekte();
            string unesenoKorisnickoIme = tbKorisnickoIme.Text.Trim();
            string unesenaLozinka = tbLozinka.Text;

            if (string.IsNullOrWhiteSpace(unesenoKorisnickoIme) || string.IsNullOrWhiteSpace(unesenaLozinka))
            {
                if (string.IsNullOrWhiteSpace(unesenoKorisnickoIme))
                {
                    ShowError(lblKorisnickoImeError, "Molimo unesite korisničko ime");
                    SetErrorState(tbKorisnickoIme);
                }
                if (string.IsNullOrWhiteSpace(unesenaLozinka))
                {
                    ShowError(lblLozinkaError, "Molimo unesite lozinku");
                    SetErrorState(tbLozinka);
                }
                return;
            }

            if (credentials.TryGetValue(unesenoKorisnickoIme, out var userInfo))
            {
                if (unesenaLozinka == userInfo.password)
                {
                    GlobalVariables.KorisnickoIme = unesenoKorisnickoIme;
                    if (userInfo.isLibrarian)
                    {
                        var pocetna = new LibrarianDashboard(unesenoKorisnickoIme);
                        pocetna.Show();
                    }
                    else
                    {
                        var korisnikPocetna = new KorisnikPocetna(unesenoKorisnickoIme);
                        korisnikPocetna.Show();
                    }
                    this.Hide();
                    return;
                }
                ShowError(lblLozinkaError, "Lozinka nije ispravna");
                SetErrorState(tbLozinka);
            }
            else
            {
                ShowError(lblKorisnickoImeError, "Korisničko ime nije pronađeno");
                SetErrorState(tbKorisnickoIme);
            }
        }

        private void SetErrorState(TextBox textBox)
        {
            Color errorColor = Color.FromArgb(254, 242, 242);
            textBox.Parent.BackColor = errorColor;
            textBox.BackColor = errorColor;
        }

        private void ResetujVizualneEfekte()
        {
            Color normalColor = Color.FromArgb(249, 250, 251);
            tbKorisnickoIme.Parent.BackColor = normalColor;
            tbKorisnickoIme.BackColor = normalColor;
            tbLozinka.Parent.BackColor = normalColor;
            tbLozinka.BackColor = normalColor;
            lblKorisnickoImeError.Visible = false;
            lblLozinkaError.Visible = false;
        }

        private void ShowError(Label errorLabel, string message)
        {
            errorLabel.Text = message;
            errorLabel.Visible = true;
        }
    }
}