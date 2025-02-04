using IS_za_biblioteku.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IS_za_biblioteku.Forms
{
    public partial class DodajKorisnika : Form
    {
        private ErrorProvider errorProvider;

        public DodajKorisnika()
        {
            InitializeComponent();
            PopulateComboBox();
            errorProvider = new ErrorProvider();
        }

        private void PopulateComboBox()
        {
            cmbVrstaClanarine.DataSource = PodaciBiblioteke.Clanarine;
            cmbVrstaClanarine.DisplayMember = "Naziv";
            cmbVrstaClanarine.ValueMember = "Id";
        }

        private void btnDodaj_Click(object sender, EventArgs e)
        {
            // Validacija unosa
            errorProvider.Clear();

            bool validno = true;

            if (string.IsNullOrEmpty(tbIme.Text))
            {
                errorProvider.SetError(tbIme, "Molimo unesite ime.");
                validno = false;
            }

            if (string.IsNullOrEmpty(tbPrezime.Text))
            {
                errorProvider.SetError(tbPrezime, "Molimo unesite prezime.");
                validno = false;
            }

            if (string.IsNullOrEmpty(tbEmail.Text) || !IsValidEmail(tbEmail.Text))
            {
                errorProvider.SetError(tbEmail, "Molimo unesite validnu email adresu.");
                validno = false;
            }

            if (string.IsNullOrEmpty(tbBrojTelefona.Text) || !IsValidPhoneNumber(tbBrojTelefona.Text))
            {
                errorProvider.SetError(tbBrojTelefona, "Molimo unesite validan broj telefona +3876XXXXXXXX.");
                validno = false;
            }

            if (!validno)
            {
                return;
            }

            // Kreiranje novog korisnika
            Korisnik noviKorisnik = new Korisnik
            {
                Ime = tbIme.Text,
                Prezime = tbPrezime.Text,
                Email = tbEmail.Text,
                //Clanarina = (Clanarina)cmbVrstaClanarine.SelectedItem,
                DatumIsteka = dateTimePicker1.Value
            };
            noviKorisnik.Id = PodaciBiblioteke.GetNextKorisnikId();

            // Dodavanje korisnika u listu i bazu podataka
            PodaciBiblioteke.Korisnici.Add(noviKorisnik);
            MessageBox.Show("Korisnik uspješno dodan.", "Potvrda", MessageBoxButtons.OK, MessageBoxIcon.Information);
            DialogResult = DialogResult.OK;
            Close();
        }

        private bool IsValidEmail(string email)
        {
            // Regex za validaciju email adrese
            string emailPattern = @"^[^\s@]+@[^\s@]+\.[^\s@]+$";
            return Regex.IsMatch(email, emailPattern);
        }

        private bool IsValidPhoneNumber(string phoneNumber)
        {
            // Regex za bosanski format brojeva telefona
            // Format: +387XXXXXXXX 
            string phonePattern = @" ^\+3876\d{ 6} (\d{ 1})?$";
            return Regex.IsMatch(phoneNumber, phonePattern);
        }

        private void btnOdustani_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel; // Zatvaranje forme bez potvrde
            Close();
        }

        private void DodajKorisnika_Load(object sender, EventArgs e)
        {

        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
