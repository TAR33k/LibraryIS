using IS_za_biblioteku.Entities;
using IS_za_biblioteku.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IS_za_biblioteku
{
    public partial class Pocetna : Form
    {
        private Panel panel1;
        private Panel panel2;
        private Panel panel3;
        private Panel topPanel;

        public Pocetna(string korisnickoIme)
        {
            InitializeComponent();
            PodaciBiblioteke.PopuniPodatke();
            InitializeLayout(korisnickoIme);
            InitializeCards();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            topPanel.Controls[2].Location = new Point(this.Width - 250, 15); // Ažuriranje pozicije korisničkog imena
            topPanel.Controls[3].Location = new Point(this.Width - 120, 10); // Ažuriranje pozicije dugmeta za odjavu
        }

        private void InitializeLayout(string korisnickoIme)
        {
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
                Text = $"{korisnickoIme}",
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
            listaKnjigaMenuItem.Click += (s, e) => PrikaziListuKnjiga();
            dodajNovuKnjiguMenuItem.Click += (s, e) => DodajNovuKnjigu();
            listaKorisnikaMenuItem.Click += (s, e) => PrikaziListuKorisnika();
            dodajNovogKorisnikaMenuItem.Click += (s, e) => DodajNovogKorisnika();
            aktivnePosudbeMenuItem.Click += (s, e) => PrikaziAktivnePosudbe();
            dodajNovuPosudbuMenuItem.Click += (s, e) => DodajNovuPosudbu();
            evidentirajVracanjeMenuItem.Click += (s, e) => EvidentirajVracanje();
        }


        // Event Handleri za svaku stavku
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

        private void InitializeCards()
        {
            // Panel za karticu aktivnih posudbi
            panel1 = new Panel();
            panel1.Size = new System.Drawing.Size(250, 150);
            panel1.Location = new System.Drawing.Point((this.ClientSize.Width - 750) / 2, (this.ClientSize.Height - 150) / 2); // Pozicioniranje u sredinu
            panel1.BackColor = System.Drawing.Color.Wheat; // Zemljani ton
            panel1.BorderStyle = BorderStyle.FixedSingle;
            SetRoundedPanel(panel1); // Poziv funkcije za zaobljene rubove

            Label label1 = new Label();
            label1.Text = $"Aktivne posudbe:\n {CountActiveLoans()}"; // Poziv metode za brojanje aktivnih posudbi
            label1.Location = new System.Drawing.Point(20, 50);
            label1.Font = new System.Drawing.Font("Arial", 14, System.Drawing.FontStyle.Bold); // Veći font
            label1.AutoSize = true;
            panel1.Controls.Add(label1);

            // Panel za karticu aktivnih korisnika
            panel2 = new Panel();
            panel2.Size = new System.Drawing.Size(250, 150);
            panel2.Location = new System.Drawing.Point(panel1.Right + 20, (this.ClientSize.Height - 150) / 2); // 20px razmak između kartica
            panel2.BackColor = System.Drawing.Color.PaleGoldenrod; // Zemljani ton
            panel2.BorderStyle = BorderStyle.FixedSingle;
            SetRoundedPanel(panel2); // Poziv funkcije za zaobljene rubove

            Label label2 = new Label();
            label2.Text = $"Aktivni korisnici:\n {CountActiveUsers()}"; // Poziv metode za brojanje aktivnih korisnika
            label2.Location = new System.Drawing.Point(20, 50);
            label2.Font = new System.Drawing.Font("Arial", 14, System.Drawing.FontStyle.Bold); // Veći font
            label2.AutoSize = true;
            panel2.Controls.Add(label2);

            // Panel za karticu dostupnih knjiga
            panel3 = new Panel();
            panel3.Size = new System.Drawing.Size(250, 150);
            panel3.Location = new System.Drawing.Point(panel2.Right + 20, (this.ClientSize.Height - 150) / 2); // 20px razmak između kartica
            panel3.BackColor = System.Drawing.Color.OliveDrab; // Zemljani ton
            panel3.BorderStyle = BorderStyle.FixedSingle;
            SetRoundedPanel(panel3); // Poziv funkcije za zaobljene rubove

            Label label3 = new Label();
            label3.Text = $"Dostupne knjige:\n {CountAvailableBooks()}"; // Poziv metode za brojanje dostupnih knjiga
            label3.Location = new System.Drawing.Point(20, 50);
            label3.Font = new System.Drawing.Font("Arial", 14, System.Drawing.FontStyle.Bold); // Veći font
            label3.AutoSize = true;
            panel3.Controls.Add(label3);

            // Dodaj panele na formu
            this.Controls.Add(panel1);
            this.Controls.Add(panel2);
            this.Controls.Add(panel3);
        }

        // Metoda za brojanje aktivnih korisnika
        private int CountActiveUsers()
        {
            return PodaciBiblioteke.Korisnici.Count(k => k.Aktivni);
        }

        // Metoda za brojanje dostupnih knjiga
        private int CountAvailableBooks()
        {
            return PodaciBiblioteke.Knjige.Count(k => k.Dostupna);
        }

        // Metoda za brojanje aktivnih posudbi
        private int CountActiveLoans()
        {
            return PodaciBiblioteke.Posudbe.Count(p => p.Status);
        }

        // Funkcija za postavljanje zaobljenih rubova na panel
        private void SetRoundedPanel(Panel panel)
        {
            var path = new GraphicsPath();
            int radius = 20; // Radijus za zaobljenje kutova

            // Dodaj zaobljene kutove za sve 4 kuta panela
            path.AddArc(0, 0, radius, radius, 180, 90); // Gornji lijevi kut
            path.AddArc(panel.Width - radius - 1, 0, radius, radius, 270, 90); // Gornji desni kut
            path.AddArc(panel.Width - radius - 1, panel.Height - radius - 1, radius, radius, 0, 90); // Donji desni kut
            path.AddArc(0, panel.Height - radius - 1, radius, radius, 90, 90); // Donji lijevi kut

            // Zatvori putanju i primijeni ga na region panela
            path.CloseFigure();
            panel.Region = new Region(path);
        }

        // Code to handle form resizing and keeping the panels centered
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            // Provjerite da li su paneli inicijalizirani prije nego što se pozove metoda
            if (panel1 != null && panel2 != null && panel3 != null)
            {
                int panelWidth = 250;
                int panelHeight = 150;
                int spacing = 20;

                // Calculate the total width and center them horizontally
                int totalWidth = (panelWidth * 3) + (spacing * 2); // 3 panels + 2 spacings
                int startX = (this.ClientSize.Width - totalWidth) / 2;

                // Update locations
                panel1.Location = new System.Drawing.Point(startX, (this.ClientSize.Height - panelHeight) / 2);
                panel2.Location = new System.Drawing.Point(panel1.Right + spacing, (this.ClientSize.Height - panelHeight) / 2);
                panel3.Location = new System.Drawing.Point(panel2.Right + spacing, (this.ClientSize.Height - panelHeight) / 2);
            }
        }
    }
}

