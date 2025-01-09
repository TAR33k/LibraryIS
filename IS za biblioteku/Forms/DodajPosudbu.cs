using IS_za_biblioteku.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace IS_za_biblioteku.Forms
{
    public partial class DodajPosudbu : Form
    {
        private List<Korisnik> originalKorisnici; // Originalni podaci za korisnike
        private List<Knjiga> originalKnjige;   // Originalni podaci za knjige
        private bool isFiltering = false;     // Zastavica za praćenje filtriranja

        public DodajPosudbu()
        {
            InitializeComponent();

            // Inicijalizacija originalnih podataka
            originalKorisnici = PodaciBiblioteke.Korisnici.ToList(); // Pretpostavljamo da je PodaciBiblioteke.Korisnici lista korisnika
            originalKnjige = PodaciBiblioteke.Knjige.ToList(); // Pretpostavljamo da je PodaciBiblioteke.Knjige lista knjiga

            // Dodajte knjige u ComboBox
            cbKnjige.Items.AddRange(originalKnjige.ToArray());
            cbKnjige.DisplayMember = "Naslov";  // Koristi Naslov za prikaz u ComboBox-u
            cbKnjige.ValueMember = "ID";  // Ako želite povezati ID knjige s vrijednostima

            // Dodajte korisnike u ComboBox
            cbKorisnici.Items.AddRange(originalKorisnici.ToArray());
            cbKorisnici.DisplayMember = "ImePrezime";  // Koristi ImePrezime za prikaz u ComboBox-u
            cbKorisnici.ValueMember = "ID";  // Ako želite povezati korisnički ID s vrijednostima

            // Omogući filtriranje
            cbKorisnici.TextChanged += CbKorisnici_TextChanged;
            cbKnjige.TextChanged += CbKnjige_TextChanged;

            // Poveži događaj za zatvaranje padajućeg menija nakon odabira
            cbKorisnici.SelectionChangeCommitted += (s, e) => cbKorisnici.DroppedDown = false;
            cbKnjige.SelectionChangeCommitted += (s, e) => cbKnjige.DroppedDown = false;
        }

        // Metoda za filtriranje korisnika
        private void CbKorisnici_TextChanged(object sender, EventArgs e)
        {
            FilterComboBox<Korisnik>(cbKorisnici, originalKorisnici);
        }

        // Metoda za filtriranje knjiga
        private void CbKnjige_TextChanged(object sender, EventArgs e)
        {
            FilterComboBox<Knjiga>(cbKnjige, originalKnjige);
        }

        // Generička metoda za filtriranje podataka
        private void FilterComboBox<T>(ComboBox comboBox, List<T> originalData) where T : class
        {
            // Spriječi beskonačno aktiviranje
            if (isFiltering) return;

            try
            {
                isFiltering = true; // Postavi zastavicu da označi da je filtriranje u toku

                // Sačuvaj unos korisnika
                string userInput = comboBox.Text;

                // Filtriraj podatke
                var filteredData = originalData
                    .Where(item => item != null && item.ToString().ToLower().Contains(userInput.ToLower()))
                    .ToList();

                // Pauziraj događaj TextChanged da spriječi ponavljanje
                comboBox.TextChanged -= CbKorisnici_TextChanged;
                comboBox.TextChanged -= CbKnjige_TextChanged;

                // Ažuriraj stavke u ComboBox-u
                if (filteredData.Count > 0)
                {
                    comboBox.Items.Clear();
                    comboBox.Items.AddRange(filteredData.ToArray());
                    comboBox.DroppedDown = true; // Otvorite padajući meni ako ima rezultata
                }
                else
                {
                    comboBox.DroppedDown = false; // Zatvorite padajući meni ako nema rezultata
                }

                // Ponovo postavi korisnikov unos
                comboBox.Text = userInput;
                comboBox.SelectionStart = userInput.Length;
                comboBox.SelectionLength = 0;
            }
            finally
            {
                // Vratimo zastavicu i ponovo povežemo događaj
                isFiltering = false;

                if (comboBox == cbKorisnici)
                    comboBox.TextChanged += CbKorisnici_TextChanged;
                else
                    comboBox.TextChanged += CbKnjige_TextChanged;
            }
        }

        private void DodajPosudbu_Load(object sender, EventArgs e)
        {
            // Dodatna inicijalizacija, ako je potrebna
        }

        private void materialRaisedButton1_Click(object sender, EventArgs e)
        {
            // Provjerite da li je selektiran korisnik i knjiga
            var odabraniKorisnik = (Korisnik)cbKorisnici.SelectedItem;
            var odabranaKnjiga = (Knjiga)cbKnjige.SelectedItem;

            if (odabraniKorisnik == null || odabranaKnjiga == null)
            {
                MessageBox.Show("Molimo odaberite korisnika i knjigu.");
                return;
            }

            // Kreirajte novu posudbu
            var novaPosudba = new Posudba
            {
                Knjiga = odabranaKnjiga,
                Korisnik = odabraniKorisnik,
                DatumPosudbe = DateTime.Now,
                DatumVracanja = DateTime.Now.AddMonths(1),
            };

            // Dodavanje nove posudbe u listu
            PodaciBiblioteke.Posudbe.Add(novaPosudba);

            // Osvježite podatke u aktivnim posudbama
            Aktivne_posudbe.Instance.OsvjeziPodatke();

            MessageBox.Show("Posudba je uspješno unesena!", "Uspjeh", MessageBoxButtons.OK, MessageBoxIcon.Information);
            DialogResult = DialogResult.OK;

            // Zatvorite formu nakon unosa
            this.Close();
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
