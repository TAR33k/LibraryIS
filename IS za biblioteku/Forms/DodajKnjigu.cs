using IS_za_biblioteku.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace IS_za_biblioteku.Forms
{
    public partial class DodajKnjigu : Form
    {
        private List<Zanr> zanrovi;
        private ErrorProvider errorProvider;

        public DodajKnjigu()
        {
            InitializeComponent();
            zanrovi = PodaciBiblioteke.Zanrovi.ToList();
            errorProvider = new ErrorProvider();
            LoadFormData();
        }

        private void LoadFormData()
        {
            cmbZanr.SelectedIndex = -1;

            cmbZanr.DataSource = zanrovi;
            cmbZanr.DisplayMember = "Naziv";
            cmbZanr.ValueMember = "Id";

            var godine = new List<int>();
            for (int i = 1900; i <= DateTime.Now.Year; i++)
            {
                godine.Add(i);
            }

            cmbGodine.DataSource = godine;
        }

        private bool ValidateInputs()
        {
            bool isValid = true;

            // Validacija za naziv knjige
            if (string.IsNullOrWhiteSpace(tbNazivKnjige.Text))
            {
                HighlightControl(tbNazivKnjige, "Unesite naziv knjige.");
                isValid = false;
            }
            else
            {
                ClearHighlight(tbNazivKnjige);
            }

            // Validacija za autora
            if (string.IsNullOrWhiteSpace(tbNazivAutora.Text))
            {
                HighlightControl(tbNazivAutora, "Unesite ime autora.");
                isValid = false;
            }
            else
            {
                ClearHighlight(tbNazivAutora);
            }

            // Validacija za dostupnu količinu
            if (string.IsNullOrWhiteSpace(tbDostupnaKolicina.Text) || !int.TryParse(tbDostupnaKolicina.Text, out _))
            {
                HighlightControl(tbDostupnaKolicina, "Unesite validnu količinu.");
                isValid = false;
            }
            else
            {
                ClearHighlight(tbDostupnaKolicina);
            }

            // Validacija za žanr
            if (cmbZanr.SelectedValue == null)
            {
                HighlightControl(cmbZanr, "Odaberite žanr.");
                isValid = false;
            }
            else
            {
                ClearHighlight(cmbZanr);
            }

            return isValid;
        }

        private void HighlightControl(Control control, string errorMessage)
        {
            errorProvider.SetError(control, errorMessage);
            control.BackColor = Color.LightCoral; // Postavlja crveni okvir
        }

        private void ClearHighlight(Control control)
        {
            errorProvider.SetError(control, "");
            control.BackColor = SystemColors.Window; // Vraća na originalnu boju
        }

        private void btnSpremi_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs())
            {
                return;
            }

            try
            {
                Knjiga novaKnjiga = new Knjiga
                {
                    Naslov = tbNazivKnjige.Text,
                    Autor = new Autor { Ime = tbNazivAutora.Text },
                    DostupnaKolicina = int.Parse(tbDostupnaKolicina.Text),
                    Zanr = zanrovi.FirstOrDefault(z => z.Id == (int)cmbZanr.SelectedValue),
                    GodinaIzdavanja = (int)cmbGodine.SelectedItem,
                    Dostupna = true
                };

                novaKnjiga.Id = PodaciBiblioteke.GetNextKnjigaId();
                PodaciBiblioteke.Knjige.Add(novaKnjiga);

                MessageBox.Show("Knjiga je uspješno dodana.", "Uspješno", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Došlo je do greške: {ex.Message}", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void DodajKnjigu_Load(object sender, EventArgs e)
        {

        }

        private void metroButton1_Click_1(object sender, EventArgs e)
        {
            Close();
        }
    }
}
