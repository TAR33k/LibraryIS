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
    public partial class AžurirajKnjigu : Form
    {
        private Knjiga odabranaKnjiga;
        private List<Zanr> zanrovi;

        public AžurirajKnjigu(Knjiga odabranaKnjiga)
        {
            InitializeComponent();
            this.odabranaKnjiga = odabranaKnjiga;
            zanrovi = PodaciBiblioteke.Zanrovi.ToList(); // Pretpostavljamo da imaš listu svih žanrova
            LoadFormData();
        }

        private void LoadFormData()
        {
            // Popuni polja s podacima knjige
            tbNazivKnjige.Text = odabranaKnjiga.Naslov;
            tbNazivAutora.Text = odabranaKnjiga.Autor.Ime;
            tbDostupnaKolicina.Text = odabranaKnjiga.DostupnaKolicina.ToString();

            // Popuni ComboBox sa žanrovima
            cmbZanr.DataSource = zanrovi;
            cmbZanr.DisplayMember = "Naziv";
            cmbZanr.ValueMember = "Id";

            // Postavi trenutni žanr knjige
            cmbZanr.SelectedValue = odabranaKnjiga.Zanr.Id;

            var godine = new List<int>();
            for (int i = 1900; i <= DateTime.Now.Year; i++)
            {
                godine.Add(i);
            }

            // Povežite ComboBox sa godinama
            cmbGodine.DataSource = godine;
            cmbGodine.SelectedItem = odabranaKnjiga.GodinaIzdavanja;
        }

        private void btnSpremi_Click(object sender, EventArgs e)
        {
            // Validacija unosa
            if (string.IsNullOrWhiteSpace(tbNazivKnjige.Text) ||
                string.IsNullOrWhiteSpace(tbNazivAutora.Text) ||
                string.IsNullOrWhiteSpace(tbDostupnaKolicina.Text) ||
                cmbZanr.SelectedValue == null)
            {
                DisplayErrorMessage("Molimo popunite sva polja.");
                return;
            }

            try
            {
                // Ažuriraj podatke knjige
                odabranaKnjiga.Naslov = tbNazivKnjige.Text;
                odabranaKnjiga.Autor.Ime = tbNazivAutora.Text;
                odabranaKnjiga.DostupnaKolicina = int.Parse(tbDostupnaKolicina.Text);
                odabranaKnjiga.Zanr = zanrovi.FirstOrDefault(z => z.Id == (int)cmbZanr.SelectedValue);

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

        private void AžurirajKnjigu_Load(object sender, EventArgs e)
        {

        }

        private void DisplaySuccessMessage()
        {
            MessageBox.Show("Podaci o knjizi su uspješno ažurirani.", "Uspješno", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void DisplayErrorMessage(string message)
        {
            MessageBox.Show(message, "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
