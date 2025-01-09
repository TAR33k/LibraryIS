using IS_za_biblioteku.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace IS_za_biblioteku.Forms
{
    public partial class ListaKorisnika : Form
    {
        private static ListaKorisnika instance; // Singleton instance forme
        private List<Korisnik> korisnici; // Originalna lista korisnika
        private Panel topPanel;


        public static ListaKorisnika GetInstance()
        {
            if (instance == null || instance.IsDisposed)
            {
                instance = new ListaKorisnika();
            }
            return instance;
        }

        public ListaKorisnika()
        {
            InitializeComponent();
            InitializeLayout();
            korisnici = PodaciBiblioteke.Korisnici.ToList(); // Pretpostavka da imate listu svih korisnika
            ConfigureDataGridView();
            LoadUsers();
            PopulateComboBox();
            AttachEvents();
            dgvKorisnici.AutoGenerateColumns = false;
            dgvKorisnici.CellClick += DgvKorisnici_CellClick;
            dgvKorisnici.CellFormatting += DgvKorisnici_CellFormatting;
        }

        private void PopulateComboBox()
        {
            // Dodajte prazan objekt kao opciju "Svi"
            List<Clanarina> clanarineWithDefault = new List<Clanarina>
            {
                new Clanarina { Id = -1, Naziv = "Sve" } // Stavka koja označava sve korisnike
            };

            clanarineWithDefault.AddRange(PodaciBiblioteke.Clanarine); // Dodajte postojeće članarine

            cmbClanarina.DataSource = clanarineWithDefault;
            cmbClanarina.DisplayMember = "Naziv"; // Prikazivanje naziva članarine
            cmbClanarina.ValueMember = "Id"; // Postavljamo ID kao vrijednost

            // Postavite SelectedIndex na -1 da ništa nije odabrano po defaultu
            cmbClanarina.SelectedIndex = -1;
        }

        private void InitializeLayout()
        {
            string korisnickoIme1 = GlobalVariables.KorisnickoIme;

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
                Image = Image.FromFile(imagePath), 
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
                Text = Text = $"{korisnickoIme1}",
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
            pocetnaMenuItem.Click += (s, e) => IdiNaPočetnu();
            listaKnjigaMenuItem.Click += (s, e) => PrikaziListuKnjiga();
            dodajNovuKnjiguMenuItem.Click += (s, e) => DodajNovuKnjigu();
            //listaKorisnikaMenuItem.Click += (s, e) => PrikaziListuKorisnika();
            dodajNovogKorisnikaMenuItem.Click += (s, e) => DodajNovogKorisnika();
            aktivnePosudbeMenuItem.Click += (s, e) => PrikaziAktivnePosudbe();
            dodajNovuPosudbuMenuItem.Click += (s, e) => DodajNovuPosudbu();
            //evidentirajVracanjeMenuItem.Click += (s, e) => EvidentirajVracanje();
        }

        private void DodajNovuKnjigu()
        {
            DodajKnjigu dodajKnjigu = new DodajKnjigu();
            dodajKnjigu.ShowDialog();
        }

        private void PrikaziListuKnjiga()
        {
            ListaKnjiga listaKnjiga = new ListaKnjiga();
            listaKnjiga.ShowDialog();
        }

        private void IdiNaPočetnu()
        {
            Pocetna pocetna = new Pocetna(null);
            pocetna.ShowDialog();
        }

        private void DodajNovogKorisnika()
        {
            DodajKorisnika novaForma = new DodajKorisnika();

            if (novaForma.ShowDialog() == DialogResult.OK)
            {
                korisnici = PodaciBiblioteke.Korisnici.ToList();
                LoadUsers();
            }
        }
        private void DodajNovuPosudbu()
        {
            DodajPosudbu dodajPosudbu = new DodajPosudbu();
            dodajPosudbu.ShowDialog();
        }

        private void PrikaziAktivnePosudbe()
        {
            Aktivne_posudbe aktivne_Posudbe = new Aktivne_posudbe();
            aktivne_Posudbe.ShowDialog();
        }

        private void PrikaziListuKorisnika()
        {
            ListaKorisnika listaKorisnika = new ListaKorisnika();
            listaKorisnika.ShowDialog();
        }

        private void ConfigureDataGridView()
        {
            dgvKorisnici.BackgroundColor = Color.White;
            dgvKorisnici.BorderStyle = BorderStyle.None;
            dgvKorisnici.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvKorisnici.AutoGenerateColumns = false;
            dgvKorisnici.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.OrangeRed,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            dgvKorisnici.DefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.White,
                ForeColor = Color.Black,
                SelectionBackColor = Color.LightCoral,
                SelectionForeColor = Color.White
            };
            dgvKorisnici.RowTemplate.Height = 30;
        }

        private void LoadUsers()
        {
            dgvKorisnici.DataSource = null;
            dgvKorisnici.DataSource = korisnici.Select(k => new
            {
                Ime = k.Ime,
                Prezime = k.Prezime,
                Email = k.Email,
                StatusClanarine = k.DatumIsteka < DateTime.Today ? "❌Istekla" : "✅Aktivna",
                VrstaClanarine = k.VrstaClanarine, // Ovo je VrstaClanarine
                DatumIsteka = k.DatumIsteka != null ? k.DatumIsteka.Value.ToString("dd.MM.yyyy") : "N/A" // Format datuma
            }).ToList();
            dgvKorisnici.Refresh();
        }

        private void AttachEvents()
        {
            tbIme.TextChanged += (s, e) => FilterUsers();
            tbPrezime.TextChanged += (s, e) => FilterUsers();
            cmbClanarina.SelectedIndexChanged += (s, e) => FilterUsers();
        }

        private void FilterUsers()
        {
            string imeFilter = tbIme.Text.ToLower();
            string prezimeFilter = tbPrezime.Text.ToLower();
            int selectedClanarinaId = cmbClanarina.SelectedValue != null ? (int)cmbClanarina.SelectedValue : 0;

            var filteredUsers = korisnici.AsEnumerable();

            // Primjena filtara samo ako nisu prazni
            if (!string.IsNullOrWhiteSpace(imeFilter))
            {
                filteredUsers = filteredUsers.Where(k => k.Ime.ToLower().Contains(imeFilter));
            }

            if (!string.IsNullOrWhiteSpace(prezimeFilter))
            {
                filteredUsers = filteredUsers.Where(k => k.Prezime.ToLower().Contains(prezimeFilter));
            }

            if (selectedClanarinaId != -1)
            {
                filteredUsers = filteredUsers.Where(k => k.Clanarina?.Id == selectedClanarinaId);
            }

            dgvKorisnici.DataSource = filteredUsers.Select(k => new
            {
                Ime = k.Ime,
                Prezime = k.Prezime,
                Email = k.Email,
                StatusClanarine = k.DatumIsteka < DateTime.Today ? "❌Istekla" : "✅Aktivna", // Status
                VrstaClanarine = k.VrstaClanarine, // Vrsta članarine
                DatumIsteka = k.DatumIsteka != null ? k.DatumIsteka.Value.ToString("dd.MM.yyyy") : "N/A" // Format datuma
            }).ToList();
        }

        private void DgvKorisnici_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string ime = dgvKorisnici.Rows[e.RowIndex].Cells["Ime"].Value.ToString();
                string prezime = dgvKorisnici.Rows[e.RowIndex].Cells["Prezime"].Value.ToString();
                Korisnik odabraniKorisnik = korisnici.FirstOrDefault(k => k.Ime == ime && k.Prezime == prezime);

                if (odabraniKorisnik != null)
                {
                    AžurirajKorisnika updateForm = new AžurirajKorisnika(odabraniKorisnik);
                    if (updateForm.ShowDialog() == DialogResult.OK)
                    {
                        korisnici = PodaciBiblioteke.Korisnici.ToList();
                        LoadUsers();
                    }
                }
            }
        }

        private void DgvKorisnici_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Provjera kolone StatusClanarine
            if (dgvKorisnici.Columns[e.ColumnIndex].Name == "StatusClanarine" && e.Value != null)
            {
                string status = e.Value.ToString();

                // Ako je status "❌Istekla", postavljamo crvenu boju
                if (status.Contains("❌Istekla"))
                {
                    e.CellStyle.ForeColor = Color.Red; // Crvena boja za istekle članarine
                }
                // Ako je status "✅Aktivna", postavljamo zelenu boju
                else if (status.Contains("✅Aktivna"))
                {
                    e.CellStyle.ForeColor = Color.Green; // Zelena boja za aktivne članarine
                }
            }
        }


        private void ListaKorisnika_Load(object sender, EventArgs e)
        {
            this.Text = "Lista korisnika - Biblioteka";
            this.BackColor = Color.White;
            topPanel.Controls[2].Location = new Point(this.Width - 250, 15); // Ažuriranje pozicije korisničkog imena
            topPanel.Controls[3].Location = new Point(this.Width - 120, 10); // Ažuriranje pozicije dugmeta za odjavu
        }

        private void materialLabel1_Click(object sender, EventArgs e)
        {

        }

        private void btnDodaj_Click(object sender, EventArgs e)
        {

            DodajKorisnika dodajKorisnika = new DodajKorisnika();
            if(dodajKorisnika.ShowDialog() == DialogResult.OK)
            {
                korisnici = PodaciBiblioteke.Korisnici.ToList();
                LoadUsers();
            }
        }
    }
}
