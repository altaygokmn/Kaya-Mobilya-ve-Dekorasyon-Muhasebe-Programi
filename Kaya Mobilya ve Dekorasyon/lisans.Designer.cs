namespace Kaya_Mobilya_ve_Dekorasyon
{
    partial class lisans
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(lisans));
            this.tbxLisans = new System.Windows.Forms.TextBox();
            this.btnGönder = new System.Windows.Forms.Button();
            this.lblLisans = new System.Windows.Forms.Label();
            this.lblLisansAl = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // tbxLisans
            // 
            this.tbxLisans.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbxLisans.Font = new System.Drawing.Font("Franklin Gothic Book", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.tbxLisans.Location = new System.Drawing.Point(168, 270);
            this.tbxLisans.Name = "tbxLisans";
            this.tbxLisans.Size = new System.Drawing.Size(405, 30);
            this.tbxLisans.TabIndex = 0;
            // 
            // btnGönder
            // 
            this.btnGönder.BackColor = System.Drawing.Color.White;
            this.btnGönder.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnGönder.Font = new System.Drawing.Font("Franklin Gothic Book", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnGönder.Location = new System.Drawing.Point(451, 306);
            this.btnGönder.Name = "btnGönder";
            this.btnGönder.Size = new System.Drawing.Size(122, 30);
            this.btnGönder.TabIndex = 1;
            this.btnGönder.Text = "Gönder";
            this.btnGönder.UseVisualStyleBackColor = false;
            this.btnGönder.Click += new System.EventHandler(this.btnGönder_Click);
            // 
            // lblLisans
            // 
            this.lblLisans.AutoSize = true;
            this.lblLisans.Font = new System.Drawing.Font("Franklin Gothic Book", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblLisans.Location = new System.Drawing.Point(163, 242);
            this.lblLisans.Name = "lblLisans";
            this.lblLisans.Size = new System.Drawing.Size(177, 25);
            this.lblLisans.TabIndex = 2;
            this.lblLisans.Text = "Lütfen Lisans Girin:";
            // 
            // lblLisansAl
            // 
            this.lblLisansAl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblLisansAl.AutoSize = true;
            this.lblLisansAl.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblLisansAl.ForeColor = System.Drawing.Color.Blue;
            this.lblLisansAl.Location = new System.Drawing.Point(712, 425);
            this.lblLisansAl.Name = "lblLisansAl";
            this.lblLisansAl.Size = new System.Drawing.Size(61, 16);
            this.lblLisansAl.TabIndex = 3;
            this.lblLisansAl.Text = "Lisans Al";
            this.lblLisansAl.Click += new System.EventHandler(this.lblLisansAl_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.lblLisans);
            this.panel1.Controls.Add(this.btnGönder);
            this.panel1.Controls.Add(this.tbxLisans);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(800, 450);
            this.panel1.TabIndex = 4;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::Kaya_Mobilya_ve_Dekorasyon.Properties.Resources.licence;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox1.Cursor = System.Windows.Forms.Cursors.Help;
            this.pictureBox1.Location = new System.Drawing.Point(168, 34);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(405, 163);
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // lisans
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.lblLisansAl);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "lisans";
            this.Text = "Lisans Bilgilendirme";
            this.Load += new System.EventHandler(this.lisans_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbxLisans;
        private System.Windows.Forms.Button btnGönder;
        private System.Windows.Forms.Label lblLisans;
        private System.Windows.Forms.Label lblLisansAl;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}