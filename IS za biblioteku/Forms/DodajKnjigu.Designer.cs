namespace IS_za_biblioteku.Forms
{
    partial class DodajKnjigu
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DodajKnjigu));
            this.cmbGodine = new MetroFramework.Controls.MetroComboBox();
            this.metroLabel5 = new MetroFramework.Controls.MetroLabel();
            this.metroButton1 = new MetroFramework.Controls.MetroButton();
            this.metroLabel4 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel3 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel2 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            this.btnSpremi = new MaterialSkin.Controls.MaterialRaisedButton();
            this.tbDostupnaKolicina = new MetroFramework.Controls.MetroTextBox();
            this.cmbZanr = new MetroFramework.Controls.MetroComboBox();
            this.tbNazivAutora = new MetroFramework.Controls.MetroTextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tbNazivKnjige = new MetroFramework.Controls.MetroTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbGodine
            // 
            this.cmbGodine.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbGodine.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGodine.FontSize = MetroFramework.MetroLinkSize.Medium;
            this.cmbGodine.FontWeight = MetroFramework.MetroLinkWeight.Regular;
            this.cmbGodine.FormattingEnabled = true;
            this.cmbGodine.ItemHeight = 24;
            this.cmbGodine.Location = new System.Drawing.Point(147, 424);
            this.cmbGodine.Name = "cmbGodine";
            this.cmbGodine.Size = new System.Drawing.Size(289, 30);
            this.cmbGodine.Style = MetroFramework.MetroColorStyle.Blue;
            this.cmbGodine.StyleManager = null;
            this.cmbGodine.TabIndex = 25;
            this.cmbGodine.Theme = MetroFramework.MetroThemeStyle.Light;
            // 
            // metroLabel5
            // 
            this.metroLabel5.AutoSize = true;
            this.metroLabel5.CustomBackground = false;
            this.metroLabel5.FontSize = MetroFramework.MetroLabelSize.Medium;
            this.metroLabel5.FontWeight = MetroFramework.MetroLabelWeight.Light;
            this.metroLabel5.LabelMode = MetroFramework.Controls.MetroLabelMode.Default;
            this.metroLabel5.Location = new System.Drawing.Point(147, 401);
            this.metroLabel5.Name = "metroLabel5";
            this.metroLabel5.Size = new System.Drawing.Size(114, 20);
            this.metroLabel5.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroLabel5.StyleManager = null;
            this.metroLabel5.TabIndex = 24;
            this.metroLabel5.Text = "Godina izdavanja";
            this.metroLabel5.Theme = MetroFramework.MetroThemeStyle.Light;
            this.metroLabel5.UseStyleColors = false;
            // 
            // metroButton1
            // 
            this.metroButton1.Highlight = false;
            this.metroButton1.Location = new System.Drawing.Point(147, 472);
            this.metroButton1.Name = "metroButton1";
            this.metroButton1.Size = new System.Drawing.Size(149, 33);
            this.metroButton1.Style = MetroFramework.MetroColorStyle.Red;
            this.metroButton1.StyleManager = null;
            this.metroButton1.TabIndex = 23;
            this.metroButton1.Text = "ODUSTANI";
            this.metroButton1.Theme = MetroFramework.MetroThemeStyle.Light;
            this.metroButton1.Click += new System.EventHandler(this.metroButton1_Click_1);
            // 
            // metroLabel4
            // 
            this.metroLabel4.AutoSize = true;
            this.metroLabel4.CustomBackground = false;
            this.metroLabel4.FontSize = MetroFramework.MetroLabelSize.Medium;
            this.metroLabel4.FontWeight = MetroFramework.MetroLabelWeight.Light;
            this.metroLabel4.LabelMode = MetroFramework.Controls.MetroLabelMode.Default;
            this.metroLabel4.Location = new System.Drawing.Point(147, 332);
            this.metroLabel4.Name = "metroLabel4";
            this.metroLabel4.Size = new System.Drawing.Size(118, 20);
            this.metroLabel4.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroLabel4.StyleManager = null;
            this.metroLabel4.TabIndex = 22;
            this.metroLabel4.Text = "Dostupna količina";
            this.metroLabel4.Theme = MetroFramework.MetroThemeStyle.Light;
            this.metroLabel4.UseStyleColors = false;
            // 
            // metroLabel3
            // 
            this.metroLabel3.AutoSize = true;
            this.metroLabel3.CustomBackground = false;
            this.metroLabel3.FontSize = MetroFramework.MetroLabelSize.Medium;
            this.metroLabel3.FontWeight = MetroFramework.MetroLabelWeight.Light;
            this.metroLabel3.LabelMode = MetroFramework.Controls.MetroLabelMode.Default;
            this.metroLabel3.Location = new System.Drawing.Point(147, 271);
            this.metroLabel3.Name = "metroLabel3";
            this.metroLabel3.Size = new System.Drawing.Size(38, 20);
            this.metroLabel3.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroLabel3.StyleManager = null;
            this.metroLabel3.TabIndex = 21;
            this.metroLabel3.Text = "Žanr";
            this.metroLabel3.Theme = MetroFramework.MetroThemeStyle.Light;
            this.metroLabel3.UseStyleColors = false;
            // 
            // metroLabel2
            // 
            this.metroLabel2.AutoSize = true;
            this.metroLabel2.CustomBackground = false;
            this.metroLabel2.FontSize = MetroFramework.MetroLabelSize.Medium;
            this.metroLabel2.FontWeight = MetroFramework.MetroLabelWeight.Light;
            this.metroLabel2.LabelMode = MetroFramework.Controls.MetroLabelMode.Default;
            this.metroLabel2.Location = new System.Drawing.Point(147, 210);
            this.metroLabel2.Name = "metroLabel2";
            this.metroLabel2.Size = new System.Drawing.Size(75, 20);
            this.metroLabel2.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroLabel2.StyleManager = null;
            this.metroLabel2.TabIndex = 20;
            this.metroLabel2.Text = "Ime autora";
            this.metroLabel2.Theme = MetroFramework.MetroThemeStyle.Light;
            this.metroLabel2.UseStyleColors = false;
            // 
            // metroLabel1
            // 
            this.metroLabel1.AutoSize = true;
            this.metroLabel1.CustomBackground = false;
            this.metroLabel1.FontSize = MetroFramework.MetroLabelSize.Medium;
            this.metroLabel1.FontWeight = MetroFramework.MetroLabelWeight.Light;
            this.metroLabel1.LabelMode = MetroFramework.Controls.MetroLabelMode.Default;
            this.metroLabel1.Location = new System.Drawing.Point(147, 147);
            this.metroLabel1.Name = "metroLabel1";
            this.metroLabel1.Size = new System.Drawing.Size(85, 20);
            this.metroLabel1.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroLabel1.StyleManager = null;
            this.metroLabel1.TabIndex = 19;
            this.metroLabel1.Text = "Naziv knjige";
            this.metroLabel1.Theme = MetroFramework.MetroThemeStyle.Light;
            this.metroLabel1.UseStyleColors = false;
            // 
            // btnSpremi
            // 
            this.btnSpremi.Depth = 0;
            this.btnSpremi.Location = new System.Drawing.Point(302, 472);
            this.btnSpremi.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnSpremi.Name = "btnSpremi";
            this.btnSpremi.Primary = true;
            this.btnSpremi.Size = new System.Drawing.Size(134, 33);
            this.btnSpremi.TabIndex = 18;
            this.btnSpremi.Text = "Spremi";
            this.btnSpremi.UseVisualStyleBackColor = true;
            this.btnSpremi.Click += new System.EventHandler(this.btnSpremi_Click);
            // 
            // tbDostupnaKolicina
            // 
            this.tbDostupnaKolicina.FontSize = MetroFramework.MetroTextBoxSize.Small;
            this.tbDostupnaKolicina.FontWeight = MetroFramework.MetroTextBoxWeight.Regular;
            this.tbDostupnaKolicina.Location = new System.Drawing.Point(147, 358);
            this.tbDostupnaKolicina.Multiline = false;
            this.tbDostupnaKolicina.Name = "tbDostupnaKolicina";
            this.tbDostupnaKolicina.SelectedText = "";
            this.tbDostupnaKolicina.Size = new System.Drawing.Size(289, 30);
            this.tbDostupnaKolicina.Style = MetroFramework.MetroColorStyle.Blue;
            this.tbDostupnaKolicina.StyleManager = null;
            this.tbDostupnaKolicina.TabIndex = 17;
            this.tbDostupnaKolicina.Theme = MetroFramework.MetroThemeStyle.Light;
            this.tbDostupnaKolicina.UseStyleColors = false;
            // 
            // cmbZanr
            // 
            this.cmbZanr.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbZanr.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbZanr.FontSize = MetroFramework.MetroLinkSize.Medium;
            this.cmbZanr.FontWeight = MetroFramework.MetroLinkWeight.Regular;
            this.cmbZanr.FormattingEnabled = true;
            this.cmbZanr.ItemHeight = 24;
            this.cmbZanr.Location = new System.Drawing.Point(147, 294);
            this.cmbZanr.Name = "cmbZanr";
            this.cmbZanr.Size = new System.Drawing.Size(289, 30);
            this.cmbZanr.Style = MetroFramework.MetroColorStyle.Blue;
            this.cmbZanr.StyleManager = null;
            this.cmbZanr.TabIndex = 16;
            this.cmbZanr.Theme = MetroFramework.MetroThemeStyle.Light;
            // 
            // tbNazivAutora
            // 
            this.tbNazivAutora.FontSize = MetroFramework.MetroTextBoxSize.Small;
            this.tbNazivAutora.FontWeight = MetroFramework.MetroTextBoxWeight.Regular;
            this.tbNazivAutora.Location = new System.Drawing.Point(147, 234);
            this.tbNazivAutora.Multiline = false;
            this.tbNazivAutora.Name = "tbNazivAutora";
            this.tbNazivAutora.SelectedText = "";
            this.tbNazivAutora.Size = new System.Drawing.Size(289, 30);
            this.tbNazivAutora.Style = MetroFramework.MetroColorStyle.Blue;
            this.tbNazivAutora.StyleManager = null;
            this.tbNazivAutora.TabIndex = 15;
            this.tbNazivAutora.Theme = MetroFramework.MetroThemeStyle.Light;
            this.tbNazivAutora.UseStyleColors = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(238, 47);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(100, 87);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 14;
            this.pictureBox1.TabStop = false;
            // 
            // tbNazivKnjige
            // 
            this.tbNazivKnjige.FontSize = MetroFramework.MetroTextBoxSize.Small;
            this.tbNazivKnjige.FontWeight = MetroFramework.MetroTextBoxWeight.Regular;
            this.tbNazivKnjige.Location = new System.Drawing.Point(147, 173);
            this.tbNazivKnjige.Multiline = false;
            this.tbNazivKnjige.Name = "tbNazivKnjige";
            this.tbNazivKnjige.SelectedText = "";
            this.tbNazivKnjige.Size = new System.Drawing.Size(289, 30);
            this.tbNazivKnjige.Style = MetroFramework.MetroColorStyle.Blue;
            this.tbNazivKnjige.StyleManager = null;
            this.tbNazivKnjige.TabIndex = 13;
            this.tbNazivKnjige.Theme = MetroFramework.MetroThemeStyle.Light;
            this.tbNazivKnjige.UseStyleColors = false;
            // 
            // DodajKnjigu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(582, 553);
            this.Controls.Add(this.cmbGodine);
            this.Controls.Add(this.metroLabel5);
            this.Controls.Add(this.metroButton1);
            this.Controls.Add(this.metroLabel4);
            this.Controls.Add(this.metroLabel3);
            this.Controls.Add(this.metroLabel2);
            this.Controls.Add(this.metroLabel1);
            this.Controls.Add(this.btnSpremi);
            this.Controls.Add(this.tbDostupnaKolicina);
            this.Controls.Add(this.cmbZanr);
            this.Controls.Add(this.tbNazivAutora);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.tbNazivKnjige);
            this.Name = "DodajKnjigu";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Dodaj novu knjigu";
            this.Load += new System.EventHandler(this.DodajKnjigu_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MetroFramework.Controls.MetroComboBox cmbGodine;
        private MetroFramework.Controls.MetroLabel metroLabel5;
        private MetroFramework.Controls.MetroButton metroButton1;
        private MetroFramework.Controls.MetroLabel metroLabel4;
        private MetroFramework.Controls.MetroLabel metroLabel3;
        private MetroFramework.Controls.MetroLabel metroLabel2;
        private MetroFramework.Controls.MetroLabel metroLabel1;
        private MaterialSkin.Controls.MaterialRaisedButton btnSpremi;
        private MetroFramework.Controls.MetroTextBox tbDostupnaKolicina;
        private MetroFramework.Controls.MetroComboBox cmbZanr;
        private MetroFramework.Controls.MetroTextBox tbNazivAutora;
        private System.Windows.Forms.PictureBox pictureBox1;
        private MetroFramework.Controls.MetroTextBox tbNazivKnjige;
    }
}