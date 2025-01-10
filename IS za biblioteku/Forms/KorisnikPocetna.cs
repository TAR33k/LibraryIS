using IS_za_biblioteku.Entities;
using Microsoft.SqlServer.Server;
using System;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IS_za_biblioteku.Forms
{
    public partial class KorisnikPocetna : Form
    {
        private readonly string korisnickoIme;
        private Korisnik trenutniKorisnik;
        private Panel mainContentPanel;
        private Panel topPanel;
        private readonly PrivateFontCollection privateFontCollection;
        private readonly Font TitleFont;
        private readonly Font RegularFont;
        private readonly Font BoldFont;

        private readonly Color PrimaryColor = Color.FromArgb(255, 140, 0);    // Orange
        private readonly Color SecondaryColor = Color.White;
        private readonly Color AccentColor = Color.FromArgb(240, 240, 240);   // Light Gray
        private readonly Color TextColor = Color.FromArgb(51, 51, 51);        // Dark Gray
        private readonly Color HoverColor = Color.FromArgb(255, 160, 20);     // Lighter Orange

        public KorisnikPocetna(string korisnickoIme)
        {
            InitializeComponent();

            // Initialize fonts
            privateFontCollection = new PrivateFontCollection();
            try
            {
                string fontPath = Path.Combine(Application.StartupPath, "Resources");

                // Load Regular font
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

                // Initialize font objects
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
                // Fallback fonts
                TitleFont = new Font("Segoe UI", 24, FontStyle.Bold);
                RegularFont = new Font("Segoe UI", 12, FontStyle.Regular);
                BoldFont = new Font("Segoe UI", 10, FontStyle.Bold);
            }

            this.korisnickoIme = korisnickoIme;
            this.Size = new Size(1200, 820);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Biblioteka";
            PodaciBiblioteke.PopuniPodatke();
            trenutniKorisnik = PodaciBiblioteke.Korisnici[0];

            InitializeLayout();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            // Clean up fonts
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
            // Top Panel with fixed height and padding
            topPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 70,
                BackColor = PrimaryColor,
                Padding = new Padding(20, 0, 20, 0)
            };

            // Title with logo/icon
            var titleContainer = new Panel
            {
                AutoSize = true,
                Height = 70,
                BackColor = Color.Transparent,
                Dock = DockStyle.Left
            };

            // Create and configure the icon
            var iconPictureBox = new PictureBox
            {
                Size = new Size(48, 48),
                Location = new Point(0, 10),
                SizeMode = PictureBoxSizeMode.StretchImage,
                BackColor = Color.Transparent
            };

            try
            {
                // Load the library icon
                iconPictureBox.Image = Image.FromFile("Resources/library-icon.png");
            }
            catch
            {
                // Create a default icon if the image file is not found
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
                Location = new Point(47, 12) // Positioned right after the icon
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
                Text = trenutniKorisnik.Ime + " " + trenutniKorisnik.Prezime,
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

            // Remove border and add hover effects
            logoutButton.FlatAppearance.BorderSize = 0;
            logoutButton.Click += (s, e) => OdjaviSe();
            logoutButton.MouseEnter += (s, e) => {
                logoutButton.BackColor = AccentColor;
                logoutButton.ForeColor = PrimaryColor;
            };
            logoutButton.MouseLeave += (s, e) => {
                logoutButton.BackColor = SecondaryColor;
                logoutButton.ForeColor = PrimaryColor;
            };

            userContainer.Controls.AddRange(new Control[] { userLabel, logoutButton });

            // Add containers to top panel
            topPanel.Controls.AddRange(new Control[] { titleContainer, userContainer });
            this.Controls.Add(topPanel);

            // Main Content Panel
            mainContentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20),
                BackColor = SecondaryColor,
            };
            this.Controls.Add(mainContentPanel);

            InitializeMenu();
            ShowDashboard();
        }

        private void InitializeMenu()
        {
            var menuPanel = new Panel
            {
                Dock = DockStyle.Left,
                Width = 250,
                BackColor = TextColor,
                Padding = new Padding(0)
            };

            // Add spacer panel that matches the top panel height
            var spacerPanel = new Panel
            {
                Height = topPanel.Height,
                Width = 250,
                BackColor = TextColor,
                Dock = DockStyle.Top
            };
            menuPanel.Controls.Add(spacerPanel);

            // Define menu items with their icons
            var menuItems = new[]
            {
        ("Početna", "pocetna", "home.png", "🏠"),
        ("Pretraga knjiga", "pretraga", "search.png", "🔍"),
        ("Moje posudbe", "posudbe", "books.png", "📚"),
        ("Moj profil", "profil", "profile.png", "👤")
    };

            Panel firstButtonPanel = null;
            Button firstButton = null;
            int buttonY = spacerPanel.Height;
            foreach (var (text, tag, iconFile, fallbackIcon) in menuItems)
            {
                // Rest of your menu item creation code remains the same
                var buttonPanel = new Panel
                {
                    Size = new Size(250, 60),
                    Location = new Point(0, buttonY),
                    BackColor = Color.Transparent,
                    Cursor = Cursors.Hand
                };

                var iconLabel = new Label
                {
                    AutoSize = false,
                    Size = new Size(30, 30), // Larger icon size
                    Location = new Point(25, 15), // Adjusted position
                    Text = fallbackIcon,
                    Font = new Font("Segoe UI Emoji", 14), // Larger font for emoji
                    ForeColor = SecondaryColor,
                    TextAlign = ContentAlignment.MiddleCenter
                };

                try
                {
                    string iconPath = Path.Combine(Application.StartupPath, "Resources", iconFile);
                    if (File.Exists(iconPath))
                    {
                        var icon = new PictureBox
                        {
                            Size = new Size(30, 30), // Larger icon size
                            Location = new Point(25, 15), // Adjusted position
                            SizeMode = PictureBoxSizeMode.StretchImage,
                            Image = Image.FromFile(iconPath),
                            BackColor = Color.Transparent
                        };
                        buttonPanel.Controls.Add(icon);
                    }
                    else
                    {
                        buttonPanel.Controls.Add(iconLabel);
                    }
                }
                catch
                {
                    buttonPanel.Controls.Add(iconLabel);
                }

                var button = new Button
                {
                    Text = text,
                    Tag = tag,
                    Size = new Size(185, 60), // Full height of panel
                    Location = new Point(65, 0), // Adjusted position
                    FlatStyle = FlatStyle.Flat,
                    ForeColor = SecondaryColor,
                    Font = RegularFont, // Using the custom font
                    TextAlign = ContentAlignment.MiddleLeft,
                    BackColor = Color.Transparent,
                    Cursor = Cursors.Hand
                };

                button.FlatAppearance.BorderSize = 0;
                button.FlatAppearance.MouseOverBackColor = Color.Transparent; // Remove default hover
                button.Click += MenuItem_Click;

                buttonPanel.Controls.Add(button);
                menuPanel.Controls.Add(buttonPanel);

                if (tag == "pocetna")
                {
                    firstButtonPanel = buttonPanel;
                    firstButton = button;
                }

                buttonY += 60;

                // Hover effect for the entire panel
                EventHandler mouseEnter = (s, e) => {
                    if (button.BackColor != PrimaryColor) // If not selected
                    {
                        buttonPanel.BackColor = HoverColor;
                        button.BackColor = HoverColor;
                        foreach (Control c in buttonPanel.Controls)
                        {
                            if (c is PictureBox || c is Label)
                                c.BackColor = HoverColor;
                        }
                    }
                };

                EventHandler mouseLeave = (s, e) => {
                    if (button.BackColor != PrimaryColor) // If not selected
                    {
                        buttonPanel.BackColor = Color.Transparent;
                        button.BackColor = Color.Transparent;
                        foreach (Control c in buttonPanel.Controls)
                        {
                            if (c is PictureBox || c is Label)
                                c.BackColor = Color.Transparent;
                        }
                    }
                };

                // Apply hover events to both panel and button
                buttonPanel.MouseEnter += mouseEnter;
                buttonPanel.MouseLeave += mouseLeave;
                button.MouseEnter += mouseEnter;
                button.MouseLeave += mouseLeave;
            }

            this.Controls.Add(menuPanel);
            mainContentPanel.Location = new Point(menuPanel.Width, topPanel.Height);
            mainContentPanel.Size = new Size(
                this.ClientSize.Width - menuPanel.Width,
                this.ClientSize.Height - topPanel.Height
            );

            // Apply selected styling to home tab
            if (firstButtonPanel != null && firstButton != null)
            {
                firstButtonPanel.BackColor = PrimaryColor;
                firstButton.BackColor = PrimaryColor;
                firstButton.ForeColor = SecondaryColor;
                foreach (Control c in firstButtonPanel.Controls)
                {
                    if (c is PictureBox || c is Label)
                        c.BackColor = PrimaryColor;
                }
                ShowDashboard();
            }
        }

        private void MenuItem_Click(object sender, EventArgs e)
        {
            Button clickedButton;
            Panel buttonPanel;

            if (sender is Button btn)
            {
                clickedButton = btn;
                buttonPanel = btn.Parent as Panel;
            }
            else if (sender is Panel pnl)
            {
                buttonPanel = pnl;
                clickedButton = pnl.Controls.OfType<Button>().FirstOrDefault();
            }
            else return;

            if (buttonPanel?.Parent is Panel menuPanel)
            {
                // Reset all button panels
                foreach (Control ctrl in menuPanel.Controls)
                {
                    if (ctrl is Panel panel)
                    {
                        panel.BackColor = Color.Transparent;
                        foreach (Control c in panel.Controls)
                        {
                            c.BackColor = Color.Transparent;
                            if (c is Button b)
                                b.ForeColor = SecondaryColor;
                        }
                    }
                }

                // Highlight selected button
                buttonPanel.BackColor = PrimaryColor;
                clickedButton.BackColor = PrimaryColor;
                clickedButton.ForeColor = SecondaryColor;
                foreach (Control c in buttonPanel.Controls)
                {
                    if (c is PictureBox || c is Label)
                        c.BackColor = PrimaryColor;
                }

                mainContentPanel.Controls.Clear();

                switch (clickedButton.Tag.ToString())
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
                AutoScroll = false,
                Padding = new Padding(30),
                BackColor = SecondaryColor
            };

            // Welcome section with icon
            var welcomeSection = new Panel
            {
                AutoSize = true,
                Dock = DockStyle.Top,
                Padding = new Padding(0, 0, 0, 30),
            };

            var welcomeLabel = new Label
            {
                Text = $"Dobrodošli, {trenutniKorisnik.Ime}!",
                Font = TitleFont,
                ForeColor = TextColor,
                AutoSize = true,
                Location = new Point(0, 65)
            };

            // Membership card with modern design
            var membershipPanel = new Panel
            {
                Size = new Size(400, 120),
                Location = new Point(0, welcomeLabel.Bottom + 35),
                BackColor = trenutniKorisnik.Aktivni ? Color.FromArgb(240, 249, 255) : Color.FromArgb(255, 245, 245)
            };

            // Add shadow effect to membership panel
            membershipPanel.Paint += (s, e) =>
            {
                var shadowColor = Color.FromArgb(20, 0, 0, 0);
                using (var brush = new SolidBrush(shadowColor))
                {
                    e.Graphics.FillRectangle(brush, new Rectangle(3, 3, membershipPanel.Width - 3, membershipPanel.Height - 3));
                }
            };

            var statusIcon = new Label
            {
                Text = trenutniKorisnik.Aktivni ? "✓" : "!",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = trenutniKorisnik.Aktivni ? Color.FromArgb(34, 197, 94) : Color.FromArgb(239, 68, 68),
                Location = new Point(20, 47),
                AutoSize = true,
                BackColor = Color.Transparent
            };

            var membershipTitle = new Label
            {
                Text = "Status članarine",
                Font = BoldFont,
                ForeColor = TextColor,
                Location = new Point(60, 20),
                AutoSize = true,
                BackColor = Color.Transparent // Make background transparent
            };

            var membershipStatus = new Label
            {
                Text = trenutniKorisnik.Aktivni ? "Aktivna" : "Neaktivna",
                Font = RegularFont,
                ForeColor = trenutniKorisnik.Aktivni ? Color.FromArgb(34, 197, 94) : Color.FromArgb(239, 68, 68),
                Location = new Point(60, membershipTitle.Bottom + 8), // Increased spacing
                AutoSize = true,
                BackColor = Color.Transparent
            };

            var membershipType = new Label
            {
                Text = trenutniKorisnik.Clanarina.Naziv,
                Font = RegularFont,
                ForeColor = TextColor,
                Location = new Point(60, membershipStatus.Bottom + 8), // Increased spacing
                AutoSize = true,
                BackColor = Color.Transparent
            };

            var expiryDate = new Label
            {
                Text = $"Ističe: {trenutniKorisnik.DatumIsteka:dd.MM.yyyy}",
                Font = RegularFont,
                ForeColor = Color.FromArgb(107, 114, 128),
                Location = new Point(membershipPanel.Width - 180, 20), // Adjusted position
                AutoSize = true,
                BackColor = Color.Transparent
            };

            // Update the membership panel size to better fit the content
            membershipPanel.Size = new Size(400, membershipType.Bottom + 20);

            membershipPanel.Controls.AddRange(new Control[]
            {
        statusIcon,
        membershipTitle,
        membershipStatus,
        membershipType,
        expiryDate
            });

            // Current books section with divider
            var divider = new Panel
            {
                Height = 1,
                BackColor = Color.FromArgb(229, 231, 235), // Light gray divider
                Dock = DockStyle.Top,
                Margin = new Padding(0, 40, 0, 40)
            };

            var borrowedBooksLabel = new Label
            {
                Text = "Trenutno posuđene knjige",
                Font = new Font(TitleFont.FontFamily, 18, FontStyle.Bold),
                ForeColor = TextColor,
                AutoSize = true,
                Location = new Point(0, membershipPanel.Bottom + 40)
            };

            var borrowedBooksPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                AutoSize = true,
                Location = new Point(0, borrowedBooksLabel.Bottom + 20),
                Width = mainContentPanel.Width - 60,
                WrapContents = false
            };

            // No books message
            if (!trenutniKorisnik.PosudjeneKnjige.Any())
            {
                var noBooksPanel = new Panel
                {
                    Size = new Size(borrowedBooksPanel.Width, 100),
                    BackColor = AccentColor,
                    Margin = new Padding(0, 10, 0, 10)
                };

                var noBooksLabel = new Label
                {
                    Text = "Trenutno nemate posuđenih knjiga",
                    Font = RegularFont,
                    ForeColor = Color.FromArgb(107, 114, 128),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Fill,
                    BackColor = Color.Transparent
                };

                noBooksPanel.Controls.Add(noBooksLabel);
                borrowedBooksPanel.Controls.Add(noBooksPanel);
            }
            else
            {
                foreach (var knjiga in trenutniKorisnik.PosudjeneKnjige)
                {
                    borrowedBooksPanel.Controls.Add(CreateBorrowedBookPanel(knjiga));
                }
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
                Location = new Point(0, borrowedBooksPanel.Bottom + 20),
                Width = mainContentPanel.Width - 60
            };

            // Section title with subtitle
            var popularBooksHeader = new Panel
            {
                AutoSize = true,
                Width = popularBooksSection.Width,
                Padding = new Padding(0, 0, 0, 0)
            };

            var popularBooksLabel = new Label
            {
                Text = "Popularne knjige",
                Font = new Font(TitleFont.FontFamily, 18, FontStyle.Bold),
                ForeColor = TextColor,
                AutoSize = true
            };

            popularBooksHeader.Controls.AddRange(new Control[] { popularBooksLabel });

            // Carousel container
            var carouselPanel = new Panel
            {
                AutoScroll = true,
                Width = mainContentPanel.Width - 60,
                Height = 250,
                Location = new Point(0, popularBooksLabel.Bottom),
                BackColor = SecondaryColor,
                Padding = new Padding(40, 0, 40, 0)
            };

            var booksContainer = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.LeftToRight,
                AutoSize = true,
                Height = 200,
                WrapContents = false,
                Padding = new Padding(0),
                BackColor = Color.Transparent
            };

            // Get popular books (random for now)
            var popularBooks = PodaciBiblioteke.Knjige
                .OrderByDescending(k => new Random().Next())
                .Take(8);

            // Add books to container
            foreach (var knjiga in popularBooks)
            {
                booksContainer.Controls.Add(CreatePopularBookPanel(knjiga));
            }

            // Scroll buttons with modern design
            var leftButton = new Button
            {
                Text = "❮",
                Size = new Size(40, 40),
                Location = new Point(15, (carouselPanel.Height - 35) / 2),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(240, 240, 240),
                ForeColor = TextColor,
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                Cursor = Cursors.Hand
            };

            var rightButton = new Button
            {
                Text = "❯",
                Size = new Size(40, 40),
                Location = new Point(carouselPanel.Width - 40, (carouselPanel.Height - 35) / 2),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(240, 240, 240),
                ForeColor = TextColor,
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                Cursor = Cursors.Hand
            };

            // Button styling
            foreach (var button in new[] { leftButton, rightButton })
            {
                button.FlatAppearance.BorderSize = 0;
                button.FlatAppearance.MouseOverBackColor = Color.FromArgb(220, 220, 220);
            }

            // Scroll handlers with smooth animation
            leftButton.Click += (s, e) =>
            {
                var targetValue = Math.Max(0, carouselPanel.HorizontalScroll.Value - 600);
                SmoothScroll(carouselPanel, targetValue);
            };

            rightButton.Click += (s, e) =>
            {
                var targetValue = Math.Min(
                    carouselPanel.HorizontalScroll.Maximum,
                    carouselPanel.HorizontalScroll.Value + 600);
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

        private Panel CreateBorrowedBookPanel(Knjiga knjiga)
        {
            var panel = new Panel
            {
                Width = mainContentPanel.Width - 80,
                Height = 120,
                Margin = new Padding(0, 0, 0, 15),
                BackColor = Color.White,
                Padding = new Padding(25)
            };

            // Add shadow and rounded corners
            panel.Paint += (s, e) =>
            {
                var graphics = e.Graphics;
                var shadowColor = Color.FromArgb(15, 0, 0, 0);
                var roundedRect = new Rectangle(0, 0, panel.Width - 4, panel.Height - 4);

                using (var shadowBrush = new SolidBrush(shadowColor))
                {
                    graphics.FillRectangle(shadowBrush, new Rectangle(4, 4, panel.Width - 4, panel.Height - 4));
                }
            };

            // Book icon or placeholder
            var bookIcon = new Label
            {
                Text = "📚",
                Font = new Font("Segoe UI Emoji", 24),
                Size = new Size(40, 40),
                Location = new Point(20, 20),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };

            // Book details container
            var detailsPanel = new Panel
            {
                Location = new Point(80, 15),
                Size = new Size(panel.Width - 300, panel.Height - 30),
                BackColor = Color.Transparent
            };

            var titleLabel = new Label
            {
                Text = knjiga.Naslov,
                Font = BoldFont,
                ForeColor = TextColor,
                AutoSize = true,
                BackColor = Color.Transparent
            };

            var authorLabel = new Label
            {
                Text = $"{knjiga.Autor.Ime} {knjiga.Autor.Prezime}",
                Font = RegularFont,
                ForeColor = Color.FromArgb(107, 114, 128),
                Location = new Point(0, titleLabel.Bottom + 5),
                AutoSize = true,
                BackColor = Color.Transparent
            };

            // Borrow details container
            var borrowDetailsPanel = new Panel
            {
                Location = new Point(panel.Width - 210, 15),
                Size = new Size(300, panel.Height - 30),
                BackColor = Color.Transparent
            };

            var datumPosudbe = DateTime.Now.AddDays(-14); // Example date, replace with actual
            var datumVracanja = DateTime.Now.AddDays(14); // Example date, replace with actual

            var borrowDateLabel = new Label
            {
                Text = $"Posuđeno: {datumPosudbe:dd.MM.yyyy}",
                Font = RegularFont,
                ForeColor = Color.FromArgb(107, 114, 128),
                AutoSize = true,
                BackColor = Color.Transparent,
                Location = new Point(26, 0)
            };

            var returnDateLabel = new Label
            {
                Text = $"Rok vraćanja: {datumVracanja:dd.MM.yyyy}",
                Font = RegularFont,
                ForeColor = datumVracanja < DateTime.Now ? Color.FromArgb(239, 68, 68) : Color.FromArgb(34, 197, 94),
                Location = new Point(0, borrowDateLabel.Bottom + 5),
                AutoSize = true,
                BackColor = Color.Transparent
            };

            // Add controls to their containers
            detailsPanel.Controls.AddRange(new Control[] { titleLabel, authorLabel });
            borrowDetailsPanel.Controls.AddRange(new Control[] { borrowDateLabel, returnDateLabel });
            panel.Controls.AddRange(new Control[] { bookIcon, detailsPanel, borrowDetailsPanel });

            return panel;
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
            // Main container with modern styling
            var searchPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(30),
                BackColor = SecondaryColor
            };

            // Header section with improved styling
            var titleLabel = new Label
            {
                Text = "Pretraga knjiga",
                Font = TitleFont,
                ForeColor = TextColor,
                AutoSize = true,
                Location = new Point(0, 65)
            };

            // Modern search controls container
            var searchContainer = new Panel
            {
                Height = 70,
                Width = mainContentPanel.Width - 60,
                Location = new Point(0, titleLabel.Bottom + 35),
                BackColor = SecondaryColor
            };

            // Create a panel to host the search box for consistent height
            var searchBoxContainer = new Panel
            {
                Width = 320,
                Height = 31,
                Location = new Point(10, 12),
                BackColor = AccentColor
            };

            // Styled search box with placeholder
            var searchBox = new TextBox
            {
                Width = searchBoxContainer.Width - 20, // Account for padding
                Font = RegularFont,
                Location = new Point(10, 3), // Center vertically
                BorderStyle = BorderStyle.None,
                BackColor = AccentColor,
                ForeColor = TextColor
            };

            // Add search box to its container
            searchBoxContainer.Controls.Add(searchBox);

            // Modern genre dropdown
            var genreCombo = new ComboBox
            {
                Width = 200,
                Height = 35,
                Location = new Point(searchBoxContainer.Right + 20, 12),
                Font = RegularFont,
                DropDownStyle = ComboBoxStyle.DropDownList,
                FlatStyle = FlatStyle.Flat,
                BackColor = AccentColor,
                ForeColor = TextColor
            };

            // Remove blue selection highlight and improve text rendering
            genreCombo.DrawMode = DrawMode.OwnerDrawFixed;
            genreCombo.DrawItem += (sender, e) =>
            {
                if (e.Index < 0) return;

                ComboBox combo = (ComboBox)sender;
                bool isSelected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;

                // Enable high quality text rendering
                e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

                // Fill background
                using (SolidBrush brush = new SolidBrush(isSelected ? PrimaryColor : AccentColor))
                {
                    e.Graphics.FillRectangle(brush, e.Bounds);
                }

                // Calculate vertical center for text
                int textHeight = (int)e.Graphics.MeasureString("Tg", e.Font).Height;
                int textY = e.Bounds.Top + (e.Bounds.Height - textHeight) / 2;

                // Draw text
                using (SolidBrush brush = new SolidBrush(TextColor))
                {
                    e.Graphics.DrawString(
                        combo.Items[e.Index].ToString(),
                        e.Font,
                        brush,
                        e.Bounds.X + 10,
                        textY,
                        StringFormat.GenericDefault
                    );
                }

                e.DrawFocusRectangle();
            };

            genreCombo.Items.Add("Svi žanrovi");
            genreCombo.Items.AddRange(PodaciBiblioteke.Zanrovi.Select(z => z.Naziv).ToArray());
            genreCombo.SelectedIndex = 0;

            // Styled checkbox
            var availableOnly = new CheckBox
            {
                Text = "Samo dostupne knjige",
                Location = new Point(genreCombo.Right + 20, 12), // Adjusted Y position
                AutoSize = true,
                Font = RegularFont,
                Cursor = Cursors.Hand,
                ForeColor = TextColor,
                TextAlign = ContentAlignment.TopLeft,
                Padding = new Padding(0), // Reset padding
                UseVisualStyleBackColor = true // Ensures consistent rendering
            };

            // Results container with modern styling
            var resultsContainer = new Panel
            {
                Location = new Point(0, searchContainer.Bottom + 20),
                Width = mainContentPanel.Width - 60,
                Height = mainContentPanel.Height - searchContainer.Bottom - 100,
                BackColor = SecondaryColor
            };

            // Results flow panel with improved spacing
            var resultsFlowPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                BackColor = SecondaryColor
            };

            void PerformFilteredSearch()
            {
                resultsFlowPanel.SuspendLayout(); // Add this for better performance
                resultsFlowPanel.Controls.Clear();
                var searchTerm = searchBox.Text.ToLower();
                var selectedGenre = genreCombo.SelectedItem?.ToString();

                // Start with all books
                var query = PodaciBiblioteke.Knjige.AsQueryable();

                // Apply search term filter
                if (!string.IsNullOrEmpty(searchTerm) && !searchTerm.Equals("pretraži po naslovu, autoru ili žanru...", StringComparison.OrdinalIgnoreCase))
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

                // Results header
                var headerPanel = new Panel
                {
                    Width = resultsFlowPanel.Width - 25,
                    Height = 50,
                    Margin = new Padding(0, 0, 0, 20),
                    BackColor = SecondaryColor
                };

                var resultsCountLabel = new Label
                {
                    Text = $"Pronađeno {results.Count} knjiga",
                    Font = BoldFont,
                    ForeColor = TextColor,
                    AutoSize = true,
                    Location = new Point(0, 15)
                };

                headerPanel.Controls.Add(resultsCountLabel);
                resultsFlowPanel.Controls.Add(headerPanel);

                if (!results.Any())
                {
                    var noResultsPanel = new Panel
                    {
                        Width = resultsFlowPanel.Width - 25,
                        Height = 100,
                        BackColor = AccentColor,
                        Margin = new Padding(0, 10, 0, 10)
                    };

                    var noResultsLabel = new Label
                    {
                        Text = "Nema pronađenih knjiga prema zadatim kriterijima.",
                        Font = RegularFont,
                        ForeColor = TextColor,
                        TextAlign = ContentAlignment.MiddleCenter,
                        Dock = DockStyle.Fill
                    };

                    noResultsPanel.Controls.Add(noResultsLabel);
                    resultsFlowPanel.Controls.Add(noResultsPanel);
                }
                else
                {
                    foreach (var knjiga in results)
                    {
                        resultsFlowPanel.Controls.Add(CreateSearchResultPanel(knjiga));
                    }
                }

                resultsFlowPanel.ResumeLayout();
            }

            // Add placeholder text
            searchBox.Enter += (s, e) =>
            {
                if (searchBox.Text == "Pretraži po naslovu, autoru ili žanru...")
                {
                    searchBox.Text = "";
                    searchBox.ForeColor = TextColor;
                }
            };

            searchBox.Leave += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(searchBox.Text))
                {
                    searchBox.Text = "Pretraži po naslovu, autoru ili žanru...";
                    searchBox.ForeColor = Color.Gray;
                }
            };

            // Initial placeholder text
            searchBox.Text = "Pretraži po naslovu, autoru ili žanru...";
            searchBox.ForeColor = Color.Gray;

            // Wire up search events
            searchBox.TextChanged += (s, e) => PerformFilteredSearch();
            genreCombo.SelectedIndexChanged += (s, e) => PerformFilteredSearch();
            availableOnly.CheckedChanged += (s, e) => PerformFilteredSearch();

            // Layout hierarchy
            searchContainer.Controls.AddRange(new Control[] { searchBoxContainer, genreCombo, availableOnly });
            resultsContainer.Controls.Add(resultsFlowPanel);
            searchPanel.Controls.AddRange(new Control[] { titleLabel, searchContainer, resultsContainer });

            // Perform initial search
            PerformFilteredSearch();

            mainContentPanel.Controls.Add(searchPanel);
        }

        private Panel CreateSearchResultPanel(Knjiga knjiga)
        {
            var panel = new Panel
            {
                Width = 840,
                Height = 145,
                Margin = new Padding(0, 0, 0, 15),
                BackColor = Color.White,
                Padding = new Padding(20),
                Cursor = Cursors.Hand
            };

            // Add shadow effect
            panel.Paint += (s, e) =>
            {
                var graphics = e.Graphics;
                var shadowColor = Color.FromArgb(20, 0, 0, 0);
                graphics.FillRectangle(new SolidBrush(shadowColor),
                    new Rectangle(4, 4, panel.Width - 4, panel.Height - 4));
            };

            // Book icon
            var bookIcon = new Label
            {
                Text = "📚",
                Font = new Font("Segoe UI Emoji", 24),
                Size = new Size(40, 40),
                Location = new Point(20, 20),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };

            // Title with larger, bold font
            var titleLabel = new Label
            {
                Text = knjiga.Naslov,
                Font = new Font(TitleFont.FontFamily, 16, FontStyle.Bold),
                ForeColor = TextColor,
                Location = new Point(76, 20),
                AutoSize = true,
                BackColor = Color.Transparent
            };

            // Author with regular font
            var authorLabel = new Label
            {
                Text = $"{knjiga.Autor.Ime} {knjiga.Autor.Prezime}",
                Font = RegularFont,
                ForeColor = Color.FromArgb(107, 114, 128),
                Location = new Point(80, titleLabel.Bottom + 12),
                AutoSize = true,
                BackColor = Color.Transparent
            };

            // Genre with styled background
            var genreLabel = new Label
            {
                Text = knjiga.Zanr.Naziv,
                Font = RegularFont,
                ForeColor = PrimaryColor,
                Location = new Point(80, authorLabel.Bottom + 8),
                AutoSize = true,
                Padding = new Padding(8, 4, 8, 4),
                BackColor = Color.FromArgb(255, 240, 230)
            };

            // Status indicator with icon
            var statusIcon = new Label
            {
                Text = knjiga.Dostupna ? "✓" : "×",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = knjiga.Dostupna ? Color.FromArgb(34, 197, 94) : Color.FromArgb(239, 68, 68),
                Location = new Point(panel.Width - 160, 20),
                AutoSize = true,
                BackColor = Color.Transparent
            };

            var statusLabel = new Label
            {
                Text = knjiga.Dostupna ? "DOSTUPNO" : "NIJE DOSTUPNO",
                Font = BoldFont,
                ForeColor = knjiga.Dostupna ? Color.FromArgb(34, 197, 94) : Color.FromArgb(239, 68, 68),
                Location = new Point(panel.Width - 130, 22),
                AutoSize = true,
                BackColor = Color.Transparent
            };

            // Available copies with subtle color
            var availabilityLabel = new Label
            {
                Text = $"Dostupno primjeraka: {knjiga.DostupnaKolicina}",
                Font = RegularFont,
                ForeColor = Color.FromArgb(107, 114, 128),
                Location = new Point(panel.Width - 220, statusLabel.Bottom + 10),
                AutoSize = true,
                BackColor = Color.Transparent
            };

            // Make sure all labels (except genreLabel) have transparent background initially
            foreach (Control control in panel.Controls)
            {
                if (control != genreLabel)
                {
                    control.BackColor = Color.Transparent;
                }
            }

            panel.Controls.AddRange(new Control[]
            {
        bookIcon,
        titleLabel,
        authorLabel,
        genreLabel,
        statusIcon,
        statusLabel,
        availabilityLabel
            });

            // Click handler
            EventHandler clickHandler = (s, e) =>
            {
                if (!trenutniKorisnik.Aktivni)
                {
                    MessageBox.Show("Vaša članarina nije aktivna. Molimo obnovite članarinu kako biste mogli rezervisati knjige.",
                                  "Neaktivna članarina",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Warning);
                    return;
                }

                if (!knjiga.Dostupna)
                {
                    MessageBox.Show("Knjiga trenutno nije dostupna za rezervaciju.",
                                  "Knjiga nedostupna",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Information);
                    return;
                }

                ShowBookDetails(knjiga); // Moved inside the click handler
            };

            panel.Click += clickHandler;
            foreach (Control control in panel.Controls)
            {
                control.Click += clickHandler;
                control.Cursor = Cursors.Hand;
            }

            // Hover effect
            panel.MouseEnter += (s, e) => {
                panel.BackColor = AccentColor;
            };

            panel.MouseLeave += (s, e) => {
                panel.BackColor = Color.White;
            };

            if (statusLabel.Text == "NIJE DOSTUPNO")
                availabilityLabel.Text = "Dostupno primjeraka: 0";

            return panel;
        }

        private void ShowBookDetails(Knjiga knjiga)
        {
            var detailsForm = new Form
            {
                Text = knjiga.Naslov,
                Size = new Size(500, 400),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                BackColor = SecondaryColor
            };

            var panel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(30)
            };

            // Book icon
            var bookIcon = new Label
            {
                Text = "📚",
                Font = new Font("Segoe UI Emoji", 36),
                Size = new Size(60, 60),
                Location = new Point(30, 30),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Title with larger font
            var titleLabel = new Label
            {
                Text = knjiga.Naslov,
                Font = TitleFont,
                ForeColor = TextColor,
                AutoSize = true,
                Location = new Point(100, 30)
            };

            // Book details with consistent spacing
            var authorLabel = new Label
            {
                Text = $"Autor: {knjiga.Autor.Ime} {knjiga.Autor.Prezime}",
                Font = RegularFont,
                ForeColor = TextColor,
                AutoSize = true,
                Location = new Point(30, 120)
            };

            var genreLabel = new Label
            {
                Text = $"Žanr: {knjiga.Zanr.Naziv}",
                Font = RegularFont,
                ForeColor = TextColor,
                AutoSize = true,
                Location = new Point(30, authorLabel.Bottom + 20)
            };

            var yearLabel = new Label
            {
                Text = $"Godina izdavanja: {knjiga.GodinaIzdavanja}",
                Font = RegularFont,
                ForeColor = TextColor,
                AutoSize = true,
                Location = new Point(30, genreLabel.Bottom + 20)
            };

            var availabilityLabel = new Label
            {
                Text = $"Dostupno primjeraka: {knjiga.DostupnaKolicina}",
                Font = RegularFont,
                ForeColor = knjiga.DostupnaKolicina > 0 ? Color.FromArgb(34, 197, 94) : Color.FromArgb(239, 68, 68),
                AutoSize = true,
                Location = new Point(30, yearLabel.Bottom + 20)
            };

            var reserveButton = new Button
            {
                Text = "Rezerviši knjigu",
                Size = new Size(200, 45),
                Location = new Point((detailsForm.Width / 2) - 106, availabilityLabel.Bottom + 30),
                Enabled = knjiga.Dostupna && knjiga.DostupnaKolicina > 0 && trenutniKorisnik.Aktivni,
                BackColor = PrimaryColor,
                ForeColor = SecondaryColor,
                FlatStyle = FlatStyle.Flat,
                Font = BoldFont,
                Cursor = Cursors.Hand
            };

            // Add hover effect
            reserveButton.MouseEnter += (s, e) => reserveButton.BackColor = HoverColor;
            reserveButton.MouseLeave += (s, e) => reserveButton.BackColor = PrimaryColor;
            reserveButton.FlatAppearance.BorderSize = 0;

            reserveButton.Click += (s, e) => ReserveBook(knjiga, detailsForm);

            panel.Controls.AddRange(new Control[]
            {
        bookIcon,
        titleLabel,
        authorLabel,
        genreLabel,
        yearLabel,
        availabilityLabel,
        reserveButton
            });

            detailsForm.Controls.Add(panel);
            detailsForm.ShowDialog();
        }

        private void ReserveBook(Knjiga knjiga, Form detailsForm)
        {
            if (!trenutniKorisnik.Aktivni)
            {
                MessageBox.Show(
                    "Ne možete rezervisati knjigu jer vaša članarina nije aktivna.",
                    "Neaktivna članarina",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            var activeReservations = PodaciBiblioteke.Rezervacije
                .Count(r => r.Korisnik.Id == trenutniKorisnik.Id && !r.IsComplete);

            if (activeReservations >= 3)
            {
                MessageBox.Show(
                    "Dostigli ste maksimalan broj aktivnih rezervacija (3).",
                    "Limit rezervacija",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            var result = CustomMessageBox.Show(
                $"Da li ste sigurni da želite rezervisati knjigu '{knjiga.Naslov}'?\n\n" +
                "Rezervacija će biti aktivna 3 dana od trenutka kada knjiga postane dostupna.",
                "Potvrda rezervacije",
                "Da",
                "Ne",
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
                    "Knjiga je uspješno rezervisana.",
                    "Uspjeh",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                detailsForm.Close();
                ShowBookSearch(); // Refresh the current view
            }
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

                form.Controls.AddRange(new Control[] { message, okButton, cancelButton });
                form.AcceptButton = okButton;
                form.CancelButton = cancelButton;

                return form.ShowDialog();
            }
        }

        private void ShowMyBorrowings()
        {
            var borrowingsPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(30),
                AutoScroll = true,
                BackColor = SecondaryColor
            };

            // Title with consistent styling
            var titleLabel = new Label
            {
                Text = "Moje posudbe",
                Font = TitleFont,
                ForeColor = TextColor,
                AutoSize = true,
                Location = new Point(0, 65)
            };

            // Active borrowings section
            var activeBorrowingsLabel = new Label
            {
                Text = "Aktivne posudbe",
                Font = new Font(TitleFont.FontFamily, 18, FontStyle.Bold),
                ForeColor = TextColor,
                AutoSize = true,
                Location = new Point(0, titleLabel.Bottom + 35)
            };

            var activeBorrowingsPanel = new Panel
            {
                Location = new Point(0, activeBorrowingsLabel.Bottom + 15),
                Width = mainContentPanel.Width - 60,
                AutoSize = true,
                BackColor = SecondaryColor,
                Padding = new Padding(0)
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

            // History section with modern styling
            var historyLabel = new Label
            {
                Text = "Historija posudbi",
                Font = new Font(TitleFont.FontFamily, 18, FontStyle.Bold),
                ForeColor = TextColor,
                AutoSize = true,
                Location = new Point(0, activeBorrowingsPanel.Bottom + 70)
            };

            var historyPanel = new Panel
            {
                Location = new Point(0, historyLabel.Bottom + 15),
                Width = mainContentPanel.Width - 60,
                Height = 600,
                BackColor = SecondaryColor,
                Padding = new Padding(0),
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
                    WrapContents = false,
                    Padding = new Padding(0)
                };

                foreach (var posudba in pastBorrowings)
                {
                    flowPanel.Controls.Add(CreateHistoryPanel(posudba));
                }
                historyPanel.Controls.Add(flowPanel);
            }
            else
            {
                var emptyLabel = new Label
                {
                    Text = "Nemate prethodnih posudbi.",
                    Font = RegularFont,
                    ForeColor = Color.FromArgb(107, 114, 128),
                    AutoSize = true,
                    Location = new Point(10, 10)
                };
                historyPanel.Controls.Add(emptyLabel);
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
                Height = 130,
                Margin = new Padding(0, 0, 0, 15),
                BackColor = Color.White,
                Padding = new Padding(20)
            };

            // Add shadow effect
            panel.Paint += (s, e) =>
            {
                var graphics = e.Graphics;
                var shadowColor = Color.FromArgb(20, 0, 0, 0);
                graphics.FillRectangle(new SolidBrush(shadowColor),
                    new Rectangle(4, 4, panel.Width - 4, panel.Height - 4));
            };

            var bookIcon = new Label
            {
                Text = "📚",
                Font = new Font("Segoe UI Emoji", 24),
                Size = new Size(40, 40),
                Location = new Point(20, 20),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };

            var titleLabel = new Label
            {
                Text = posudba.Knjiga.Naslov,
                Font = new Font(TitleFont.FontFamily, 16, FontStyle.Bold),
                ForeColor = TextColor,
                Location = new Point(76, 20),
                AutoSize = true,
                BackColor = Color.Transparent
            };

            var borrowDateLabel = new Label
            {
                Text = $"Datum posudbe: {posudba.DatumPosudbe:dd.MM.yyyy}",
                Font = RegularFont,
                ForeColor = Color.FromArgb(107, 114, 128),
                Location = new Point(80, titleLabel.Bottom + 12),
                AutoSize = true,
                BackColor = Color.Transparent
            };

            var returnDateLabel = new Label
            {
                Text = $"Rok vraćanja: {posudba.DatumVracanja:dd.MM.yyyy}",
                Font = RegularFont,
                ForeColor = posudba.DatumVracanja < DateTime.Now ? Color.FromArgb(239, 68, 68) : Color.FromArgb(34, 197, 94),
                Location = new Point(80, borrowDateLabel.Bottom + 8),
                AutoSize = true,
                BackColor = Color.Transparent
            };

            panel.Controls.AddRange(new Control[]
            {
        bookIcon,
        titleLabel,
        borrowDateLabel,
        returnDateLabel
            });

            return panel;
        }

        private void ShowProfile()
        {
            mainContentPanel.Controls.Clear();

            var profilePanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(40),
                AutoScroll = true,
                BackColor = Color.White
            };

            // Main Title with consistent spacing
            var titleLabel = new Label
            {
                Text = "Moj profil",
                Font = TitleFont,
                ForeColor = TextColor,
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 40),
                Location= new Point(0, 65)
            };

            // Personal Info Section with fixed width and spacing
            var personalSection = new Panel
            {
                AutoSize = false,
                Width = mainContentPanel.Width - 100,
                Margin = new Padding(0, 0, 0, 40),
                Location = new Point(0, titleLabel.Bottom + 40),
                Height = 420
            };

            var personalInfoLabel = new Label
            {
                Text = "Lični podaci",
                Font = new Font(TitleFont.FontFamily, 18, FontStyle.Bold),
                ForeColor = TextColor,
                AutoSize = true,
                Margin = new Padding(10, 0, 0, 35)
            };

            // Update spacing in personal info section
            var personalInfoPanel = CreateCardPanel(mainContentPanel.Width - 100);
            var infoTable = new TableLayoutPanel
            {
                ColumnCount = 2,
                RowCount = 9,
                AutoSize = true,
                Width = mainContentPanel.Width - 160,
                Location = new Point(30, 40),
                Height = 400,
                Margin = new Padding(0),
                Padding = new Padding(30)
            };

            // Set initial row heights - make error rows 0 height by default
            for (int i = 0; i < 9; i++)
            {
                // Set error rows (3 and 5) to 0 height initially
                float height = (i == 3 || i == 5) ? 0F : 40F;
                infoTable.RowStyles.Add(new RowStyle(SizeType.Absolute, height));
            }

            // Add rows only once with proper spacing
            AddProfileRow(infoTable, "Ime:", trenutniKorisnik.Ime, 0);
            AddProfileRow(infoTable, "Prezime:", trenutniKorisnik.Prezime, 1);
            AddProfileRow(infoTable, "Email:", "", 2);
            AddProfileRow(infoTable, "Telefon:", "", 4);
            AddProfileRow(infoTable, "Članarina:", trenutniKorisnik.Clanarina.Naziv, 6);
            AddProfileRow(infoTable, "Važi do:", String.Format("{0:dd.MM.yyyy}", trenutniKorisnik.DatumIsteka), 7);

            // Modern styled textboxes
            var emailBox = CreateStyledTextBox(trenutniKorisnik.Email);
            var phoneBox = CreateStyledTextBox(trenutniKorisnik.BrojTelefona);

            // Error labels with consistent styling
            var emailErrorLabel = CreateErrorLabel();
            var phoneErrorLabel = CreateErrorLabel();

            // Modern save button
            var saveButton = new Button
            {
                Text = "Sačuvaj promjene",
                Size = new Size(180, 45),
                BackColor = PrimaryColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = BoldFont,
                Cursor = Cursors.Hand,
                Margin = new Padding(0, 20, 0, 0),
                Enabled = false
            };
            saveButton.FlatAppearance.BorderSize = 0;

            // Add hover effects
            AddButtonHoverEffects(saveButton);

            // Validation setup
            SetupValidation(emailBox, phoneBox, emailErrorLabel, phoneErrorLabel, saveButton, infoTable);

            // Add hover effects
            AddButtonHoverEffects(saveButton);

            // Validation setup
            SetupValidation(emailBox, phoneBox, emailErrorLabel, phoneErrorLabel, saveButton, infoTable);

            // Add controls to table (only once)
            infoTable.Controls.Add(emailBox, 1, 2);
            infoTable.Controls.Add(emailErrorLabel, 1, 3);
            infoTable.Controls.Add(phoneBox, 1, 4);
            infoTable.Controls.Add(phoneErrorLabel, 1, 5);
            infoTable.Controls.Add(saveButton, 1, 8);

            // Create membership status first
            var membershipStatus = CreateMembershipStatus(trenutniKorisnik.Aktivni);

            // Build the hierarchy in the correct order
            personalSection.Controls.Add(membershipStatus);
            personalSection.Controls.Add(personalInfoLabel);
            personalSection.Controls.Add(personalInfoPanel);

            // Remove the duplicate membership status addition
            // personalInfoPanel.Controls.Add(CreateMembershipStatus(trenutniKorisnik.Aktivni)); // Remove this line
            personalInfoPanel.Controls.Add(infoTable);

            // Reservations section with consistent styling
            var reservationsSection = CreateReservationsSection();

            // Final assembly
            profilePanel.Controls.AddRange(new Control[]
            {
        titleLabel,
        personalSection,
        reservationsSection
            });

            mainContentPanel.Controls.Add(profilePanel);
        }

        // Helper methods
        private Panel CreateCardPanel(int width)
        {
            var panel = new Panel
            {
                AutoSize = true,
                Width = width,
                BackColor = Color.White,
                Padding = new Padding(30),
                Margin = new Padding(0, 0, 0, 40)
            };

            return panel;
        }

        private Label CreateErrorLabel()
        {
            return new Label
            {
                ForeColor = Color.FromArgb(239, 68, 68),
                AutoSize = true,
                Font = RegularFont,
                Visible = false,
                Margin = new Padding(0)
            };
        }

        private void AddButtonHoverEffects(Button button)
        {
            button.MouseEnter += (s, e) => {
                if (button.Enabled)
                    button.BackColor = Color.FromArgb(
                        (int)(button.BackColor.R * 0.9),
                        (int)(button.BackColor.G * 0.9),
                        (int)(button.BackColor.B * 0.9));
            };

            button.MouseLeave += (s, e) => {
                if (button.Enabled)
                    button.BackColor = PrimaryColor;
            };
        }

        private Panel CreateMembershipStatus(bool isActive)
        {
            var panel = new Panel
            {
                Size = new Size(300, 50),
                Margin = new Padding(0, 0, 0, 0), // Adjust top and bottom margins
                Location = new Point(273, 80)
            };

            var icon = new Label
            {
                Text = isActive ? "✓" : "!",
                Font = new Font(RegularFont.FontFamily, 16, FontStyle.Bold),
                ForeColor = isActive ? Color.FromArgb(34, 197, 94) : Color.FromArgb(239, 68, 68),
                AutoSize = true
            };

            var text = new Label
            {
                Text = isActive ? "Aktivna članarina" : "Neaktivna članarina",
                Font = BoldFont,
                ForeColor = isActive ? Color.FromArgb(34, 197, 94) : Color.FromArgb(239, 68, 68),
                Location = new Point(icon.Right - 60, icon.Top + 5),
                AutoSize = true
            };

            panel.Controls.AddRange(new Control[] { icon, text });
            return panel;
        }

        private Panel CreateReservationsSection()
        {
            var section = new Panel
            {
                AutoSize = true,
                Width = mainContentPanel.Width - 100,
                Location = new Point(0, 550),
                Margin = new Padding(0, 40, 0, 0) // Add top margin for separation from personal info
            };

            var titleLabel = new Label
            {
                Text = "Moje rezervacije",
                Font = new Font(TitleFont.FontFamily, 18, FontStyle.Bold),
                ForeColor = TextColor,
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 25)
            };

            var reservationsPanel = CreateCardPanel(mainContentPanel.Width - 100);
            var flowPanel = new FlowLayoutPanel
            {
                AutoSize = true,
                Width = reservationsPanel.Width - 60,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                Padding = new Padding(30, 20, 30, 20), // Add padding for content
                MinimumSize = new Size(0, 70),
                Location = new Point(0, titleLabel.Bottom + 20)
            };

            var activeReservations = PodaciBiblioteke.Rezervacije
                .Where(r => r.Korisnik.Id == trenutniKorisnik.Id && !r.IsComplete)
                .ToList();

            if (activeReservations.Any())
            {
                foreach (var rezervacija in activeReservations)
                {
                    flowPanel.Controls.Add(CreateReservationPanel(rezervacija));
                }
            }
            else
            {
                flowPanel.Controls.Add(CreateEmptyStateLabel("Trenutno nemate aktivnih rezervacija."));
            }

            reservationsPanel.Controls.Add(flowPanel);
            section.Controls.AddRange(new Control[] { titleLabel, reservationsPanel });
            return section;
        }

        private Panel CreateReservationPanel(Rezervacija rezervacija)
        {
            var panel = new Panel
            {
                Width = mainContentPanel.Width - 220,
                Height = 100,
                Margin = new Padding(0, 0, 0, 15),
                BackColor = Color.White,
                Padding = new Padding(20)
            };

            // Add shadow effect
            panel.Paint += (s, e) =>
            {
                var graphics = e.Graphics;
                var shadowColor = Color.FromArgb(15, 0, 0, 0);
                graphics.FillRectangle(new SolidBrush(shadowColor),
                    new Rectangle(2, 2, panel.Width - 2, panel.Height - 2));
            };

            var titleLabel = new Label
            {
                Text = rezervacija.Knjiga.Naslov,
                Font = BoldFont,
                ForeColor = TextColor,
                Location = new Point(15, 15),
                AutoSize = true,
                BackColor = Color.Transparent
            };

            var dateLabel = new Label
            {
                Text = $"Rezervisano: {rezervacija.DatumRezervacije:dd.MM.yyyy}",
                Font = RegularFont,
                ForeColor = Color.FromArgb(107, 114, 128),
                Location = new Point(15, 45),
                AutoSize = true,
                BackColor = Color.Transparent
            };

            var cancelButton = new Button
            {
                Text = "Otkaži rezervaciju",
                Size = new Size(140, 35),
                Location = new Point(panel.Width - 160, 32),
                BackColor = Color.FromArgb(239, 68, 68), // Red color
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = RegularFont,
                Cursor = Cursors.Hand
            };
            cancelButton.FlatAppearance.BorderSize = 0;

            // Custom hover effects for cancel button
            cancelButton.MouseEnter += (s, e) => {
                cancelButton.BackColor = Color.FromArgb(220, 38, 38); // Darker red
            };
            cancelButton.MouseLeave += (s, e) => {
                cancelButton.BackColor = Color.FromArgb(239, 68, 68); // Original red
            };

            cancelButton.Click += (s, e) => CancelReservation(rezervacija, panel);


            panel.Controls.AddRange(new Control[] { titleLabel, dateLabel, cancelButton });
            return panel;
        }

        private void SetupValidation(TextBox emailBox, TextBox phoneBox, Label emailError,
            Label phoneError, Button saveButton, TableLayoutPanel table)
        {
            var originalEmail = trenutniKorisnik.Email;
            var originalPhone = trenutniKorisnik.BrojTelefona;
            bool isEmailValid = true;
            bool isPhoneValid = true;

            void CheckChanges()
            {
                bool hasChanges = emailBox.Text != originalEmail || phoneBox.Text != originalPhone;
                saveButton.Enabled = hasChanges && isEmailValid && isPhoneValid;
                saveButton.BackColor = saveButton.Enabled ? PrimaryColor : Color.FromArgb(156, 163, 175);
            }

            emailBox.TextChanged += (s, e) =>
            {
                isEmailValid = ValidateEmail(emailBox.Text);
                UpdateValidationUI(isEmailValid, emailBox, emailError, table, 3,
                    "Unesite ispravnu email adresu (npr. ime@domena.com)");
                CheckChanges();
            };

            phoneBox.TextChanged += (s, e) =>
            {
                isPhoneValid = ValidatePhone(phoneBox.Text);
                UpdateValidationUI(isPhoneValid, phoneBox, phoneError, table, 5,
                    "Unesite validan broj telefona (npr. +38712345678 ili 061234567)");
                CheckChanges();
            };

            saveButton.Click += async (s, e) =>
            {
                if (isEmailValid && isPhoneValid)
                {
                    await SaveChanges(emailBox.Text, phoneBox.Text);
                    originalEmail = emailBox.Text;
                    originalPhone = phoneBox.Text;
                    saveButton.Enabled = false;
                    ShowSuccessMessage();
                }
            };
        }

        private async Task SaveChanges(string email, string phone)
        {
            trenutniKorisnik.Email = email;
            trenutniKorisnik.BrojTelefona = phone;
            // Add any additional save logic here
            await Task.Delay(100); // Simulate save operation
        }

        private void UpdateValidationUI(bool isValid, TextBox textBox, Label errorLabel,
            TableLayoutPanel table, int errorRow, string errorMessage)
        {
            if (!isValid)
            {
                errorLabel.Text = errorMessage;
                errorLabel.Visible = true;
                textBox.BackColor = Color.FromArgb(254, 242, 242);
                table.RowStyles[errorRow] = new RowStyle(SizeType.Absolute, 20F);
            }
            else
            {
                errorLabel.Visible = false;
                textBox.BackColor = Color.White;
                table.RowStyles[errorRow] = new RowStyle(SizeType.Absolute, 0F);
            }
            table.PerformLayout();
        }

        private Label CreateEmptyStateLabel(string text)
        {
            return new Label
            {
                Text = text,
                Font = RegularFont,
                ForeColor = Color.FromArgb(107, 114, 128),
                AutoSize = true,
                Margin = new Padding(0, 20, 0, 20)
            };
        }

        private void ShowSuccessMessage()
        {
            var successMessage = new Form
            {
                Size = new Size(300, 150),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.None,
                BackColor = Color.White
            };

            var messageLabel = new Label
            {
                Text = "Promjene su uspješno sačuvane",
                Font = RegularFont,
                ForeColor = Color.FromArgb(34, 197, 94),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill
            };

            successMessage.Controls.Add(messageLabel);
            successMessage.Show();
            Task.Delay(1500).ContinueWith(_ => successMessage.Invoke(new Action(() => successMessage.Close())));
        }

        private void AddProfileRow(TableLayoutPanel panel, string label, string value, int row)
        {
            panel.Controls.Add(new Label
            {
                Text = label,
                Font = BoldFont,
                ForeColor = TextColor,
                AutoSize = true,
                Anchor = AnchorStyles.Left | AnchorStyles.Top,
                Margin = new Padding(0, 8, 0, 0) // Adjust top margin for better spacing
            }, 0, row);

            if (!string.IsNullOrEmpty(value))
            {
                panel.Controls.Add(new Label
                {
                    Text = value,
                    Font = RegularFont,
                    ForeColor = Color.FromArgb(75, 85, 99),
                    AutoSize = true,
                    Anchor = AnchorStyles.Left | AnchorStyles.Top,
                    Margin = new Padding(0, 8, 0, 0) // Match the label margin
                }, 1, row);
            }
        }

        private TextBox CreateStyledTextBox(string initialText)
        {
            var textBox = new TextBox
            {
                Text = initialText,
                Font = RegularFont,
                Width = 300,
                Height = 35,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(8, 5, 8, 5),
                Margin = new Padding(0, 5, 0, 5),
                BackColor = Color.White
            };

            // Add focus effects
            textBox.Enter += (s, e) =>
            {
                textBox.BackColor = Color.FromArgb(249, 250, 251);
            };

            textBox.Leave += (s, e) =>
            {
                if (textBox.BackColor != Color.FromArgb(254, 242, 242)) // Don't override error state
                    textBox.BackColor = Color.White;
            };

            return textBox;
        }

        private async void CancelReservation(Rezervacija rezervacija, Panel reservationPanel)
        {
            var result = MessageBox.Show(
                $"Da li ste sigurni da želite otkazati rezervaciju za knjigu '{rezervacija.Knjiga.Naslov}'?",
                "Potvrda otkazivanja",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                var parentControl = reservationPanel.Parent;

                // Update reservation and book status
                rezervacija.IsComplete = true;
                rezervacija.Knjiga.DostupnaKolicina++;

                // Animate panel removal
                await AnimatePanelRemoval(reservationPanel);
                parentControl.Controls.Remove(reservationPanel);

                // Check if there are any remaining reservations
                if (!parentControl.Controls.OfType<Panel>().Any())
                {
                    parentControl.Controls.Add(CreateEmptyStateLabel("Trenutno nemate aktivnih rezervacija."));
                }

                // Refresh the view
                parentControl.PerformLayout();

                ShowNotification("Rezervacija otkazana.");
            }
        }

        private async Task AnimatePanelRemoval(Panel panel)
        {
            for (double opacity = 1.0; opacity > 0; opacity -= 0.1)
            {
                panel.BackColor = Color.FromArgb(
                    (int)(opacity * 255),
                    panel.BackColor.R,
                    panel.BackColor.G,
                    panel.BackColor.B
                );
                await Task.Delay(20);
            }
        }

        private bool ValidateEmail(string email)
        {
            string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            return System.Text.RegularExpressions.Regex.IsMatch(email, emailPattern);
        }

        private bool ValidatePhone(string phone)
        {
            string phonePattern = @"^(\+387|0)\d{2}[-]?\d{3}[-]?\d{3,4}$";
            return System.Text.RegularExpressions.Regex.IsMatch(phone.Replace(" ", ""), phonePattern);
        }

        private void CheckNotifications(object sender, EventArgs e)
        {
            // Check for books due soon
            var dueSoonBooks = PodaciBiblioteke.Posudbe
                .Where(p => p.Korisnik.Id == trenutniKorisnik.Id &&
                            p.DatumVracanja > DateTime.Now &&
                            p.DatumVracanja <= DateTime.Now.AddDays(3))
                .ToList();

            foreach (var posudba in dueSoonBooks)
            {
                ShowNotification(
                    $"Knjiga '{posudba.Knjiga.Naslov}' dospijeva za vraćanje {posudba.DatumVracanja:dd.MM.yyyy}",
                    ToolTipIcon.Warning);
            }

            // Check for available reserved books
            var availableReservations = PodaciBiblioteke.Rezervacije
                .Where(r => r.Korisnik.Id == trenutniKorisnik.Id &&
                            r.Knjiga.Dostupna &&
                            !r.Notified)
                .ToList();

            foreach (var rezervacija in availableReservations)
            {
                ShowNotification(
                    $"Knjiga '{rezervacija.Knjiga.Naslov}' je sada dostupna!",
                    ToolTipIcon.Info);
                rezervacija.Notified = true;
            }
        }

        private void ShowNotification(string message, ToolTipIcon icon)
        {
            MessageBox.Show(
                message,
                "Biblioteka",
                MessageBoxButtons.OK,
                icon == ToolTipIcon.Warning ? MessageBoxIcon.Warning : MessageBoxIcon.Information
            );
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
                Width = 280,
                Height = 180,
                Margin = new Padding(10, 0, 10, 0),
                BackColor = Color.White,
                Padding = new Padding(20),
                Cursor = Cursors.Hand
            };

            // Add shadow
            panel.Paint += (s, e) =>
            {
                var graphics = e.Graphics;
                var shadowColor = Color.FromArgb(20, 0, 0, 0);
                graphics.FillRectangle(new SolidBrush(shadowColor),
                    new Rectangle(4, 4, panel.Width - 4, panel.Height - 4));
            };

            // Book icon
            var bookIcon = new Label
            {
                Text = "📚",
                Font = new Font("Segoe UI Emoji", 24),
                Size = new Size(40, 40),
                Location = new Point(15, 15),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };

            // Book details
            var titleLabel = new Label
            {
                Text = knjiga.Naslov,
                Font = BoldFont,
                ForeColor = TextColor,
                Location = new Point(65, 15),
                Size = new Size(180, 20),
                BackColor = Color.Transparent
            };

            var authorLabel = new Label
            {
                Text = $"{knjiga.Autor.Ime} {knjiga.Autor.Prezime}",
                Font = RegularFont,
                ForeColor = Color.FromArgb(107, 114, 128),
                Location = new Point(65, titleLabel.Bottom + 5),
                Size = new Size(180, 20),
                BackColor = Color.Transparent
            };

            var genreLabel = new Label
            {
                Text = knjiga.Zanr.Naziv,
                Font = RegularFont,
                ForeColor = PrimaryColor,
                Location = new Point(65, authorLabel.Bottom + 5),
                AutoSize = true,
                Padding = new Padding(6, 3, 6, 3),
                BackColor = Color.FromArgb(255, 240, 230) // Light orange background
            };

            // Status indicators
            var availableIcon = new Label
            {
                Text = knjiga.Dostupna ? "✓" : "×",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = knjiga.Dostupna ? Color.FromArgb(34, 197, 94) : Color.FromArgb(239, 68, 68),
                Location = new Point(65, genreLabel.Bottom + 10),
                AutoSize = true,
                BackColor = Color.Transparent
            };

            var availableLabel = new Label
            {
                Text = knjiga.Dostupna ? "Dostupna" : "Nedostupna",
                Font = RegularFont,
                ForeColor = knjiga.Dostupna ? Color.FromArgb(34, 197, 94) : Color.FromArgb(239, 68, 68),
                Location = new Point(85, genreLabel.Bottom + 10),
                AutoSize = true,
                BackColor = Color.Transparent
            };

            int count = new Random().Next(20, 192);

            // Times borrowed indicator
            var borrowedCountLabel = new Label
            {
                Text = $"Posuđena {count} puta",
                Font = RegularFont,
                ForeColor = Color.FromArgb(107, 114, 128),
                Location = new Point(65, availableLabel.Bottom + 10),
                AutoSize = true,
                BackColor = Color.Transparent
            };

            panel.Controls.AddRange(new Control[]
            {
        bookIcon,
        titleLabel,
        authorLabel,
        genreLabel,
        availableIcon,
        availableLabel,
        borrowedCountLabel
            });

            // Make sure all labels (except genreLabel) have transparent background initially
            foreach (Control control in panel.Controls)
            {
                if (control != genreLabel)
                {
                    control.BackColor = Color.Transparent;
                }
            }

            // Add click handler for the panel
            EventHandler clickHandler = (s, e) =>
            {
                if (!trenutniKorisnik.Aktivni)
                {
                    MessageBox.Show("Vaša članarina nije aktivna. Molimo obnovite članarinu kako biste mogli rezervisati knjige.",
                                  "Neaktivna članarina",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Warning);
                    return;
                }

                if (!knjiga.Dostupna)
                {
                    MessageBox.Show("Knjiga trenutno nije dostupna za rezervaciju.",
                                  "Knjiga nedostupna",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Information);
                    return;
                }

                ShowBookDetails(knjiga);
            };

            // Add click handler to panel and all child controls
            panel.Click += clickHandler;
            foreach (Control control in panel.Controls)
            {
                control.Click += clickHandler;
                control.Cursor = Cursors.Hand;
            }

            // Hover effect
            panel.MouseEnter += (s, e) => {
                panel.BackColor = AccentColor;
            };

            panel.MouseLeave += (s, e) => {
                panel.BackColor = Color.White;
            };

            return panel;
        }

        private Panel CreateHistoryPanel(Posudba posudba)
        {
            var panel = new Panel
            {
                Width = mainContentPanel.Width - 100,
                Height = 130,
                Margin = new Padding(0, 0, 0, 15),
                BackColor = Color.White,
                Padding = new Padding(20)
            };

            // Add shadow effect
            panel.Paint += (s, e) =>
            {
                var graphics = e.Graphics;
                var shadowColor = Color.FromArgb(20, 0, 0, 0);
                graphics.FillRectangle(new SolidBrush(shadowColor),
                    new Rectangle(4, 4, panel.Width - 4, panel.Height - 4));
            };

            // Book icon
            var bookIcon = new Label
            {
                Text = "📚",
                Font = new Font("Segoe UI Emoji", 24),
                Size = new Size(40, 40),
                Location = new Point(20, 20),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };

            // Title with larger, bold font
            var titleLabel = new Label
            {
                Text = posudba.Knjiga.Naslov,
                Font = new Font(TitleFont.FontFamily, 16, FontStyle.Bold),
                ForeColor = TextColor,
                Location = new Point(76, 20),
                AutoSize = true,
                BackColor = Color.Transparent
            };

            // Dates with subtle color
            var datesLabel = new Label
            {
                Text = $"Posuđeno: {posudba.DatumPosudbe:dd.MM.yyyy}",
                Font = RegularFont,
                ForeColor = Color.FromArgb(107, 114, 128),
                Location = new Point(80, titleLabel.Bottom + 12),
                AutoSize = true,
                BackColor = Color.Transparent
            };

            var returnedLabel = new Label
            {
                Text = $"Vraćeno: {posudba.DatumVracanja:dd.MM.yyyy}",
                Font = RegularFont,
                ForeColor = Color.FromArgb(107, 114, 128),
                Location = new Point(80, datesLabel.Bottom + 8),
                AutoSize = true,
                BackColor = Color.Transparent
            };

            // Status indicator
            var statusLabel = new Label
            {
                Text = "✓ Vraćeno",
                Font = BoldFont,
                ForeColor = Color.FromArgb(34, 197, 94), // Success green
                Location = new Point(panel.Width - 160, 20),
                AutoSize = true,
                BackColor = Color.Transparent
            };

            panel.Controls.AddRange(new Control[]
            {
        bookIcon,
        titleLabel,
        datesLabel,
        returnedLabel,
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
                var loginForm = new NewLogin();
                loginForm.Show();
            }
        }
    }
}