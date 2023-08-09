namespace Kaya_Mobilya_ve_Dekorasyon
{
    partial class girisForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(girisForm));
            this.gbxLogin = new System.Windows.Forms.GroupBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnChangePassword = new System.Windows.Forms.Label();
            this.lblLoginPageInfo = new System.Windows.Forms.Label();
            this.btnGiris = new System.Windows.Forms.Button();
            this.tbxPassword = new System.Windows.Forms.TextBox();
            this.tbxKullaniciAdi = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.lblKullaniciAdi = new System.Windows.Forms.Label();
            this.SifremiUnuttum = new System.Windows.Forms.Label();
            this.gbxLogin.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // gbxLogin
            // 
            this.gbxLogin.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.gbxLogin.BackColor = System.Drawing.Color.White;
            this.gbxLogin.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.gbxLogin.Controls.Add(this.SifremiUnuttum);
            this.gbxLogin.Controls.Add(this.pictureBox1);
            this.gbxLogin.Controls.Add(this.btnChangePassword);
            this.gbxLogin.Controls.Add(this.lblLoginPageInfo);
            this.gbxLogin.Controls.Add(this.btnGiris);
            this.gbxLogin.Controls.Add(this.tbxPassword);
            this.gbxLogin.Controls.Add(this.tbxKullaniciAdi);
            this.gbxLogin.Controls.Add(this.lblPassword);
            this.gbxLogin.Controls.Add(this.lblKullaniciAdi);
            this.gbxLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.gbxLogin.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.gbxLogin.Location = new System.Drawing.Point(327, 78);
            this.gbxLogin.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gbxLogin.Name = "gbxLogin";
            this.gbxLogin.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gbxLogin.Size = new System.Drawing.Size(405, 603);
            this.gbxLogin.TabIndex = 1;
            this.gbxLogin.TabStop = false;
            this.gbxLogin.Text = "Yönetici Girişi";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::Kaya_Mobilya_ve_Dekorasyon.Properties.Resources.account_icon;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.pictureBox1.Location = new System.Drawing.Point(3, 25);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(399, 184);
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            // 
            // btnChangePassword
            // 
            this.btnChangePassword.AutoSize = true;
            this.btnChangePassword.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnChangePassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnChangePassword.ForeColor = System.Drawing.SystemColors.GrayText;
            this.btnChangePassword.Location = new System.Drawing.Point(161, 438);
            this.btnChangePassword.Name = "btnChangePassword";
            this.btnChangePassword.Size = new System.Drawing.Size(92, 18);
            this.btnChangePassword.TabIndex = 6;
            this.btnChangePassword.Text = "Şifre Değiştir";
            this.btnChangePassword.Click += new System.EventHandler(this.btnChangePassword_Click);
            // 
            // lblLoginPageInfo
            // 
            this.lblLoginPageInfo.AutoSize = true;
            this.lblLoginPageInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblLoginPageInfo.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblLoginPageInfo.Location = new System.Drawing.Point(104, 250);
            this.lblLoginPageInfo.Name = "lblLoginPageInfo";
            this.lblLoginPageInfo.Size = new System.Drawing.Size(208, 25);
            this.lblLoginPageInfo.TabIndex = 5;
            this.lblLoginPageInfo.Text = "Hesabınıza Giriş Yapın";
            this.lblLoginPageInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnGiris
            // 
            this.btnGiris.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(62)))), ((int)(((byte)(70)))));
            this.btnGiris.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGiris.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGiris.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnGiris.ForeColor = System.Drawing.Color.White;
            this.btnGiris.Location = new System.Drawing.Point(34, 497);
            this.btnGiris.Name = "btnGiris";
            this.btnGiris.Size = new System.Drawing.Size(339, 54);
            this.btnGiris.TabIndex = 3;
            this.btnGiris.Text = "Giriş";
            this.btnGiris.UseVisualStyleBackColor = false;
            this.btnGiris.Click += new System.EventHandler(this.btnGiris_Click);
            // 
            // tbxPassword
            // 
            this.tbxPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbxPassword.Font = new System.Drawing.Font("HoloLens MDL2 Assets", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbxPassword.Location = new System.Drawing.Point(34, 404);
            this.tbxPassword.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbxPassword.Name = "tbxPassword";
            this.tbxPassword.PasswordChar = '*';
            this.tbxPassword.Size = new System.Drawing.Size(339, 26);
            this.tbxPassword.TabIndex = 1;
            this.tbxPassword.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbxPassword_KeyPress);
            // 
            // tbxKullaniciAdi
            // 
            this.tbxKullaniciAdi.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbxKullaniciAdi.Font = new System.Drawing.Font("HoloLens MDL2 Assets", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbxKullaniciAdi.Location = new System.Drawing.Point(34, 340);
            this.tbxKullaniciAdi.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbxKullaniciAdi.Name = "tbxKullaniciAdi";
            this.tbxKullaniciAdi.Size = new System.Drawing.Size(339, 26);
            this.tbxKullaniciAdi.TabIndex = 0;
            this.tbxKullaniciAdi.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbxKullaniciAdi_KeyPress);
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblPassword.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblPassword.Location = new System.Drawing.Point(29, 375);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(42, 20);
            this.lblPassword.TabIndex = 2;
            this.lblPassword.Text = "Şifre";
            // 
            // lblKullaniciAdi
            // 
            this.lblKullaniciAdi.AutoSize = true;
            this.lblKullaniciAdi.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblKullaniciAdi.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblKullaniciAdi.Location = new System.Drawing.Point(29, 311);
            this.lblKullaniciAdi.Name = "lblKullaniciAdi";
            this.lblKullaniciAdi.Size = new System.Drawing.Size(93, 20);
            this.lblKullaniciAdi.TabIndex = 1;
            this.lblKullaniciAdi.Text = "Kullanıcı Adı";
            // 
            // SifremiUnuttum
            // 
            this.SifremiUnuttum.AutoSize = true;
            this.SifremiUnuttum.Cursor = System.Windows.Forms.Cursors.Hand;
            this.SifremiUnuttum.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.SifremiUnuttum.ForeColor = System.Drawing.SystemColors.GrayText;
            this.SifremiUnuttum.Location = new System.Drawing.Point(259, 438);
            this.SifremiUnuttum.Name = "SifremiUnuttum";
            this.SifremiUnuttum.Size = new System.Drawing.Size(114, 18);
            this.SifremiUnuttum.TabIndex = 8;
            this.SifremiUnuttum.Text = "Şifremi Unuttum";
            this.SifremiUnuttum.Click += new System.EventHandler(this.SifremiUnuttum_Click);
            // 
            // girisForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            this.ClientSize = new System.Drawing.Size(1028, 609);
            this.Controls.Add(this.gbxLogin);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "girisForm";
            this.Text = "Giriş";
            this.Load += new System.EventHandler(this.girisForm_Load);
            this.gbxLogin.ResumeLayout(false);
            this.gbxLogin.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbxLogin;
        private System.Windows.Forms.Label btnChangePassword;
        private System.Windows.Forms.Label lblLoginPageInfo;
        private System.Windows.Forms.Button btnGiris;
        private System.Windows.Forms.TextBox tbxPassword;
        private System.Windows.Forms.TextBox tbxKullaniciAdi;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.Label lblKullaniciAdi;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label SifremiUnuttum;
    }
}