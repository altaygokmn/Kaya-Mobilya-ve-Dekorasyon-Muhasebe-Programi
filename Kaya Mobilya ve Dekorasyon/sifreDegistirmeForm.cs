using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FireSharp;
using FireSharp.Config;
using FireSharp.Response;
using FireSharp.Interfaces;
using static Kaya_Mobilya_ve_Dekorasyon.girisForm;

namespace Kaya_Mobilya_ve_Dekorasyon
{
    public partial class sifreDegistirmeForm : Form
    {
        public sifreDegistirmeForm()
        {
            InitializeComponent();
        }
        IFirebaseConfig fc = new FirebaseConfig()
        {
            AuthSecret = "hvPYAAJGV0tyFGAdvydpHRwbLEFIvQTrnilIan3y",
            BasePath = "https://lisans-19952-default-rtdb.europe-west1.firebasedatabase.app/"
        };
        private void lblLeftArrow_Click(object sender, EventArgs e)
        {
            girisForm girisForm = new girisForm();
            girisForm.Show();
            this.Close();
        }

        private void sifreDegistirmeForm_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            this.StartPosition = FormStartPosition.CenterScreen;
         
            this.TopMost = true;
           
        }
        // Sifreyi unuttum için
        IFirebaseClient client;
        private void btnPasswordApprove_Click(object sender, EventArgs e)
        {
            string oldPassword = tbxEskiSifre.Text;
            string newPassword = tbxYeniSifre.Text;
            try
            {
                client = new FireSharp.FirebaseClient(fc);
               
                FrLisans frs = new FrLisans()
                {
                    key = newPassword
                };
                var setet = client.Set("sifretbl/" + newPassword, frs);
            }
            catch 
            {
                MessageBox.Show("İnternete Bağlı olmalısınız. Şifre daha sonra unutmanız halinde İnternet üzerinden saklanıyor");
                
            }
        
            if (ChangePassword(oldPassword, newPassword))
            {
                MessageBox.Show("Şifreniz başarıyla değiştirildi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                girisForm girisform = new girisForm(); 
                girisform.Show();
                this.Close();//şifre 2023
            }
            else
            {
                MessageBox.Show("Kullanıcı adı veya eski şifre yanlış!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private bool ChangePassword(string oldPassword, string newPassword)
        {
            string connectionString = "Data Source=MyDatabase.db;Version=3;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                // Eski şifreyi kontrol etme sorgusu
                string checkPasswordQuery = "SELECT COUNT(*) FROM Kullanicilar WHERE KullaniciAdi=@username AND Sifre=@oldPassword;";

                using (SQLiteCommand command = new SQLiteCommand(checkPasswordQuery, connection))
                {
                    command.Parameters.AddWithValue("@username", "admin");
                    command.Parameters.AddWithValue("@oldPassword", oldPassword);

                    int count = Convert.ToInt32(command.ExecuteScalar());

                    if (count == 0)
                    {
                        return false; // Eski şifre yanlış
                    }
                }

                // Yeni şifreyi güncelleme sorgusu
                string updatePasswordQuery = "UPDATE Kullanicilar SET Sifre=@newPassword WHERE KullaniciAdi=@username;";

                using (SQLiteCommand command = new SQLiteCommand(updatePasswordQuery, connection))
                {
                    command.Parameters.AddWithValue("@newPassword", newPassword);
                    command.Parameters.AddWithValue("@username", "admin");

                    command.ExecuteNonQuery();
                }

                return true;
            }
        }

        private void tbxEskiSifre_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                tbxYeniSifre.Focus();
            }
        }

        private void tbxYeniSifre_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true; 
                tbxYeniSifreTekrar.Focus();
            }
        }

        private void tbxYeniSifreTekrar_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                string oldPassword = tbxEskiSifre.Text;
                string newPassword = tbxYeniSifre.Text;

                if (ChangePassword(oldPassword, newPassword))
                {
                    MessageBox.Show("Şifreniz başarıyla değiştirildi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    girisForm girisform = new girisForm();
                    girisform.Show();
                    this.Close();//şifre 2023
                }
                else
                {
                    MessageBox.Show("Kullanıcı adı veya eski şifre yanlış!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        public class FrLisans
        {
            public string key { get; set; }
        }
    }
}
