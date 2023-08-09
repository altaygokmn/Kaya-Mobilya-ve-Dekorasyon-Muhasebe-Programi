using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FireSharp;
using FireSharp.Config;
using FireSharp.Response;
using FireSharp.Interfaces;

using System.Management;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Windows.Forms.DataVisualization.Charting;

namespace Kaya_Mobilya_ve_Dekorasyon
{

    public partial class girisForm : Form
    {
       
        public string serialKey;
        public girisForm()
        {
            InitializeComponent();
           

        }

        public class FrLisans
        {
            public string key { get; set; }
        }
        

        IFirebaseConfig fc = new FirebaseConfig()
        {
            AuthSecret = "hvPYAAJGV0tyFGAdvydpHRwbLEFIvQTrnilIan3y",
            BasePath = "https://lisans-19952-default-rtdb.europe-west1.firebasedatabase.app/"
        };
        IFirebaseClient client;
        private void CheckLicenseStatus()
        {

            this.WindowState = FormWindowState.Maximized;
            client = new FireSharp.FirebaseClient(fc);
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from Win32_Processor");
            foreach (ManagementObject obj in searcher.Get())
            {
                string cpuMarka = obj["Name"].ToString();
                string cpuCekirdekSayisi = obj["NumberOfCores"].ToString();
                string cpuThreadSayisi = obj["NumberOfLogicalProcessors"].ToString();

                serialKey = GenerateRandomKey(cpuMarka, cpuCekirdekSayisi, cpuThreadSayisi);
            }
            FirebaseResponse response = client.Get("Lisanstbl/" + serialKey);
            FrLisans key = response.ResultAs<FrLisans>();

            if (key == null)
            {
                this.Hide();
                LisansForm Lisansform = new LisansForm();
                Lisansform.Show();
                tbxKullaniciAdi.Enabled = false;
                tbxPassword.Enabled = false;
                btnGiris.Enabled = false;
                btnChangePassword.Enabled = false;
                MessageBox.Show("Lütfen internet bağlantınızı kontrol edin!");
            }





        }
        private void girisForm_Load(object sender, EventArgs e)
        {
            CheckLicenseStatus();
        }
        private string GenerateRandomKey(string cpuMarka, string cpuCekirdekSayisi, string cpuThreadSayisi)
        {
            string cpuBilgileri = cpuMarka + cpuCekirdekSayisi + cpuThreadSayisi;
            byte[] cpuBilgileriBytes = Encoding.UTF8.GetBytes(cpuBilgileri);

            using (var sha256 = new SHA256Managed())
            {
                byte[] hash = sha256.ComputeHash(cpuBilgileriBytes);
                return BitConverter.ToString(hash).Replace("-", string.Empty);
            }
        }

        private void btnGiris_Click(object sender, EventArgs e)
        {
            string username = tbxKullaniciAdi.Text;
            string password = tbxPassword.Text;

            if (CheckCredentials(username, password))
            {
                anaForm anaForm = new anaForm();
                anaForm.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Kullanıcı adı veya şifre yanlış!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

       
        private bool CheckCredentials(string username, string password)
        {
            string connectionString = "Data Source=MyDatabase.db;Version=3;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandText = "SELECT COUNT(*) FROM Kullanicilar WHERE KullaniciAdi=@username AND Sifre=@password;";
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password", password);

                    int count = Convert.ToInt32(command.ExecuteScalar());

                    return count > 0;
                }
            }
        }

        private void btnChangePassword_Click(object sender, EventArgs e)
        {
               sifreDegistirmeForm sifredegistirmeform = new sifreDegistirmeForm();
            sifredegistirmeform.Show();
            this.Hide();
        }

        private void tbxKullaniciAdi_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true; // Eğer bu satır eklenirse, Enter tuşunun basılmasını engellemiş olursunuz.
                tbxPassword.Focus();
            }
        }

        private void tbxPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                string username = tbxKullaniciAdi.Text;
                string password = tbxPassword.Text;

                if (CheckCredentials(username, password))
                {
                    anaForm anaForm = new anaForm();
                    anaForm.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Kullanıcı adı veya şifre yanlış!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void SifremiUnuttum_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Şifrenizi istemek için acdgd3141@gmail.com a mail atın");
        }
    }
}

