using IS_za_biblioteku.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IS_za_biblioteku.Forms
{
    public partial class EvidentiranjeVraćanja : Form
    {
        private Posudba posudba;

        public EvidentiranjeVraćanja(Posudba posudba)
        {
            InitializeComponent();
            this.posudba = posudba;
            PopuniPodatke();
        }

        private void EvidentiranjeVraćanja_Load(object sender, EventArgs e)
        {

        }
        private void PopuniPodatke()
        {
            // Popunjavanje podataka o knjizi
            tbNazivKnjige.Text = posudba.Knjiga.Naslov;
            tbNazivAutora.Text = posudba.Knjiga.Autor.ToString(); // Pretpostavljamo da Autor ima ToString() metodu koja vraća ime autora

            // Popunjavanje podataka o korisniku
            tbImePrezime.Text = posudba.Korisnik.ImePrezime;
            tbBrojTelefona.Text = posudba.Korisnik.BrojTelefona;
            tbEmail.Text = posudba.Korisnik.Email;

            // Status korisničke članarine
            tbStatus.Text = posudba.Korisnik.StatusClanarine;

            // Datum isteka članarine
            tbDatumIsteka.Text = posudba.Korisnik.DatumIsteka?.ToString("dd.MM.yyyy") ?? "Nema";

            // Status posudbe
            tbStatus.Text = posudba.Status ? "Aktivna" : "Istekla";  // Prikazujemo status posudbe

            // Dodavanje podataka za datum posudbe i datum vraćanja
            tbDatumPosudbe.Text = posudba.DatumPosudbe.ToString("dd.MM.yyyy");
            tbDatumVracanja.Text = posudba.DatumVracanja.ToString("dd.MM.yyyy");

            // Računanje kasnjenja
            if (posudba.DatumVracanja < DateTime.Now)
            {
                // Ako je datum vraćanja prošao, računamo koliko dana je knjiga kasnila
                int kasniDan = (DateTime.Now - posudba.DatumVracanja).Days;
                tbKasni.Text = kasniDan.ToString();  // Prikazujemo broj dana kašnjenja
            }
            else
            {
                // Ako knjiga nije kasnila, prikazujemo 0
                tbKasni.Text = "0 " + "dana";
            }
        }

        private void materialRaisedButton1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Evidencija vraćanja je uspješno izvršena. Knjiga je sada označena kao vraćena.",
                 "Uspješno vraćanje",
                 MessageBoxButtons.OK,
                 MessageBoxIcon.Information);
            DialogResult = DialogResult.OK;
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
