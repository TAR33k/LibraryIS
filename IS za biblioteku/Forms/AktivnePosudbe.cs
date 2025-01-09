using IS_za_biblioteku.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace IS_za_biblioteku.Forms
{
    public partial class Aktivne_posudbe : Form
    {
        private static Aktivne_posudbe _instance;
        private List<Posudba> posudba;
        private Panel topPanel;

        public Aktivne_posudbe()
        {
            InitializeComponent();
            //ConfigureDataGridView();
            InitializeLayout();
            dgvAktivnePosudbe.CellFormatting += dgvAktivnePosudbe_CellFormatting;  // Dodajte ovo
            dgvAktivnePosudbe.CellContentClick += dgvAktivnePosudbe_CellContentClick;
        }

        public static Aktivne_posudbe Instance
        {
            get
            {
                if (_instance == null || _instance.IsDisposed)  // Provjera da li instanca postoji ili je zatvorena
                {
                    _instance = new Aktivne_posudbe();
                }
                return _instance;
            }
        }

        private void Aktivne_posudbe_Load(object sender, EventArgs e)
        {
            // Poziv funkcije za učitavanje podataka
            UcitajPodatke();
            ConfigureDataGridView();
            // Dodavanje opcija u ComboBox
            cmbStatus.Items.Add("Sve");
            cmbStatus.Items.Add("Aktivna");
            cmbStatus.Items.Add("Istekla");
            cmbStatus.SelectedIndex = 0; // Početni odabir je "Sve"

            tbNaziv.TextChanged += tbNaziv_TextChanged;
            cmbStatus.SelectedIndexChanged += cmbStatus_SelectedIndexChanged;

            topPanel.Controls[2].Location = new Point(this.Width - 250, 15); // Ažuriranje pozicije korisničkog imena
            topPanel.Controls[3].Location = new Point(this.Width - 120, 10); // Ažuriranje pozicije dugmeta za odjavu

        }

        private void UcitajPodatke()
        {
            dgvAktivnePosudbe.DataSource = null;
            dgvAktivnePosudbe.DataSource = PodaciBiblioteke.Posudbe;
        }

        private void cmbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            FiltrirajPodatke();
        }

        private void tbNaziv_TextChanged(object sender, EventArgs e)
        {
            FiltrirajPodatke();
        }

        private void FiltrirajPodatke()
        {
            // Dobijamo unos iz TextBoxa i ComboBoxa
            string nazivFilter = tbNaziv.Text.Trim().ToLower();
            string statusFilter = cmbStatus.SelectedItem?.ToString(); // Ovdje pretpostavljamo da ComboBox ima vrijednosti "Aktivna" i "Istekla"

            // Filtriramo listu Posudbi
            var filtriranePosudbe = PodaciBiblioteke.Posudbe.Where(p =>
                (string.IsNullOrEmpty(nazivFilter) ||
                    p.Knjiga.Naslov.ToLower().Contains(nazivFilter) ||    // Filtriranje po nazivu knjige
                    p.Korisnik.Ime.ToLower().Contains(nazivFilter)) && // Filtriranje po nazivu korisnika
                (statusFilter == "Sve" || // Ako je odabrano "Sve", ne filtriramo po statusu
                    (statusFilter == "Aktivna" && p.Status) ||
                    (statusFilter == "Istekla" && !p.Status))
            ).ToList();

            // Postavljanje filtriranih podataka u DataGridView
            dgvAktivnePosudbe.DataSource = filtriranePosudbe;
        }

        private void dgvAktivnePosudbe_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Provjera da li je trenutna kolona "Status"
            if (dgvAktivnePosudbe.Columns[e.ColumnIndex].Name == "Status")
            {
                if (e.Value != null && e.Value is bool)
                {
                    bool status = (bool)e.Value;

                    e.Value = status ? "Aktivna" : "Istekla";

                    if (!status)
                    {
                        e.CellStyle.ForeColor = Color.Red;  // Crvena boja za tekst
                        e.Value = "❗" + "Istekla ";  // Dodavanje emojija kao ikona
                    }
                    else
                    {
                        e.CellStyle.ForeColor = Color.Green;  // Zeleni za aktivne
                    }
                }
                e.FormattingApplied = true;  // Označava da je formatiranje primijenjeno
            }

            // Formatiranje datuma
            if (dgvAktivnePosudbe.Columns[e.ColumnIndex].Name == "DatumPosudbe" || dgvAktivnePosudbe.Columns[e.ColumnIndex].Name == "DatumVracanja")
            {
                if (e.Value != null)
                {
                    if (DateTime.TryParse(e.Value.ToString(), out DateTime datum))
                    {
                        e.Value = datum.ToString("dd.MM.yyyy");
                    }
                }
                e.FormattingApplied = true;  // Označava da je formatiranje primijenjeno
            }
        }

        private void InitializeLayout()
        {
            string korisnickoIme = GlobalVariables.KorisnickoIme;

            // Top Panel (za ikonicu, naslov i korisnika)
            topPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 50,
            };
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string imagePath = Path.Combine(basePath, "Slike", "knjiga2.png");
            // Ikonica knjiga
            PictureBox bookIcon = new PictureBox
            {
                Image = Image.FromFile(imagePath), // Postavi putanju do ikonice knjige
                SizeMode = PictureBoxSizeMode.Zoom,
                Size = new Size(40, 40),
                Location = new Point(10, 5) // Lijevo poravnanje
            };
            topPanel.Controls.Add(bookIcon);

            // Naslov
            Label titleLabel = new Label
            {
                Text = "Biblioteka",
                Font = new Font("Arial", 25, FontStyle.Italic),
                ForeColor = Color.Black,
                AutoSize = true,
                Location = new Point(60, 15) // Desno od ikonice
            };
            topPanel.Controls.Add(titleLabel);

            // Prijavljeni korisnik
            Label userLabel = new Label
            {
                Text = Text = $"{korisnickoIme}",
                Font = new Font("Arial", 14, FontStyle.Bold),
                ForeColor = Color.Black,
                AutoSize = true,
                Location = new Point(this.Width - 250, 15), // Desno poravnanje
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            topPanel.Controls.Add(userLabel);

            // Dugme Odjavi se
            Button logoutButton = new Button
            {
                Text = "Odjavi se",
                Font = new Font("Arial", 10, FontStyle.Regular),
                BackColor = Color.LightCoral,
                ForeColor = Color.White,
                Size = new Size(100, 30),
                Location = new Point(this.Width - 120, 10), // Desno od korisničkog imena
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            logoutButton.Click += (s, e) => OdjaviSe();
            topPanel.Controls.Add(logoutButton);

            this.Controls.Add(topPanel);

            // MenuStrip ispod
            InitializeMenu();
        }
        private void OdjaviSe()
        {
            this.Close();
            Login login = new Login();
            login.ShowDialog();
        }

        private void InitializeMenu()
        {
            // Kreiranje MenuStrip komponente
            MenuStrip menuStrip = new MenuStrip();
            menuStrip.BackColor = Color.LightGray;

            // Kreiranje stavki za horizontalnu navigaciju
            ToolStripMenuItem pocetnaMenuItem = new ToolStripMenuItem("Početna");
            ToolStripMenuItem knjigeMenuItem = new ToolStripMenuItem("Knjige");
            ToolStripMenuItem korisniciMenuItem = new ToolStripMenuItem("Korisnici");
            ToolStripMenuItem posudbeMenuItem = new ToolStripMenuItem("Posudbe");

            // Postavljanje fonta za stavke menija
            Font menuFont = new Font("Arial", 20, FontStyle.Bold);
            pocetnaMenuItem.Font = menuFont;
            knjigeMenuItem.Font = menuFont;
            korisniciMenuItem.Font = menuFont;
            posudbeMenuItem.Font = menuFont;

            // Dodavanje podmenija za "Knjige"
            ToolStripMenuItem listaKnjigaMenuItem = new ToolStripMenuItem("Lista knjiga");
            ToolStripMenuItem dodajNovuKnjiguMenuItem = new ToolStripMenuItem("Dodaj novu knjigu");
            knjigeMenuItem.DropDownItems.Add(listaKnjigaMenuItem);
            knjigeMenuItem.DropDownItems.Add(dodajNovuKnjiguMenuItem);

            // Dodavanje podmenija za "Korisnici"
            ToolStripMenuItem listaKorisnikaMenuItem = new ToolStripMenuItem("Lista korisnika");
            ToolStripMenuItem dodajNovogKorisnikaMenuItem = new ToolStripMenuItem("Dodaj novog korisnika");
            korisniciMenuItem.DropDownItems.Add(listaKorisnikaMenuItem);
            korisniciMenuItem.DropDownItems.Add(dodajNovogKorisnikaMenuItem);

            // Dodavanje podmenija za "Posudbe"
            ToolStripMenuItem aktivnePosudbeMenuItem = new ToolStripMenuItem("Aktivne posudbe");
            ToolStripMenuItem dodajNovuPosudbuMenuItem = new ToolStripMenuItem("Dodaj novu posudbu");
            ToolStripMenuItem evidentirajVracanjeMenuItem = new ToolStripMenuItem("Evidentiraj vraćanje");
            posudbeMenuItem.DropDownItems.Add(aktivnePosudbeMenuItem);
            posudbeMenuItem.DropDownItems.Add(dodajNovuPosudbuMenuItem);
            posudbeMenuItem.DropDownItems.Add(evidentirajVracanjeMenuItem);

            // Dodavanje svih stavki u MenuStrip
            menuStrip.Items.Add(pocetnaMenuItem);
            menuStrip.Items.Add(knjigeMenuItem);
            menuStrip.Items.Add(korisniciMenuItem);
            menuStrip.Items.Add(posudbeMenuItem);

            // Dodavanje separatora za ravnomjerno raspoređivanje
            menuStrip.Items.Add(new ToolStripSeparator()); // Separator koji omogućava razmak između stavki

            // Dodavanje MenuStrip na formu ispod top panela
            menuStrip.Dock = DockStyle.None;
            menuStrip.Location = new Point(0, topPanel.Bottom);  // MenuStrip ide odmah ispod topPanel-a

            this.Controls.Add(menuStrip);

            // Postavljanje širine svakog item-a da zauzima isti prostor
            pocetnaMenuItem.Margin = new Padding(109, 0, 109, 0);  // Dodavanje margine za širenje
            knjigeMenuItem.Margin = new Padding(109, 0, 109, 0);
            korisniciMenuItem.Margin = new Padding(109, 0, 109, 0);
            posudbeMenuItem.Margin = new Padding(109, 0, 109, 0);

            // Koristimo hover za promjenu boje
            pocetnaMenuItem.MouseHover += (s, e) => pocetnaMenuItem.BackColor = System.Drawing.Color.LightBlue;
            pocetnaMenuItem.MouseLeave += (s, e) => pocetnaMenuItem.BackColor = System.Drawing.Color.Transparent;
            knjigeMenuItem.MouseHover += (s, e) => knjigeMenuItem.BackColor = System.Drawing.Color.LightBlue;
            knjigeMenuItem.MouseLeave += (s, e) => knjigeMenuItem.BackColor = System.Drawing.Color.Transparent;
            korisniciMenuItem.MouseHover += (s, e) => korisniciMenuItem.BackColor = System.Drawing.Color.LightBlue;
            korisniciMenuItem.MouseLeave += (s, e) => korisniciMenuItem.BackColor = System.Drawing.Color.Transparent;
            posudbeMenuItem.MouseHover += (s, e) => posudbeMenuItem.BackColor = System.Drawing.Color.LightBlue;
            posudbeMenuItem.MouseLeave += (s, e) => posudbeMenuItem.BackColor = System.Drawing.Color.Transparent;

            // Event Handleri za klikove na stavke
            pocetnaMenuItem.Click += (s, e) => IdiNaPocetnu();
            listaKnjigaMenuItem.Click += (s, e) => PrikaziListuKnjiga();
            dodajNovuKnjiguMenuItem.Click += (s, e) => DodajNovuKnjigu();
            listaKorisnikaMenuItem.Click += (s, e) => PrikaziListuKorisnika();
            dodajNovogKorisnikaMenuItem.Click += (s, e) => DodajNovogKorisnika();
            aktivnePosudbeMenuItem.Click += (s, e) => PrikaziAktivnePosudbe();
            dodajNovuPosudbuMenuItem.Click += (s, e) => DodajNovuPosudbu();
            evidentirajVracanjeMenuItem.Click += (s, e) => EvidentirajVracanje();
        }

        private void IdiNaPocetnu()
        {
            Pocetna pocetna = new Pocetna(null);
            pocetna.ShowDialog();
        }

        private void PrikaziListuKnjiga()
        {
            ListaKnjiga listaKnjiga = new ListaKnjiga();
            listaKnjiga.Show();
        }

        private void DodajNovuKnjigu()
        {
            DodajKnjigu dodajKnjigu = new DodajKnjigu();
            dodajKnjigu.Show();
        }

        private void PrikaziListuKorisnika()
        {
            ListaKorisnika listaKorisnika = new ListaKorisnika();
            listaKorisnika.Show();
        }

        private void DodajNovogKorisnika()
        {
            DodajKorisnika dodajKorisnika = new DodajKorisnika();
            dodajKorisnika.Show();
        }

        private void PrikaziAktivnePosudbe()
        {
            Aktivne_posudbe aktivne_Posudbe = new Aktivne_posudbe();
            aktivne_Posudbe.Show();
        }

        private void DodajNovuPosudbu()
        {
            DodajPosudbu dodajPosudbu = new DodajPosudbu();
            dodajPosudbu.Show();
        }

        private void EvidentirajVracanje()
        {
            MessageBox.Show("Evidentiranje vraćanja knjige.");
        }
        public void OsvjeziPodatke()
        {
            UcitajPodatke();  // Poziv funkcije koja će učitati podatke u DataGridView
            FiltrirajPodatke();  // Osigurava filtriranje podataka prema uvjetima
        }
        private void ConfigureDataGridView()
        {
            dgvAktivnePosudbe.AutoGenerateColumns = false;
            dgvAktivnePosudbe.BackgroundColor = Color.White;
            dgvAktivnePosudbe.BorderStyle = BorderStyle.None;
            dgvAktivnePosudbe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvAktivnePosudbe.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.OrangeRed,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            dgvAktivnePosudbe.DefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.White,
                ForeColor = Color.Black,
                SelectionBackColor = Color.LightCoral,
                SelectionForeColor = Color.White
            };
            dgvAktivnePosudbe.RowTemplate.Height = 30;
        }

        private void dgvAktivnePosudbe_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Provjera da li je kliknuta kolona s indeksom 5 (6. kolona jer indeks počinje od 0)
            if (e.ColumnIndex == 5 && e.RowIndex >= 0)
            {
                // Otvoriti formu za evidentiranje vraćanja
                EvidentirajVracanje(e.RowIndex);
            }
        }

        private void EvidentirajVracanje(int rowIndex)
        {
            // Dobivanje podataka o posudbi na temelju odabranog reda
            Posudba posudba = PodaciBiblioteke.Posudbe[rowIndex];

            // Otvoriti formu za evidenciju vraćanja s podacima iz odabrane posudbe
            EvidentiranjeVraćanja vraćanjeForm = new EvidentiranjeVraćanja(posudba);

            if (vraćanjeForm.ShowDialog() == DialogResult.OK)
            {
                PodaciBiblioteke.Posudbe.RemoveAt(rowIndex);

            }
            // Osvježavanje prikaza DataGridView-a
            OsvjeziPodatke();
        }
        private void metroButton1_Click(object sender, EventArgs e)
        {
            DodajPosudbu dodajPosudbu = new DodajPosudbu();
            if (dodajPosudbu.ShowDialog() == DialogResult.OK)
            {
                // Ažuriranje podataka nakon dodavanja posudbe
                posudba=PodaciBiblioteke.Posudbe.ToList();
                OsvjeziPodatke();
            }
        }
    }
}
