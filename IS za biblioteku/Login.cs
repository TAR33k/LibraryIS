using IS_za_biblioteku.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IS_za_biblioteku
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            // Povećaj font
            lblPrijava.Font = new Font("Arial", 18, FontStyle.Bold); // Povećanje fonta i podešavanje stila

            // Podesi boju fonta
            lblPrijava.ForeColor = Color.Black;

            // Postavi tekst na sredinu unutar labela
            lblPrijava.TextAlign = ContentAlignment.MiddleCenter;
            lblPrijava.Location = new Point((this.ClientSize.Width - lblPrijava.Width) / 2, lblPrijava.Location.Y);

            tbKorisnickoIme.Text = "korisnik1";
            tbLozinka.Text = "korisnik1";
        }

        private readonly Dictionary<string, (string password, bool isLibrarian)> credentials = new Dictionary<string, (string, bool)>
        {
            { "bibliotekar1", ("bibliotekar1", true) },
            { "korisnik1", ("korisnik1", false) }
        };
        private void btnPrijaviSe_Click(object sender, EventArgs e)
        {
            ResetujVizualneEfekte();
            string unesenoKorisnickoIme = tbKorisnickoIme.Text.Trim();
            string unesenaLozinka = tbLozinka.Text;

            // Provjera da li su polja prazna
            if (string.IsNullOrWhiteSpace(unesenoKorisnickoIme) || string.IsNullOrWhiteSpace(unesenaLozinka))
            {
                MessageBox.Show("Molimo popunite sva polja.", "Upozorenje", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Provjera kredencijala
            if (credentials.TryGetValue(unesenoKorisnickoIme, out var userInfo))
            {
                if (unesenaLozinka == userInfo.password)
                {
                    GlobalVariables.KorisnickoIme = unesenoKorisnickoIme;

                    // Otvaranje odgovarajuće forme bazirano na tipu korisnika
                    if (userInfo.isLibrarian)
                    {
                        var pocetna = new Pocetna(unesenoKorisnickoIme);
                        pocetna.Show();
                    }
                    else
                    {
                        var korisnikPocetna = new KorisnikPocetna(unesenoKorisnickoIme);
                        korisnikPocetna.Show();
                    }

                    this.Hide(); // Sakrivamo umjesto zatvaranja
                    return;
                }
                tbLozinka.BackColor = Color.LightCoral;
                MessageBox.Show("Lozinka nije ispravna.", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                tbKorisnickoIme.BackColor = Color.LightCoral;
                MessageBox.Show("Korisničko ime nije pronađeno.", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Add form closing handler
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            if (e.CloseReason == CloseReason.UserClosing)
            {
                Application.Exit(); // Zatvara cijelu aplikaciju
            }
        }

        private void ResetujVizualneEfekte()
        {
            // Vraćanje defaultne boje polja
            tbKorisnickoIme.BackColor = Color.White;
            tbLozinka.BackColor = Color.White;
        }
    }
}
