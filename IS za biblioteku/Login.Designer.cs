namespace IS_za_biblioteku
{
    partial class Login
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
            this.materialLabel2 = new MaterialSkin.Controls.MaterialLabel();
            this.materialLabel3 = new MaterialSkin.Controls.MaterialLabel();
            this.lblPrijava = new MaterialSkin.Controls.MaterialLabel();
            this.btnPrijaviSe = new MaterialSkin.Controls.MaterialRaisedButton();
            this.tbLozinka = new MaterialSkin.Controls.MaterialSingleLineTextField();
            this.tbKorisnickoIme = new MaterialSkin.Controls.MaterialSingleLineTextField();
            this.SuspendLayout();
            // 
            // materialLabel2
            // 
            this.materialLabel2.AutoSize = true;
            this.materialLabel2.Depth = 0;
            this.materialLabel2.Font = new System.Drawing.Font("Roboto", 11F);
            this.materialLabel2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialLabel2.Location = new System.Drawing.Point(102, 146);
            this.materialLabel2.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel2.Name = "materialLabel2";
            this.materialLabel2.Size = new System.Drawing.Size(137, 24);
            this.materialLabel2.TabIndex = 3;
            this.materialLabel2.Text = "Korisničko ime";
            // 
            // materialLabel3
            // 
            this.materialLabel3.AutoSize = true;
            this.materialLabel3.Depth = 0;
            this.materialLabel3.Font = new System.Drawing.Font("Roboto", 11F);
            this.materialLabel3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialLabel3.Location = new System.Drawing.Point(102, 226);
            this.materialLabel3.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel3.Name = "materialLabel3";
            this.materialLabel3.Size = new System.Drawing.Size(75, 24);
            this.materialLabel3.TabIndex = 4;
            this.materialLabel3.Text = "Lozinka";
            // 
            // lblPrijava
            // 
            this.lblPrijava.AutoSize = true;
            this.lblPrijava.Depth = 0;
            this.lblPrijava.Font = new System.Drawing.Font("Roboto", 11F);
            this.lblPrijava.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblPrijava.Location = new System.Drawing.Point(217, 66);
            this.lblPrijava.MouseState = MaterialSkin.MouseState.HOVER;
            this.lblPrijava.Name = "lblPrijava";
            this.lblPrijava.Size = new System.Drawing.Size(67, 24);
            this.lblPrijava.TabIndex = 5;
            this.lblPrijava.Text = "Prijava";
            // 
            // btnPrijaviSe
            // 
            this.btnPrijaviSe.Depth = 0;
            this.btnPrijaviSe.Location = new System.Drawing.Point(106, 318);
            this.btnPrijaviSe.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnPrijaviSe.Name = "btnPrijaviSe";
            this.btnPrijaviSe.Primary = true;
            this.btnPrijaviSe.Size = new System.Drawing.Size(300, 40);
            this.btnPrijaviSe.TabIndex = 6;
            this.btnPrijaviSe.Text = "Prijavi se";
            this.btnPrijaviSe.UseVisualStyleBackColor = true;
            this.btnPrijaviSe.Click += new System.EventHandler(this.btnPrijaviSe_Click);
            // 
            // tbLozinka
            // 
            this.tbLozinka.Depth = 0;
            this.tbLozinka.Hint = "";
            this.tbLozinka.Location = new System.Drawing.Point(106, 253);
            this.tbLozinka.MouseState = MaterialSkin.MouseState.HOVER;
            this.tbLozinka.Name = "tbLozinka";
            this.tbLozinka.PasswordChar = '\0';
            this.tbLozinka.SelectedText = "";
            this.tbLozinka.SelectionLength = 0;
            this.tbLozinka.SelectionStart = 0;
            this.tbLozinka.Size = new System.Drawing.Size(384, 28);
            this.tbLozinka.TabIndex = 8;
            this.tbLozinka.UseSystemPasswordChar = true;
            // 
            // tbKorisnickoIme
            // 
            this.tbKorisnickoIme.Depth = 0;
            this.tbKorisnickoIme.Hint = "";
            this.tbKorisnickoIme.Location = new System.Drawing.Point(106, 174);
            this.tbKorisnickoIme.MouseState = MaterialSkin.MouseState.HOVER;
            this.tbKorisnickoIme.Name = "tbKorisnickoIme";
            this.tbKorisnickoIme.PasswordChar = '\0';
            this.tbKorisnickoIme.SelectedText = "";
            this.tbKorisnickoIme.SelectionLength = 0;
            this.tbKorisnickoIme.SelectionStart = 0;
            this.tbKorisnickoIme.Size = new System.Drawing.Size(384, 28);
            this.tbKorisnickoIme.TabIndex = 9;
            this.tbKorisnickoIme.UseSystemPasswordChar = false;
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(493, 436);
            this.Controls.Add(this.tbKorisnickoIme);
            this.Controls.Add(this.tbLozinka);
            this.Controls.Add(this.btnPrijaviSe);
            this.Controls.Add(this.lblPrijava);
            this.Controls.Add(this.materialLabel3);
            this.Controls.Add(this.materialLabel2);
            this.Name = "Login";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Prijavi se";
            this.Load += new System.EventHandler(this.Login_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private MaterialSkin.Controls.MaterialLabel materialLabel2;
        private MaterialSkin.Controls.MaterialLabel materialLabel3;
        private MaterialSkin.Controls.MaterialLabel lblPrijava;
        private MaterialSkin.Controls.MaterialRaisedButton btnPrijaviSe;
        private MaterialSkin.Controls.MaterialSingleLineTextField tbLozinka;
        private MaterialSkin.Controls.MaterialSingleLineTextField tbKorisnickoIme;
    }
}