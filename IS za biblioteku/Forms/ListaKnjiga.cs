using IS_za_biblioteku.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace IS_za_biblioteku.Forms
{
    public partial class ListaKnjiga : Form
    {
        private static ListaKnjiga instance; // Singleton instance forme
        private List<Knjiga> knjige; // Originalna lista knjiga
        private Panel topPanel;

        public static ListaKnjiga GetInstance()
        {
            if (instance == null || instance.IsDisposed)
            {
                instance = new ListaKnjiga();
            }
            return instance;
        }

        public ListaKnjiga()
        {
            InitializeComponent();
            InitializeLayout();
            knjige = PodaciBiblioteke.Knjige.ToList(); // Pretpostavka da imate listu svih knjiga
            ConfigureDataGridView();
            LoadBooks();
            AttachEvents();
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
            //listaKnjigaMenuItem.Click += (s, e) => PrikaziListuKnjiga();
            pocetnaMenuItem.Click += (s, e) => IdiNaPočetnu();
            dodajNovuKnjiguMenuItem.Click += (s, e) => DodajNovuKnjigu();
            listaKorisnikaMenuItem.Click += (s, e) => PrikaziListuKorisnika();
            dodajNovogKorisnikaMenuItem.Click += (s, e) => DodajNovogKorisnika();
            aktivnePosudbeMenuItem.Click += (s, e) => PrikaziAktivnePosudbe();
            dodajNovuPosudbuMenuItem.Click += (s, e) => DodajNovuPosudbu();
            //evidentirajVracanjeMenuItem.Click += (s, e) => EvidentirajVracanje();
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

        private void DodajNovogKorisnika()
        {
            DodajKorisnika dodajKorisnika = new DodajKorisnika();
            dodajKorisnika.ShowDialog();
        }

        private void PrikaziListuKorisnika()
        {
            ListaKorisnika listaKorisnika = new ListaKorisnika();
            listaKorisnika.ShowDialog();
        }

        private void IdiNaPočetnu()
        {
            Pocetna pocetna = new Pocetna(null);
            pocetna.ShowDialog();
        }

        private void DodajNovuKnjigu()
        {
            DodajKnjigu novaForma = new DodajKnjigu();

            if (novaForma.ShowDialog() == DialogResult.OK)
            {
                knjige = PodaciBiblioteke.Knjige.ToList();
                LoadBooks();
            }
        }

        private void ConfigureDataGridView()
        {
            dgvKnjige.BackgroundColor = Color.White;
            dgvKnjige.BorderStyle = BorderStyle.None;
            dgvKnjige.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvKnjige.AutoGenerateColumns = false;
            dgvKnjige.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.OrangeRed,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            dgvKnjige.DefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.White,
                ForeColor = Color.Black,
                SelectionBackColor = Color.LightCoral,
                SelectionForeColor = Color.White
            };
            dgvKnjige.RowTemplate.Height = 30;
        }

        private void LoadBooks()
        {
            dgvKnjige.DataSource = null;
            dgvKnjige.DataSource = knjige.Select(k => new
            {
                Naslov = k.Naslov,
                Autor = k.Autor.ToString(),
                DostupnaKolicina = k.DostupnaKolicina,
                GodinaIzdavanja = k.GodinaIzdavanja,
                Zanr = k.Zanr.Naziv
            }).ToList();
        }

        private void AttachEvents()
        {
            textBox1.TextChanged += (s, e) => FilterBooks();
            textBox2.TextChanged += (s, e) => FilterBooks();
            dgvKnjige.CellClick += DgvKnjige_CellClick;
        }

        private void FilterBooks()
        {
            string nazivFilter = textBox1.Text.ToLower();
            string autorFilter = textBox2.Text.ToLower();

            var filteredBooks = knjige.Where(k =>
                (string.IsNullOrWhiteSpace(nazivFilter) || k.Naslov.ToLower().Contains(nazivFilter)) &&
                (string.IsNullOrWhiteSpace(autorFilter) || k.Autor.Ime.ToLower().Contains(autorFilter))
            );

            dgvKnjige.DataSource = filteredBooks.Select(k => new
            {
                Naslov = k.Naslov,
                Autor = k.Autor.ToString(),
                DostupnaKolicina = k.DostupnaKolicina,
                GodinaIzdavanja = k.GodinaIzdavanja,
                Zanr = k.Zanr.Naziv
            }).ToList();
        }

        private void DgvKnjige_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string naslov = dgvKnjige.Rows[e.RowIndex].Cells["Naslov"].Value.ToString();
                Knjiga odabranaKnjiga = knjige.FirstOrDefault(k => k.Naslov == naslov);

                if (odabranaKnjiga != null)
                {
                    AžurirajKnjigu updateForm = new AžurirajKnjigu(odabranaKnjiga);
                    if (updateForm.ShowDialog() == DialogResult.OK)
                    {
                        knjige = PodaciBiblioteke.Knjige.ToList();
                        LoadBooks();
                    }
                }
            }
        }

        private void ListaKnjiga_Load(object sender, EventArgs e)
        {
            this.Text = "Lista knjiga - Biblioteka";
            this.BackColor = Color.White;
            topPanel.Controls[2].Location = new Point(this.Width - 250, 15); // Ažuriranje pozicije korisničkog imena
            topPanel.Controls[3].Location = new Point(this.Width - 120, 10); // Ažuriranje pozicije dugmeta za odjavu
        }

        private void btnDodaj_Click(object sender, EventArgs e)
        {
            DodajKnjigu novaForma = new DodajKnjigu();

            if (novaForma.ShowDialog() == DialogResult.OK)
            {
                knjige = PodaciBiblioteke.Knjige.ToList();
                LoadBooks();
            }
        }
    }
}
