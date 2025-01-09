using IS_za_biblioteku.Entities;
using Microsoft.SqlServer.Server;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace IS_za_biblioteku.Forms
{
    public partial class KorisnikPocetna : Form
    {
        private readonly string korisnickoIme;
        private Korisnik trenutniKorisnik;
        private Panel mainContentPanel;
        private Panel topPanel;

        public KorisnikPocetna(string korisnickoIme)
        {
            InitializeComponent();
            this.korisnickoIme = korisnickoIme;
            this.Size = new Size(1200, 800);
            this.StartPosition = FormStartPosition.CenterScreen;
            PodaciBiblioteke.PopuniPodatke();
            trenutniKorisnik = PodaciBiblioteke.Korisnici[0];
            InitializeLayout();
        }

        private void InitializeLayout()
        {
            // Top Panel
            topPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.WhiteSmoke
            };

            var titleLabel = new Label
            {
                Text = "Biblioteka",
                Font = new Font("Arial", 24, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, 12)
            };

            var userLabel = new Label
            {
                Text = trenutniKorisnik.Ime + " " + trenutniKorisnik.Prezime,
                Font = new Font("Arial", 12),
                AutoSize = true,
                Location = new Point(this.Width - 250, 20),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };

            var logoutButton = new Button
            {
                Text = "Odjavi se",
                Size = new Size(100, 30),
                Location = new Point(this.Width - 120, 15),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                BackColor = Color.IndianRed,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            logoutButton.Click += (s, e) => OdjaviSe();

            topPanel.Controls.AddRange(new Control[] { titleLabel, userLabel, logoutButton });
            this.Controls.Add(topPanel);

            // Main Content Panel
            mainContentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20),
                BackColor = Color.White
            };
            this.Controls.Add(mainContentPanel);

            InitializeMenu();
            ShowDashboard(); // Show dashboard by default
        }

        private void InitializeMenu()
        {
            var menuPanel = new Panel
            {
                Dock = DockStyle.Left,
                Width = 200,
                BackColor = Color.FromArgb(51, 51, 76)
            };

            var menuItems = new[]
            {
                ("Početna", "pocetna"),
                ("Pretraga knjiga", "pretraga"),
                ("Moje posudbe", "posudbe"),
                ("Moj profil", "profil")
            };

            int buttonY = 20;
            foreach (var (text, tag) in menuItems)
            {
                var button = new Button
                {
                    Text = text,
                    Tag = tag,
                    Size = new Size(180, 40),
                    Location = new Point(10, buttonY),
                    FlatStyle = FlatStyle.Flat,
                    ForeColor = Color.White,
                    Font = new Font("Arial", 12),
                    TextAlign = ContentAlignment.MiddleLeft,
                    Padding = new Padding(10, 0, 0, 0)
                };

                button.FlatAppearance.BorderSize = 0;
                button.Click += MenuItem_Click;
                menuPanel.Controls.Add(button);
                buttonY += 50;
            }

            this.Controls.Add(menuPanel);
            mainContentPanel.Location = new Point(menuPanel.Width, topPanel.Height);
            mainContentPanel.Size = new Size(
                this.ClientSize.Width - menuPanel.Width,
                this.ClientSize.Height - topPanel.Height
            );
        }

        private void MenuItem_Click(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (button?.Parent != null)
            {
                // Reset all button colors
                foreach (Control ctrl in button.Parent.Controls)
                {
                    if (ctrl is Button btn)
                    {
                        btn.BackColor = Color.FromArgb(51, 51, 76);
                    }
                }

                // Highlight selected button
                button.BackColor = Color.FromArgb(71, 71, 96);
                mainContentPanel.Controls.Clear();

                switch (button.Tag.ToString())
                {
                    case "pocetna":
                        ShowDashboard();
                        break;
                    case "pretraga":
                        ShowBookSearch();
                        break;
                    case "posudbe":
                        ShowMyBorrowings();
                        break;
                    case "profil":
                        ShowProfile();
                        break;
                }
            }
        }

        private void ShowDashboard()
        {
            var dashboardPanel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(20)
            };

            var welcomeLabel = new Label
            {
                Text = $"Dobro došli, {trenutniKorisnik.Ime}!",
                Font = new Font("Arial", 24, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(0, 0)
            };

            var membershipPanel = new Panel
            {
                Location = new Point(0, welcomeLabel.Bottom + 20),
                Size = new Size(400, 100),
                BackColor = trenutniKorisnik.Aktivni ? Color.FromArgb(200, 255, 200) : Color.FromArgb(255, 200, 200)
            };

            var membershipLabel = new Label
            {
                Text = $"Status članarine: {(trenutniKorisnik.Aktivni ? "Aktivna" : "Neaktivna")}\n" +
                      $"Tip članarine: {trenutniKorisnik.Clanarina.Naziv}\n" +
                      $"Datum isteka: {trenutniKorisnik.DatumIsteka:dd.MM.yyyy}",
                Font = new Font("Arial", 12),
                Location = new Point(10, 10),
                AutoSize = true
            };
            membershipPanel.Controls.Add(membershipLabel);

            var borrowedBooksLabel = new Label
            {
                Text = "Trenutno posuđene knjige:",
                Font = new Font("Arial", 14, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(0, membershipPanel.Bottom + 20)
            };

            var borrowedBooksPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                AutoSize = true,
                Location = new Point(0, borrowedBooksLabel.Bottom + 10),
                Width = mainContentPanel.Width - 60
            };

            foreach (var knjiga in trenutniKorisnik.PosudjeneKnjige)
            {
                borrowedBooksPanel.Controls.Add(CreateBookPanel(knjiga));
            }

            dashboardPanel.Controls.AddRange(new Control[]
            {
                welcomeLabel,
                membershipPanel,
                borrowedBooksLabel,
                borrowedBooksPanel
            });
            mainContentPanel.Controls.Add(dashboardPanel);

            // Popular books section
            var popularBooksSection = new Panel
            {
                AutoSize = true,
                Location = new Point(0, borrowedBooksPanel.Bottom + 30),
                Width = mainContentPanel.Width - 60
            };

            var popularBooksLabel = new Label
            {
                Text = "Popularne knjige",
                Font = new Font("Arial", 18, FontStyle.Bold),
                AutoSize = true
            };

            var carouselPanel = new Panel
            {
                AutoScroll = true,
                Width = mainContentPanel.Width - 60,
                Height = 280,
                Location = new Point(0, popularBooksLabel.Bottom + 15),
                BackColor = Color.White
            };

            var booksContainer = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.LeftToRight,
                AutoSize = true,
                Width = mainContentPanel.Width - 80,
                WrapContents = false,
                Padding = new Padding(10, 0, 10, 0)
            };

            // Get top 8 most borrowed books
            var popularBooks = PodaciBiblioteke.Knjige
                .OrderByDescending(k => new Random().Next())
                .Take(8);

            // Add books to container
            foreach (var knjiga in popularBooks)
            {
                booksContainer.Controls.Add(CreatePopularBookPanel(knjiga));
            }

            carouselPanel.Controls.Add(booksContainer);

            // Hide scrollbars but keep scrolling enabled
            carouselPanel.HorizontalScroll.Visible = false;
            carouselPanel.VerticalScroll.Visible = false;
            carouselPanel.HorizontalScroll.Enabled = true;
            carouselPanel.VerticalScroll.Enabled = false;

            // Updated scroll buttons
            var leftButton = new Button
            {
                Text = "❮",
                Size = new Size(40, 40),
                Location = new Point(0, (carouselPanel.Height - 40) / 2),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.White,
                Font = new Font("Arial", 18),
                Cursor = Cursors.Hand
            };
            leftButton.FlatAppearance.BorderSize = 0;
            leftButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(245, 245, 245);

            var rightButton = new Button
            {
                Text = "❯",
                Size = new Size(40, 40),
                Location = new Point(carouselPanel.Width - 40, (carouselPanel.Height - 40) / 2),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.White,
                Font = new Font("Arial", 18),
                Cursor = Cursors.Hand
            };
            rightButton.FlatAppearance.BorderSize = 0;
            rightButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(245, 245, 245);

            // Scroll handlers with smooth animation
            leftButton.Click += (s, e) =>
            {
                var targetValue = Math.Max(0, carouselPanel.HorizontalScroll.Value - 520);
                SmoothScroll(carouselPanel, targetValue);
            };

            rightButton.Click += (s, e) =>
            {
                var targetValue = Math.Min(
                    carouselPanel.HorizontalScroll.Maximum,
                    carouselPanel.HorizontalScroll.Value + 520);
                SmoothScroll(carouselPanel, targetValue);
            };

            carouselPanel.Controls.Add(booksContainer);
            popularBooksSection.Controls.AddRange(new Control[]
            {
    popularBooksLabel,
    leftButton,
    rightButton,
    carouselPanel
            });

            dashboardPanel.Controls.Add(popularBooksSection);
        }

        private void SmoothScroll(Panel panel, int targetValue)
        {
            var timer = new Timer();
            var startValue = panel.HorizontalScroll.Value;
            var steps = 20;
            var step = 0;

            timer.Interval = 16; // ~60fps
            timer.Tick += (s, e) =>
            {
                step++;
                var progress = (double)step / steps;
                var easedProgress = EaseInOutCubic(progress);
                var currentValue = startValue + (targetValue - startValue) * easedProgress;

                panel.HorizontalScroll.Value = (int)currentValue;

                if (step >= steps)
                {
                    timer.Stop();
                    timer.Dispose();
                }
            };
            timer.Start();
        }

        private double EaseInOutCubic(double x)
        {
            return x < 0.5 ? 4 * x * x * x : 1 - Math.Pow(-2 * x + 2, 3) / 2;
        }

        private void ShowBookSearch()
        {
            // Main container
            var searchPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20),
                BackColor = Color.White
            };

            // Fixed header section with increased height
            var headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 150, // Increased height
                BackColor = Color.White
            };

            var titleLabel = new Label
            {
                Text = "Pretraga knjiga",
                Font = new Font("Arial", 20, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(0, 35) // Added top margin
            };

            // Search controls container with adjusted position
            var searchContainer = new Panel
            {
                Height = 50,
                Width = mainContentPanel.Width - 60,
                Location = new Point(0, titleLabel.Bottom + 20),
                BackColor = Color.White
            };

            // Search box with placeholder
            var searchBox = new TextBox
            {
                Width = 300,
                Height = 30,
                Font = new Font("Arial", 12),
                Location = new Point(0, 5),
                BorderStyle = BorderStyle.FixedSingle
            };

            // Genre filter
            var genreCombo = new ComboBox
            {
                Width = 150,
                Height = 30,
                Location = new Point(searchBox.Right + 20, 5),
                Font = new Font("Arial", 12),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            genreCombo.Items.Add("Svi žanrovi");
            genreCombo.Items.AddRange(PodaciBiblioteke.Zanrovi.Select(z => z.Naziv).ToArray());
            genreCombo.SelectedIndex = 0;

            // Available only checkbox
            var availableOnly = new CheckBox
            {
                Text = "Samo dostupne knjige",
                Location = new Point(genreCombo.Right + 20, 10),
                AutoSize = true,
                Font = new Font("Arial", 12),
                Cursor = Cursors.Hand
            };

            // Results container with proper spacing
            var resultsContainer = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(0, 20, 0, 0), // Added top padding
            };

            var resultsFlowPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                AutoSize = true,
                Width = resultsContainer.Width - 20,
                WrapContents = false,
                AutoScroll = true,
                Dock = DockStyle.Fill,
                Padding = new Padding(0, 150, 0, 0)
            };

            void PerformFilteredSearch()
            {
                resultsFlowPanel.Controls.Clear();
                var searchTerm = searchBox.Text.ToLower();
                var selectedGenre = genreCombo.SelectedItem?.ToString();

                // Start with all books
                var query = PodaciBiblioteke.Knjige.AsQueryable();

                // Apply search term filter
                if (!string.IsNullOrEmpty(searchTerm) && !searchTerm.Equals("unesite naslov, autora ili žanr..."))
                {
                    query = query.Where(k =>
                        k.Naslov.ToLower().Contains(searchTerm) ||
                        k.Autor.Ime.ToLower().Contains(searchTerm) ||
                        k.Autor.Prezime.ToLower().Contains(searchTerm) ||
                        k.Zanr.Naziv.ToLower().Contains(searchTerm));
                }

                // Apply genre filter
                if (!string.IsNullOrEmpty(selectedGenre) && selectedGenre != "Svi žanrovi")
                {
                    query = query.Where(k => k.Zanr.Naziv == selectedGenre);
                }

                // Apply availability filter
                if (availableOnly.Checked)
                {
                    query = query.Where(k => k.Dostupna && k.DostupnaKolicina > 0);
                }

                var results = query.OrderBy(k => k.Naslov).ToList();

                // Add results header
                var headerLabel = new Label
                {
                    Text = $"Pronađeno {results.Count} knjiga",
                    Font = new Font("Arial", 12, FontStyle.Bold),
                    AutoSize = true,
                    Margin = new Padding(0, 0, 0, 15)
                };
                resultsFlowPanel.Controls.Add(headerLabel);

                if (!results.Any())
                {
                    var noResultsLabel = new Label
                    {
                        Text = "Nema pronađenih knjiga prema zadatim kriterijima.",
                        Font = new Font("Arial", 12),
                        AutoSize = true,
                        ForeColor = Color.Gray,
                        Margin = new Padding(0, 10, 0, 0)
                    };
                    resultsFlowPanel.Controls.Add(noResultsLabel);
                }
                else
                {
                    foreach (var knjiga in results)
                    {
                        resultsFlowPanel.Controls.Add(CreateSearchResultPanel(knjiga));
                    }
                }
            }

            // Wire up search events
            searchBox.TextChanged += (s, e) => PerformFilteredSearch();
            genreCombo.SelectedIndexChanged += (s, e) => PerformFilteredSearch();
            availableOnly.CheckedChanged += (s, e) => PerformFilteredSearch();

            // Add placeholder text
            searchBox.Enter += (s, e) =>
            {
                if (searchBox.Text == "Unesite naslov, autora ili žanr...")
                {
                    searchBox.Text = "";
                    searchBox.ForeColor = Color.Black;
                }
            };

            searchBox.Leave += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(searchBox.Text))
                {
                    searchBox.Text = "Unesite naslov, autora ili žanr...";
                    searchBox.ForeColor = Color.Gray;
                }
            };
            searchBox.Text = "Unesite naslov, autora ili žanr...";
            searchBox.ForeColor = Color.Gray;

            // Layout hierarchy
            searchContainer.Controls.AddRange(new Control[] { searchBox, genreCombo, availableOnly });
            headerPanel.Controls.AddRange(new Control[] { titleLabel, searchContainer });
            resultsContainer.Controls.Add(resultsFlowPanel);

            // Add a separator line
            var separator = new Panel
            {
                Height = 1,
                Dock = DockStyle.Top,
                BackColor = Color.LightGray
            };

            searchPanel.Controls.AddRange(new Control[] { headerPanel, separator, resultsContainer });
            mainContentPanel.Controls.Add(searchPanel);

            // Modified resize handler
            mainContentPanel.Resize += (s, e) =>
            {
                searchContainer.Width = mainContentPanel.Width - 60;
                resultsFlowPanel.Width = resultsContainer.Width - 20;
                foreach (Control c in resultsFlowPanel.Controls)
                {
                    if (c is Panel bookPanel)
                    {
                        bookPanel.Width = resultsFlowPanel.Width - 40;
                    }
                }
            };

            // Initial search
            PerformFilteredSearch();
        }

        // Update ShowAllBooks to include proper spacing for the header
        private void ShowAllBooks(FlowLayoutPanel resultsPanel)
        {
            resultsPanel.Controls.Clear();

            // Add header with proper spacing
            var headerLabel = new Label
            {
                Text = $"Ukupno knjiga: {PodaciBiblioteke.Knjige.Count}",
                Font = new Font("Arial", 12, FontStyle.Bold),
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 20) // Increased bottom margin
            };
            resultsPanel.Controls.Add(headerLabel);

            // Show all books
            foreach (var knjiga in PodaciBiblioteke.Knjige.OrderBy(k => k.Naslov))
            {
                resultsPanel.Controls.Add(CreateSearchResultPanel(knjiga));
            }
        }

        private Panel CreateSearchResultPanel(Knjiga knjiga)
        {
            var panel = new Panel
            {
                Width = mainContentPanel.Width - 100,
                Height = 130,
                Margin = new Padding(0, 0, 0, 15), // Increased bottom margin for better spacing
                BackColor = Color.WhiteSmoke,
                Padding = new Padding(15),
                Cursor = Cursors.Hand
            };

            var titleLabel = new Label
            {
                Text = knjiga.Naslov,
                Font = new Font("Arial", 14, FontStyle.Bold),
                Location = new Point(10, 10),
                AutoSize = true
            };

            var authorLabel = new Label
            {
                Text = $"Autor: {knjiga.Autor.Ime} {knjiga.Autor.Prezime}",
                Font = new Font("Arial", 12),
                Location = new Point(10, 35),
                AutoSize = true
            };

            var genreLabel = new Label
            {
                Text = $"Žanr: {knjiga.Zanr.Naziv}",
                Font = new Font("Arial", 12),
                Location = new Point(10, 60),
                AutoSize = true
            };

            var availabilityLabel = new Label
            {
                Text = $"Dostupno primjeraka: {knjiga.DostupnaKolicina}",
                Font = new Font("Arial", 12),
                Location = new Point(10, 85),
                AutoSize = true,
                ForeColor = knjiga.DostupnaKolicina > 0 ? Color.Green : Color.Red
            };

            // Add status indicator
            var statusLabel = new Label
            {
                Text = knjiga.Dostupna ? "DOSTUPNO" : "NIJE DOSTUPNO",
                Font = new Font("Arial", 10, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(panel.Width - 150, 10),
                ForeColor = knjiga.Dostupna ? Color.Green : Color.Red
            };

            panel.Controls.AddRange(new Control[]
            {
        titleLabel,
        authorLabel,
        genreLabel,
        availabilityLabel,
        statusLabel
            });

            panel.Click += (s, e) => ShowBookDetails(knjiga);
            foreach (Control control in panel.Controls)
            {
                control.Click += (s, e) => ShowBookDetails(knjiga);
            }

            // Add hover effect
            panel.MouseEnter += (s, e) => panel.BackColor = Color.FromArgb(240, 240, 240);
            panel.MouseLeave += (s, e) => panel.BackColor = Color.WhiteSmoke;

            return panel;
        }

        private void ShowBookDetails(Knjiga knjiga)
        {
            var detailsForm = new Form
            {
                Text = knjiga.Naslov,
                Size = new Size(600, 500),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            };

            var panel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20)
            };

            var controls = new Control[]
            {
        new Label
        {
            Text = knjiga.Naslov,
            Font = new Font("Arial", 20, FontStyle.Bold),
            AutoSize = true,
            Location = new Point(0, 0)
        },
        new Label
        {
            Text = $"Autor: {knjiga.Autor.Ime} {knjiga.Autor.Prezime}",
            Font = new Font("Arial", 14),
            AutoSize = true,
            Location = new Point(0, 40)
        },
        new Label
        {
            Text = $"Žanr: {knjiga.Zanr.Naziv}",
            Font = new Font("Arial", 14),
            AutoSize = true,
            Location = new Point(0, 70)
        },
        new Label
        {
            Text = $"Godina izdavanja: {knjiga.GodinaIzdavanja}",
            Font = new Font("Arial", 14),
            AutoSize = true,
            Location = new Point(0, 100)
        },
        new Label
        {
            Text = $"Dostupno primjeraka: {knjiga.DostupnaKolicina}",
            Font = new Font("Arial", 14),
            AutoSize = true,
            Location = new Point(0, 130)
        }
            };

            var reserveButton = new Button
            {
                Text = "Rezerviši knjigu",
                Size = new Size(150, 40),
                Location = new Point(0, 180),
                Enabled = knjiga.Dostupna && knjiga.DostupnaKolicina > 0 && trenutniKorisnik.Aktivni,
                BackColor = Color.FromArgb(51, 51, 76),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };

            reserveButton.Click += (s, e) => ReserveBook(knjiga, detailsForm);

            panel.Controls.AddRange(controls);
            panel.Controls.Add(reserveButton);
            detailsForm.Controls.Add(panel);
            detailsForm.ShowDialog();
        }

        private void ReserveBook(Knjiga knjiga, Form detailsForm)
        {
            if (!trenutniKorisnik.Aktivni)
            {
                MessageBox.Show("Ne možete rezervisati knjigu jer vaša članarina nije aktivna.",
                    "Neaktivna članarina", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var activeReservations = PodaciBiblioteke.Rezervacije
                .Count(r => r.Korisnik.Id == trenutniKorisnik.Id && !r.IsComplete);

            if (activeReservations >= 3)
            {
                MessageBox.Show("Dostigli ste maksimalan broj aktivnih rezervacija (3).",
                    "Limit rezervacija", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show(
                $"Da li ste sigurni da želite rezervisati knjigu '{knjiga.Naslov}'?\n\n" +
                "Rezervacija će biti aktivna 3 dana od trenutka kada knjiga postane dostupna.",
                "Potvrda rezervacije",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                var rezervacija = new Rezervacija
                {
                    Knjiga = knjiga,
                    Korisnik = trenutniKorisnik,
                    DatumRezervacije = DateTime.Now,
                    IsComplete = false
                };

                PodaciBiblioteke.Rezervacije.Add(rezervacija);
                knjiga.DostupnaKolicina--;

                MessageBox.Show(
                    "Knjiga je uspješno rezervisana.\n\n" +
                    "Bićete obaviješteni kada knjiga postane dostupna.",
                    "Uspjeh",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                detailsForm.Close();
                ShowBookSearch(); // Refresh the current view
            }
        }

        private void ShowMyBorrowings()
        {
            var borrowingsPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20),
                AutoScroll = true
            };

            // Title
            var titleLabel = new Label
            {
                Text = "Moje posudbe",
                Font = new Font("Arial", 24, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(0, 0)
            };

            // Active borrowings section with more spacing
            var activeBorrowingsLabel = new Label
            {
                Text = "Aktivne posudbe",
                Font = new Font("Arial", 18, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(0, titleLabel.Bottom + 30)  // Increased spacing
            };

            var activeBorrowingsPanel = new Panel
            {
                Location = new Point(0, activeBorrowingsLabel.Bottom + 15),
                Width = mainContentPanel.Width - 60,
                AutoSize = true,
                BackColor = Color.FromArgb(240, 240, 240),
                Padding = new Padding(15)
            };

            var activeBorrowings = PodaciBiblioteke.Posudbe
                .Where(p => p.Korisnik.Id == trenutniKorisnik.Id && p.DatumVracanja > DateTime.Now)
                .ToList();

            if (!activeBorrowings.Any())
            {
                activeBorrowingsPanel.Controls.Add(new Label
                {
                    Text = "Trenutno nemate aktivnih posudbi.",
                    Font = new Font("Arial", 12),
                    ForeColor = Color.Gray,
                    AutoSize = true,
                    Location = new Point(10, 10)
                });
                activeBorrowingsPanel.Height = 50;
            }
            else
            {
                var flowPanel = new FlowLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    FlowDirection = FlowDirection.TopDown,
                    AutoSize = true,
                    WrapContents = false
                };

                foreach (var posudba in activeBorrowings)
                {
                    flowPanel.Controls.Add(CreateBorrowingPanel(posudba));
                }
                activeBorrowingsPanel.Controls.Add(flowPanel);
            }

            var historyLabel = new Label
            {
                Text = "Historija posudbi",
                Font = new Font("Arial", 18, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(0, activeBorrowingsPanel.Bottom + 70)  // Increased spacing
            };

            var historyPanel = new Panel
            {
                Location = new Point(0, historyLabel.Bottom + 15),
                Width = mainContentPanel.Width - 60,
                Height = 600,
                BackColor = Color.FromArgb(240, 240, 240),
                Padding = new Padding(15),
                AutoScroll = true
            };

            var pastBorrowings = PodaciBiblioteke.Posudbe
                .Where(p => p.Korisnik.Id == trenutniKorisnik.Id && p.DatumVracanja <= DateTime.Now)
                .OrderByDescending(p => p.DatumPosudbe)
                .ToList();

            if (pastBorrowings.Any())
            {
                var flowPanel = new FlowLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    FlowDirection = FlowDirection.TopDown,
                    AutoSize = true,
                    WrapContents = false
                };

                foreach (var posudba in pastBorrowings)
                {
                    flowPanel.Controls.Add(CreateHistoryPanel(posudba));
                }
                historyPanel.Controls.Add(flowPanel);
            }
            else
            {
                historyPanel.Controls.Add(new Label
                {
                    Text = "Nemate prethodnih posudbi.",
                    Font = new Font("Arial", 12),
                    ForeColor = Color.Gray,
                    AutoSize = true,
                    Location = new Point(10, 10)
                });
            }

            borrowingsPanel.Controls.AddRange(new Control[]
            {
                titleLabel,
                activeBorrowingsLabel,
                activeBorrowingsPanel,
                historyLabel,  // Add the title label here
                historyPanel
            });

            mainContentPanel.Controls.Add(borrowingsPanel);
        }

        private Panel CreateBorrowingPanel(Posudba posudba)
        {
            var panel = new Panel
            {
                Width = mainContentPanel.Width - 100,
                Height = 120,
                Margin = new Padding(0, 0, 0, 10),
                BackColor = Color.WhiteSmoke
            };

            var titleLabel = new Label
            {
                Text = posudba.Knjiga.Naslov,
                Font = new Font("Arial", 14, FontStyle.Bold),
                Location = new Point(10, 10),
                AutoSize = true
            };

            var borrowDateLabel = new Label
            {
                Text = $"Datum posudbe: {posudba.DatumPosudbe:dd.MM.yyyy}",
                Font = new Font("Arial", 12),
                Location = new Point(10, 40),
                AutoSize = true
            };

            var returnDateLabel = new Label
            {
                Text = $"Rok za vraćanje: {posudba.DatumVracanja:dd.MM.yyyy}",
                Font = new Font("Arial", 12),
                Location = new Point(10, 65),
                AutoSize = true,
                ForeColor = posudba.DatumVracanja < DateTime.Now ? Color.Red : Color.Black
            };

            panel.Controls.AddRange(new Control[]
            {
                titleLabel,
                borrowDateLabel,
                returnDateLabel
            });
            return panel;
        }

        private void ShowProfile()
        {
            var profilePanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20),
                AutoScroll = true
            };

            // Main Title
            var titleLabel = new Label
            {
                Text = "Moj profil",
                Font = new Font("Arial", 24, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(0, 0)
            };

            // Personal Info Section
            var personalSection = new Panel
            {
                Location = new Point(0, titleLabel.Bottom + 30),
                Width = mainContentPanel.Width - 60,
                AutoSize = true
            };

            var personalInfoLabel = new Label
            {
                Text = "Lični podaci",
                Font = new Font("Arial", 18, FontStyle.Bold),
                AutoSize = true
            };

            var personalInfoPanel = new Panel
            {
                Location = new Point(0, personalInfoLabel.Bottom + 15),
                Width = 600,
                AutoSize = true,
                BackColor = Color.FromArgb(240, 240, 240),
                Padding = new Padding(20)
            };

            var infoTable = new TableLayoutPanel
            {
                ColumnCount = 2,
                RowCount = 9,
                AutoSize = true,
                Padding = new Padding(10),
                Width = 560
            };

            // Set column styles with fixed widths
            infoTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 140F));
            infoTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 300F));

            // Set initial row heights
            for (int i = 0; i < 9; i++)
            {
                if (i == 3 || i == 5) // Error message rows
                {
                    infoTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 0F)); // Start with 0 height
                }
                else
                {
                    infoTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));
                }
            }

            // Update error label styling
            var emailErrorLabel = new Label
            {
                ForeColor = Color.Red,
                AutoSize = true,
                Font = new Font("Arial", 9),
                Visible = false,
                Margin = new Padding(0, 0, 0, 0)
            };

            var phoneErrorLabel = new Label
            {
                ForeColor = Color.Red,
                AutoSize = true,
                Font = new Font("Arial", 9),
                Visible = false,
                Margin = new Padding(0, 0, 0, 0)
            };

            // Create editable fields with consistent sizing
            var emailBox = new TextBox
            {
                Text = trenutniKorisnik.Email,
                Font = new Font("Arial", 12),
                Width = 250,
                Height = 25,
                BorderStyle = BorderStyle.FixedSingle,
                Anchor = AnchorStyles.Left | AnchorStyles.Top,
                Margin = new Padding(0, 3, 0, 0)
            };

            var phoneBox = new TextBox
            {
                Text = trenutniKorisnik.BrojTelefona,
                Font = new Font("Arial", 12),
                Width = 250,
                Height = 25,
                BorderStyle = BorderStyle.FixedSingle,
                Anchor = AnchorStyles.Left | AnchorStyles.Top,
                Margin = new Padding(0, 3, 0, 0)
            };

            var saveButton = new Button
            {
                Text = "Sačuvaj promjene",
                Size = new Size(150, 30),
                BackColor = Color.FromArgb(51, 51, 76),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Margin = new Padding(0, 3, 0, 0),
                Enabled = false // Initially disabled
            };

            // Track original values
            var originalEmail = trenutniKorisnik.Email;
            var originalPhone = trenutniKorisnik.BrojTelefona;
            bool isEmailValid = true;
            bool isPhoneValid = true;

            // Validation function
            bool ValidateEmail(string email)
            {
                string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
                return System.Text.RegularExpressions.Regex.IsMatch(email, emailPattern);
            }

            bool ValidatePhone(string phone)
            {
                string phonePattern = @"^(\+387|0)\d{2}[-]?\d{3}[-]?\d{3,4}$";
                return System.Text.RegularExpressions.Regex.IsMatch(phone.Replace(" ", ""), phonePattern);
            }

            // Check if any changes were made
            void CheckChanges()
            {
                bool hasChanges = emailBox.Text != originalEmail || phoneBox.Text != originalPhone;
                saveButton.Enabled = hasChanges && isEmailValid && isPhoneValid;
            }

            // Email validation handler
            emailBox.TextChanged += (s, e) =>
            {
                isEmailValid = ValidateEmail(emailBox.Text);
                if (!isEmailValid)
                {
                    emailErrorLabel.Text = "Unesite ispravnu email adresu (npr. ime@domena.com)";
                    emailErrorLabel.Visible = true;
                    emailBox.BackColor = Color.MistyRose;
                    infoTable.RowStyles[3] = new RowStyle(SizeType.Absolute, 20F);
                }
                else
                {
                    emailErrorLabel.Visible = false;
                    emailBox.BackColor = Color.White;
                    infoTable.RowStyles[3] = new RowStyle(SizeType.Absolute, 0F);
                }
                infoTable.PerformLayout();
                CheckChanges();
            };

            phoneBox.TextChanged += (s, e) =>
            {
                isPhoneValid = ValidatePhone(phoneBox.Text);
                if (!isPhoneValid)
                {
                    phoneErrorLabel.Text = "Unesite validan broj telefona (npr.+38712345678 ili 061234567)";
                    phoneErrorLabel.Visible = true;
                    phoneBox.BackColor = Color.MistyRose;
                    infoTable.RowStyles[5] = new RowStyle(SizeType.Absolute, 20F);
                }
                else
                {
                    phoneErrorLabel.Visible = false;
                    phoneBox.BackColor = Color.White;
                    infoTable.RowStyles[5] = new RowStyle(SizeType.Absolute, 0F);
                }
                infoTable.PerformLayout();
                CheckChanges();
            };

            saveButton.Click += (s, e) =>
            {
                if (isEmailValid && isPhoneValid)
                {
                    trenutniKorisnik.Email = emailBox.Text;
                    trenutniKorisnik.BrojTelefona = phoneBox.Text;
                    originalEmail = emailBox.Text;
                    originalPhone = phoneBox.Text;
                    saveButton.Enabled = false;
                    MessageBox.Show("Promjene su uspješno sačuvane.", "Uspjeh",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            };

            var emailTooltip = new ToolTip();
            emailTooltip.SetToolTip(emailBox, "Unesite validnu email adresu (npr. ime@domena.com)");

            var phoneTooltip = new ToolTip();
            phoneTooltip.SetToolTip(phoneBox, "Unesite validan broj telefona (npr.+38712345678 ili 061234567)");

            // Add rows to table
            AddProfileRow(infoTable, "Ime:", trenutniKorisnik.Ime, 0);
            AddProfileRow(infoTable, "Prezime:", trenutniKorisnik.Prezime, 1);
            AddProfileRow(infoTable, "Email:", "", 2);
            // Row 3 is for email error
            AddProfileRow(infoTable, "Telefon:", "", 4);
            // Row 5 is for phone error
            AddProfileRow(infoTable, "Članarina:", trenutniKorisnik.Clanarina.Naziv, 6);
            AddProfileRow(infoTable, "Važi do:", String.Format("{0:dd.MM.yyyy}", trenutniKorisnik.DatumIsteka), 7);

            infoTable.Controls.Add(emailBox, 1, 2);
            infoTable.Controls.Add(emailErrorLabel, 1, 3);
            infoTable.Controls.Add(phoneBox, 1, 4);
            infoTable.Controls.Add(phoneErrorLabel, 1, 5);
            infoTable.Controls.Add(saveButton, 1, 8);

            personalInfoPanel.Controls.Add(infoTable);
            personalSection.Controls.AddRange(new Control[] { personalInfoLabel, personalInfoPanel });

            // Reservations Section
            var reservationsSection = new Panel
            {
                Location = new Point(0, personalSection.Bottom + 270),
                Width = mainContentPanel.Width - 60,
                AutoSize = true
            };

            var reservationsLabel = new Label
            {
                Text = "Moje rezervacije",
                Font = new Font("Arial", 18, FontStyle.Bold),
                AutoSize = true
            };

            var reservationsPanel = new Panel
            {
                Location = new Point(0, reservationsLabel.Bottom + 15),
                Width = mainContentPanel.Width - 60,
                BackColor = Color.FromArgb(240, 240, 240),
                Padding = new Padding(15),
                AutoSize = true,
            };

            var reservationsFlow = new FlowLayoutPanel
            {
                AutoSize = true,
                Width = reservationsPanel.Width - 30,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                MinimumSize = new Size(0, 70) // This ensures minimum height for the flow panel
            };

            var activeReservations = PodaciBiblioteke.Rezervacije
                .Where(r => r.Korisnik.Id == trenutniKorisnik.Id && !r.IsComplete)
                .ToList();

            if (activeReservations.Any())
            {
                foreach (var rezervacija in activeReservations)
                {
                    reservationsFlow.Controls.Add(CreateReservationPanel(rezervacija));
                }
            }
            else
            {
                reservationsFlow.Controls.Add(new Label
                {
                    Text = "Trenutno nemate aktivnih rezervacija.",
                    Font = new Font("Arial", 12),
                    ForeColor = Color.Gray,
                    AutoSize = true,
                    Margin = new Padding(10)
                });
            }

            reservationsPanel.Controls.Add(reservationsFlow);
            reservationsSection.Controls.AddRange(new Control[] { reservationsLabel, reservationsPanel });

            // Add sections to main panel
            profilePanel.Controls.AddRange(new Control[]
            {
        titleLabel,
        personalSection,
        reservationsSection
            });

            mainContentPanel.Controls.Add(profilePanel);
        }

        private Panel CreateReservationPanel(Rezervacija rezervacija)
        {
            var panel = new Panel
            {
                Width = mainContentPanel.Width - 130,
                Height = 100,
                Margin = new Padding(30, 30, 0, 10),
                BackColor = Color.White,
                Padding = new Padding(15)
            };

            var titleLabel = new Label
            {
                Text = rezervacija.Knjiga.Naslov,
                Font = new Font("Arial", 14, FontStyle.Bold),
                Location = new Point(10, 10),
                AutoSize = true
            };

            var dateLabel = new Label
            {
                Text = $"Rezervisano: {rezervacija.DatumRezervacije:dd.MM.yyyy}",
                Font = new Font("Arial", 12),
                Location = new Point(10, 40),
                AutoSize = true
            };

            var cancelButton = new Button
            {
                Text = "Otkaži rezervaciju",
                Size = new Size(130, 30),
                Location = new Point(panel.Width - 150, 35),
                BackColor = Color.IndianRed,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };

            cancelButton.Click += (s, e) => CancelReservation(rezervacija, panel);

            panel.Controls.AddRange(new Control[] { titleLabel, dateLabel, cancelButton });
            return panel;
        }

        private void CancelReservation(Rezervacija rezervacija, Panel panel)
        {
            if (MessageBox.Show(
                $"Da li ste sigurni da želite otkazati rezervaciju za knjigu '{rezervacija.Knjiga.Naslov}'?",
                "Potvrda otkazivanja",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // Store the parent control before removing the panel
                var parentControl = panel.Parent;

                // Update the reservation and book
                rezervacija.IsComplete = true;
                rezervacija.Knjiga.DostupnaKolicina++;

                // Remove the panel
                parentControl.Controls.Remove(panel);

                // Check if there are any remaining reservation panels
                if (!parentControl.Controls.OfType<Panel>().Any())
                {
                    parentControl.Controls.Add(new Label
                    {
                        Text = "Trenutno nemate aktivnih rezervacija.",
                        Font = new Font("Arial", 12),
                        ForeColor = Color.Gray,
                        AutoSize = true,
                        Margin = new Padding(10)
                    });
                }

                // Refresh the view
                ShowProfile();
            }
        }

        private void AddProfileRow(TableLayoutPanel panel, string label, string value, int row)
        {
            panel.Controls.Add(new Label
            {
                Text = label,
                Font = new Font("Arial", 12, FontStyle.Bold),
                AutoSize = true,
                Anchor = AnchorStyles.Left | AnchorStyles.Top,
                Margin = new Padding(0, 7, 0, 0)
            }, 0, row);

            if (!string.IsNullOrEmpty(value))  // Only add value label if there's a value
            {
                panel.Controls.Add(new Label
                {
                    Text = value,
                    Font = new Font("Arial", 12),
                    AutoSize = true,
                    Anchor = AnchorStyles.Left | AnchorStyles.Top,
                    Margin = new Padding(0, 7, 0, 0)
                }, 1, row);
            }
        }

        private Panel CreateBookPanel(Knjiga knjiga)
        {
            var panel = new Panel
            {
                Width = mainContentPanel.Width - 100,
                Height = 100,
                Margin = new Padding(0, 0, 0, 10),
                BackColor = Color.WhiteSmoke
            };

            var titleLabel = new Label
            {
                Text = knjiga.Naslov,
                Font = new Font("Arial", 14, FontStyle.Bold),
                Location = new Point(10, 10),
                AutoSize = true
            };

            var authorLabel = new Label
            {
                Text = $"Autor: {knjiga.Autor.Ime} {knjiga.Autor.Prezime}",
                Font = new Font("Arial", 12),
                Location = new Point(10, 35),
                AutoSize = true
            };

            var posudba = PodaciBiblioteke.Posudbe
                .FirstOrDefault(p => p.Knjiga.Id == knjiga.Id &&
                                   p.Korisnik.Id == trenutniKorisnik.Id &&
                                   p.DatumVracanja > DateTime.Now);

            if (posudba != null)
            {
                var dueDateLabel = new Label
                {
                    Text = $"Rok za vraćanje: {posudba.DatumVracanja:dd.MM.yyyy}",
                    Font = new Font("Arial", 12),
                    Location = new Point(10, 60),
                    AutoSize = true,
                    ForeColor = posudba.DatumVracanja < DateTime.Now ? Color.Red : Color.Black
                };
                panel.Controls.Add(dueDateLabel);
            }

            panel.Controls.AddRange(new Control[] { titleLabel, authorLabel });
            return panel;
        }

        private void InitializeNotifications()
        {
            var notificationTimer = new Timer
            {
                Interval = 60000 // Check every minute
            };
            notificationTimer.Tick += CheckNotifications;
            notificationTimer.Start();
        }

        private void CheckNotifications(object sender, EventArgs e)
        {
            // Check for books due soon
            var dueSoonBooks = PodaciBiblioteke.Posudbe
                .Where(p => p.Korisnik.Id == trenutniKorisnik.Id &&
                            p.DatumVracanja > DateTime.Now &&
                            p.DatumVracanja <= DateTime.Now.AddDays(3));

            foreach (var posudba in dueSoonBooks)
            {
                ShowNotification($"Knjiga '{posudba.Knjiga.Naslov}' dospijeva za vraćanje {posudba.DatumVracanja:dd.MM.yyyy}");
            }

            // Check for available reserved books
            var availableReservations = PodaciBiblioteke.Rezervacije
                .Where(r => r.Korisnik.Id == trenutniKorisnik.Id &&
                            r.Knjiga.Dostupna &&
                            !r.Notified);

            foreach (var rezervacija in availableReservations)
            {
                ShowNotification($"Knjiga '{rezervacija.Knjiga.Naslov}' je sada dostupna!");
                rezervacija.Notified = true;
            }
        }

        private void ShowNotification(string message)
        {
            var notification = new NotifyIcon
            {
                Visible = true,
                Icon = SystemIcons.Information,
                BalloonTipTitle = "Biblioteka",
                BalloonTipText = message
            };
            notification.ShowBalloonTip(5000);
        }

        private Panel CreatePopularBookPanel(Knjiga knjiga)
        {
            var panel = new Panel
            {
                Width = 250,
                Height = 200,
                Margin = new Padding(10),
                BackColor = Color.White,
                Padding = new Padding(50), // Increased padding
                Cursor = Cursors.Hand
            };

            // Add shadow effect
            panel.Paint += (s, e) =>
            {
                var shadowColor = Color.FromArgb(20, 0, 0, 0);
                using (var brush = new SolidBrush(shadowColor))
                {
                    e.Graphics.FillRectangle(brush,
                        new Rectangle(3, 3, panel.Width - 3, panel.Height - 3));
                }
            };

            var titleLabel = new Label
            {
                Text = knjiga.Naslov,
                Font = new Font("Arial", 14, FontStyle.Bold),
                Location = new Point(35, 20), // Adjusted position
                Width = 210, // Adjusted width to account for padding
                Height = 50,
                BackColor = Color.Transparent // Changed to transparent
            };

            var authorLabel = new Label
            {
                Text = $"{knjiga.Autor.Ime} {knjiga.Autor.Prezime}",
                Font = new Font("Arial", 12),
                Location = new Point(35, 75), // Adjusted position
                AutoSize = true,
                BackColor = Color.Transparent // Changed to transparent
            };

            var borrowCount = new Random().Next(50, 200);
            var popularityLabel = new Label
            {
                Text = $"Posuđeno {borrowCount} puta",
                Font = new Font("Arial", 11),
                Location = new Point(35, 105), // Adjusted position
                ForeColor = Color.FromArgb(100, 100, 100),
                AutoSize = true,
                BackColor = Color.Transparent // Changed to transparent
            };

            var availabilityLabel = new Label
            {
                Text = knjiga.Dostupna ? "DOSTUPNO" : "NIJE DOSTUPNO",
                Font = new Font("Arial", 10, FontStyle.Bold),
                Location = new Point(35, 140), // Adjusted position
                ForeColor = knjiga.Dostupna ? Color.Green : Color.Red,
                AutoSize = true,
                BackColor = Color.Transparent // Changed to transparent
            };

            panel.Controls.AddRange(new Control[]
            {
        titleLabel,
        authorLabel,
        popularityLabel,
        availabilityLabel
            });

            // Hover effect
            panel.MouseEnter += (s, e) =>
            {
                panel.BackColor = Color.FromArgb(245, 245, 245);
                // No need to change label backgrounds since they're transparent
            };

            panel.MouseLeave += (s, e) =>
            {
                panel.BackColor = Color.White;
                // No need to change label backgrounds since they're transparent
            };

            panel.Click += (s, e) => ShowBookDetails(knjiga);

            return panel;
        }

        private Panel CreateHistoryPanel(Posudba posudba)
        {
            var panel = new Panel
            {
                Width = mainContentPanel.Width - 100,
                Height = 100,
                Margin = new Padding(0, 0, 0, 10),
                BackColor = Color.WhiteSmoke,
                Padding = new Padding(10)
            };

            var titleLabel = new Label
            {
                Text = posudba.Knjiga.Naslov,
                Font = new Font("Arial", 12, FontStyle.Bold),
                Location = new Point(10, 10),
                AutoSize = true
            };

            var datesLabel = new Label
            {
                Text = $"Posuđeno: {posudba.DatumPosudbe:dd.MM.yyyy} - Vraćeno: {posudba.DatumVracanja:dd.MM.yyyy}",
                Font = new Font("Arial", 10),
                Location = new Point(10, 35),
                AutoSize = true
            };

            var statusLabel = new Label
            {
                Text = posudba.DatumVracanja < DateTime.Now ? "Vraćeno" : "U toku",
                Font = new Font("Arial", 10, FontStyle.Bold),
                Location = new Point(10, 60),
                ForeColor = posudba.DatumVracanja < DateTime.Now ? Color.Green : Color.Orange,
                AutoSize = true
            };

            panel.Controls.AddRange(new Control[]
            {
        titleLabel,
        datesLabel,
        statusLabel
            });

            return panel;
        }

        private void OdjaviSe()
        {
            if (MessageBox.Show("Da li ste sigurni da se želite odjaviti?", "Potvrda odjave",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Hide();
                var loginForm = new Login();
                loginForm.Show();
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            if (e.CloseReason == CloseReason.UserClosing)
            {
                Application.Exit();
            }
        }
    }
}