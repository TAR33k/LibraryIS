using IS_za_biblioteku.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace IS_za_biblioteku.Forms
{
    public partial class LibrarianDashboard : Form
    {
        private Panel mainContentPanel;
        private Panel topPanel;
        private readonly PrivateFontCollection privateFontCollection;
        private readonly Font TitleFont;
        private readonly Font RegularFont;
        private readonly Font BoldFont;

        // Color scheme matching the member frontend
        private readonly Color PrimaryColor = Color.FromArgb(255, 140, 0);    // Orange
        private readonly Color SecondaryColor = Color.White;
        private readonly Color AccentColor = Color.FromArgb(240, 240, 240);   // Light Gray
        private readonly Color TextColor = Color.FromArgb(51, 51, 51);        // Dark Gray
        private readonly Color HoverColor = Color.FromArgb(255, 160, 20);     // Lighter Orange

        private readonly Korisnik trenutniBibliotekar;

        public LibrarianDashboard(string korisnickoIme)
        {
            InitializeComponent();

            // Initialize fonts
            privateFontCollection = new PrivateFontCollection();
            try
            {
                string fontPath = Path.Combine(Application.StartupPath, "Resources");
                string regularFontPath = Path.Combine(fontPath, "Poppins-Regular.ttf");
                string boldFontPath = Path.Combine(fontPath, "Poppins-Bold.ttf");

                if (File.Exists(regularFontPath))
                {
                    privateFontCollection.AddFontFile(regularFontPath);
                }

                if (File.Exists(boldFontPath))
                {
                    privateFontCollection.AddFontFile(boldFontPath);
                }

                if (privateFontCollection.Families.Length > 0)
                {
                    TitleFont = new Font(privateFontCollection.Families[0], 24, FontStyle.Bold);
                    RegularFont = new Font(privateFontCollection.Families[0], 12, FontStyle.Regular);
                    BoldFont = new Font(privateFontCollection.Families[0], 10, FontStyle.Bold);
                }
                else
                {
                    throw new Exception("No fonts were loaded successfully.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading custom fonts: {ex.Message}\nFalling back to system fonts.");
                TitleFont = new Font("Segoe UI", 24, FontStyle.Bold);
                RegularFont = new Font("Segoe UI", 12, FontStyle.Regular);
                BoldFont = new Font("Segoe UI", 10, FontStyle.Bold);
            }

            this.Size = new Size(1200, 820);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Biblioteka - Bibliotekar";

            if (PodaciBiblioteke.Korisnici.Count == 0)
                PodaciBiblioteke.PopuniPodatke();

            // Get librarian data
            trenutniBibliotekar = PodaciBiblioteke.Korisnici.Last();

            InitializeLayout();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            if (privateFontCollection != null)
            {
                privateFontCollection.Dispose();
            }

            if (e.CloseReason == CloseReason.UserClosing)
            {
                Application.Exit();
            }
        }

        private void InitializeLayout()
        {
            // Initialize top panel with user info
            InitializeTopPanel();

            // Initialize navigation bar
            InitializeNavigationBar();

            // Initialize main content panel
            mainContentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(30),
                BackColor = SecondaryColor,
            };
            this.Controls.Add(mainContentPanel);

            // Show default view (dashboard)
            ShowDashboard();
        }

        private void InitializeTopPanel()
        {
            // Top Panel with fixed height and padding
            topPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 70,
                BackColor = PrimaryColor,
                Padding = new Padding(20, 0, 20, 0)
            };

            // Title container with logo
            var titleContainer = new Panel
            {
                AutoSize = true,
                Height = 70,
                BackColor = Color.Transparent,
                Dock = DockStyle.Left
            };

            // Library icon
            var iconPictureBox = new PictureBox
            {
                Size = new Size(48, 48),
                Location = new Point(0, 10),
                SizeMode = PictureBoxSizeMode.StretchImage,
                BackColor = Color.Transparent
            };

            try
            {
                iconPictureBox.Image = Image.FromFile("Resources/library-icon.png");
            }
            catch
            {
                var defaultIcon = new Bitmap(32, 32);
                using (var g = Graphics.FromImage(defaultIcon))
                {
                    g.Clear(PrimaryColor);
                    g.DrawString("B", new Font("Arial", 20, FontStyle.Bold),
                                new SolidBrush(SecondaryColor), new Point(8, 2));
                }
                iconPictureBox.Image = defaultIcon;
            }

            var titleLabel = new Label
            {
                Text = "Biblioteka",
                Font = TitleFont,
                ForeColor = SecondaryColor,
                AutoSize = true,
                Location = new Point(47, 12)
            };

            titleContainer.Controls.AddRange(new Control[] { iconPictureBox, titleLabel });

            // User info and logout container
            var userContainer = new Panel
            {
                AutoSize = true,
                Height = 70,
                BackColor = Color.Transparent,
                Dock = DockStyle.Right,
                Padding = new Padding(0, 15, 0, 15)
            };

            var userLabel = new Label
            {
                Text = $"{trenutniBibliotekar.Ime} {trenutniBibliotekar.Prezime}",
                Font = RegularFont,
                ForeColor = SecondaryColor,
                AutoSize = true,
                Location = new Point(0, 22)
            };

            var logoutButton = new Button
            {
                Text = "Odjavi se",
                Size = new Size(120, 35),
                Location = new Point(userLabel.Right + 38, 18),
                BackColor = SecondaryColor,
                ForeColor = PrimaryColor,
                FlatStyle = FlatStyle.Flat,
                Font = BoldFont,
                Cursor = Cursors.Hand
            };

            logoutButton.FlatAppearance.BorderSize = 0;
            logoutButton.Click += (s, e) => OdjaviSe();
            logoutButton.MouseEnter += (s, e) =>
            {
                logoutButton.BackColor = AccentColor;
            };
            logoutButton.MouseLeave += (s, e) =>
            {
                logoutButton.BackColor = SecondaryColor;
            };

            userContainer.Controls.AddRange(new Control[] { userLabel, logoutButton });
            topPanel.Controls.AddRange(new Control[] { titleContainer, userContainer });
            this.Controls.Add(topPanel);
        }

        private void InitializeNavigationBar()
        {
            var navPanel = new Panel
            {
                Height = 60,
                BackColor = Color.White,
                Padding = new Padding(20, 0, 20, 0),
                BorderStyle = BorderStyle.None,
                Dock = DockStyle.None, // Change from DockStyle.Top
                Width = this.ClientSize.Width
            };

            // Position the nav panel below the top panel
            navPanel.Location = new Point(0, topPanel.Bottom);

            // Add bottom border
            navPanel.Paint += (s, e) =>
            {
                using (var pen = new Pen(Color.FromArgb(229, 231, 235)))
                {
                    e.Graphics.DrawLine(pen, 0, navPanel.Height - 1, navPanel.Width, navPanel.Height - 1);
                }
            };

            // Ensure nav panel resizes with form
            this.Resize += (s, e) =>
            {
                navPanel.Width = this.ClientSize.Width;
            };

            var menuItems = new[]
            {
        ("Početna", "dashboard", "🏠"),
        ("Knjige", "books", "📚"),
        ("Članovi", "members", "👥"),
        ("Posudbe", "loans", "🔄")
    };

            int buttonX = 0;
            Button firstButton = null;

            foreach (var (text, tag, icon) in menuItems)
            {
                var button = new Button
                {
                    Text = $"{icon} {text}",
                    Tag = tag,
                    Size = new Size(296, 60),
                    Location = new Point(buttonX, 0),
                    FlatStyle = FlatStyle.Flat,
                    Font = RegularFont,
                    TextAlign = ContentAlignment.MiddleCenter,
                    BackColor = Color.Transparent,
                    ForeColor = TextColor,
                    Cursor = Cursors.Hand
                };

                button.FlatAppearance.BorderSize = 0;
                button.Click += MenuItem_Click;

                // Hover effects
                button.MouseEnter += (s, e) =>
                {
                    if (button.BackColor != PrimaryColor)
                    {
                        button.BackColor = AccentColor;
                    }
                };

                button.MouseLeave += (s, e) =>
                {
                    if (button.BackColor != PrimaryColor)
                    {
                        button.BackColor = Color.Transparent;
                    }
                };

                if (tag == "dashboard")
                {
                    firstButton = button;
                    button.BackColor = PrimaryColor;
                    button.ForeColor = SecondaryColor;
                }

                navPanel.Controls.Add(button);
                buttonX += button.Width;
            }

            this.Controls.Add(navPanel);


        }

        private void MenuItem_Click(object sender, EventArgs e)
        {
            if (sender is Button clickedButton)
            {
                // Reset all buttons
                foreach (Control control in clickedButton.Parent.Controls)
                {
                    if (control is Button button)
                    {
                        button.BackColor = Color.Transparent;
                        button.ForeColor = TextColor;
                    }
                }

                // Highlight selected button
                clickedButton.BackColor = PrimaryColor;
                clickedButton.ForeColor = SecondaryColor;

                mainContentPanel.Controls.Clear();

                // Navigate to selected section
                switch (clickedButton.Tag.ToString())
                {
                    case "dashboard":
                        ShowDashboard();
                        break;
                    case "books":
                        ShowBooks();
                        break;
                    case "members":
                        ShowMembers();
                        break;
                    case "loans":
                        ShowLoans();
                        break;
                }
            }
        }

        private void OdjaviSe()
        {
            var result = CustomMessageBox.Show(
                "Da li ste sigurni da se želite odjaviti?",
                "Potvrda odjave",
                "Da",
                "Ne",
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.Hide();
                var loginForm = new NewLogin();
                loginForm.Show();
            }
        }

        private void ShowDashboard()
        {
            var dashboardPanel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(30),
                BackColor = SecondaryColor,
            };

            // Welcome section
            var welcomeLabel = new Label
            {
                Text = $"Dobrodošli, {trenutniBibliotekar.Ime}!",
                Font = TitleFont,
                ForeColor = TextColor,
                AutoSize = true,
                Location = new Point(0, 125)
            };

            // Stats cards container
            var statsContainer = new FlowLayoutPanel
            {
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                Location = new Point(0, welcomeLabel.Bottom + 30),
                Width = mainContentPanel.Width - 100,
                WrapContents = true
            };

            // Create stat cards
            var totalBooks = PodaciBiblioteke.Knjige.Count;
            var activeMembers = PodaciBiblioteke.Korisnici.Count(k => k.Aktivni);
            var activeLoans = PodaciBiblioteke.Posudbe.Count(p => !p.Status);
            var overdueLoans = PodaciBiblioteke.Posudbe.Count(p => !p.Status && p.DatumVracanja < DateTime.Now);

            var statCards = new[]
            {
        CreateStatCard("Ukupno knjiga", totalBooks.ToString(), "📚", Color.FromArgb(34, 197, 94)),
        CreateStatCard("Aktivni članovi", activeMembers.ToString(), "👥", Color.FromArgb(59, 130, 246)),
        CreateStatCard("Aktivne posudbe", activeLoans.ToString(), "🔄", Color.FromArgb(168, 85, 247)),
        CreateStatCard("Zakašnjele posudbe", overdueLoans.ToString(), "⚠️", Color.FromArgb(239, 68, 68))
    };

            foreach (var card in statCards)
            {
                statsContainer.Controls.Add(card);
            }

            // Quick Actions section
            var quickActionsLabel = new Label
            {
                Text = "Brze akcije",
                Font = new Font(TitleFont.FontFamily, 18, FontStyle.Bold),
                ForeColor = TextColor,
                AutoSize = true,
                Location = new Point(0, statsContainer.Bottom + 100)
            };

            var actionsContainer = new FlowLayoutPanel
            {
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                Location = new Point(0, quickActionsLabel.Bottom + 20),
                Width = mainContentPanel.Width - 100,
                WrapContents = true
            };

            var actionButtons = new[]
            {
        CreateActionButton("Nova posudba", "🔄", () => ShowAddLoanDialog()),
        CreateActionButton("Dodaj knjigu", "📚", () => ShowAddBookDialog()),
        CreateActionButton("Novi član", "👤", () => ShowAddMemberDialog()),
    };

            foreach (var button in actionButtons)
            {
                actionsContainer.Controls.Add(button);
            }

            // Recent Activity section
            var recentActivityLabel = new Label
            {
                Text = "Nedavne aktivnosti",
                Font = new Font(TitleFont.FontFamily, 18, FontStyle.Bold),
                ForeColor = TextColor,
                AutoSize = true,
                Location = new Point(0, actionsContainer.Bottom)
            };

            var activityContainer = new Panel
            {
                AutoSize = true,
                Location = new Point(0, recentActivityLabel.Bottom + 20),
                Width = mainContentPanel.Width - 100
            };

            // Get recent activities (last 5 loans)
            var recentLoans = PodaciBiblioteke.Posudbe
                .OrderByDescending(p => p.DatumPosudbe)
                .Take(5)
                .ToList();

            if (recentLoans.Any())
            {
                var activityList = new FlowLayoutPanel
                {
                    FlowDirection = FlowDirection.TopDown,
                    AutoSize = true,
                    Width = activityContainer.Width,
                    WrapContents = false
                };

                foreach (var loan in recentLoans)
                {
                    activityList.Controls.Add(CreateActivityCard(loan));
                }

                activityContainer.Controls.Add(activityList);
            }
            else
            {
                activityContainer.Controls.Add(CreateEmptyStateLabel("Nema nedavnih aktivnosti."));
            }

            // Add all sections to dashboard
            dashboardPanel.Controls.AddRange(new Control[]
            {
        welcomeLabel,
        statsContainer,
        quickActionsLabel,
        actionsContainer,
        recentActivityLabel,
        activityContainer
            });

            mainContentPanel.Controls.Add(dashboardPanel);
        }

        private Panel CreateStatCard(string title, string value, string icon, Color accentColor)
        {
            var card = new Panel
            {
                Size = new Size(250, 180),
                Margin = new Padding(0, 0, 20, 20),
                BackColor = Color.White,
                Padding = new Padding(20)
            };

            // Add shadow effect
            card.Paint += (s, e) =>
            {
                var graphics = e.Graphics;
                var shadowColor = Color.FromArgb(20, 0, 0, 0);
                graphics.FillRectangle(new SolidBrush(shadowColor),
                    new Rectangle(2, 2, card.Width - 2, card.Height - 2));
            };

            var iconLabel = new Label
            {
                Text = icon,
                Font = new Font("Segoe UI Emoji", 24),
                ForeColor = accentColor,
                BackColor = Color.Transparent,
                AutoSize = true,
                Location = new Point(15, 15)
            };

            var titleLabel = new Label
            {
                Text = title,
                Font = RegularFont,
                ForeColor = Color.FromArgb(107, 114, 128),
                BackColor = Color.Transparent,
                AutoSize = true,
                Location = new Point(20, iconLabel.Bottom + 30)
            };

            var valueLabel = new Label
            {
                Text = value,
                Font = new Font(TitleFont.FontFamily, 24, FontStyle.Bold),
                ForeColor = TextColor,
                BackColor = Color.Transparent,
                AutoSize = true,
                Location = new Point(20, titleLabel.Bottom + 5)
            };

            card.Controls.AddRange(new Control[] { iconLabel, titleLabel, valueLabel });
            return card;
        }

        private Button CreateActionButton(string text, string icon, Action onClick)
        {
            var button = new Button
            {
                Text = $"{icon}  {text}",
                Size = new Size(200, 50),
                Margin = new Padding(0, 0, 20, 20),
                FlatStyle = FlatStyle.Flat,
                Font = RegularFont,
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = PrimaryColor,
                ForeColor = SecondaryColor,
                Cursor = Cursors.Hand
            };

            button.FlatAppearance.BorderSize = 0;
            button.Click += (s, e) => onClick();

            // Hover effects
            button.MouseEnter += (s, e) => button.BackColor = HoverColor;
            button.MouseLeave += (s, e) => button.BackColor = PrimaryColor;

            return button;
        }

        private Panel CreateActivityCard(Posudba posudba)
        {
            var card = new Panel
            {
                Width = mainContentPanel.Width - 200,
                Height = 80,
                Margin = new Padding(0, 0, 0, 10),
                BackColor = Color.White,
                Padding = new Padding(20)
            };

            var titleLabel = new Label
            {
                Text = $"Nova posudba: {posudba.Knjiga.Naslov}",
                Font = BoldFont,
                ForeColor = TextColor,
                AutoSize = true,
                Location = new Point(20, 20)
            };

            var detailsLabel = new Label
            {
                Text = $"Član: {posudba.Korisnik.Ime} {posudba.Korisnik.Prezime} | Datum: {posudba.DatumPosudbe:dd.MM.yyyy}",
                Font = RegularFont,
                ForeColor = Color.FromArgb(107, 114, 128),
                AutoSize = true,
                Location = new Point(20, titleLabel.Bottom + 5)
            };

            card.Controls.AddRange(new Control[] { titleLabel, detailsLabel });
            return card;
        }

        private Label CreateEmptyStateLabel(string text)
        {
            return new Label
            {
                Text = text,
                Font = RegularFont,
                ForeColor = Color.FromArgb(107, 114, 128),
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter,
                Padding = new Padding(0, 20, 0, 20)
            };
        }

        private int currentPage = 1;
        private const int itemsPerPage = 8;
        private Label pageInfoLabel;

        private void ShowBooks()
        {
            mainContentPanel.Controls.Clear();

            var booksPanel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(30),
                BackColor = SecondaryColor
            };

            // Header with title and add button
            var headerPanel = new Panel
            {
                Width = mainContentPanel.Width - 60,
                Height = 60,
                Location = new Point(0, 120)
            };

            var titleLabel = new Label
            {
                Text = "Upravljanje knjigama",
                Font = new Font(TitleFont.FontFamily, 20, FontStyle.Bold),
                ForeColor = TextColor,
                AutoSize = true,
                Location = new Point(0, 15)
            };

            var addButton = new Button
            {
                Text = "➕ Dodaj knjigu",
                Size = new Size(150, 40),
                Location = new Point(headerPanel.Width - 170, 10),
                FlatStyle = FlatStyle.Flat,
                Font = RegularFont,
                BackColor = PrimaryColor,
                ForeColor = SecondaryColor,
                Cursor = Cursors.Hand
            };
            addButton.FlatAppearance.BorderSize = 0;
            addButton.Click += (s, e) => ShowAddBookDialog();

            headerPanel.Controls.AddRange(new Control[] { titleLabel, addButton });

            // Search panel
            var searchPanel = new Panel
            {
                Width = mainContentPanel.Width - 60,
                Height = 60,
                Location = new Point(0, headerPanel.Bottom + 20)
            };

            var searchBox = new TextBox
            {
                Size = new Size(300, 35),
                Location = new Point(0, 10),
                Font = RegularFont,
                ForeColor = TextColor
            };

            var filterCombo = new ComboBox
            {
                Size = new Size(150, 35),
                Location = new Point(searchBox.Right + 20, 10),
                Font = RegularFont,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            filterCombo.Items.AddRange(new object[] { "Sve knjige", "Dostupne", "Posuđene" });
            filterCombo.SelectedIndex = 0;

            searchPanel.Controls.AddRange(new Control[] { searchBox, filterCombo });

            // Books grid
            var booksGrid = new DataGridView
            {
                Location = new Point(0, searchPanel.Bottom + 20),
                Width = mainContentPanel.Width - 70,
                Height = mainContentPanel.Height - searchPanel.Bottom - 146,
                BackgroundColor = SecondaryColor,
                BorderStyle = BorderStyle.Fixed3D,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                RowHeadersVisible = false,
                AllowUserToResizeRows = false,
                MultiSelect = false,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                RowTemplate = { Height = 40 }
            };

            // Style the grid
            booksGrid.EnableHeadersVisualStyles = false;
            booksGrid.ColumnHeadersDefaultCellStyle.BackColor = AccentColor;
            booksGrid.ColumnHeadersDefaultCellStyle.ForeColor = TextColor;
            booksGrid.ColumnHeadersDefaultCellStyle.Font = BoldFont;
            booksGrid.ColumnHeadersDefaultCellStyle.SelectionBackColor = AccentColor; // Prevent header selection color
            booksGrid.ColumnHeadersHeight = 50;
            booksGrid.DefaultCellStyle.Font = RegularFont;
            booksGrid.DefaultCellStyle.SelectionBackColor = PrimaryColor;
            booksGrid.DefaultCellStyle.SelectionForeColor = SecondaryColor;

            // Add this after creating columns to prevent default selection
            booksGrid.CellClick += (s, e) => booksGrid.ClearSelection();

            // Configure grid columns
            booksGrid.Columns.AddRange(new DataGridViewColumn[]
            {
                new DataGridViewTextBoxColumn { Name = "Id", HeaderText = "ID", Width = 50 },
                new DataGridViewTextBoxColumn { Name = "Naslov", HeaderText = "Naslov", Width = 200 },
                new DataGridViewTextBoxColumn { Name = "Autor", HeaderText = "Autor", Width = 150 },
                new DataGridViewTextBoxColumn { Name = "Zanr", HeaderText = "Žanr", Width = 100 },
                new DataGridViewTextBoxColumn { Name = "Status", HeaderText = "Status", Width = 100 },
                new DataGridViewButtonColumn { Name = "Akcije", HeaderText = "Akcije", Width = 100 }
            });

            // Add all controls to the books panel
            booksPanel.Controls.AddRange(new Control[] { headerPanel, searchPanel, booksGrid });

            // Add the books panel to the main content panel
            mainContentPanel.Controls.Add(booksPanel);

            // Load initial data
            LoadBooksData(booksGrid);

            // Add event handlers
            searchBox.TextChanged += (s, e) => FilterBooks(booksGrid, searchBox.Text, filterCombo.SelectedItem.ToString());
            filterCombo.SelectedIndexChanged += (s, e) => FilterBooks(booksGrid, searchBox.Text, filterCombo.SelectedItem.ToString());
            booksGrid.CellClick += (s, e) => HandleBookGridAction(booksGrid, e);

            // Add placeholder text behavior
            searchBox.Text = "Pretraži knjige...";
            searchBox.ForeColor = Color.Gray;

            searchBox.GotFocus += (s, e) =>
            {
                if (searchBox.Text == "Pretraži knjige...")
                {
                    searchBox.Text = "";
                    searchBox.ForeColor = TextColor;
                }
            };

            searchBox.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(searchBox.Text))
                {
                    searchBox.Text = "Pretraži knjige...";
                    searchBox.ForeColor = Color.Gray;
                }
            };

            var paginationPanel = new Panel
            {
                Width = mainContentPanel.Width - 60,
                Height = 50,
                Location = new Point(0, booksGrid.Bottom + 10)
            };

            var prevButton = new Button
            {
                Text = "◀ Prethodna",
                Size = new Size(180, 35),
                Location = new Point(0, 7),
                FlatStyle = FlatStyle.Flat,
                Font = RegularFont,
                BackColor = PrimaryColor,
                ForeColor = SecondaryColor,
                Cursor = Cursors.Hand
            };
            prevButton.FlatAppearance.BorderSize = 0;

            var nextButton = new Button
            {
                Text = "Sljedeća ▶",
                Size = new Size(180, 35),
                Location = new Point(paginationPanel.Width - 190, 7),
                FlatStyle = FlatStyle.Flat,
                Font = RegularFont,
                BackColor = PrimaryColor,
                ForeColor = SecondaryColor,
                Cursor = Cursors.Hand
            };
            nextButton.FlatAppearance.BorderSize = 0;

            pageInfoLabel = new Label
            {
                AutoSize = true,
                Font = RegularFont,
                ForeColor = TextColor,
                Location = new Point((paginationPanel.Width - 100) / 2, 15)
            };

            paginationPanel.Controls.AddRange(new Control[] { prevButton, pageInfoLabel, nextButton });

            // Add pagination panel to books panel
            booksPanel.Controls.Add(paginationPanel);

            // Style alternating rows
            booksGrid.RowsDefaultCellStyle.BackColor = Color.White;
            booksGrid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 240, 235);

            // Add pagination event handlers
            prevButton.Click += (s, e) =>
            {
                if (currentPage > 1)
                {
                    currentPage--;
                    FilterBooks(booksGrid, searchBox.Text, filterCombo.SelectedItem.ToString());
                }
            };

            nextButton.Click += (s, e) =>
            {
                var totalItems = GetFilteredBooks(searchBox.Text, filterCombo.SelectedItem.ToString()).Count;
                var totalPages = (int)Math.Ceiling(totalItems / (double)itemsPerPage);
                if (currentPage < totalPages)
                {
                    currentPage++;
                    FilterBooks(booksGrid, searchBox.Text, filterCombo.SelectedItem.ToString());
                }
            };

            // Initial load
            LoadBooksData(booksGrid);
        }

        private void UpdatePaginationControls(int totalItems)
        {
            if (pageInfoLabel != null)
            {
                int totalPages = (int)Math.Ceiling(totalItems / (double)itemsPerPage);
                pageInfoLabel.Text = $"Stranica {currentPage} od {totalPages}";

                // Find and update button states
                var prevButton = pageInfoLabel.Parent.Controls.OfType<Button>()
                    .FirstOrDefault(b => b.Text.Contains("Prethodna"));
                var nextButton = pageInfoLabel.Parent.Controls.OfType<Button>()
                    .FirstOrDefault(b => b.Text.Contains("Sljedeća"));

                if (prevButton != null)
                    prevButton.Enabled = currentPage > 1;
                if (nextButton != null)
                    nextButton.Enabled = currentPage < totalPages;
            }
        }

        private List<Knjiga> GetFilteredBooks(string searchText, string filter)
        {
            return PodaciBiblioteke.Knjige
                .Where(k =>
                    (searchText == "" ||
                     searchText == "Pretraži knjige..." ||
                     k.Naslov.ToLower().Contains(searchText.ToLower()) ||
                     k.Autor.Ime.ToLower().Contains(searchText.ToLower()) ||
                     k.Autor.Prezime.ToLower().Contains(searchText.ToLower())) &&
                    (filter == "Sve knjige" ||
                     (filter == "Dostupne" && k.Dostupna) ||
                     (filter == "Posuđene" && !k.Dostupna)))
                .ToList();
        }

        private void LoadBooksData(DataGridView grid)
        {
            grid.Rows.Clear();
            var allBooks = PodaciBiblioteke.Knjige
                .Skip((currentPage - 1) * itemsPerPage)
                .Take(itemsPerPage);

            foreach (var knjiga in allBooks)
            {
                var row = grid.Rows[grid.Rows.Add()];
                row.Cells["Id"].Value = knjiga.Id;
                row.Cells["Naslov"].Value = knjiga.Naslov;
                row.Cells["Autor"].Value = $"{knjiga.Autor.Ime} {knjiga.Autor.Prezime}";
                row.Cells["Zanr"].Value = knjiga.Zanr.Naziv;

                // Set status cell
                var statusCell = row.Cells["Status"];
                if (knjiga.Dostupna)
                {
                    statusCell.Value = "✓ Dostupna";
                    statusCell.Style.ForeColor = Color.FromArgb(34, 197, 94); // Green
                }
                else
                {
                    statusCell.Value = "✗ Posuđena";
                    statusCell.Style.ForeColor = Color.FromArgb(239, 68, 68); // Red
                }

                row.Cells["Akcije"].Value = "Uredi";
            }

            UpdatePaginationControls(PodaciBiblioteke.Knjige.Count);
        }

        private void FilterBooks(DataGridView grid, string searchText, string filter)
        {
            grid.Rows.Clear();
            var filteredBooks = GetFilteredBooks(searchText, filter)
                .Skip((currentPage - 1) * itemsPerPage)
                .Take(itemsPerPage);

            foreach (var knjiga in filteredBooks)
            {
                var row = grid.Rows[grid.Rows.Add()];
                row.Cells["Id"].Value = knjiga.Id;
                row.Cells["Naslov"].Value = knjiga.Naslov;
                row.Cells["Autor"].Value = $"{knjiga.Autor.Ime} {knjiga.Autor.Prezime}";
                row.Cells["Zanr"].Value = knjiga.Zanr.Naziv;

                // Set status cell
                var statusCell = row.Cells["Status"];
                if (knjiga.Dostupna)
                {
                    statusCell.Value = "✓ Dostupna";
                    statusCell.Style.ForeColor = Color.FromArgb(34, 197, 94); // Green
                }
                else
                {
                    statusCell.Value = "✗ Posuđena";
                    statusCell.Style.ForeColor = Color.FromArgb(239, 68, 68); // Red
                }

                row.Cells["Akcije"].Value = "Uredi";
            }

            UpdatePaginationControls(GetFilteredBooks(searchText, filter).Count);
        }

        private void HandleBookGridAction(DataGridView grid, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == grid.Columns["Akcije"].Index && e.RowIndex >= 0)
            {
                var bookId = (int)grid.Rows[e.RowIndex].Cells["Id"].Value;
                var book = PodaciBiblioteke.Knjige.First(k => k.Id == bookId);
                ShowEditBookDialog(book);
            }
        }

        private void ShowAddBookDialog()
        {
            var addBookForm = new Form
            {
                Text = "Dodaj novu knjigu",
                Size = new Size(740, 800),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                BackColor = SecondaryColor
            };

            // Header Panel
            var headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                BackColor = PrimaryColor,
                Padding = new Padding(30)
            };

            var headerLabel = new Label
            {
                Text = "📚 Nova knjiga",
                Font = new Font(TitleFont.FontFamily, 22, FontStyle.Bold),
                ForeColor = SecondaryColor,
                AutoSize = true,
                Location = new Point(30, 15)
            };

            headerPanel.Controls.Add(headerLabel);
            addBookForm.Controls.Add(headerPanel);

            // Main Content Panel
            var contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(50),
                BackColor = SecondaryColor
            };

            int yPosition = 100;
            const int spacing = 30;
            const int inputWidth = 600;
            const int halfInputWidth = 290;

            // Title Section
            var titleLabel = CreateFieldLabel("Naslov knjige *", yPosition);
            var titleTextBox = CreateStyledTextBox(yPosition + 35, inputWidth);

            yPosition += spacing * 3;

            // Author Section
            var authorLabel = CreateFieldLabel("Autor *", yPosition);
            var authorFirstNameBox = CreateStyledTextBox(yPosition + 30, halfInputWidth, "Ime autora");
            var authorLastNameBox = CreateStyledTextBox(yPosition + 30, halfInputWidth, "Prezime autora");
            authorLastNameBox.Location = new Point(authorFirstNameBox.Right + 20, authorFirstNameBox.Top);

            yPosition += spacing * 3;

            // Genre Section
            var genreLabel = CreateFieldLabel("Žanr *", yPosition);
            var genreCombo = new ComboBox
            {
                Location = new Point(50, yPosition + 30),
                Width = inputWidth,
                Height = 40,
                Font = RegularFont,
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            genreCombo.Items.AddRange(PodaciBiblioteke.Zanrovi.Select(z => z.Naziv).ToArray());
            if (genreCombo.Items.Count > 0)
                genreCombo.SelectedIndex = 0;

            yPosition += spacing * 3;

            // Year Section
            var yearLabel = CreateFieldLabel("Godina izdavanja *", yPosition);
            var yearNumeric = new NumericUpDown
            {
                Location = new Point(50, yPosition + 30),
                Width = inputWidth,
                Font = RegularFont,
                Minimum = 1900,
                Maximum = DateTime.Now.Year,
                Value = DateTime.Now.Year,
                BackColor = Color.White
            };

            yPosition += spacing * 3;

            // Quantity Section
            var quantityLabel = CreateFieldLabel("Količina *", yPosition);
            var quantityNumeric = new NumericUpDown
            {
                Location = new Point(50, yPosition + 30),
                Width = inputWidth,
                Font = RegularFont,
                Minimum = 1,
                Maximum = 1000,
                Value = 1,
                BackColor = Color.White
            };

            yPosition += spacing * 4;

            // Required fields note
            var noteLabel = new Label
            {
                Text = "* Obavezna polja",
                Font = new Font(RegularFont.FontFamily, 10),
                ForeColor = Color.FromArgb(107, 114, 128),
                AutoSize = true,
                Location = new Point(50, yPosition)
            };

            yPosition += spacing * 2;

            // Buttons
            var buttonPanel = new Panel
            {
                Width = inputWidth,
                Height = 50,
                Location = new Point(50, yPosition)
            };

            var saveButton = new Button
            {
                Text = "Sačuvaj",
                Size = new Size(180, 45),
                Location = new Point(buttonPanel.Width - 500, 0),
                BackColor = PrimaryColor,
                ForeColor = SecondaryColor,
                FlatStyle = FlatStyle.Flat,
                Font = new Font(RegularFont.FontFamily, 12),
                Cursor = Cursors.Hand
            };
            saveButton.FlatAppearance.BorderSize = 0;

            var cancelButton = new Button
            {
                Text = "Odustani",
                Size = new Size(180, 45),
                Location = new Point(buttonPanel.Width - 280, 0),
                BackColor = Color.FromArgb(229, 231, 235),
                ForeColor = TextColor,
                FlatStyle = FlatStyle.Flat,
                Font = new Font(RegularFont.FontFamily, 12),
                Cursor = Cursors.Hand
            };
            cancelButton.FlatAppearance.BorderSize = 0;

            var errorProvider = new ErrorProvider();
            errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;

            // Add validation to input controls
            titleTextBox.Tag = "required";
            titleTextBox.Name = "Naslov";
            authorFirstNameBox.Tag = "required";
            authorFirstNameBox.Name = "Ime autora";
            authorLastNameBox.Tag = "required";
            authorLastNameBox.Name = "Prezime autora";

            // Add validation events
            titleTextBox.TextChanged += (s, e) => ValidateTextBox(titleTextBox, "Naslov", errorProvider);
            titleTextBox.Leave += (s, e) => ValidateTextBox(titleTextBox, "Naslov", errorProvider);
            authorFirstNameBox.TextChanged += (s, e) => ValidateTextBox(authorFirstNameBox, "Ime autora", errorProvider);
            authorLastNameBox.TextChanged += (s, e) => ValidateTextBox(authorLastNameBox, "Prezime autora", errorProvider);


            // Add hover effects
            saveButton.MouseEnter += (s, e) => saveButton.BackColor = HoverColor;
            saveButton.MouseLeave += (s, e) => saveButton.BackColor = PrimaryColor;
            cancelButton.MouseEnter += (s, e) => cancelButton.BackColor = Color.FromArgb(220, 220, 220);
            cancelButton.MouseLeave += (s, e) => cancelButton.BackColor = Color.FromArgb(229, 231, 235);

            buttonPanel.Controls.AddRange(new Control[] { saveButton, cancelButton });

            AddValidationEffects(titleTextBox, authorFirstNameBox, authorLastNameBox);

            // Save functionality
            saveButton.Click += (s, e) =>
            {
                if (!ValidateForm(contentPanel, errorProvider))
                {
                    MessageBox.Show(
                        "Molimo popunite sva obavezna polja.",
                        "Validacija",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                    return;
                }

                try
                {
                    SaveBook(
                        titleTextBox.Text,
                        authorFirstNameBox.Text,
                        authorLastNameBox.Text,
                        genreCombo.SelectedItem.ToString(),
                        (int)yearNumeric.Value,
                        (int)quantityNumeric.Value,
                        addBookForm
                    );
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        $"Došlo je do greške:\n{ex.Message}",
                        "Greška",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                }
            };

            cancelButton.Click += (s, e) => addBookForm.Close();

            // Add controls
            buttonPanel.Controls.AddRange(new Control[] { saveButton, cancelButton });
            // Add all controls to content panel
            contentPanel.Controls.AddRange(new Control[] {
        titleLabel, titleTextBox,
        authorLabel, authorFirstNameBox, authorLastNameBox,
        genreLabel, genreCombo,
        yearLabel, yearNumeric,
        quantityLabel, quantityNumeric,
        noteLabel,
        buttonPanel
    });

            addBookForm.Controls.Add(contentPanel);

            if (addBookForm.ShowDialog() == DialogResult.OK)
            {
                var booksPanel = mainContentPanel.Controls.OfType<Panel>().FirstOrDefault();
                if (booksPanel != null)
                {
                    var grid = booksPanel.Controls.OfType<DataGridView>().FirstOrDefault();
                    if (grid != null)
                    {
                        LoadBooksData(grid);
                    }
                }
            }
        }

        private void ValidateTextBox(TextBox textBox, string fieldName, ErrorProvider errorProvider)
        {
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                errorProvider.SetError(textBox, $"{fieldName} je obavezno polje.");
                textBox.BackColor = Color.FromArgb(254, 226, 226); // Light red background
                return;
            }

            errorProvider.SetError(textBox, "");
            textBox.BackColor = Color.White;
        }

        private bool ValidateForm(Control container, ErrorProvider errorProvider)
        {
            bool isValid = true;
            foreach (Control control in container.Controls)
            {
                if (control is TextBox textBox && textBox.Tag?.ToString() == "required")
                {
                    ValidateTextBox(textBox, textBox.Name, errorProvider);
                    if (!string.IsNullOrWhiteSpace(errorProvider.GetError(textBox)))
                        isValid = false;
                }
                else if (control.HasChildren)
                {
                    if (!ValidateForm(control, errorProvider))
                        isValid = false;
                }
            }
            return isValid;
        }

        private Label CreateFieldLabel(string text, int yPosition)
        {
            return new Label
            {
                Text = text,
                Font = new Font(RegularFont.FontFamily, 12),
                ForeColor = TextColor,
                AutoSize = true,
                Location = new Point(50, yPosition)
            };
        }

        private TextBox CreateStyledTextBox(int yPosition, int width, string placeholder = "")
        {
            var textBox = new TextBox
            {
                Location = new Point(50, yPosition),
                Width = width,
                Height = 40,
                Font = RegularFont,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                ForeColor = TextColor
            };

            if (!string.IsNullOrEmpty(placeholder))
            {
                textBox.Text = placeholder;
                textBox.ForeColor = Color.Gray;

                textBox.GotFocus += (s, e) =>
                {
                    if (textBox.Text == placeholder)
                    {
                        textBox.Text = "";
                        textBox.ForeColor = TextColor;
                    }
                };

                textBox.LostFocus += (s, e) =>
                {
                    if (string.IsNullOrWhiteSpace(textBox.Text))
                    {
                        textBox.Text = placeholder;
                        textBox.ForeColor = Color.Gray;
                    }
                };
            }

            return textBox;
        }

        private void AddValidationEffects(params TextBox[] textBoxes)
        {
            foreach (var textBox in textBoxes)
            {
                textBox.TextChanged += (s, e) =>
                {
                    textBox.BackColor = string.IsNullOrWhiteSpace(textBox.Text) ||
                                       textBox.Text == "Ime autora" ||
                                       textBox.Text == "Prezime autora"
                        ? Color.FromArgb(254, 226, 226)
                        : Color.White;
                };
            }
        }

        private bool ValidateInputs(TextBox titleBox, TextBox firstNameBox, TextBox lastNameBox)
        {
            var isValid = true;
            var errorMessage = new StringBuilder("Molimo popunite sva obavezna polja:\n");

            if (string.IsNullOrWhiteSpace(titleBox.Text))
            {
                errorMessage.AppendLine("- Naslov knjige");
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(firstNameBox.Text) || firstNameBox.Text == "Ime autora")
            {
                errorMessage.AppendLine("- Ime autora");
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(lastNameBox.Text) || lastNameBox.Text == "Prezime autora")
            {
                errorMessage.AppendLine("- Prezime autora");
                isValid = false;
            }

            if (!isValid)
            {
                MessageBox.Show(
                    errorMessage.ToString(),
                    "Validacija",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
            }

            return isValid;
        }

        private void SaveBook(string title, string authorFirstName, string authorLastName,
            string genreName, int year, int quantity, Form dialogForm)
        {
            try
            {
                var author = PodaciBiblioteke.Autori.FirstOrDefault(a =>
                    a.Ime.Equals(authorFirstName, StringComparison.OrdinalIgnoreCase) &&
                    a.Prezime.Equals(authorLastName, StringComparison.OrdinalIgnoreCase));

                if (author == null)
                {
                    author = new Autor
                    {
                        Id = PodaciBiblioteke.Autori.Count + 1,
                        Ime = authorFirstName,
                        Prezime = authorLastName
                    };
                    PodaciBiblioteke.Autori.Add(author);
                }

                var genre = PodaciBiblioteke.Zanrovi.First(z => z.Naziv == genreName);

                var newBook = new Knjiga
                {
                    Id = PodaciBiblioteke.Knjige.Count + 1,
                    Naslov = title,
                    Autor = author,
                    Zanr = genre,
                    DostupnaKolicina = quantity,
                    Dostupna = true,
                    GodinaIzdavanja = year
                };

                PodaciBiblioteke.Knjige.Add(newBook);
                dialogForm.DialogResult = DialogResult.OK;
                dialogForm.Close();

                MessageBox.Show(
                    "Knjiga je uspješno dodana!",
                    "Uspjeh",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Došlo je do greške prilikom dodavanja knjige:\n{ex.Message}",
                    "Greška",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void ShowEditBookDialog(Knjiga knjiga)
        {
            var editBookForm = new Form
            {
                Text = "Uredi knjigu",
                Size = new Size(740, 860),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                BackColor = SecondaryColor
            };

            // Header Panel
            var headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                BackColor = PrimaryColor,
                Padding = new Padding(30)
            };

            var headerLabel = new Label
            {
                Text = "📚 Uredi knjigu",
                Font = new Font(TitleFont.FontFamily, 22, FontStyle.Bold),
                ForeColor = SecondaryColor,
                AutoSize = true,
                Location = new Point(30, 15)
            };

            headerPanel.Controls.Add(headerLabel);
            editBookForm.Controls.Add(headerPanel);

            // Main Content Panel
            var contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(50),
                BackColor = SecondaryColor
            };

            int yPosition = 100;
            const int spacing = 30;
            const int inputWidth = 600;
            const int halfInputWidth = 290;

            // Title Section
            var titleLabel = CreateFieldLabel("Naslov knjige *", yPosition);
            var titleTextBox = CreateStyledTextBox(yPosition + 35, inputWidth);
            titleTextBox.Text = knjiga.Naslov;

            yPosition += spacing * 3;

            // Author Section
            var authorLabel = CreateFieldLabel("Autor *", yPosition);
            var authorFirstNameBox = CreateStyledTextBox(yPosition + 30, halfInputWidth);
            var authorLastNameBox = CreateStyledTextBox(yPosition + 30, halfInputWidth);
            authorLastNameBox.Location = new Point(authorFirstNameBox.Right + 20, authorFirstNameBox.Top);

            authorFirstNameBox.Text = knjiga.Autor.Ime;
            authorLastNameBox.Text = knjiga.Autor.Prezime;

            yPosition += spacing * 3;

            // Genre Section
            var genreLabel = CreateFieldLabel("Žanr *", yPosition);
            var genreCombo = new ComboBox
            {
                Location = new Point(50, yPosition + 30),
                Width = inputWidth,
                Height = 40,
                Font = RegularFont,
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            genreCombo.Items.AddRange(PodaciBiblioteke.Zanrovi.Select(z => z.Naziv).ToArray());
            genreCombo.SelectedItem = knjiga.Zanr.Naziv;

            yPosition += spacing * 3;

            // Year Section
            var yearLabel = CreateFieldLabel("Godina izdavanja *", yPosition);
            var yearNumeric = new NumericUpDown
            {
                Location = new Point(50, yPosition + 30),
                Width = inputWidth,
                Font = RegularFont,
                Minimum = 1900,
                Maximum = DateTime.Now.Year,
                Value = knjiga.GodinaIzdavanja,
                BackColor = Color.White
            };

            yPosition += spacing * 3;

            // Quantity Section
            var quantityLabel = CreateFieldLabel("Količina *", yPosition);
            var quantityNumeric = new NumericUpDown
            {
                Location = new Point(50, yPosition + 30),
                Width = inputWidth,
                Font = RegularFont,
                Minimum = 1,
                Maximum = 1000,
                Value = knjiga.DostupnaKolicina,
                BackColor = Color.White
            };

            yPosition += spacing * 4;

            // Required fields note
            var noteLabel = new Label
            {
                Text = "* Obavezna polja",
                Font = new Font(RegularFont.FontFamily, 10),
                ForeColor = Color.FromArgb(107, 114, 128),
                AutoSize = true,
                Location = new Point(50, yPosition)
            };

            yPosition += spacing * 2;

            // Buttons Panel
            var buttonPanel = new Panel
            {
                Width = inputWidth,
                Height = 50,
                Location = new Point(50, yPosition)
            };

            // Primary action buttons (right-aligned)
            var saveButton = new Button
            {
                Text = "Sačuvaj",
                Size = new Size(180, 45),
                Location = new Point(buttonPanel.Width - 500, 0),
                BackColor = PrimaryColor,
                ForeColor = SecondaryColor,
                FlatStyle = FlatStyle.Flat,
                Font = new Font(RegularFont.FontFamily, 12),
                Cursor = Cursors.Hand
            };
            saveButton.FlatAppearance.BorderSize = 0;

            var cancelButton = new Button
            {
                Text = "Odustani",
                Size = new Size(180, 45),
                Location = new Point(buttonPanel.Width - 280, 0),
                BackColor = Color.FromArgb(229, 231, 235),
                ForeColor = TextColor,
                FlatStyle = FlatStyle.Flat,
                Font = new Font(RegularFont.FontFamily, 12),
                Cursor = Cursors.Hand
            };
            cancelButton.FlatAppearance.BorderSize = 0;

            // Delete button (left-aligned, with warning color)
            var deleteButton = new Button
            {
                Text = "Obriši knjigu",
                Size = new Size(180, 45),
                Location = new Point(50, buttonPanel.Bottom + 50),  // Below the main button panel
                BackColor = Color.FromArgb(239, 68, 68),  // Red color
                ForeColor = SecondaryColor,
                FlatStyle = FlatStyle.Flat,
                Font = new Font(RegularFont.FontFamily, 12),
                Cursor = Cursors.Hand
            };
            deleteButton.FlatAppearance.BorderSize = 0;

            // Add hover effects
            saveButton.MouseEnter += (s, e) => saveButton.BackColor = HoverColor;
            saveButton.MouseLeave += (s, e) => saveButton.BackColor = PrimaryColor;

            cancelButton.MouseEnter += (s, e) => cancelButton.BackColor = Color.FromArgb(220, 220, 220);
            cancelButton.MouseLeave += (s, e) => cancelButton.BackColor = Color.FromArgb(229, 231, 235);

            deleteButton.MouseEnter += (s, e) => deleteButton.BackColor = Color.FromArgb(220, 38, 38);  // Darker red
            deleteButton.MouseLeave += (s, e) => deleteButton.BackColor = Color.FromArgb(239, 68, 68);


            cancelButton.Click += (s, e) => editBookForm.Close();

            // Save functionality
            saveButton.Click += (s, e) =>
            {
                if (ValidateInputs(titleTextBox, authorFirstNameBox, authorLastNameBox))
                {
                    UpdateBook(
                        knjiga,
                        titleTextBox.Text,
                        authorFirstNameBox.Text,
                        authorLastNameBox.Text,
                        genreCombo.SelectedItem.ToString(),
                        (int)yearNumeric.Value,
                        (int)quantityNumeric.Value,
                        editBookForm
                    );
                }
            };

            // Delete functionality
            deleteButton.Click += (s, e) =>
            {
                var result = CustomMessageBox.Show(
                "Da li ste sigurni da želite obrisati ovu knjigu?\nOva akcija se ne može poništiti.",
                "Potvrda brisanja",
                "Da",
                "Ne",
                MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    DeleteBook(knjiga, editBookForm);
                }
            };

            buttonPanel.Controls.AddRange(new Control[] { saveButton, cancelButton });
            contentPanel.Controls.Add(deleteButton);

            // Add all controls to content panel
            contentPanel.Controls.AddRange(new Control[] {
        titleLabel, titleTextBox,
        authorLabel, authorFirstNameBox, authorLastNameBox,
        genreLabel, genreCombo,
        yearLabel, yearNumeric,
        quantityLabel, quantityNumeric,
        noteLabel,
        buttonPanel
    });

            editBookForm.Controls.Add(contentPanel);

            if (editBookForm.ShowDialog() == DialogResult.OK)
            {
                var booksPanel = mainContentPanel.Controls.OfType<Panel>().FirstOrDefault();
                if (booksPanel != null)
                {
                    var grid = booksPanel.Controls.OfType<DataGridView>().FirstOrDefault();
                    if (grid != null)
                    {
                        LoadBooksData(grid);
                    }
                }
            }
        }

        private void UpdateBook(Knjiga knjiga, string title, string authorFirstName, string authorLastName,
            string genreName, int year, int quantity, Form dialogForm)
        {
            try
            {
                var author = PodaciBiblioteke.Autori.FirstOrDefault(a =>
                    a.Ime.Equals(authorFirstName, StringComparison.OrdinalIgnoreCase) &&
                    a.Prezime.Equals(authorLastName, StringComparison.OrdinalIgnoreCase));

                if (author == null)
                {
                    author = new Autor
                    {
                        Id = PodaciBiblioteke.Autori.Count + 1,
                        Ime = authorFirstName,
                        Prezime = authorLastName
                    };
                    PodaciBiblioteke.Autori.Add(author);
                }

                var genre = PodaciBiblioteke.Zanrovi.First(z => z.Naziv == genreName);

                knjiga.Naslov = title;
                knjiga.Autor = author;
                knjiga.Zanr = genre;
                knjiga.DostupnaKolicina = quantity;
                knjiga.GodinaIzdavanja = year;

                dialogForm.DialogResult = DialogResult.OK;
                dialogForm.Close();

                MessageBox.Show(
                    "Knjiga je uspješno ažurirana!",
                    "Uspjeh",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Došlo je do greške prilikom ažuriranja knjige:\n{ex.Message}",
                    "Greška",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void DeleteBook(Knjiga knjiga, Form dialogForm)
        {
            try
            {
                PodaciBiblioteke.Knjige.Remove(knjiga);
                dialogForm.DialogResult = DialogResult.OK;
                dialogForm.Close();

                MessageBox.Show(
                    "Knjiga je uspješno obrisana!",
                    "Uspjeh",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Došlo je do greške prilikom brisanja knjige:\n{ex.Message}",
                    "Greška",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void ShowMembers()
        {
            mainContentPanel.Controls.Clear();

            var membersPanel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(30),
                BackColor = SecondaryColor
            };

            // Header with title and add button
            var headerPanel = new Panel
            {
                Width = mainContentPanel.Width - 60,
                Height = 60,
                Location = new Point(0, 120)
            };

            var titleLabel = new Label
            {
                Text = "Upravljanje članovima",
                Font = new Font(TitleFont.FontFamily, 20, FontStyle.Bold),
                ForeColor = TextColor,
                AutoSize = true,
                Location = new Point(0, 15)
            };

            var addButton = new Button
            {
                Text = "➕ Dodaj člana",
                Size = new Size(150, 40),
                Location = new Point(headerPanel.Width - 170, 10),
                FlatStyle = FlatStyle.Flat,
                Font = RegularFont,
                BackColor = PrimaryColor,
                ForeColor = SecondaryColor,
                Cursor = Cursors.Hand
            };
            addButton.FlatAppearance.BorderSize = 0;
            addButton.Click += (s, e) => ShowAddMemberDialog();

            headerPanel.Controls.AddRange(new Control[] { titleLabel, addButton });

            // Search panel
            var searchPanel = new Panel
            {
                Width = mainContentPanel.Width - 60,
                Height = 60,
                Location = new Point(0, headerPanel.Bottom + 20)
            };

            var searchBox = new TextBox
            {
                Size = new Size(300, 35),
                Location = new Point(0, 10),
                Font = RegularFont,
                ForeColor = Color.Gray,
                Text = "Pretraži članove..."
            };

            var filterCombo = new ComboBox
            {
                Size = new Size(150, 35),
                Location = new Point(searchBox.Right + 20, 10),
                Font = RegularFont,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            filterCombo.Items.AddRange(new object[] { "Svi članovi", "Aktivni", "Neaktivni" });
            filterCombo.SelectedIndex = 0;

            searchPanel.Controls.AddRange(new Control[] { searchBox, filterCombo });

            // Members grid
            var membersGrid = new DataGridView
            {
                Location = new Point(0, searchPanel.Bottom + 20),
                Width = mainContentPanel.Width - 70,
                Height = mainContentPanel.Height - searchPanel.Bottom - 146,
                BackgroundColor = SecondaryColor,
                BorderStyle = BorderStyle.Fixed3D,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None, // Change this line
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                RowHeadersVisible = false,
                AllowUserToResizeRows = false,
                MultiSelect = false,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                RowTemplate = { Height = 40 }
            };

            // Style the grid
            membersGrid.EnableHeadersVisualStyles = false;
            membersGrid.ColumnHeadersDefaultCellStyle.BackColor = AccentColor;
            membersGrid.ColumnHeadersDefaultCellStyle.ForeColor = TextColor;
            membersGrid.ColumnHeadersDefaultCellStyle.Font = BoldFont;
            membersGrid.ColumnHeadersDefaultCellStyle.SelectionBackColor = AccentColor;
            membersGrid.ColumnHeadersHeight = 50;

            // Add alternating row colors and hover hint
            membersGrid.RowsDefaultCellStyle.BackColor = Color.White;
            membersGrid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 240, 235);
            membersGrid.DefaultCellStyle.Font = RegularFont;
            membersGrid.DefaultCellStyle.SelectionBackColor = PrimaryColor;
            membersGrid.DefaultCellStyle.SelectionForeColor = SecondaryColor;

            // Add cursor change on hover
            membersGrid.CellMouseEnter += (s, e) =>
            {
                if (e.RowIndex >= 0)
                {
                    membersGrid.Cursor = Cursors.Hand;
                    var row = membersGrid.Rows[e.RowIndex];
                    row.DefaultCellStyle.BackColor = Color.FromArgb(255, 230, 210);
                }
            };

            membersGrid.CellMouseLeave += (s, e) =>
            {
                if (e.RowIndex >= 0)
                {
                    membersGrid.Cursor = Cursors.Default;
                    var row = membersGrid.Rows[e.RowIndex];
                    row.DefaultCellStyle.BackColor = e.RowIndex % 2 == 0 ?
                        Color.White : Color.FromArgb(250, 240, 235);
                }
            };

            // Configure grid columns
            membersGrid.Columns.AddRange(new DataGridViewColumn[]
            {
                new DataGridViewTextBoxColumn { Name = "Id", HeaderText = "ID", Width = 50, AutoSizeMode = DataGridViewAutoSizeColumnMode.None },
                new DataGridViewTextBoxColumn { Name = "ImePrezime", HeaderText = "Ime i Prezime", Width = 180, AutoSizeMode = DataGridViewAutoSizeColumnMode.None },
                new DataGridViewTextBoxColumn { Name = "Email", HeaderText = "Email", Width = 280, AutoSizeMode = DataGridViewAutoSizeColumnMode.None },
                new DataGridViewTextBoxColumn { Name = "BrojTelefona", HeaderText = "Broj Telefona", Width = 160, AutoSizeMode = DataGridViewAutoSizeColumnMode.None },
                new DataGridViewTextBoxColumn { Name = "Clanarina", HeaderText = "Članarina", Width = 120, AutoSizeMode = DataGridViewAutoSizeColumnMode.None },
                new DataGridViewTextBoxColumn { Name = "Status", HeaderText = "Status", Width = 120, AutoSizeMode = DataGridViewAutoSizeColumnMode.None },
                new DataGridViewButtonColumn { Name = "Edit", HeaderText = "", Width = 100, AutoSizeMode = DataGridViewAutoSizeColumnMode.None },
                new DataGridViewButtonColumn { Name = "Extend", HeaderText = "", Width = 100, AutoSizeMode = DataGridViewAutoSizeColumnMode.None }
            });

            // Add search functionality
            searchBox.GotFocus += (s, e) =>
            {
                if (searchBox.Text == "Pretraži članove...")
                {
                    searchBox.Text = "";
                    searchBox.ForeColor = TextColor;
                }
            };

            searchBox.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(searchBox.Text))
                {
                    searchBox.Text = "Pretraži članove...";
                    searchBox.ForeColor = Color.Gray;
                }
            };

            searchBox.TextChanged += (s, e) => FilterMembers(membersGrid, searchBox.Text, filterCombo.SelectedItem.ToString());
            filterCombo.SelectedIndexChanged += (s, e) => FilterMembers(membersGrid, searchBox.Text, filterCombo.SelectedItem.ToString());
            membersGrid.CellClick += (s, e) => HandleMemberGridAction(membersGrid, e);

            // Style alternating rows
            membersGrid.RowsDefaultCellStyle.BackColor = Color.White;
            membersGrid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 240, 235);

            // Add pagination panel
            var paginationPanel = new Panel
            {
                Width = mainContentPanel.Width - 60,
                Height = 50,
                Location = new Point(0, membersGrid.Bottom + 10)
            };

            var prevButton = new Button
            {
                Text = "◀ Prethodna",
                Size = new Size(180, 35),
                Location = new Point(0, 7),
                FlatStyle = FlatStyle.Flat,
                Font = RegularFont,
                BackColor = PrimaryColor,
                ForeColor = SecondaryColor,
                Cursor = Cursors.Hand
            };
            prevButton.FlatAppearance.BorderSize = 0;

            var nextButton = new Button
            {
                Text = "Sljedeća ▶",
                Size = new Size(180, 35),
                Location = new Point(paginationPanel.Width - 190, 7),
                FlatStyle = FlatStyle.Flat,
                Font = RegularFont,
                BackColor = PrimaryColor,
                ForeColor = SecondaryColor,
                Cursor = Cursors.Hand
            };
            nextButton.FlatAppearance.BorderSize = 0;

            pageInfoLabel = new Label
            {
                AutoSize = true,
                Font = RegularFont,
                ForeColor = TextColor,
                Location = new Point((paginationPanel.Width - 100) / 2, 15)
            };

            // Add pagination event handlers
            prevButton.Click += (s, e) =>
            {
                if (currentPage > 1)
                {
                    currentPage--;
                    FilterMembers(membersGrid, searchBox.Text, filterCombo.SelectedItem.ToString());
                }
            };

            nextButton.Click += (s, e) =>
            {
                var totalItems = GetFilteredMembers(searchBox.Text, filterCombo.SelectedItem.ToString()).Count;
                var totalPages = (int)Math.Ceiling(totalItems / (double)itemsPerPage);
                if (currentPage < totalPages)
                {
                    currentPage++;
                    FilterMembers(membersGrid, searchBox.Text, filterCombo.SelectedItem.ToString());
                }
            };

            // Add controls to panels
            membersPanel.Controls.AddRange(new Control[] { headerPanel, searchPanel, membersGrid });
            paginationPanel.Controls.AddRange(new Control[] { prevButton, pageInfoLabel, nextButton });
            membersPanel.Controls.Add(paginationPanel);
            mainContentPanel.Controls.Add(membersPanel);

            // Load initial data
            LoadMembersData(membersGrid);
        }

        // Add this method to get filtered members
        private List<Korisnik> GetFilteredMembers(string searchText, string filter)
        {
            return PodaciBiblioteke.Korisnici
                .Where(k => k.ImePrezime != trenutniBibliotekar.ImePrezime)
                .Where(k =>
                    (searchText == "" ||
                     searchText == "Pretraži članove..." ||
                     k.ImePrezime.ToLower().Contains(searchText.ToLower()) ||
                     k.Email.ToLower().Contains(searchText.ToLower())) &&
                    (filter == "Svi članovi" ||
                     (filter == "Aktivni" && k.Aktivni) ||
                     (filter == "Neaktivni" && !k.Aktivni)))
                .ToList();
        }

        private void LoadMembersData(DataGridView grid)
        {
            grid.Rows.Clear();
            var members = PodaciBiblioteke.Korisnici
                .Where(k => k != trenutniBibliotekar)
                .Skip((currentPage - 1) * itemsPerPage)
                .Take(itemsPerPage);

            foreach (var clan in members)
            {
                var row = grid.Rows[grid.Rows.Add()];
                row.Cells["Id"].Value = clan.Id;
                row.Cells["ImePrezime"].Value = clan.ImePrezime;
                row.Cells["Email"].Value = clan.Email;
                row.Cells["BrojTelefona"].Value = clan.BrojTelefona ?? "Nije uneseno";
                row.Cells["Clanarina"].Value = clan.VrstaClanarine;

                var statusCell = row.Cells["Status"];
                if (clan.Aktivni)
                {
                    statusCell.Value = "✓ Aktivan";
                    statusCell.Style.ForeColor = Color.FromArgb(34, 197, 94);
                }
                else
                {
                    statusCell.Value = "✗ Neaktivan";
                    statusCell.Style.ForeColor = Color.FromArgb(239, 68, 68);
                }

                row.Cells["Edit"].Value = "Uredi";
                row.Cells["Extend"].Value = "Članarina";
            }

            UpdatePaginationControls(PodaciBiblioteke.Korisnici.Count(k => k != trenutniBibliotekar));
        }

        private void HandleMemberGridAction(DataGridView grid, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var memberId = (int)grid.Rows[e.RowIndex].Cells["Id"].Value;
                var member = PodaciBiblioteke.Korisnici.First(k => k.Id == memberId);

                // Handle action buttons
                if (e.ColumnIndex == grid.Columns["Edit"].Index)
                {
                    ShowEditMemberDialog(member);
                }
                else if (e.ColumnIndex == grid.Columns["Extend"].Index)
                {
                    ShowMembershipExtensionDialog(member);
                }
                // Handle row click (excluding action button columns)
                else if (e.ColumnIndex != grid.Columns["Edit"].Index &&
                        e.ColumnIndex != grid.Columns["Extend"].Index)
                {
                    ShowMemberProfileDialog(member);
                }
            }
        }

        private void ShowMembershipExtensionDialog(Korisnik member)
        {
            var dialog = new Form
            {
                Text = "Produži članarinu",
                Size = new Size(600, 600),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                BackColor = SecondaryColor
            };

            // Header Panel
            var headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                BackColor = PrimaryColor
            };

            var headerLabel = new Label
            {
                Text = $"Produži članarinu - {member.ImePrezime}",
                Font = new Font(TitleFont.FontFamily, 20, FontStyle.Bold),
                ForeColor = SecondaryColor,
                AutoSize = true,
                Location = new Point(20, 20)
            };

            headerPanel.Controls.Add(headerLabel);

            // Content Panel
            var contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20)
            };

            var currentMembershipLabel = new Label
            {
                Text = $"Trenutna članarina: {member.VrstaClanarine}",
                Font = RegularFont,
                Location = new Point(20, 20),
                AutoSize = true
            };

            var expiryLabel = new Label
            {
                Text = $"Datum isteka: {member.DatumIsteka?.ToString("dd.MM.yyyy") ?? "Nema aktivne članarine"}",
                Font = RegularFont,
                Location = new Point(20, currentMembershipLabel.Bottom + 50),
                AutoSize = true
            };

            // Membership types panel
            var membershipPanel = new FlowLayoutPanel
            {
                AutoSize = true,
                FlowDirection = FlowDirection.TopDown,
                Location = new Point(20, expiryLabel.Bottom + 20),
                Width = 540,
                Height = 360
            };

            foreach (var tip in PodaciBiblioteke.TipoviClanarine)
            {
                membershipPanel.Controls.Add(CreateMembershipCard(tip, member, dialog));
            }

            // Cancel button
            var cancelButton = new Button
            {
                Text = "Odustani",
                Size = new Size(150, 40),
                Location = new Point(200, membershipPanel.Bottom),
                FlatStyle = FlatStyle.Flat,
                Font = RegularFont,
                BackColor = Color.FromArgb(229, 231, 235),
                ForeColor = TextColor,
                Cursor = Cursors.Hand
            };
            cancelButton.FlatAppearance.BorderSize = 0;
            cancelButton.Click += (s, e) => dialog.Close();

            // Add hover effects
            cancelButton.MouseEnter += (s, e) => cancelButton.BackColor = Color.FromArgb(220, 220, 220);
            cancelButton.MouseLeave += (s, e) => cancelButton.BackColor = Color.FromArgb(229, 231, 235);

            contentPanel.Controls.AddRange(new Control[] {
                currentMembershipLabel,
                expiryLabel,
                membershipPanel,
                cancelButton
            });

            dialog.Controls.AddRange(new Control[] { headerPanel, contentPanel });
            dialog.ShowDialog();
        }

        private Panel CreateMembershipCard(TipClanarine tip, Korisnik member, Form dialog)
        {
            var card = new Panel
            {
                Width = 540,
                Height = 100,
                Margin = new Padding(0, 0, 0, 10),
                BackColor = Color.White,
                Padding = new Padding(20),
                Cursor = Cursors.Hand
            };

            // Add border
            card.Paint += (s, e) =>
            {
                using (var pen = new Pen(Color.FromArgb(229, 231, 235)))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, card.Width - 1, card.Height - 1);
                }
            };

            var titleLabel = new Label
            {
                Text = tip.Naziv,
                Font = new Font(RegularFont.FontFamily, 14, FontStyle.Bold),
                Location = new Point(15, 15),
                AutoSize = true
            };

            var durationLabel = new Label
            {
                Text = $"Trajanje: {tip.TrajanjeMjeseci} mjeseci",
                Font = RegularFont,
                Location = new Point(15, titleLabel.Bottom + 10),
                AutoSize = true
            };

            var priceLabel = new Label
            {
                Text = $"Cijena: {tip.Cijena:F2} KM",
                Font = new Font(RegularFont.FontFamily, 14, FontStyle.Bold),
                ForeColor = PrimaryColor,
                Location = new Point(card.Width - 200, 35),
                AutoSize = true
            };

            card.Controls.AddRange(new Control[] { titleLabel, durationLabel, priceLabel });

            // Hover effects
            card.MouseEnter += (s, e) =>
            {
                card.BackColor = Color.FromArgb(255, 247, 237);
            };

            card.MouseLeave += (s, e) =>
            {
                card.BackColor = Color.White;
            };

            // Click handler
            card.Click += (s, e) =>
            {
                var result = CustomMessageBox.Show(
                $"Da li želite produžiti članarinu za člana {member.ImePrezime} na {tip.Naziv}?\n" +
                $"Cijena: {tip.Cijena:F2} KM\n" +
                $"Trajanje: {tip.TrajanjeMjeseci} mjeseci",
                "Potvrda produženja članarine",
                "Da",
                "Ne",
                MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    ExtendMembership(member, tip);
                    dialog.Close();
                }
            };

            return card;
        }

        private void ExtendMembership(Korisnik member, TipClanarine tip)
        {
            try
            {
                // Calculate new expiry date
                DateTime startDate = member.DatumIsteka.HasValue && member.DatumIsteka.Value > DateTime.Now
                    ? member.DatumIsteka.Value
                    : DateTime.Now;

                member.Clanarina = tip;
                member.DatumUclanjenja = DateTime.Now;
                member.DatumIsteka = startDate.AddMonths(tip.TrajanjeMjeseci);
                member.Aktivni = true;

                MessageBox.Show(
                    $"Članarina je uspješno produžena!\n\n" +
                    $"Član: {member.ImePrezime}\n" +
                    $"Tip članarine: {tip.Naziv}\n" +
                    $"Važi do: {member.DatumIsteka:dd.MM.yyyy}",
                    "Uspjeh",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                // Refresh the members grid
                var membersPanel = mainContentPanel.Controls.OfType<Panel>().FirstOrDefault();
                if (membersPanel != null)
                {
                    var grid = membersPanel.Controls.OfType<DataGridView>().FirstOrDefault();
                    if (grid != null)
                    {
                        LoadMembersData(grid);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Došlo je do greške prilikom produženja članarine:\n{ex.Message}",
                    "Greška",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void FilterMembers(DataGridView grid, string searchText, string filter)
        {
            grid.Rows.Clear();
            var filteredMembers = PodaciBiblioteke.Korisnici
                .Where(k => k != trenutniBibliotekar)
                .Where(k =>
                    (searchText == "" || searchText == "Pretraži članove..." ||
                     k.ImePrezime.ToLower().Contains(searchText.ToLower()) ||
                     k.Email.ToLower().Contains(searchText.ToLower())) &&
                    (filter == "Svi članovi" ||
                     (filter == "Aktivni" && k.Aktivni) ||
                     (filter == "Neaktivni" && !k.Aktivni)))
                .Skip((currentPage - 1) * itemsPerPage)
                .Take(itemsPerPage);

            foreach (var clan in filteredMembers)
            {
                AddMemberToGrid(grid, clan);
            }

            UpdatePaginationControls(GetFilteredMembersCount(searchText, filter));
        }

        private int GetFilteredMembersCount(string searchText, string filter)
        {
            return PodaciBiblioteke.Korisnici
                .Where(k => k != trenutniBibliotekar)
                .Count(k =>
                    (searchText == "" || searchText == "Pretraži članove..." ||
                     k.ImePrezime.ToLower().Contains(searchText.ToLower()) ||
                     k.Email.ToLower().Contains(searchText.ToLower())) &&
                    (filter == "Svi članovi" ||
                     (filter == "Aktivni" && k.Aktivni) ||
                     (filter == "Neaktivni" && !k.Aktivni)));
        }

        private void AddMemberToGrid(DataGridView grid, Korisnik clan)
        {
            var row = grid.Rows[grid.Rows.Add()];
            row.Cells["Id"].Value = clan.Id;
            row.Cells["ImePrezime"].Value = clan.ImePrezime;
            row.Cells["Email"].Value = clan.Email;
            row.Cells["BrojTelefona"].Value = clan.BrojTelefona ?? "Nije uneseno";
            row.Cells["Clanarina"].Value = clan.VrstaClanarine;

            var statusCell = row.Cells["Status"];
            if (clan.Aktivni)
            {
                statusCell.Value = "✓ Aktivan";
                statusCell.Style.ForeColor = Color.FromArgb(34, 197, 94);
            }
            else
            {
                statusCell.Value = "✗ Neaktivan";
                statusCell.Style.ForeColor = Color.FromArgb(239, 68, 68);
            }

            row.Cells["Edit"].Value = "Uredi";
            row.Cells["Extend"].Value = "Članarina";
        }

        private void ShowAddMemberDialog()
        {
            var dialog = new Form
            {
                Text = "Dodaj novog člana",
                Size = new Size(600, 600),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                BackColor = SecondaryColor
            };

            // Header Panel
            var headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                BackColor = PrimaryColor
            };

            var headerLabel = new Label
            {
                Text = "👤 Novi član",
                Font = new Font(TitleFont.FontFamily, 20, FontStyle.Bold),
                ForeColor = SecondaryColor,
                AutoSize = true,
                Location = new Point(20, 20)
            };

            headerPanel.Controls.Add(headerLabel);

            // Content Panel
            var contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20)
            };

            int yPos = 100;
            const int spacing = 80;

            // Name input
            var nameLabel = CreateFieldLabel("Ime i prezime *", yPos);
            var nameTextBox = CreateStyledTextBox(yPos + 35, 480);

            yPos += spacing;

            // Email input
            var emailLabel = CreateFieldLabel("Email *", yPos);
            var emailTextBox = CreateStyledTextBox(yPos + 35, 480);

            yPos += spacing;

            // Phone input
            var phoneLabel = CreateFieldLabel("Broj telefona *", yPos);
            var phoneTextBox = CreateStyledTextBox(yPos + 35, 480);

            yPos += spacing;

            // Membership type selection
            var membershipLabel = CreateFieldLabel("Tip članarine *", yPos);
            var membershipCombo = new ComboBox
            {
                Location = new Point(phoneTextBox.Left, yPos + 35),
                Width = 480,
                Height = 35,
                Font = RegularFont,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            membershipCombo.Items.AddRange(PodaciBiblioteke.TipoviClanarine.Select(t => t.Naziv).ToArray());
            if (membershipCombo.Items.Count > 0)
                membershipCombo.SelectedIndex = 0;

            yPos += spacing + 20;

            // Required fields note
            var noteLabel = new Label
            {
                Text = "* Obavezna polja",
                Font = new Font(RegularFont.FontFamily, 10),
                ForeColor = Color.FromArgb(107, 114, 128),
                AutoSize = true,
                Location = new Point(phoneTextBox.Left, yPos)
            };

            yPos += 40;

            // Buttons
            var buttonPanel = new Panel
            {
                Width = 540,
                Height = 50,
                Location = new Point(20, yPos)
            };

            var saveButton = new Button
            {
                Text = "Sačuvaj",
                Size = new Size(150, 40),
                Location = new Point(buttonPanel.Width - 415, 0),
                FlatStyle = FlatStyle.Flat,
                Font = RegularFont,
                BackColor = PrimaryColor,
                ForeColor = SecondaryColor,
                Cursor = Cursors.Hand
            };
            saveButton.FlatAppearance.BorderSize = 0;

            var cancelButton = new Button
            {
                Text = "Odustani",
                Size = new Size(150, 40),
                Location = new Point(buttonPanel.Width - 235, 0),
                FlatStyle = FlatStyle.Flat,
                Font = RegularFont,
                BackColor = Color.FromArgb(229, 231, 235),
                ForeColor = TextColor,
                Cursor = Cursors.Hand
            };
            cancelButton.FlatAppearance.BorderSize = 0;

            var errorProvider = new ErrorProvider();
            errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;

            // Add validation to input controls
            nameTextBox.Tag = "required";
            nameTextBox.Name = "Ime";
            emailTextBox.Tag = "required";
            emailTextBox.Name = "Email";
            phoneTextBox.Tag = "required";
            phoneTextBox.Name = "Telefon";

            // Add validation events
            nameTextBox.TextChanged += (s, e) =>
            {
                ValidateTextBox(nameTextBox, "Ime", errorProvider);
                if (!string.IsNullOrWhiteSpace(nameTextBox.Text) && !nameTextBox.Text.Contains(" "))
                {
                    errorProvider.SetError(nameTextBox, "Unesite ime i prezime.");
                    nameTextBox.BackColor = Color.FromArgb(254, 226, 226);
                }
            };

            emailTextBox.TextChanged += (s, e) =>
            {
                ValidateTextBox(emailTextBox, "Email", errorProvider);
                if (!string.IsNullOrWhiteSpace(emailTextBox.Text) && !emailTextBox.Text.Contains("@"))
                {
                    errorProvider.SetError(emailTextBox, "Unesite validnu email adresu.");
                    emailTextBox.BackColor = Color.FromArgb(254, 226, 226);
                }
            };

            phoneTextBox.TextChanged += (s, e) =>
            {
                ValidateTextBox(phoneTextBox, "Telefon", errorProvider);
                if (!string.IsNullOrWhiteSpace(phoneTextBox.Text) && !IsValidPhone(phoneTextBox.Text))
                {
                    errorProvider.SetError(phoneTextBox, "Unesite validan broj telefona.");
                    phoneTextBox.BackColor = Color.FromArgb(254, 226, 226);
                }
            };

            nameTextBox.Leave += (s, e) =>
            {
                ValidateTextBox(nameTextBox, "Ime", errorProvider);
                if (!string.IsNullOrWhiteSpace(nameTextBox.Text) && !nameTextBox.Text.Contains(" "))
                {
                    errorProvider.SetError(nameTextBox, "Unesite ime i prezime.");
                    nameTextBox.BackColor = Color.FromArgb(254, 226, 226);
                }
            };

            emailTextBox.Leave += (s, e) =>
            {
                ValidateTextBox(emailTextBox, "Email", errorProvider);
                if (!string.IsNullOrWhiteSpace(emailTextBox.Text) && !emailTextBox.Text.Contains("@"))
                {
                    errorProvider.SetError(emailTextBox, "Unesite validnu email adresu.");
                    emailTextBox.BackColor = Color.FromArgb(254, 226, 226);
                }
            };

            phoneTextBox.Leave += (s, e) =>
            {
                ValidateTextBox(phoneTextBox, "Telefon", errorProvider);
                if (!string.IsNullOrWhiteSpace(phoneTextBox.Text) && !IsValidPhone(phoneTextBox.Text))
                {
                    errorProvider.SetError(phoneTextBox, "Unesite validan broj telefona.");
                    phoneTextBox.BackColor = Color.FromArgb(254, 226, 226);
                }
            };

            // Add hover effects
            saveButton.MouseEnter += (s, e) => saveButton.BackColor = HoverColor;
            saveButton.MouseLeave += (s, e) => saveButton.BackColor = PrimaryColor;
            cancelButton.MouseEnter += (s, e) => cancelButton.BackColor = Color.FromArgb(220, 220, 220);
            cancelButton.MouseLeave += (s, e) => cancelButton.BackColor = Color.FromArgb(229, 231, 235);

            // Save functionality
            saveButton.Click += (s, e) =>
            {
                if (ValidateMemberInputs(nameTextBox, emailTextBox, phoneTextBox))
                {
                    SaveMember(
                        nameTextBox.Text,
                        emailTextBox.Text,
                        phoneTextBox.Text,
                        membershipCombo.SelectedItem.ToString(),
                        dialog
                    );
                }
            };

            cancelButton.Click += (s, e) => dialog.Close();

            buttonPanel.Controls.AddRange(new Control[] { saveButton, cancelButton });

            // Add all controls to content panel
            contentPanel.Controls.AddRange(new Control[] {
        nameLabel, nameTextBox,
        emailLabel, emailTextBox,
        phoneLabel, phoneTextBox,
        membershipLabel, membershipCombo,
        noteLabel,
        buttonPanel
    });

            dialog.Controls.AddRange(new Control[] { headerPanel, contentPanel });
            dialog.ShowDialog();
        }

        private bool ValidateMemberInputs(TextBox nameBox, TextBox emailBox, TextBox phoneBox)
        {
            var isValid = true;
            var errorMessage = new StringBuilder("Molimo popunite sva obavezna polja:\n");

            if (string.IsNullOrWhiteSpace(nameBox.Text))
            {
                errorMessage.AppendLine("- Ime i prezime");
                isValid = false;
            }
            else
            {
                var nameParts = nameBox.Text.Split(new[] { ' ' }, 2);
                if (nameParts.Length < 2)
                {
                    errorMessage.AppendLine("- Unesite i ime i prezime");
                    isValid = false;
                }
            }

            if (string.IsNullOrWhiteSpace(emailBox.Text))
            {
                errorMessage.AppendLine("- Email");
                isValid = false;
            }
            else if (!IsValidEmail(emailBox.Text))
            {
                errorMessage.AppendLine("- Unesite ispravan email format");
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(phoneBox.Text))
            {
                errorMessage.AppendLine("- Telefon");
                isValid = false;
            }
            else if (!IsValidPhone(phoneBox.Text))
            {
                errorMessage.AppendLine("- Unesite ispravan broj telefona");
                isValid = false;
            }

            if (!isValid)
            {
                MessageBox.Show(
                    errorMessage.ToString(),
                    "Validacija",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
            }

            return isValid;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private bool IsValidPhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return false;

            // Remove non-numeric characters
            string cleanPhone = new string(phone.Where(char.IsDigit).ToArray());

            // Ensure length is between 9 and 12 digits
            if (cleanPhone.Length < 9 || cleanPhone.Length > 12)
                return false;

            // Mobile prefixes: 060, 061, 062, 063, 064, 065, 066, 067
            string[] mobilePrefixes = { "060", "061", "062", "063", "064", "065", "066", "067" };

            // Landline prefixes: 030-037, 049, 050-057
            string[] landlinePrefixes = { "049" };
            landlinePrefixes = landlinePrefixes.Concat(Enumerable.Range(30, 8).Select(x => x.ToString())).ToArray();
            landlinePrefixes = landlinePrefixes.Concat(Enumerable.Range(50, 8).Select(x => x.ToString())).ToArray();

            // Check for valid prefix
            bool isValidPrefix = mobilePrefixes.Any(cleanPhone.StartsWith) || landlinePrefixes.Any(cleanPhone.StartsWith);

            return isValidPrefix;
        }


        private void SaveMember(string name, string email, string phone, string membershipType, Form dialog)
        {
            try
            {
                var membership = PodaciBiblioteke.TipoviClanarine.First(t => t.Naziv == membershipType);

                // Split the full name into first and last name
                var nameParts = name.Split(new[] { ' ' }, 2);
                if (nameParts.Length < 2)
                {
                    MessageBox.Show(
                        "Molimo unesite i ime i prezime.",
                        "Validacija",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                    return;
                }

                var newMember = new Korisnik
                {
                    Id = PodaciBiblioteke.Korisnici.Count + 1,
                    Ime = nameParts[0],
                    Prezime = nameParts[1],
                    Email = email,
                    BrojTelefona = phone,
                    Clanarina = membership,
                    DatumUclanjenja = DateTime.Now,
                    DatumIsteka = DateTime.Now.AddMonths(membership.TrajanjeMjeseci),
                    Aktivni = true
                };

                PodaciBiblioteke.Korisnici.Add(newMember);
                dialog.DialogResult = DialogResult.OK;
                dialog.Close();

                MessageBox.Show(
                    "Član je uspješno dodan!",
                    "Uspjeh",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                // Refresh the grid
                var membersPanel = mainContentPanel.Controls.OfType<Panel>().FirstOrDefault();
                if (membersPanel != null)
                {
                    var grid = membersPanel.Controls.OfType<DataGridView>().FirstOrDefault();
                    if (grid != null)
                    {
                        LoadMembersData(grid);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Došlo je do greške prilikom dodavanja člana:\n{ex.Message}",
                    "Greška",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void ShowEditMemberDialog(Korisnik member)
        {
            var dialog = new Form
            {
                Text = "Uredi člana",
                Size = new Size(600, 600),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                BackColor = SecondaryColor
            };

            // Header Panel
            var headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                BackColor = PrimaryColor
            };

            var headerLabel = new Label
            {
                Text = $"👤 Uredi člana - {member.ImePrezime}",
                Font = new Font(TitleFont.FontFamily, 20, FontStyle.Bold),
                ForeColor = SecondaryColor,
                AutoSize = true,
                Location = new Point(20, 20)
            };

            headerPanel.Controls.Add(headerLabel);

            // Content Panel
            var contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20)
            };

            int yPos = 100;
            const int spacing = 80;

            // Name input
            var nameLabel = CreateFieldLabel("Ime i prezime *", yPos);
            var nameTextBox = CreateStyledTextBox(yPos + 35, 480);
            nameTextBox.Text = member.ImePrezime;

            yPos += spacing;

            // Email input
            var emailLabel = CreateFieldLabel("Email *", yPos);
            var emailTextBox = CreateStyledTextBox(yPos + 35, 480);
            emailTextBox.Text = member.Email;

            yPos += spacing;

            // Phone input
            var phoneLabel = CreateFieldLabel("Broj telefona", yPos);
            var phoneTextBox = CreateStyledTextBox(yPos + 35, 480);
            phoneTextBox.Text = member.BrojTelefona;

            yPos += spacing + 20;

            // Required fields note
            var noteLabel = new Label
            {
                Text = "* Obavezna polja",
                Font = new Font(RegularFont.FontFamily, 10),
                ForeColor = Color.FromArgb(107, 114, 128),
                AutoSize = true,
                Location = new Point(phoneTextBox.Left, yPos)
            };

            yPos += 40;

            // Buttons Panel
            var buttonPanel = new Panel
            {
                Width = 540,
                Height = 50,
                Location = new Point(20, yPos)
            };

            var saveButton = new Button
            {
                Text = "Sačuvaj",
                Size = new Size(150, 40),
                Location = new Point(buttonPanel.Width - 415, 0),
                FlatStyle = FlatStyle.Flat,
                Font = RegularFont,
                BackColor = PrimaryColor,
                ForeColor = SecondaryColor,
                Cursor = Cursors.Hand
            };
            saveButton.FlatAppearance.BorderSize = 0;

            var cancelButton = new Button
            {
                Text = "Odustani",
                Size = new Size(150, 40),
                Location = new Point(buttonPanel.Width - 235, 0),
                FlatStyle = FlatStyle.Flat,
                Font = RegularFont,
                BackColor = Color.FromArgb(229, 231, 235),
                ForeColor = TextColor,
                Cursor = Cursors.Hand
            };
            cancelButton.FlatAppearance.BorderSize = 0;

            // Delete button
            var deleteButton = new Button
            {
                Text = "Obriši člana",
                Size = new Size(150, 40),
                Location = new Point(20, buttonPanel.Bottom + 20),
                FlatStyle = FlatStyle.Flat,
                Font = RegularFont,
                BackColor = Color.FromArgb(239, 68, 68),
                ForeColor = SecondaryColor,
                Cursor = Cursors.Hand
            };
            deleteButton.FlatAppearance.BorderSize = 0;

            // Add hover effects
            saveButton.MouseEnter += (s, e) => saveButton.BackColor = HoverColor;
            saveButton.MouseLeave += (s, e) => saveButton.BackColor = PrimaryColor;
            cancelButton.MouseEnter += (s, e) => cancelButton.BackColor = Color.FromArgb(220, 220, 220);
            cancelButton.MouseLeave += (s, e) => cancelButton.BackColor = Color.FromArgb(229, 231, 235);
            deleteButton.MouseEnter += (s, e) => deleteButton.BackColor = Color.FromArgb(220, 38, 38);
            deleteButton.MouseLeave += (s, e) => deleteButton.BackColor = Color.FromArgb(239, 68, 68);

            // Button click handlers
            saveButton.Click += (s, e) =>
            {
                if (ValidateMemberInputs(nameTextBox, emailTextBox, phoneTextBox))
                {
                    UpdateMember(member, nameTextBox.Text, emailTextBox.Text, phoneTextBox.Text, dialog);
                }
            };

            cancelButton.Click += (s, e) => dialog.Close();

            deleteButton.Click += (s, e) =>
            {
                var result = CustomMessageBox.Show(
                $"Da li ste sigurni da želite obrisati člana {member.ImePrezime}?\nOva akcija se ne može poništiti.",
                "Potvrda brisanja",
                "Da",
                "Ne",
                MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    DeleteMember(member, dialog);
                }
            };

            buttonPanel.Controls.AddRange(new Control[] { saveButton, cancelButton });
            contentPanel.Controls.Add(deleteButton);

            // Add all controls to content panel
            contentPanel.Controls.AddRange(new Control[] {
        nameLabel, nameTextBox,
        emailLabel, emailTextBox,
        phoneLabel, phoneTextBox,
        noteLabel,
        buttonPanel
    });

            dialog.Controls.AddRange(new Control[] { headerPanel, contentPanel });

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var membersPanel = mainContentPanel.Controls.OfType<Panel>().FirstOrDefault();
                if (membersPanel != null)
                {
                    var grid = membersPanel.Controls.OfType<DataGridView>().FirstOrDefault();
                    if (grid != null)
                    {
                        LoadMembersData(grid);
                    }
                }
            }
        }

        private void UpdateMember(Korisnik member, string name, string email, string phone, Form dialog)
        {
            try
            {
                var nameParts = name.Split(new[] { ' ' }, 2);
                if (nameParts.Length < 2)
                {
                    MessageBox.Show(
                        "Molimo unesite i ime i prezime.",
                        "Validacija",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                    return;
                }

                member.Ime = nameParts[0];
                member.Prezime = nameParts[1];
                member.Email = email;
                member.BrojTelefona = phone;

                dialog.DialogResult = DialogResult.OK;
                dialog.Close();

                MessageBox.Show(
                    "Član je uspješno ažuriran!",
                    "Uspjeh",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Došlo je do greške prilikom ažuriranja člana:\n{ex.Message}",
                    "Greška",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void DeleteMember(Korisnik member, Form dialog)
        {
            try
            {
                PodaciBiblioteke.Korisnici.Remove(member);
                dialog.DialogResult = DialogResult.OK;
                dialog.Close();

                MessageBox.Show(
                    "Član je uspješno obrisan!",
                    "Uspjeh",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Došlo je do greške prilikom brisanja člana:\n{ex.Message}",
                    "Greška",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void ShowMemberProfileDialog(Korisnik member)
        {
            var dialog = new Form
            {
                Text = $"Profil člana - {member.ImePrezime}",
                Size = new Size(900, 700),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                BackColor = SecondaryColor
            };

            // Header Panel
            var headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 100,
                BackColor = PrimaryColor,
                Padding = new Padding(20)
            };

            var headerLabel = new Label
            {
                Text = member.ImePrezime,
                Font = new Font(TitleFont.FontFamily, 18, FontStyle.Bold),
                ForeColor = SecondaryColor,
                AutoSize = true,
                Location = new Point(20, 10)
            };

            var membershipStatus = new Label
            {
                Text = member.Aktivni ? "✓ Aktivni član" : "✗ Neaktivni član",
                Font = RegularFont,
                ForeColor = SecondaryColor,
                AutoSize = true,
                Location = new Point(20, headerLabel.Bottom + 20)
            };

            headerPanel.Controls.AddRange(new Control[] { headerLabel, membershipStatus });

            // TabControl directly in the form
            var tabControl = new TabControl
            {
                Location = new Point(20, headerPanel.Bottom + 20),
                Size = new Size(dialog.ClientSize.Width - 40, dialog.ClientSize.Height - headerPanel.Height - 100),
                Font = RegularFont
            };

            // Info Tab
            var infoTab = new TabPage("Informacije");
            infoTab.Controls.Add(CreateMemberInfoPanel(member));

            // Loan History Tab
            var historyTab = new TabPage("Historija posudbi");
            historyTab.Controls.Add(CreateLoanHistoryPanel(member));

            // Reservations Tab
            var reservationsTab = new TabPage("Rezervacije");
            reservationsTab.Controls.Add(CreateReservationsPanel(member));

            tabControl.TabPages.AddRange(new TabPage[] { infoTab, historyTab, reservationsTab });

            // Close button
            var closeButton = new Button
            {
                Text = "Zatvori",
                Size = new Size(150, 40),
                Location = new Point((dialog.ClientSize.Width - 150) / 2, tabControl.Bottom + 10),
                FlatStyle = FlatStyle.Flat,
                Font = RegularFont,
                BackColor = PrimaryColor,
                ForeColor = SecondaryColor,
                Cursor = Cursors.Hand
            };
            closeButton.FlatAppearance.BorderSize = 0;
            closeButton.Click += (s, e) => dialog.Close();

            dialog.Controls.AddRange(new Control[] { headerPanel, tabControl, closeButton });
            dialog.ShowDialog();
        }

        private Panel CreateMemberInfoPanel(Korisnik member)
        {
            var panel = new Panel
            {
                AutoScroll = true,
                Dock = DockStyle.Fill,
                Padding = new Padding(20)
            };

            var infoTable = new TableLayoutPanel
            {
                ColumnCount = 2,
                RowCount = 8,
                Dock = DockStyle.Top,
                AutoSize = true
            };

            infoTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            infoTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));

            AddInfoRow(infoTable, "ID:", member.Id.ToString(), 0);
            AddInfoRow(infoTable, "Ime:", member.Ime, 1);
            AddInfoRow(infoTable, "Prezime:", member.Prezime, 2);
            AddInfoRow(infoTable, "Email:", member.Email, 3);
            AddInfoRow(infoTable, "Telefon:", member.BrojTelefona ?? "Nije uneseno", 4);
            AddInfoRow(infoTable, "Tip članarine:", member.VrstaClanarine, 5);
            AddInfoRow(infoTable, "Datum učlanjenja:", member.DatumUclanjenja.ToString(), 6);
            AddInfoRow(infoTable, "Datum isteka:", member.DatumIsteka?.ToString("dd.MM.yyyy") ?? "Nije aktivno", 7);

            panel.Controls.Add(infoTable);
            return panel;
        }

        private Panel CreateLoanHistoryPanel(Korisnik member)
        {
            var panel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(20)
            };

            var grid = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoGenerateColumns = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                BackgroundColor = SecondaryColor,
                RowHeadersVisible = false,
                BorderStyle = BorderStyle.None,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                RowTemplate = { Height = 40 },
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false
            };

            // Configure columns with specific widths
            grid.Columns.AddRange(new DataGridViewColumn[]
            {
        new DataGridViewTextBoxColumn {
            Name = "Knjiga",
            HeaderText = "Knjiga",
            Width = 300,
            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        },
        new DataGridViewTextBoxColumn {
            Name = "DatumPosudbe",
            HeaderText = "Datum posudbe",
            Width = 150,
            AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        },
        new DataGridViewTextBoxColumn {
            Name = "DatumVracanja",
            HeaderText = "Datum vraćanja",
            Width = 150,
            AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        },
        new DataGridViewTextBoxColumn {
            Name = "Status",
            HeaderText = "Status",
            Width = 120,
            AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        }
            });

            // Enhanced styling
            grid.EnableHeadersVisualStyles = false;
            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.White;
            grid.ColumnHeadersDefaultCellStyle.ForeColor = TextColor;
            grid.ColumnHeadersDefaultCellStyle.Font = BoldFont;
            grid.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.White;
            grid.ColumnHeadersHeight = 50;
            grid.DefaultCellStyle.Font = RegularFont;
            grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(255, 247, 237);
            grid.DefaultCellStyle.SelectionForeColor = TextColor;
            grid.RowsDefaultCellStyle.BackColor = Color.White;
            grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(252, 252, 252);
            grid.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(255, 247, 237);
            grid.AlternatingRowsDefaultCellStyle.SelectionForeColor = TextColor;

            // Load loan history with status colors
            var loans = PodaciBiblioteke.Posudbe
                .Where(p => p.Korisnik.Id == member.Id)
                .OrderByDescending(p => p.DatumPosudbe);

            foreach (var loan in loans)
            {
                var row = grid.Rows[grid.Rows.Add()];
                row.Cells["Knjiga"].Value = loan.Knjiga.Naslov;
                row.Cells["DatumPosudbe"].Value = loan.DatumPosudbe.ToString("dd.MM.yyyy");
                row.Cells["DatumVracanja"].Value = loan.DatumVracanja.ToString("dd.MM.yyyy");

                var statusCell = row.Cells["Status"];
                if (loan.Status)
                {
                    statusCell.Value = "✓ Vraćeno";
                    statusCell.Style.ForeColor = Color.FromArgb(34, 197, 94);
                }
                else
                {
                    statusCell.Value = loan.DatumVracanja < DateTime.Now ? "⚠️ Kasni" : "🔄 Aktivno";
                    statusCell.Style.ForeColor = loan.DatumVracanja < DateTime.Now ?
                        Color.FromArgb(239, 68, 68) : Color.FromArgb(59, 130, 246);
                }
            }

            panel.Controls.Add(grid);
            return panel;
        }

        private Panel CreateReservationsPanel(Korisnik member)
        {
            var panel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(20)
            };

            var grid = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoGenerateColumns = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                BackgroundColor = SecondaryColor,
                RowHeadersVisible = false,
                BorderStyle = BorderStyle.None,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                RowTemplate = { Height = 40 },
                MultiSelect = false
            };

            grid.Columns.AddRange(new DataGridViewColumn[]
            {
                new DataGridViewTextBoxColumn {
                    Name = "Knjiga",
                    HeaderText = "Knjiga",
                    Width = 300,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                },
                new DataGridViewTextBoxColumn {
                    Name = "DatumRezervacije",
                    HeaderText = "Datum rezervacije",
                    Width = 150,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                },
                new DataGridViewTextBoxColumn {
                    Name = "Status",
                    HeaderText = "Status",
                    Width = 120,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.None
                }
            });

            // Enhanced styling
            grid.EnableHeadersVisualStyles = false;
            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.White;
            grid.ColumnHeadersDefaultCellStyle.ForeColor = TextColor;
            grid.ColumnHeadersDefaultCellStyle.Font = BoldFont;
            grid.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.White;
            grid.ColumnHeadersHeight = 50;
            grid.DefaultCellStyle.Font = RegularFont;
            grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(255, 247, 237);
            grid.DefaultCellStyle.SelectionForeColor = TextColor;
            grid.RowsDefaultCellStyle.BackColor = Color.White;
            grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(252, 252, 252);
            grid.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(255, 247, 237);
            grid.AlternatingRowsDefaultCellStyle.SelectionForeColor = TextColor;

            var reservations = PodaciBiblioteke.Rezervacije
                .Where(r => r.Korisnik.Id == member.Id)
                .OrderByDescending(r => r.DatumRezervacije);

            foreach (var reservation in reservations)
            {
                var row = grid.Rows[grid.Rows.Add()];
                row.Cells["Knjiga"].Value = reservation.Knjiga.Naslov;
                row.Cells["DatumRezervacije"].Value = reservation.DatumRezervacije.ToString("dd.MM.yyyy");

                var statusCell = row.Cells["Status"];
                if (!reservation.IsComplete)
                {
                    statusCell.Value = "🔄 Aktivna";
                    statusCell.Style.ForeColor = Color.FromArgb(59, 130, 246);
                }
                else
                {
                    statusCell.Value = "✓ Završena";
                    statusCell.Style.ForeColor = Color.FromArgb(34, 197, 94);
                }
            }

            panel.Controls.Add(grid);
            return panel;
        }

        private void AddInfoRow(TableLayoutPanel table, string label, string value, int rowIndex)
        {
            table.Controls.Add(new Label
            {
                Text = label,
                Font = BoldFont,
                AutoSize = true,
                Margin = new Padding(0, 5, 10, 5)
            }, 0, rowIndex);

            table.Controls.Add(new Label
            {
                Text = value,
                Font = RegularFont,
                AutoSize = true,
                Margin = new Padding(0, 5, 0, 5)
            }, 1, rowIndex);
        }

        private void ShowLoans()
        {
            mainContentPanel.Controls.Clear();

            var loansPanel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(30),
                BackColor = SecondaryColor
            };

            // Header with title and add button
            var headerPanel = new Panel
            {
                Width = mainContentPanel.Width - 60,
                Height = 60,
                Location = new Point(0, 120)
            };

            var titleLabel = new Label
            {
                Text = "Upravljanje posudbama",
                Font = new Font(TitleFont.FontFamily, 20, FontStyle.Bold),
                ForeColor = TextColor,
                AutoSize = true,
                Location = new Point(0, 15)
            };

            var addButton = new Button
            {
                Text = "➕ Nova posudba",
                Size = new Size(190, 40),
                Location = new Point(headerPanel.Width - 210, 10),
                FlatStyle = FlatStyle.Flat,
                Font = RegularFont,
                BackColor = PrimaryColor,
                ForeColor = SecondaryColor,
                Cursor = Cursors.Hand
            };
            addButton.FlatAppearance.BorderSize = 0;
            addButton.Click += (s, e) => ShowAddLoanDialog();

            headerPanel.Controls.AddRange(new Control[] { titleLabel, addButton });

            // Search panel
            var searchPanel = new Panel
            {
                Width = mainContentPanel.Width - 60,
                Height = 60,
                Location = new Point(0, headerPanel.Bottom + 20)
            };

            var searchBox = new TextBox
            {
                Size = new Size(300, 35),
                Location = new Point(0, 10),
                Font = RegularFont,
                ForeColor = Color.Gray,
                Text = "Pretraži posudbe..."
            };

            var filterCombo = new ComboBox
            {
                Size = new Size(150, 35),
                Location = new Point(searchBox.Right + 20, 10),
                Font = RegularFont,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            filterCombo.Items.AddRange(new object[] { "Sve posudbe", "Aktivne", "Zakašnjele" });
            filterCombo.SelectedIndex = 0;

            searchPanel.Controls.AddRange(new Control[] { searchBox, filterCombo });

            // Loans grid
            var loansGrid = new DataGridView
            {
                Location = new Point(0, searchPanel.Bottom + 20),
                Width = mainContentPanel.Width - 70,
                Height = mainContentPanel.Height - searchPanel.Bottom - 146,
                BackgroundColor = SecondaryColor,
                BorderStyle = BorderStyle.Fixed3D,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                RowHeadersVisible = false,
                AllowUserToResizeRows = false,
                MultiSelect = false,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                RowTemplate = { Height = 40 }
            };

            // Style the grid
            loansGrid.EnableHeadersVisualStyles = false;
            loansGrid.ColumnHeadersDefaultCellStyle.BackColor = AccentColor;
            loansGrid.ColumnHeadersDefaultCellStyle.ForeColor = TextColor;
            loansGrid.ColumnHeadersDefaultCellStyle.Font = BoldFont;
            loansGrid.ColumnHeadersDefaultCellStyle.SelectionBackColor = AccentColor;
            loansGrid.ColumnHeadersHeight = 50;

            // Add alternating row colors
            loansGrid.RowsDefaultCellStyle.BackColor = Color.White;
            loansGrid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 240, 235); // Light orange for alternating rows
            loansGrid.DefaultCellStyle.Font = RegularFont;
            loansGrid.DefaultCellStyle.SelectionBackColor = PrimaryColor;
            loansGrid.DefaultCellStyle.SelectionForeColor = SecondaryColor;

            // Configure grid columns
            loansGrid.Columns.AddRange(new DataGridViewColumn[]
            {
        new DataGridViewTextBoxColumn { Name = "Id", HeaderText = "ID", Width = 20 },
        new DataGridViewTextBoxColumn { Name = "Clan", HeaderText = "Član", Width = 150 },
        new DataGridViewTextBoxColumn { Name = "Knjiga", HeaderText = "Knjiga", Width = 200 },
        new DataGridViewTextBoxColumn { Name = "DatumPosudbe", HeaderText = "Datum posudbe", Width = 120 },
        new DataGridViewTextBoxColumn { Name = "DatumVracanja", HeaderText = "Rok vraćanja", Width = 120 },
        new DataGridViewTextBoxColumn { Name = "Status", HeaderText = "Status", Width = 100 },
        new DataGridViewButtonColumn { Name = "Vrati", HeaderText = "", Width = 80 },
        new DataGridViewButtonColumn { Name = "Produzi", HeaderText = "", Width = 80 }
            });

            // Add pagination panel
            var paginationPanel = new Panel
            {
                Width = mainContentPanel.Width - 60,
                Height = 50,
                Location = new Point(0, loansGrid.Bottom + 10)
            };

            var prevButton = new Button
            {
                Text = "◀ Prethodna",
                Size = new Size(180, 35),
                Location = new Point(0, 7),
                FlatStyle = FlatStyle.Flat,
                Font = RegularFont,
                BackColor = PrimaryColor,
                ForeColor = SecondaryColor,
                Cursor = Cursors.Hand
            };
            prevButton.FlatAppearance.BorderSize = 0;

            var nextButton = new Button
            {
                Text = "Sljedeća ▶",
                Size = new Size(180, 35),
                Location = new Point(paginationPanel.Width - 190, 7),
                FlatStyle = FlatStyle.Flat,
                Font = RegularFont,
                BackColor = PrimaryColor,
                ForeColor = SecondaryColor,
                Cursor = Cursors.Hand
            };
            nextButton.FlatAppearance.BorderSize = 0;

            pageInfoLabel = new Label
            {
                AutoSize = true,
                Font = RegularFont,
                ForeColor = TextColor,
                Location = new Point((paginationPanel.Width - 100) / 2, 15)
            };

            paginationPanel.Controls.AddRange(new Control[] { prevButton, pageInfoLabel, nextButton });

            // Add all controls to the loans panel
            loansPanel.Controls.AddRange(new Control[] { headerPanel, searchPanel, loansGrid, paginationPanel });

            // Add the loans panel to the main content panel
            mainContentPanel.Controls.Add(loansPanel);

            // Add event handlers
            searchBox.TextChanged += (s, e) => FilterLoans(loansGrid, searchBox.Text, filterCombo.SelectedItem.ToString());
            filterCombo.SelectedIndexChanged += (s, e) => FilterLoans(loansGrid, searchBox.Text, filterCombo.SelectedItem.ToString());
            loansGrid.CellClick += (s, e) => HandleLoanGridAction(loansGrid, e);

            // Add placeholder text behavior
            searchBox.GotFocus += (s, e) =>
            {
                if (searchBox.Text == "Pretraži posudbe...")
                {
                    searchBox.Text = "";
                    searchBox.ForeColor = TextColor;
                }
            };

            searchBox.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(searchBox.Text))
                {
                    searchBox.Text = "Pretraži posudbe...";
                    searchBox.ForeColor = Color.Gray;
                }
            };

            // Add pagination event handlers
            prevButton.Click += (s, e) =>
            {
                if (currentPage > 1)
                {
                    currentPage--;
                    FilterLoans(loansGrid, searchBox.Text, filterCombo.SelectedItem.ToString());
                }
            };

            nextButton.Click += (s, e) =>
            {
                var totalItems = GetFilteredLoans(searchBox.Text, filterCombo.SelectedItem.ToString()).Count;
                var totalPages = (int)Math.Ceiling(totalItems / (double)itemsPerPage);
                if (currentPage < totalPages)
                {
                    currentPage++;
                    FilterLoans(loansGrid, searchBox.Text, filterCombo.SelectedItem.ToString());
                }
            };

            // Initial load
            LoadLoansData(loansGrid);
        }

        private void LoadLoansData(DataGridView grid)
        {
            grid.Rows.Clear();
            var loans = PodaciBiblioteke.Posudbe
                .Where(p => !p.Vracena)
                .Skip((currentPage - 1) * itemsPerPage)
                .Take(itemsPerPage);

            foreach (var loan in loans)
            {
                AddLoanToGrid(grid, loan);
            }

            UpdatePaginationControls(PodaciBiblioteke.Posudbe.Count());
        }

        private void AddLoanToGrid(DataGridView grid, Posudba loan)
        {
            var row = grid.Rows[grid.Rows.Add()];
            row.Cells["Id"].Value = loan.Id;
            row.Cells["Clan"].Value = loan.Korisnik.ImePrezime;
            row.Cells["Knjiga"].Value = loan.Knjiga.Naslov;
            row.Cells["DatumPosudbe"].Value = loan.DatumPosudbe.ToString("dd.MM.yyyy");
            row.Cells["DatumVracanja"].Value = loan.DatumVracanja.ToString("dd.MM.yyyy");

            var statusCell = row.Cells["Status"];
            if (loan.DatumVracanja < DateTime.Now)
            {
                statusCell.Value = "⚠️ Kasni";
                statusCell.Style.ForeColor = Color.FromArgb(239, 68, 68); // Red
            }
            else
            {
                statusCell.Value = "✓ Aktivna";
                statusCell.Style.ForeColor = Color.FromArgb(34, 197, 94); // Green
            }

            // Add action buttons
            row.Cells["Vrati"].Value = "Vrati";
            row.Cells["Produzi"].Value = "Produži";
        }

        private List<Posudba> GetFilteredLoans(string searchText, string filter)
        {
            return PodaciBiblioteke.Posudbe
                .Where(p => !p.Vracena)
                .Where(p =>
                    (searchText == "" ||
                     searchText == "Pretraži posudbe..." ||
                     p.Korisnik.ImePrezime.ToLower().Contains(searchText.ToLower()) ||
                     p.Knjiga.Naslov.ToLower().Contains(searchText.ToLower())) &&
                    (filter == "Sve posudbe" ||
                     (filter == "Aktivne" && p.DatumVracanja >= DateTime.Now) ||
                     (filter == "Zakašnjele" && p.DatumVracanja < DateTime.Now)))
                .ToList();
        }

        private void FilterLoans(DataGridView grid, string searchText, string filter)
        {
            grid.Rows.Clear();
            var filteredLoans = GetFilteredLoans(searchText, filter)
                .Where(p => !p.Vracena)
                .Skip((currentPage - 1) * itemsPerPage)
                .Take(itemsPerPage);

            foreach (var loan in filteredLoans)
            {
                AddLoanToGrid(grid, loan);
            }

            UpdatePaginationControls(GetFilteredLoans(searchText, filter).Count);
        }

        private void HandleLoanGridAction(DataGridView grid, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var loanId = (int)grid.Rows[e.RowIndex].Cells["Id"].Value;
                var loan = PodaciBiblioteke.Posudbe.First(p => p.Id == loanId);

                if (e.ColumnIndex == grid.Columns["Vrati"].Index)
                {
                    ShowReturnLoanDialog(loan);
                }
                else if (e.ColumnIndex == grid.Columns["Produzi"].Index)
                {
                    ShowExtendLoanDialog(loan);
                }
            }
        }

        private void ShowExtendLoanDialog(Posudba loan)
        {
            var dialog = new Form
            {
                Text = "Produži rok vraćanja",
                Size = new Size(480, 600),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                BackColor = SecondaryColor
            };

            // Header Panel
            var headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                BackColor = PrimaryColor
            };

            var headerLabel = new Label
            {
                Text = "🔄 Produži rok vraćanja",
                Font = new Font(TitleFont.FontFamily, 20, FontStyle.Bold),
                ForeColor = SecondaryColor,
                AutoSize = true,
                Location = new Point(20, 20)
            };

            headerPanel.Controls.Add(headerLabel);

            // Content Panel
            var contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20)
            };

            int yPos = 100;
            const int spacing = 50;

            // Loan details
            var bookLabel = new Label
            {
                Text = $"Knjiga: {loan.Knjiga.Naslov}",
                Font = RegularFont,
                Location = new Point(20, yPos),
                AutoSize = true
            };

            yPos += spacing;

            var memberLabel = new Label
            {
                Text = $"Član: {loan.Korisnik.ImePrezime}",
                Font = RegularFont,
                Location = new Point(20, yPos),
                AutoSize = true
            };

            yPos += spacing;

            var loanDateLabel = new Label
            {
                Text = $"Datum posudbe: {loan.DatumPosudbe:dd.MM.yyyy}",
                Font = RegularFont,
                Location = new Point(20, yPos),
                AutoSize = true
            };

            yPos += spacing;

            var returnDateLabel = new Label
            {
                Text = $"Trenutni rok vraćanja: {loan.DatumVracanja:dd.MM.yyyy}",
                Font = RegularFont,
                Location = new Point(20, yPos),
                AutoSize = true
            };

            yPos += spacing + 35;

            // Extension days input
            var daysLabel = new Label
            {
                Text = "Broj dana produženja (max 7):",
                Font = RegularFont,
                Location = new Point(20, yPos),
                AutoSize = true
            };

            var daysNumeric = new NumericUpDown
            {
                Location = new Point(daysLabel.Left + 5, yPos + 35),
                Width = 150,
                Minimum = 1,
                Maximum = 7,
                Value = 7,
                Font = RegularFont
            };

            // New return date preview
            var newDateLabel = new Label
            {
                Text = $"Novi rok vraćanja: {loan.DatumVracanja.AddDays(7):dd.MM.yyyy}",
                Font = RegularFont,
                Location = new Point(20, yPos + 70),
                AutoSize = true
            };

            // Update new date preview when days change
            daysNumeric.ValueChanged += (s, e) =>
            {
                newDateLabel.Text = $"Novi rok vraćanja: {loan.DatumVracanja.AddDays((double)daysNumeric.Value):dd.MM.yyyy}";
            };

            yPos += spacing + 90;

            // Buttons
            var saveButton = new Button
            {
                Text = "Produži",
                Size = new Size(150, 40),
                Location = new Point(90, yPos),
                FlatStyle = FlatStyle.Flat,
                Font = RegularFont,
                BackColor = PrimaryColor,
                ForeColor = SecondaryColor,
                Cursor = Cursors.Hand
            };
            saveButton.FlatAppearance.BorderSize = 0;

            var cancelButton = new Button
            {
                Text = "Odustani",
                Size = new Size(150, 40),
                Location = new Point(260, yPos),
                FlatStyle = FlatStyle.Flat,
                Font = RegularFont,
                BackColor = Color.FromArgb(229, 231, 235),
                ForeColor = TextColor,
                Cursor = Cursors.Hand
            };
            cancelButton.FlatAppearance.BorderSize = 0;

            // Button handlers
            saveButton.Click += (s, e) =>
            {
                ExtendLoan(loan, (int)daysNumeric.Value, dialog);
            };

            cancelButton.Click += (s, e) => dialog.Close();

            contentPanel.Controls.AddRange(new Control[] {
                bookLabel, memberLabel, loanDateLabel, returnDateLabel,
                daysLabel, daysNumeric, newDateLabel,
                saveButton, cancelButton
            });

            dialog.Controls.AddRange(new Control[] { headerPanel, contentPanel });

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var loansPanel = mainContentPanel.Controls.OfType<Panel>().FirstOrDefault();
                if (loansPanel != null)
                {
                    var grid = loansPanel.Controls.OfType<DataGridView>().FirstOrDefault();
                    if (grid != null)
                    {
                        LoadLoansData(grid);
                    }
                }
            }
        }

        private void ShowReturnLoanDialog(Posudba loan)
        {
            var dialog = new Form
            {
                Text = "Vraćanje knjige",
                Size = new Size(500, 800),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                BackColor = SecondaryColor
            };

            // Header Panel
            var headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                BackColor = PrimaryColor
            };

            var headerLabel = new Label
            {
                Text = "📚 Vraćanje knjige",
                Font = new Font(TitleFont.FontFamily, 20, FontStyle.Bold),
                ForeColor = SecondaryColor,
                AutoSize = true,
                Location = new Point(20, 20)
            };

            headerPanel.Controls.Add(headerLabel);

            // Content Panel
            var contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(30)
            };

            int yPos = 100;

            // Member Section
            var memberSection = CreateSectionPanel("👤 Informacije o članu", yPos);
            memberSection.Controls.AddRange(new Control[] {
        AddDetailLabel($"Ime i prezime: {loan.Korisnik.ImePrezime}", 50),
        AddDetailLabel($"Email: {loan.Korisnik.Email}", 80),
        AddDetailLabel($"Telefon: {loan.Korisnik.BrojTelefona}", 110)
    });

            yPos += memberSection.Height - 20;

            // Book Section
            var bookSection = CreateSectionPanel("📖 Informacije o knjizi", yPos);
            bookSection.Controls.AddRange(new Control[] {
        AddDetailLabel($"Naslov: {loan.Knjiga.Naslov}", 50),
        AddDetailLabel($"Autor: {loan.Knjiga.Autor.Ime + " " + loan.Knjiga.Autor.Prezime }", 80),
        AddDetailLabel($"Žanr: {loan.Knjiga.Zanr.Naziv}", 110)
    });

            yPos += bookSection.Height - 20;

            // Loan Details Section
            var loanSection = CreateSectionPanel("🔄 Informacije o posudbi", yPos);

            var datePicker = new DateTimePicker
            {
                Location = new Point(170, 110),
                Size = new Size(200, 30),
                Format = DateTimePickerFormat.Short,
                Value = DateTime.Now,
                Font = RegularFont
            };

            loanSection.Controls.AddRange(new Control[] {
        AddDetailLabel($"Datum posudbe: {loan.DatumPosudbe:dd.MM.yyyy}", 50),
        AddDetailLabel($"Rok vraćanja: {loan.DatumVracanja:dd.MM.yyyy}", 80),
        AddDetailLabel("Datum vraćanja:", 110),
        datePicker
    });

            // Add late return warning if applicable
            if (loan.DatumVracanja < DateTime.Now)
            {
                var daysLate = (DateTime.Now - loan.DatumVracanja).Days;
                var lateLabel = new Label
                {
                    Text = $"⚠️ Kasni {daysLate} {(daysLate == 1 ? "dan" : "dana")}",
                    Font = new Font(RegularFont.FontFamily, 12, FontStyle.Bold),
                    ForeColor = Color.FromArgb(239, 68, 68),
                    Location = new Point(20, 150),
                    AutoSize = true
                };
                loanSection.Controls.Add(lateLabel);
            }

            // Buttons
            var buttonPanel = new Panel
            {
                Height = 60,
                Dock = DockStyle.Bottom,
                Padding = new Padding(0, 10, 0, 0)
            };

            var confirmButton = new Button
            {
                Text = "Potvrdi vraćanje",
                Size = new Size(180, 40),
                Location = new Point(50, 10),
                FlatStyle = FlatStyle.Flat,
                Font = RegularFont,
                BackColor = PrimaryColor,
                ForeColor = SecondaryColor,
                Cursor = Cursors.Hand
            };
            confirmButton.FlatAppearance.BorderSize = 0;

            var cancelButton = new Button
            {
                Text = "Odustani",
                Size = new Size(150, 40),
                Location = new Point(250, 10),
                FlatStyle = FlatStyle.Flat,
                Font = RegularFont,
                BackColor = Color.FromArgb(229, 231, 235),
                ForeColor = TextColor,
                Cursor = Cursors.Hand
            };
            cancelButton.FlatAppearance.BorderSize = 0;

            // Button handlers
            confirmButton.Click += (s, e) =>
            {
                ReturnBook(loan, datePicker.Value, dialog);
            };

            cancelButton.Click += (s, e) => dialog.Close();

            buttonPanel.Controls.AddRange(new Control[] { confirmButton, cancelButton });

            contentPanel.Controls.AddRange(new Control[] {
        memberSection,
        bookSection,
        loanSection,
        buttonPanel
    });

            dialog.Controls.AddRange(new Control[] { headerPanel, contentPanel });

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var loansPanel = mainContentPanel.Controls.OfType<Panel>().FirstOrDefault();
                if (loansPanel != null)
                {
                    var grid = loansPanel.Controls.OfType<DataGridView>().FirstOrDefault();
                    if (grid != null)
                    {
                        LoadLoansData(grid);
                    }
                }
            }
        }

        private Label AddDetailLabel(string text, int yPosition)
        {
            return new Label
            {
                Text = text,
                Font = RegularFont,
                ForeColor = TextColor,
                Location = new Point(20, yPosition),
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 10)
            };
        }

        private Panel CreateSectionPanel(string title, int yPosition)
        {
            var panel = new Panel
            {
                Location = new Point(0, yPosition),
                Width = 540,
                Height = 180,
                BackColor = Color.White,
                Padding = new Padding(20)
            };

            var titleLabel = new Label
            {
                Text = title,
                Font = new Font(BoldFont.FontFamily, 12, FontStyle.Bold),
                ForeColor = PrimaryColor,
                Location = new Point(20, 20),
                AutoSize = true
            };

            panel.Controls.Add(titleLabel);
            return panel;
        }

        private void ExtendLoan(Posudba loan, int days, Form dialog)
        {
            try
            {
                loan.DatumVracanja = loan.DatumVracanja.AddDays(days);

                dialog.DialogResult = DialogResult.OK;
                dialog.Close();

                MessageBox.Show(
                    $"Rok vraćanja je uspješno produžen!\n\n" +
                    $"Nova datum vraćanja: {loan.DatumVracanja:dd.MM.yyyy}",
                    "Uspjeh",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Došlo je do greške prilikom produženja roka:\n{ex.Message}",
                    "Greška",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void ReturnBook(Posudba loan, DateTime dt, Form dialog)
        {
            try
            {
                loan.Status = true; // Mark as returned
                loan.Vracena = true;
                loan.Knjiga.Dostupna = true; // Make book available again
                loan.DatumVracanja = dt; // Set actual return date

                dialog.DialogResult = DialogResult.OK;
                dialog.Close();

                // Refresh the loans grid and pagination
                var loansPanel = mainContentPanel.Controls.OfType<Panel>().FirstOrDefault();
                if (loansPanel != null)
                {
                    var grid = loansPanel.Controls.OfType<DataGridView>().FirstOrDefault();
                    if (grid != null)
                    {
                        // Reset to first page when returning a book
                        currentPage = 1;

                        // Get the current search and filter values
                        var searchBox = loansPanel.Controls.OfType<TextBox>()
                            .FirstOrDefault(tb => tb.Text.Contains("Pretraži"));
                        var filterCombo = loansPanel.Controls.OfType<ComboBox>().FirstOrDefault();

                        if (searchBox != null && filterCombo != null)
                        {
                            // Apply current filters when refreshing
                            FilterLoans(grid, searchBox.Text, filterCombo.SelectedItem.ToString());
                        }
                        else
                        {
                            // Fallback to loading all data
                            LoadLoansData(grid);
                        }
                    }
                }

                MessageBox.Show(
                    "Knjiga je uspješno vraćena!",
                    "Uspjeh",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Došlo je do greške prilikom vraćanja knjige:\n{ex.Message}",
                    "Greška",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void ShowAddLoanDialog()
        {
            var memberTitleLabel = new Label
            {
                Text = "👤 Odaberi člana",
                Font = new Font(TitleFont.FontFamily, 16, FontStyle.Bold),
                ForeColor = TextColor,
                Location = new Point(20, 100),
                AutoSize = true
            };

            var bookTitleLabel = new Label
            {
                Text = "📚 Odaberi knjigu",
                Font = new Font(TitleFont.FontFamily, 16, FontStyle.Bold),
                ForeColor = TextColor,
                Location = new Point(20, 100),
                AutoSize = true
            };

            var dialog = new Form
            {
                Text = "Nova posudba",
                Size = new Size(1000, 700),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                BackColor = SecondaryColor
            };

            // Header Panel
            var headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                BackColor = PrimaryColor
            };

            var headerLabel = new Label
            {
                Text = "🔄 Nova posudba",
                Font = new Font(TitleFont.FontFamily, 20, FontStyle.Bold),
                ForeColor = SecondaryColor,
                AutoSize = true,
                Location = new Point(20, 20)
            };
            headerPanel.Controls.Add(headerLabel);

            // Main Content Panel
            var contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20)
            };

            // Member Section (Left Side)
            var memberPanel = new Panel
            {
                Width = 450,
                Height = 500,
                Location = new Point(20, 20),
                BackColor = Color.White,
                Padding = new Padding(20)
            };

            var memberIcon = new Label
            {
                Text = "👤",
                Font = new Font("Segoe UI Emoji", 24),
                Location = new Point(20, 20),
                AutoSize = true
            };

            var memberSearchBox = new TextBox
            {
                Width = 410,
                Height = 35,
                Location = new Point(20, 150),
                Font = RegularFont,
                Text = "Pretraži članove...",
                ForeColor = Color.Gray
            };

            memberSearchBox.GotFocus += (s, e) =>
            {
                if (memberSearchBox.Text == "Pretraži članove...")
                {
                    memberSearchBox.Text = "";
                    memberSearchBox.ForeColor = TextColor;
                }
            };

            memberSearchBox.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(memberSearchBox.Text))
                {
                    memberSearchBox.Text = "Pretraži članove...";
                    memberSearchBox.ForeColor = Color.Gray;
                }
            };

            var memberResultsPanel = new Panel
            {
                Width = 350,
                Height = 200,
                Location = new Point(20, 110),
                BorderStyle = BorderStyle.FixedSingle,
                Visible = false
            };

            var selectedMemberPanel = new Panel
            {
                Width = 350,
                Height = 250,
                Location = new Point(20, 110),
                Visible = false,
                BackColor = Color.FromArgb(255, 247, 237)
            };

            var removeMemberButton = new Button
            {
                Text = "✕",
                Size = new Size(30, 30),
                Location = new Point(380, 200), // Adjust position to be visible
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(255, 150, 150),
                ForeColor = TextColor,
                Cursor = Cursors.Hand,
                Visible = false
            };
            removeMemberButton.FlatAppearance.BorderSize = 0;

            // Book Section (Right Side)
            var bookPanel = new Panel
            {
                Width = 450,
                Height = 500,
                Location = new Point(490, 20),
                BackColor = Color.White,
                Padding = new Padding(20)
            };

            var bookIcon = new Label
            {
                Text = "📚",
                Font = new Font("Segoe UI Emoji", 24),
                Location = new Point(20, 20),
                AutoSize = true
            };

            var bookSearchBox = new TextBox
            {
                Width = 410,
                Height = 35,
                Location = new Point(20, 150),
                Font = RegularFont,
                Text = "Pretraži knjige...",
                ForeColor = Color.Gray
            };

            bookSearchBox.GotFocus += (s, e) =>
            {
                if (bookSearchBox.Text == "Pretraži knjige...")
                {
                    bookSearchBox.Text = "";
                    bookSearchBox.ForeColor = TextColor;
                }
            };

            bookSearchBox.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(bookSearchBox.Text))
                {
                    bookSearchBox.Text = "Pretraži knjige...";
                    bookSearchBox.ForeColor = Color.Gray;
                }
            };

            var bookResultsPanel = new Panel
            {
                Width = 350,
                Height = 250,
                Location = new Point(20, 110),
                BorderStyle = BorderStyle.FixedSingle,
                Visible = false
            };

            var selectedBookPanel = new Panel
            {
                Width = 350,
                Height = 200,
                Location = new Point(20, 110),
                Visible = false,
                BackColor = Color.FromArgb(255, 247, 237)
            };

            var removeBookButton = new Button
            {
                Text = "✕",
                Size = new Size(30, 30),
                Location = new Point(380, 200), // Adjust position to be visible
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(255, 150, 150),
                ForeColor = TextColor,
                Cursor = Cursors.Hand,
                Visible = false
            };
            removeBookButton.FlatAppearance.BorderSize = 0;

            // Loan Details Section (Bottom)
            var loanDetailsPanel = new Panel
            {
                Width = 920,
                Height = 100,
                Location = new Point(20, 500),
                BackColor = Color.White,
                Padding = new Padding(20)
            };

            var loanDateLabel = new Label
            {
                Text = $"Datum posudbe: {DateTime.Now:dd.MM.yyyy}",
                Font = RegularFont,
                Location = new Point(20, 20),
                AutoSize = true
            };

            var returnDateLabel = new Label
            {
                Text = "Rok vraćanja:",
                Font = RegularFont,
                Location = new Point(20, 60),
                AutoSize = true
            };

            var returnDatePicker = new DateTimePicker
            {
                Location = new Point(150, 55),
                Size = new Size(200, 30),
                Format = DateTimePickerFormat.Short,
                Value = DateTime.Now.AddDays(14),
                MinDate = DateTime.Now,
                Font = RegularFont
            };

            // Member Search Results Panel
            var memberSearchResults = new Panel
            {
                Width = 410,
                Location = new Point(20, memberSearchBox.Bottom + 5),
                BackColor = Color.White,
                Visible = false,
                AutoSize = true,
                MaximumSize = new Size(410, 200)
            };
            memberSearchResults.BringToFront();

            // Book Search Results Panel
            var bookSearchResults = new Panel
            {
                Width = 410,
                Location = new Point(20, bookSearchBox.Bottom + 5),
                BackColor = Color.White,
                Visible = false,
                AutoSize = true,
                MaximumSize = new Size(410, 200)
            };
            bookSearchResults.BringToFront();

            // Selected Member Info Panel
            var selectedMemberInfo = new Panel
            {
                Width = 410,
                Height = 280,
                Location = new Point(20, memberSearchBox.Bottom + 10),
                BackColor = Color.FromArgb(255, 247, 237),
                Visible = false,
                Padding = new Padding(15)
            };

            // Selected Book Info Panel
            var selectedBookInfo = new Panel
            {
                Width = 410,
                Height = 280,
                Location = new Point(20, bookSearchBox.Bottom + 10),
                BackColor = Color.FromArgb(255, 247, 237),
                Visible = false,
                Padding = new Padding(15)
            };

            // Member search handler
            Korisnik selectedMember = null;
            memberSearchBox.TextChanged += (s, e) =>
            {
                string searchText = memberSearchBox.Text;
                memberSearchResults.Controls.Clear();

                var searchResults = searchText == "Pretraži članove..." || string.IsNullOrWhiteSpace(searchText)
                    ? PodaciBiblioteke.Korisnici
                        .Where(k => k.ImePrezime != trenutniBibliotekar.ImePrezime)
                        .Take(5)
                        .ToList()
                    : PodaciBiblioteke.Korisnici
                        .Where(k => k.ImePrezime != trenutniBibliotekar.ImePrezime &&
                                   (k.ImePrezime.ToLower().Contains(searchText.ToLower()) ||
                                    k.Email.ToLower().Contains(searchText.ToLower())))
                        .Take(5)
                        .ToList();

                ShowMemberResults(searchResults, memberSearchResults, selectedMemberInfo, removeMemberButton,
                    memberSearchBox, member => selectedMember = member);
            };

            // Book search handler
            Knjiga selectedBook = null;
            bookSearchBox.TextChanged += (s, e) =>
            {
                string searchText = bookSearchBox.Text;
                bookSearchResults.Controls.Clear();

                var searchResults = searchText == "Pretraži knjige..." || string.IsNullOrWhiteSpace(searchText)
                    ? PodaciBiblioteke.Knjige
                        .Take(5)
                        .ToList()
                    : PodaciBiblioteke.Knjige
                        .Where(k => k.Dostupna &&
                                   (k.Naslov.ToLower().Contains(searchText.ToLower()) ||
                                    k.Autor.Ime.ToLower().Contains(searchText.ToLower()) ||
                                    k.Autor.Prezime.ToLower().Contains(searchText.ToLower())))
                        .Take(5)
                        .ToList();

                ShowBookResults(searchResults, bookSearchResults, selectedBookInfo, removeBookButton,
                    bookSearchBox, book => selectedBook = book);
            };

            // Remove selection handlers
            removeMemberButton.Click += (s, e) =>
            {
                selectedMember = null;
                memberSearchBox.Text = "Pretraži članove...";
                memberSearchBox.ForeColor = Color.Gray;
                selectedMemberInfo.Visible = false;
                removeMemberButton.Visible = false;
            };

            removeBookButton.Click += (s, e) =>
            {
                selectedBook = null;
                bookSearchBox.Text = "Pretraži knjige...";
                bookSearchBox.ForeColor = Color.Gray;
                selectedBookInfo.Visible = false;
                removeBookButton.Visible = false;
            };

            // Confirm and Cancel buttons
            var buttonPanel = new Panel
            {
                Height = 60,
                Dock = DockStyle.Bottom,
                Padding = new Padding(0, 10, 0, 0)
            };

            var confirmButton = new Button
            {
                Text = "Potvrdi posudbu",
                Size = new Size(180, 40),
                Location = new Point(480, 20),
                FlatStyle = FlatStyle.Flat,
                Font = RegularFont,
                BackColor = PrimaryColor,
                ForeColor = SecondaryColor,
                Cursor = Cursors.Hand
            };
            confirmButton.FlatAppearance.BorderSize = 0;

            var cancelButton = new Button
            {
                Text = "Odustani",
                Size = new Size(150, 40),
                Location = new Point(680, 20),
                FlatStyle = FlatStyle.Flat,
                Font = RegularFont,
                BackColor = Color.FromArgb(229, 231, 235),
                ForeColor = TextColor,
                Cursor = Cursors.Hand
            };
            cancelButton.FlatAppearance.BorderSize = 0;

            // Add all controls to panels
            loanDetailsPanel.Controls.AddRange(new Control[] {
        loanDateLabel, returnDateLabel, returnDatePicker
    });

            memberPanel.Controls.Clear();
            memberPanel.Controls.AddRange(new Control[] {
    memberTitleLabel,
    memberSearchBox,
    memberSearchResults,
    selectedMemberInfo
});
            // Add remove button last and separately to ensure proper z-order
            memberPanel.Controls.Add(removeMemberButton);
            removeMemberButton.BringToFront();

            // Similarly for the book panel:
            bookPanel.Controls.Clear();
            bookPanel.Controls.AddRange(new Control[] {
    bookTitleLabel,
    bookSearchBox,
    bookSearchResults,
    selectedBookInfo
});
            // Add remove button last and separately to ensure proper z-order
            bookPanel.Controls.Add(removeBookButton);
            removeBookButton.BringToFront();

            confirmButton.Click += (s, e) =>
            {
                CreateNewLoan(selectedMember, selectedBook, returnDatePicker.Value, dialog);
            };
            cancelButton.Click += (s, e) =>
            {
                dialog.Close();
            };

            buttonPanel.Controls.AddRange(new Control[] { confirmButton, cancelButton });

            contentPanel.Controls.AddRange(new Control[] {
        memberPanel, bookPanel, loanDetailsPanel, buttonPanel
    });

            dialog.Controls.AddRange(new Control[] { headerPanel, contentPanel });

            // Show dialog
            dialog.ShowDialog();
        }

        private void ShowMemberResults(List<Korisnik> members, Panel resultsPanel, Panel infoPanel,
    Button removeButton, TextBox searchBox, Action<Korisnik> onSelect)
        {
            resultsPanel.Controls.Clear();
            resultsPanel.Visible = members.Any();
            int yPos = 0;

            foreach (var member in members)
            {
                var resultButton = new Button
                {
                    Text = $"{member.ImePrezime} ({member.Email})",
                    Size = new Size(410, 40),
                    Location = new Point(0, yPos),
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.White,
                    ForeColor = TextColor,
                    TextAlign = ContentAlignment.MiddleLeft,
                    Padding = new Padding(5, 0, 0, 0)
                };
                resultButton.FlatAppearance.BorderSize = 1;

                resultButton.MouseDown += (sender, args) =>
                {
                    onSelect(member);
                    searchBox.Text = member.ImePrezime;
                    resultsPanel.Visible = false;
                    UpdateSelectedMemberInfo(infoPanel, member);
                    infoPanel.Visible = true;
                    removeButton.Visible = true;
                    removeButton.BringToFront(); // Add this line
                };

                resultsPanel.Controls.Add(resultButton);
                yPos += 40;
            }
        }

        private void ShowBookResults(List<Knjiga> books, Panel resultsPanel, Panel infoPanel,
            Button removeButton, TextBox searchBox, Action<Knjiga> onSelect)
        {
            resultsPanel.Controls.Clear();
            resultsPanel.Visible = books.Any();
            int yPos = 0;

            foreach (var book in books)
            {
                var resultButton = new Button
                {
                    Text = $"{book.Naslov} ({book.Autor.Ime} {book.Autor.Prezime})",
                    Size = new Size(410, 40),
                    Location = new Point(0, yPos),
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.White,
                    ForeColor = TextColor,
                    TextAlign = ContentAlignment.MiddleLeft,
                    Padding = new Padding(5, 0, 0, 0)
                };
                resultButton.FlatAppearance.BorderSize = 1;

                resultButton.MouseDown += (sender, args) =>
                {
                    onSelect(book);
                    searchBox.Text = book.Naslov;
                    resultsPanel.Visible = false;
                    UpdateSelectedBookInfo(infoPanel, book);
                    infoPanel.Visible = true;
                    removeButton.Visible = true;
                    removeButton.BringToFront(); // Add this line
                };

                resultsPanel.Controls.Add(resultButton);
                yPos += 40;
            }
        }

        private void UpdateSelectedMemberInfo(Panel infoPanel, Korisnik member)
        {
            infoPanel.Controls.Clear();

            var memberInfo = new TableLayoutPanel
            {
                ColumnCount = 2,
                RowCount = 6,
                Dock = DockStyle.Fill,
                ColumnStyles = {
            new ColumnStyle(SizeType.Percent, 40),
            new ColumnStyle(SizeType.Percent, 60)
        }
            };

            AddInfoRow(memberInfo, "Ime i prezime:", member.ImePrezime, 0);
            AddInfoRow(memberInfo, "Email:", member.Email, 1);
            AddInfoRow(memberInfo, "Telefon:", member.BrojTelefona, 2);
            AddInfoRow(memberInfo, "Članarina važi do:", member.DatumIsteka.ToString(), 3);

            var daysLeft = (member.DatumIsteka - DateTime.Now).Value.Days;
            var daysLeftColor = daysLeft < 7 ? Color.FromArgb(239, 68, 68) :
                               daysLeft < 30 ? Color.FromArgb(234, 179, 8) :
                               Color.FromArgb(34, 197, 94);

            AddInfoRow(memberInfo, "Preostalo dana:",
                $"{daysLeft} {(daysLeft == 1 ? "dan" : "dana")}", 4, daysLeftColor);

            infoPanel.Controls.Add(memberInfo);
        }

        private void UpdateSelectedBookInfo(Panel infoPanel, Knjiga book)
        {
            infoPanel.Controls.Clear();

            var bookInfo = new TableLayoutPanel
            {
                ColumnCount = 2,
                RowCount = 5,
                Dock = DockStyle.Fill,
                ColumnStyles = {
            new ColumnStyle(SizeType.Percent, 40),
            new ColumnStyle(SizeType.Percent, 60)
        }
            };

            AddInfoRow(bookInfo, "Naslov:", book.Naslov, 0);
            AddInfoRow(bookInfo, "Autor:", book.Autor.Ime + " " + book.Autor.Prezime, 1);
            AddInfoRow(bookInfo, "Žanr:", book.Zanr.Naziv, 2);
            AddInfoRow(bookInfo, "Status:", book.Dostupna ? "✓ Dostupna" : "❌ Nedostupna", 4,
                book.Dostupna ? Color.FromArgb(34, 197, 94) : Color.FromArgb(239, 68, 68));

            infoPanel.Controls.Add(bookInfo);
        }

        private void AddInfoRow(TableLayoutPanel table, string label, string value, int rowIndex,
            Color? valueColor = null)
        {
            table.Controls.Add(new Label
            {
                Text = label,
                Font = BoldFont,
                ForeColor = TextColor,
                AutoSize = true,
                Margin = new Padding(0, 5, 10, 5)
            }, 0, rowIndex);

            table.Controls.Add(new Label
            {
                Text = value,
                Font = RegularFont,
                ForeColor = valueColor ?? TextColor,
                AutoSize = true,
                Margin = new Padding(0, 5, 0, 5)
            }, 1, rowIndex);
        }

        // Add this method to handle loan creation
        private void CreateNewLoan(Korisnik member, Knjiga book, DateTime returnDate, Form dialog)
        {
            // Validation checks
            var validationMessage = ValidateNewLoan(member, book, returnDate);
            if (!string.IsNullOrEmpty(validationMessage))
            {
                MessageBox.Show(
                    validationMessage,
                    "Validacija",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            try
            {
                // Create new loan
                var newLoan = new Posudba
                {
                    Id = PodaciBiblioteke.Posudbe.Count + 1,
                    Korisnik = member,
                    Knjiga = book,
                    DatumPosudbe = DateTime.Now,
                    DatumVracanja = returnDate,
                    Status = false,
                    Vracena = false
                };

                // Update book availability
                book.Dostupna = false;

                // Add to database
                PodaciBiblioteke.Posudbe.Add(newLoan);

                dialog.DialogResult = DialogResult.OK;
                dialog.Close();

                MessageBox.Show(
                    "Posudba je uspješno kreirana!",
                    "Uspjeh",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Došlo je do greške prilikom kreiranja posudbe:\n{ex.Message}",
                    "Greška",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private string ValidateNewLoan(Korisnik member, Knjiga book, DateTime returnDate)
        {
            if (member == null)
                return "Molimo odaberite člana.";

            if (book == null)
                return "Molimo odaberite knjigu.";

            if (!book.Dostupna)
                return "Odabrana knjiga nije dostupna za posudbu.";

            if (returnDate <= DateTime.Now)
                return "Datum vraćanja mora biti u budućnosti.";

            if (member.DatumIsteka < DateTime.Now)
                return "Članarina istekla.";

            // Check active loans count
            var activeLoansCount = PodaciBiblioteke.Posudbe
                .Count(p => p.Korisnik.Id == member.Id && !p.Status);
            if (activeLoansCount >= 3)
                return "Član već ima maksimalan broj aktivnih posudbi (3).";

            return string.Empty;
        }

        // Helper class for custom message boxes
        public static class CustomMessageBox
        {
            public static DialogResult Show(string text, string caption, string okText, string cancelText, MessageBoxIcon icon)
            {
                var form = new Form()
                {
                    Text = caption,
                    ClientSize = new Size(300, 170),
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    StartPosition = FormStartPosition.CenterScreen,
                    MinimizeBox = false,
                    MaximizeBox = false
                };

                var message = new Label()
                {
                    Text = text,
                    Size = new Size(280, 80),
                    Location = new Point(20, 20),
                    TextAlign = ContentAlignment.MiddleLeft
                };

                var okButton = new Button()
                {
                    DialogResult = DialogResult.Yes,
                    Name = "okButton",
                    Size = new Size(90, 30),
                    Text = okText,
                    Location = new Point((form.Width / 2) - 100, message.Bottom + 20),
                    BackColor = Color.FromArgb(255, 140, 0),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat
                };
                okButton.FlatAppearance.BorderSize = 0;

                var cancelButton = new Button()
                {
                    DialogResult = DialogResult.No,
                    Name = "cancelButton",
                    Size = new Size(90, 30),
                    Text = cancelText,
                    Location = new Point((form.Width / 2) + 0, message.Bottom + 20),
                    BackColor = Color.FromArgb(51, 51, 51),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat
                };
                cancelButton.FlatAppearance.BorderSize = 0;

                var iconBox = new PictureBox
                {
                    Size = new Size(32, 32),
                    Location = new Point(20, 20),
                    SizeMode = PictureBoxSizeMode.StretchImage
                };

                // Set the icon image based on MessageBoxIcon
                switch (icon)
                {
                    case MessageBoxIcon.Information:
                        iconBox.Image = SystemIcons.Information.ToBitmap();
                        break;
                    case MessageBoxIcon.Warning:
                        iconBox.Image = SystemIcons.Warning.ToBitmap();
                        break;
                    case MessageBoxIcon.Error:
                        iconBox.Image = SystemIcons.Error.ToBitmap();
                        break;
                    case MessageBoxIcon.Question:
                        iconBox.Image = SystemIcons.Question.ToBitmap();
                        break;
                    default:
                        iconBox.Image = null;
                        break;
                }

                message.Location = new Point(65, 20);
                message.Size = new Size(235, 80);

                form.Controls.AddRange(new Control[] { iconBox, message, okButton, cancelButton });
                form.AcceptButton = okButton;
                form.CancelButton = cancelButton;

                return form.ShowDialog();
            }
        }
    }
}