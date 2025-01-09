namespace IS_za_biblioteku.Forms
{
    partial class ListaKnjiga
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ListaKnjiga));
            this.dgvKnjige = new System.Windows.Forms.DataGridView();
            this.Naslov = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Autor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DostupnaKolicina = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GodinaIzdavanja = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Zanr = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnDodaj = new MetroFramework.Controls.MetroButton();
            this.textBox2 = new MetroFramework.Controls.MetroTextBox();
            this.textBox1 = new MetroFramework.Controls.MetroTextBox();
            this.materialLabel1 = new MaterialSkin.Controls.MaterialLabel();
            this.materialLabel2 = new MaterialSkin.Controls.MaterialLabel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvKnjige)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvKnjige
            // 
            this.dgvKnjige.AllowUserToAddRows = false;
            this.dgvKnjige.AllowUserToDeleteRows = false;
            this.dgvKnjige.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial Narrow", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvKnjige.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvKnjige.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvKnjige.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Naslov,
            this.Autor,
            this.DostupnaKolicina,
            this.GodinaIzdavanja,
            this.Zanr});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvKnjige.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvKnjige.Location = new System.Drawing.Point(32, 264);
            this.dgvKnjige.Name = "dgvKnjige";
            this.dgvKnjige.ReadOnly = true;
            this.dgvKnjige.RowHeadersWidth = 51;
            this.dgvKnjige.RowTemplate.Height = 24;
            this.dgvKnjige.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvKnjige.Size = new System.Drawing.Size(1207, 327);
            this.dgvKnjige.TabIndex = 2;
            // 
            // Naslov
            // 
            this.Naslov.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Naslov.DataPropertyName = "Naslov";
            this.Naslov.HeaderText = "Naziv knjige";
            this.Naslov.MinimumWidth = 6;
            this.Naslov.Name = "Naslov";
            this.Naslov.ReadOnly = true;
            // 
            // Autor
            // 
            this.Autor.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Autor.DataPropertyName = "Autor";
            this.Autor.HeaderText = "Ime autora";
            this.Autor.MinimumWidth = 6;
            this.Autor.Name = "Autor";
            this.Autor.ReadOnly = true;
            // 
            // DostupnaKolicina
            // 
            this.DostupnaKolicina.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.DostupnaKolicina.DataPropertyName = "DostupnaKolicina";
            this.DostupnaKolicina.HeaderText = "Broj dostupnih knjiga";
            this.DostupnaKolicina.MinimumWidth = 6;
            this.DostupnaKolicina.Name = "DostupnaKolicina";
            this.DostupnaKolicina.ReadOnly = true;
            // 
            // GodinaIzdavanja
            // 
            this.GodinaIzdavanja.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.GodinaIzdavanja.DataPropertyName = "GodinaIzdavanja";
            this.GodinaIzdavanja.HeaderText = "Godina izdavanja";
            this.GodinaIzdavanja.MinimumWidth = 6;
            this.GodinaIzdavanja.Name = "GodinaIzdavanja";
            this.GodinaIzdavanja.ReadOnly = true;
            // 
            // Zanr
            // 
            this.Zanr.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Zanr.DataPropertyName = "Zanr";
            this.Zanr.HeaderText = "Žanr";
            this.Zanr.MinimumWidth = 6;
            this.Zanr.Name = "Zanr";
            this.Zanr.ReadOnly = true;
            // 
            // btnDodaj
            // 
            this.btnDodaj.Highlight = false;
            this.btnDodaj.Location = new System.Drawing.Point(1058, 228);
            this.btnDodaj.Name = "btnDodaj";
            this.btnDodaj.Size = new System.Drawing.Size(181, 30);
            this.btnDodaj.Style = MetroFramework.MetroColorStyle.Blue;
            this.btnDodaj.StyleManager = null;
            this.btnDodaj.TabIndex = 4;
            this.btnDodaj.Text = "Dodaj novu knjigu";
            this.btnDodaj.Theme = MetroFramework.MetroThemeStyle.Light;
            this.btnDodaj.Click += new System.EventHandler(this.btnDodaj_Click);
            // 
            // textBox2
            // 
            this.textBox2.FontSize = MetroFramework.MetroTextBoxSize.Medium;
            this.textBox2.FontWeight = MetroFramework.MetroTextBoxWeight.Regular;
            this.textBox2.Location = new System.Drawing.Point(578, 228);
            this.textBox2.Multiline = false;
            this.textBox2.Name = "textBox2";
            this.textBox2.SelectedText = "";
            this.textBox2.Size = new System.Drawing.Size(474, 30);
            this.textBox2.Style = MetroFramework.MetroColorStyle.Blue;
            this.textBox2.StyleManager = null;
            this.textBox2.TabIndex = 6;
            this.textBox2.Theme = MetroFramework.MetroThemeStyle.Light;
            this.textBox2.UseStyleColors = false;
            // 
            // textBox1
            // 
            this.textBox1.FontSize = MetroFramework.MetroTextBoxSize.Medium;
            this.textBox1.FontWeight = MetroFramework.MetroTextBoxWeight.Regular;
            this.textBox1.Location = new System.Drawing.Point(32, 228);
            this.textBox1.Multiline = false;
            this.textBox1.Name = "textBox1";
            this.textBox1.SelectedText = "";
            this.textBox1.Size = new System.Drawing.Size(540, 30);
            this.textBox1.Style = MetroFramework.MetroColorStyle.Blue;
            this.textBox1.StyleManager = null;
            this.textBox1.TabIndex = 5;
            this.textBox1.Theme = MetroFramework.MetroThemeStyle.Light;
            this.textBox1.UseStyleColors = false;
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
            this.materialLabel1.Size = new System.Drawing.Size(166, 24);
            this.materialLabel1.TabIndex = 7;
            this.materialLabel1.Text = "Unesi naziv knjige:";
            // 
            // materialLabel2
            // 
            this.materialLabel2.AutoSize = true;
            this.materialLabel2.Depth = 0;
            this.materialLabel2.Font = new System.Drawing.Font("Roboto", 11F);
            this.materialLabel2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialLabel2.Location = new System.Drawing.Point(574, 201);
            this.materialLabel2.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel2.Name = "materialLabel2";
            this.materialLabel2.Size = new System.Drawing.Size(157, 24);
            this.materialLabel2.TabIndex = 8;
            this.materialLabel2.Text = "Unesi ime autora:";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(1700, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(38, 37);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 9;
            this.pictureBox1.TabStop = false;
            // 
            // ListaKnjiga
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1270, 535);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.materialLabel2);
            this.Controls.Add(this.materialLabel1);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.btnDodaj);
            this.Controls.Add(this.dgvKnjige);
            this.Name = "ListaKnjiga";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Lista knjiga";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.ListaKnjiga_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvKnjige)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.DataGridView dgvKnjige;
        private System.Windows.Forms.DataGridViewTextBoxColumn Naslov;
        private System.Windows.Forms.DataGridViewTextBoxColumn Autor;
        private System.Windows.Forms.DataGridViewTextBoxColumn DostupnaKolicina;
        private System.Windows.Forms.DataGridViewTextBoxColumn GodinaIzdavanja;
        private System.Windows.Forms.DataGridViewTextBoxColumn Zanr;
        private MetroFramework.Controls.MetroButton btnDodaj;
        private MetroFramework.Controls.MetroTextBox textBox2;
        private MetroFramework.Controls.MetroTextBox textBox1;
        private MaterialSkin.Controls.MaterialLabel materialLabel1;
        private MaterialSkin.Controls.MaterialLabel materialLabel2;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}