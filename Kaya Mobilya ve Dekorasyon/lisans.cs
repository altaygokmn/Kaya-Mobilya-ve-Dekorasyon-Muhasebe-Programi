using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FireSharp;
using FireSharp.Config;
using FireSharp.Response;
using FireSharp.Interfaces;
using System.Data.SQLite;

namespace Kaya_Mobilya_ve_Dekorasyon
{
    public partial class lisans : Form
    {
        public lisans()
        {
            InitializeComponent();
        }

        IFirebaseConfig fc = new FirebaseConfig()
        {
            AuthSecret = "hvPYAAJGV0tyFGAdvydpHRwbLEFIvQTrnilIan3y",
            BasePath = "https://lisans-19952-default-rtdb.europe-west1.firebasedatabase.app/"
        };

        IFirebaseClient client;
        
        private void lisans_Load(object sender, EventArgs e)
        {
            try
            {
                client = new FireSharp.FirebaseClient(fc);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Veri Tabanına Bağlanamadı: " + ex.Message);
            }
        }


        private void btnGönder_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(tbxLisans.Text))
                {
                    MessageBox.Show("Lisans anahtarı giriniz.");
                    return;
                }

                FirebaseResponse response = client.Get("Lisanstbl/" + tbxLisans.Text);
                FrLisans key = response.ResultAs<FrLisans>();

                if (key != null && key.Key == tbxLisans.Text)
                {
                    
                    

                    MessageBox.Show("Lisans Aktifleşti");
                    
                }
                else
                {
                    MessageBox.Show("Geçersiz Lisans Anahtarı");
                }
            }
            catch
            {
                MessageBox.Show("Sorun Oluştu");
            }
        }


        private void lblLisansAl_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Lisans edinmek için ulaşın:"+"\n"+"acgd3141@gmail.com","Destek");

        }
    }

    public class FrLisans
    {
        public string Key { get; set; }
    }

}
