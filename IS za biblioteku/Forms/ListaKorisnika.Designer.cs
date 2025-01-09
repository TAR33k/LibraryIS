namespace IS_za_biblioteku.Forms
{
    partial class ListaKorisnika
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ListaKorisnika));
            this.materialLabel2 = new MaterialSkin.Controls.MaterialLabel();
            this.materialLabel1 = new MaterialSkin.Controls.MaterialLabel();
            this.tbIme = new MetroFramework.Controls.MetroTextBox();
            this.btnDodaj = new MetroFramework.Controls.MetroButton();
            this.dgvKorisnici = new System.Windows.Forms.DataGridView();
            this.cmbClanarina = new MetroFramework.Controls.MetroComboBox();
            this.materialLabel3 = new MaterialSkin.Controls.MaterialLabel();
            this.tbPrezime = new MetroFramework.Controls.MetroTextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.Ime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Prezime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Email = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StatusClanarine = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VrstaClanarine = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DatumIsteka = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewButtonColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvKorisnici)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // materialLabel2
            // 
            this.materialLabel2.AutoSize = true;
            this.materialLabel2.Depth = 0;
            this.materialLabel2.Font = new System.Drawing.Font("Roboto", 11F);
            this.materialLabel2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialLabel2.Location = new System.Drawing.Point(730, 201);
            this.materialLabel2.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel2.Name = "materialLabel2";
            this.materialLabel2.Size = new System.Drawing.Size(135, 24);
            this.materialLabel2.TabIndex = 14;
            this.materialLabel2.Text = "Vrsta članarine";
            // 
            // materialLabel1
            // 
            this.materialLabel1.AutoSize = true;
            this.materialLabel1.Depth = 0;
            this.materialLabel1.Font = new System.Drawing.Font("Roboto", 11F);
            this.materialLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialLabel1.Location = new System.Drawing.Point(28, 201);
            this.materialLabel1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel1.Name = "materialLabel1";
            this.materialLabel1.Size = new System.Drawing.Size(111, 24);
            this.materialLabel1.TabIndex = 13;
            this.materialLabel1.Text = "Pretraži ime";
            this.materialLabel1.Click += new System.EventHandler(this.materialLabel1_Click);
            // 
            // tbIme
            // 
            this.tbIme.FontSize = MetroFramework.MetroTextBoxSize.Medium;
            this.tbIme.FontWeight = MetroFramework.MetroTextBoxWeight.Regular;
            this.tbIme.Location = new System.Drawing.Point(32, 228);
            this.tbIme.Multiline = false;
            this.tbIme.Name = "tbIme";
            this.tbIme.SelectedText = "";
            this.tbIme.Size = new System.Drawing.Size(289, 30);
            this.tbIme.Style = MetroFramework.MetroColorStyle.Blue;
            this.tbIme.StyleManager = null;
            this.tbIme.TabIndex = 11;
            this.tbIme.Theme = MetroFramework.MetroThemeStyle.Light;
            this.tbIme.UseStyleColors = false;
            // 
            // btnDodaj
            // 
            this.btnDodaj.Highlight = false;
            this.btnDodaj.Location = new System.Drawing.Point(1058, 228);
            this.btnDodaj.Name = "btnDodaj";
            this.btnDodaj.Size = new System.Drawing.Size(181, 30);
            this.btnDodaj.Style = MetroFramework.MetroColorStyle.Blue;
            this.btnDodaj.StyleManager = null;
            this.btnDodaj.TabIndex = 10;
            this.btnDodaj.Text = "Dodaj korisnika";
            this.btnDodaj.Theme = MetroFramework.MetroThemeStyle.Light;
            this.btnDodaj.Click += new System.EventHandler(this.btnDodaj_Click);
            // 
            // dgvKorisnici
            // 
            this.dgvKorisnici.AllowUserToAddRows = false;
            this.dgvKorisnici.AllowUserToDeleteRows = false;
            this.dgvKorisnici.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvKorisnici.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.SandyBrown;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial Narrow", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.SandyBrown;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvKorisnici.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvKorisnici.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvKorisnici.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Ime,
            this.Prezime,
            this.Email,
            this.StatusClanarine,
            this.VrstaClanarine,
            this.DatumIsteka,
            this.Column1});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvKorisnici.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvKorisnici.Location = new System.Drawing.Point(32, 264);
            this.dgvKorisnici.Name = "dgvKorisnici";
            this.dgvKorisnici.ReadOnly = true;
            this.dgvKorisnici.RowHeadersWidth = 51;
            this.dgvKorisnici.RowTemplate.Height = 24;
            this.dgvKorisnici.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvKorisnici.Size = new System.Drawing.Size(1207, 327);
            this.dgvKorisnici.TabIndex = 9;
            // 
            // cmbClanarina
            // 
            this.cmbClanarina.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbClanarina.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbClanarina.FontSize = MetroFramework.MetroLinkSize.Medium;
            this.cmbClanarina.FontWeight = MetroFramework.MetroLinkWeight.Regular;
            this.cmbClanarina.FormattingEnabled = true;
            this.cmbClanarina.ItemHeight = 24;
            this.cmbClanarina.Location = new System.Drawing.Point(734, 228);
            this.cmbClanarina.Name = "cmbClanarina";
            this.cmbClanarina.Size = new System.Drawing.Size(303, 30);
            this.cmbClanarina.Style = MetroFramework.MetroColorStyle.Blue;
            this.cmbClanarina.StyleManager = null;
            this.cmbClanarina.TabIndex = 15;
            this.cmbClanarina.Theme = MetroFramework.MetroThemeStyle.Light;
            // 
            // materialLabel3
            // 
            this.materialLabel3.AutoSize = true;
            this.materialLabel3.Depth = 0;
            this.materialLabel3.Font = new System.Drawing.Font("Roboto", 11F);
            this.materialLabel3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialLabel3.Location = new System.Drawing.Point(340, 201);
            this.materialLabel3.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel3.Name = "materialLabel3";
            this.materialLabel3.Size = new System.Drawing.Size(147, 24);
            this.materialLabel3.TabIndex = 16;
            this.materialLabel3.Text = "Pretraži prezime";
            // 
            // tbPrezime
            // 
            this.tbPrezime.FontSize = MetroFramework.MetroTextBoxSize.Medium;
            this.tbPrezime.FontWeight = MetroFramework.MetroTextBoxWeight.Regular;
            this.tbPrezime.Location = new System.Drawing.Point(344, 228);
            this.tbPrezime.Multiline = false;
            this.tbPrezime.Name = "tbPrezime";
            this.tbPrezime.SelectedText = "";
            this.tbPrezime.Size = new System.Drawing.Size(369, 30);
            this.tbPrezime.Style = MetroFramework.MetroColorStyle.Blue;
            this.tbPrezime.StyleManager = null;
            this.tbPrezime.TabIndex = 17;
            this.tbPrezime.Theme = MetroFramework.MetroThemeStyle.Light;
            this.tbPrezime.UseStyleColors = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(1700, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(37, 38);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 18;
            this.pictureBox1.TabStop = false;
            // 
            // Ime
            // 
            this.Ime.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Ime.DataPropertyName = "Ime";
            this.Ime.HeaderText = "Ime";
            this.Ime.MinimumWidth = 6;
            this.Ime.Name = "Ime";
            this.Ime.ReadOnly = true;
            // 
            // Prezime
            // 
            this.Prezime.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Prezime.DataPropertyName = "Prezime";
            this.Prezime.HeaderText = "Prezime";
            this.Prezime.MinimumWidth = 6;
            this.Prezime.Name = "Prezime";
            this.Prezime.ReadOnly = true;
            // 
            // Email
            // 
            this.Email.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Email.DataPropertyName = "Email";
            this.Email.HeaderText = "Email";
            this.Email.MinimumWidth = 6;
            this.Email.Name = "Email";
            this.Email.ReadOnly = true;
            // 
            // StatusClanarine
            // 
            this.StatusClanarine.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.StatusClanarine.DataPropertyName = "StatusClanarine";
            this.StatusClanarine.HeaderText = "Status članarine";
            this.StatusClanarine.MinimumWidth = 6;
            this.StatusClanarine.Name = "StatusClanarine";
            this.StatusClanarine.ReadOnly = true;
            // 
            // VrstaClanarine
            // 
            this.VrstaClanarine.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.VrstaClanarine.DataPropertyName = "VrstaClanarine";
            this.VrstaClanarine.HeaderText = "Vrsta članarine";
            this.VrstaClanarine.MinimumWidth = 6;
            this.VrstaClanarine.Name = "VrstaClanarine";
            this.VrstaClanarine.ReadOnly = true;
            // 
            // DatumIsteka
            // 
            this.DatumIsteka.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.DatumIsteka.DataPropertyName = "DatumIsteka";
            this.DatumIsteka.HeaderText = "Datum isteka članarine";
            this.DatumIsteka.MinimumWidth = 6;
            this.DatumIsteka.Name = "DatumIsteka";
            this.DatumIsteka.ReadOnly = true;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "";
            this.Column1.MinimumWidth = 6;
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Text = "Ažuriraj članarinu";
            this.Column1.UseColumnTextForButtonValue = true;
            this.Column1.Width = 125;
            // 
            // ListaKorisnika
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1270, 535);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.tbPrezime);
            this.Controls.Add(this.materialLabel3);
            this.Controls.Add(this.cmbClanarina);
            this.Controls.Add(this.materialLabel2);
            this.Controls.Add(this.materialLabel1);
            this.Controls.Add(this.tbIme);
            this.Controls.Add(this.btnDodaj);
            this.Controls.Add(this.dgvKorisnici);
            this.Name = "ListaKorisnika";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ListaKorisnika";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.ListaKorisnika_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvKorisnici)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MaterialSkin.Controls.MaterialLabel materialLabel2;
        private MaterialSkin.Controls.MaterialLabel materialLabel1;
        private MetroFramework.Controls.MetroButton btnDodaj;
        private System.Windows.Forms.DataGridView dgvKorisnici;
        private MetroFramework.Controls.MetroTextBox tbIme;
        private MetroFramework.Controls.MetroComboBox cmbClanarina;
        private MaterialSkin.Controls.MaterialLabel materialLabel3;
        private MetroFramework.Controls.MetroTextBox tbPrezime;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Ime;
        private System.Windows.Forms.DataGridViewTextBoxColumn Prezime;
        private System.Windows.Forms.DataGridViewTextBoxColumn Email;
        private System.Windows.Forms.DataGridViewTextBoxColumn StatusClanarine;
        private System.Windows.Forms.DataGridViewTextBoxColumn VrstaClanarine;
        private System.Windows.Forms.DataGridViewTextBoxColumn DatumIsteka;
        private System.Windows.Forms.DataGridViewButtonColumn Column1;
    }
}