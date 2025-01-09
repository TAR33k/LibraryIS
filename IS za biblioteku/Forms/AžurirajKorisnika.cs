using IS_za_biblioteku.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace IS_za_biblioteku.Forms
{
    public partial class AžurirajKorisnika : Form
    {
        private Korisnik odabraniKorisnik;
        private List<Clanarina> vrsteClanarina;

        public AžurirajKorisnika(Korisnik odabraniKorisnik)
        {
            InitializeComponent();
            this.odabraniKorisnik = odabraniKorisnik;
            vrsteClanarina = PodaciBiblioteke.Clanarine.ToList(); // Pretpostavljamo da imaš listu svih vrsta članarina
            LoadFormData();
        }

        private void LoadFormData()
        {
            // Popuni polja s podacima korisnika
            tbIme.Text = odabraniKorisnik.Ime;
            tbPrezime.Text = odabraniKorisnik.Prezime;
            tbBrojTelefona.Text = odabraniKorisnik.BrojTelefona;
            tbEmail.Text = odabraniKorisnik.Email;

            // Postavi ComboBox sa vrstama članarina
            cmbVrstaClanarine.DataSource = vrsteClanarina;
            cmbVrstaClanarine.DisplayMember = "Naziv";
            cmbVrstaClanarine.ValueMember = "Id";

            // Postavi trenutnu vrstu članarine
            cmbVrstaClanarine.SelectedValue = odabraniKorisnik.Clanarina?.Id;

            // Postavi datum isteka članarine
            dateTimePicker1.Value = odabraniKorisnik.DatumIsteka ?? DateTime.Now;
        }

        private void btnSpremi_Click(object sender, EventArgs e)
        {
            // Validacija unosa
            if (string.IsNullOrWhiteSpace(tbIme.Text) ||
                string.IsNullOrWhiteSpace(tbPrezime.Text) ||
                string.IsNullOrWhiteSpace(tbBrojTelefona.Text) ||
                string.IsNullOrWhiteSpace(tbEmail.Text) ||
                cmbVrstaClanarine.SelectedValue == null)
            {
                DisplayErrorMessage("Molimo popunite sva polja.");
                return;
            }

            try
            {
                // Ažuriraj podatke korisnika
                odabraniKorisnik.Ime = tbIme.Text;
                odabraniKorisnik.Prezime = tbPrezime.Text;
                odabraniKorisnik.BrojTelefona = tbBrojTelefona.Text;
                odabraniKorisnik.Email = tbEmail.Text;
                odabraniKorisnik.Clanarina = vrsteClanarina.FirstOrDefault(c => c.Id == (int)cmbVrstaClanarine.SelectedValue);
                odabraniKorisnik.DatumIsteka = dateTimePicker1.Value;

                // Vrati rezultat
                DialogResult = DialogResult.OK;
                DisplaySuccessMessage();
                Close();
            }
            catch (Exception ex)
            {
                DisplayErrorMessage($"Došlo je do greške pri ažuriranju podataka: {ex.Message}");
            }
        }

        private void AžurirajKorisnika_Load(object sender, EventArgs e)
        {
            // Dodatna inicijalizacija prilikom učitavanja forme ako je potrebno
        }

        private void DisplaySuccessMessage()
        {
            MessageBox.Show("Podaci o korisniku su uspješno ažurirani.", "Uspješno", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void DisplayErrorMessage(string message)
        {
            MessageBox.Show(message, "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void metroButton1_Click_1(object sender, EventArgs e)
        {
            Close();
        }

        private void AžurirajKorisnika_Load_1(object sender, EventArgs e)
        {

        }
    }
}
