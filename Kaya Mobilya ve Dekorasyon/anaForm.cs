using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Drawing;
using System.Linq;

using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using static System.Data.Entity.Infrastructure.Design.Executor;

namespace Kaya_Mobilya_ve_Dekorasyon
{
    public partial class anaForm : Form
    {
        private List<int> personelNoList;
        private List<int> urunNoList;
        public anaForm()
        {
            InitializeComponent();
            personelNoList = GetPersonelNoList();
            urunNoList = GetUrunNoList();
        }

        private void anaForm_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            CreateDatabaseAndTable();
            CreateGiderDurumuTable();
            CreateDatabaseAndTable();
            CreateUrunRaporTablosu();
            CreatePersonelRaporTable();
            LoadPersonelData();
            CalculateAndInsertUrunRapor();
            CalculateAndInsertPersonelRapor();
            LoadUrunData();
            LoadUrunRaporu();
            LoadGiderData();
            LoadPersonelRapor();
            LoadUrunAylikKarZarariChart();
            LoadBorcAlacakTablosu();
            LoadGenelKarZararChart();



        }
        private void CreateDatabaseAndTable()
        {
            string connectionString = "Data Source=MyDatabase.db;Version=3;";
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                // Kullanicilar tablosunu oluşturma sorgusu
                string createUsersTableQuery = "CREATE TABLE IF NOT EXISTS Kullanicilar (Id INTEGER PRIMARY KEY, KullaniciAdi TEXT, Sifre TEXT);";

                using (SQLiteCommand command = new SQLiteCommand(createUsersTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
                // Borc Tablosu
                string createBorcVeyaAlacakTableQuery = "CREATE TABLE IF NOT EXISTS BorcVeyaAlacakTablosu ( IlgiliKurumVeyaKisi TEXT, BorcVeyaAlacakTuru TEXT, BorcVeyaAlacakTutari INTEGER);";

                using (SQLiteCommand command = new SQLiteCommand(createBorcVeyaAlacakTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
                // Personel tablosunu oluşturma sorgusu
                string createPersonelTableQuery = "CREATE TABLE IF NOT EXISTS Personel (PersonelNo INTEGER PRIMARY KEY, Ad TEXT, Soyad TEXT, IsBaslangic DATE, Maas INTEGER);";

                using (SQLiteCommand command = new SQLiteCommand(createPersonelTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }

                

                using (SQLiteCommand command = new SQLiteCommand(createPersonelTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
               

                // Admin kullanıcısını ekleyelim (Varsa, tekrar eklemeyecektir)
                string insertAdminQuery = "INSERT OR IGNORE INTO Kullanicilar (KullaniciAdi, Sifre) VALUES ('admin', '1234');";

                using (SQLiteCommand command = new SQLiteCommand(insertAdminQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            string createUrunTableQuery = "CREATE TABLE IF NOT EXISTS Urun (UrunNo INTEGER PRIMARY KEY, UrunTuru TEXT, IslemTuru TEXT, IslemMiktari INTEGER, IlgiliKurumVeyaKisi TEXT, IslemTutari REAL, OdemeDurumu TEXT, OdenenMiktar REAL, IslemTarihi DATE);";

            using (SQLiteCommand command = new SQLiteCommand(createUrunTableQuery, connection))
            {
                command.ExecuteNonQuery();
            }
                
            }
        }

        private void LoadUrunAylikKarZarariChart()
        {
            string connectionString = "Data Source=MyDatabase.db;Version=3;";
            string selectQuery = "SELECT strftime('%Y-%m', IslemTarihi) AS Ay, SUM(CASE WHEN IslemTuru = 'Satış İşlemi' THEN IslemTutari ELSE 0 END) - SUM(CASE WHEN IslemTuru = 'Satın Alma İşlemi' THEN IslemTutari ELSE 0 END) AS AylikKarZarar " +
                                 "FROM Urunler GROUP BY strftime('%Y-%m', IslemTarihi);";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(selectQuery, connection))
                {
                    DataTable aylikKarZararTable = new DataTable();

                    adapter.Fill(aylikKarZararTable);

                    chartUrununAylikKarVeyaZarari.Series.Clear();

                    // Grafikteki seriyi oluştur
                    var series = new System.Windows.Forms.DataVisualization.Charting.Series
                    {
                        Name = "AylikKarZarar",
                        Color = Color.FromArgb(255, 211, 105), // Negatif değerler için kırmızı renk
                        
                        ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column
                        
                    };
                    
                    // Verileri grafik serisine yükle
                    foreach (DataRow row in aylikKarZararTable.Rows)
                    {
                        string ay = row["Ay"].ToString();
                        double karZarar = Convert.ToDouble(row["AylikKarZarar"]);

                        // Seriye veriyi ekle
                        series.Points.AddXY(ay, karZarar);
                    }

                    // Grafik alanını temizle ve seri ekleyip göster
                    chartUrununAylikKarVeyaZarari.ChartAreas[0].AxisX.Interval = 1;
                    chartUrununAylikKarVeyaZarari.ChartAreas[0].AxisX.LabelStyle.Angle = -45;
                    chartUrununAylikKarVeyaZarari.Series.Add(series);

                    DateTime now = DateTime.Now;
                    double bulundugumuzAyKarZarar = 0;

                    foreach (DataRow row in aylikKarZararTable.Rows)
                    {
                        string ay = row["Ay"].ToString();
                        double karZarar = Convert.ToDouble(row["AylikKarZarar"]);

                        if (ay == $"{now.Year}-{now.Month:00}")
                        {
                            bulundugumuzAyKarZarar = karZarar;
                        }
                        
                        
                    }

                    // Bulunduğumuz ayın kar zararını etikete yazdır
                    lblAylikKarZarar.Text = $"Bulunduğumuz Ay Ürün Alış ve Satışından elde edilen Kar/Zarar: {bulundugumuzAyKarZarar:N2}"; // N2 formatı ile virgülden sonra 2 basamak gösterilir.
                }
            }

        }

        private void LoadGenelKarZararChart()
        {
            string connectionString = "Data Source=MyDatabase.db;Version=3;";

            // Step 1: Calculate AylikKarZarar
            string selectKarZararQuery = "SELECT strftime('%Y-%m', IslemTarihi) AS Ay, " +
                                         "SUM(CASE WHEN IslemTuru = 'Satış İşlemi' THEN IslemTutari ELSE 0 END) - " +
                                         "SUM(CASE WHEN IslemTuru = 'Satın Alma İşlemi' THEN IslemTutari ELSE 0 END) AS AylikKarZarar " +
                                         "FROM Urunler GROUP BY strftime('%Y-%m', IslemTarihi);";

            // Step 2: Calculate AylikToplamGiderTutari
            string selectGiderTutariQuery = "SELECT strftime('%Y-%m', GiderTarihi) AS Ay, " +
                                            "SUM(GiderMiktari) AS AylikToplamGiderTutari " +
                                            "FROM Giderdurumu GROUP BY strftime('%Y-%m', GiderTarihi);";

            // Step 3: Retrieve ToplamMaas
            string selectToplamMaasQuery = "SELECT ToplamMaas FROM PersonelRapor;";
            DateTime now = DateTime.Now;
            string currentMonth = $"{now.Year}-{now.Month:00}";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                // Step 1: Calculate AylikKarZarar
                DataTable aylikKarZararTable = new DataTable();
                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(selectKarZararQuery, connection))
                {
                    adapter.Fill(aylikKarZararTable);
                }

                // Step 2: Calculate AylikToplamGiderTutari
                DataTable aylikToplamGiderTable = new DataTable();
                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(selectGiderTutariQuery, connection))
                {
                    adapter.Fill(aylikToplamGiderTable);
                }

                // Step 3: Retrieve ToplamMaas
                double toplamMaas;
                using (SQLiteCommand command = new SQLiteCommand(selectToplamMaasQuery, connection))
                {
                    object result = command.ExecuteScalar();
                    toplamMaas = result == null ? 0 : Convert.ToDouble(result);
                }

                // Step 4: Calculate GenelKarZararTutari and populate the chart
                chartKarZararGenel.Series.Clear();
                
                var series = new System.Windows.Forms.DataVisualization.Charting.Series
                {
                    Name = "GenelKarZararTutari",
                    ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column,
                    Color = Color.FromArgb(255,210,105)
                    
               

                
                };
                double bulundugumuzAyGenelKarZarar = 0;


                foreach (DataRow row in aylikKarZararTable.Rows)
                {
                    string ay = row["Ay"].ToString();
                    double aylikKarZarar = Convert.ToDouble(row["AylikKarZarar"]);

                    // Find the matching AylikToplamGiderTutari for the current month
                    DataRow[] giderRows = aylikToplamGiderTable.Select("Ay = '" + ay + "'");
                    double aylikToplamGiderTutari = (giderRows.Length > 0) ? Convert.ToDouble(giderRows[0]["AylikToplamGiderTutari"]) : 0;

                    // Calculate GenelKarZararTutari
                    double genelKarZararTutari = aylikKarZarar - aylikToplamGiderTutari - toplamMaas;

                    // Add data point to the chart series
                    series.Points.AddXY(ay, genelKarZararTutari);
                    if (ay == currentMonth)
                    {
                        bulundugumuzAyGenelKarZarar = genelKarZararTutari;
                    }

                }

                // Show the chart
                chartKarZararGenel.ChartAreas[0].AxisX.Interval = 1;
                chartKarZararGenel.ChartAreas[0].AxisX.LabelStyle.Angle = -45;
                chartKarZararGenel.Series.Add(series);
                lblGenelKarZarar.Text = $"Bulunduğumuz Ay Genel Kar/Zarar: {bulundugumuzAyGenelKarZarar:N2}";
            }
        }
        private List<int> GetPersonelNoList()
        {
            List<int> personelNos = new List<int>();
            string connectionString = "Data Source=MyDatabase.db;Version=3;";
            string selectQuery = "SELECT PersonelNo FROM Personel;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(selectQuery, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int personelNo = Convert.ToInt32(reader["PersonelNo"]);
                            personelNos.Add(personelNo);
                        }
                    }
                }
            }

            return personelNos;
        }
        
        private List<int> GetUrunNoList()
        {
            List<int> urunNos = new List<int>();
            string connectionString = "Data Source=MyDatabase.db;Version=3;";
            string selectQuery = "SELECT UrunNo FROM Urunler;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(selectQuery, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int urunNo = Convert.ToInt32(reader["UrunNo"]);
                            urunNos.Add(urunNo);
                        }
                    }
                }
            }

            return urunNos;
        }
        private void LoadBorcAlacakTablosu()
        {
            // Borc ve Alacakları hesapla ve tabloya doldur
            DataTable borcAlacakTable = new DataTable();
            borcAlacakTable.Columns.Add("IlgiliKurumVeyaKisi", typeof(string));
            borcAlacakTable.Columns.Add("BorcVeyaAlacakTuru", typeof(string));
            borcAlacakTable.Columns.Add("BorcVeyaAlacakTutari", typeof(int));

            string selectQuery = "SELECT IlgiliKurumVeyaKisi, IslemTuru, OdemeDurumu, IslemTutari, OdenenMiktar " +
                                 "FROM Urunler";
            string connectionString = "Data Source=MyDatabase.db;Version=3;";
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(selectQuery, connection))
                {
                    DataTable urunTable = new DataTable();
                    adapter.Fill(urunTable);

                    foreach (DataRow row in urunTable.Rows)
                    {
                        string ilgiliKurumVeyaKisi = row["IlgiliKurumVeyaKisi"].ToString();
                        string islemTuru = row["IslemTuru"].ToString();
                        string odemeDurumu = row["OdemeDurumu"].ToString();
                        double islemTutari = Convert.ToDouble(row["IslemTutari"]);
                        double odenenMiktar = Convert.ToDouble(row["OdenenMiktar"]);

                        int borcAlacakTutari = 0;

                        if (islemTuru == "Satış İşlemi")
                        {
                            if (odemeDurumu == "Ödenmedi")
                                borcAlacakTutari = Convert.ToInt32(islemTutari);
                            else if (odemeDurumu == "Belirli bir kısmı ödendi")
                                borcAlacakTutari = Convert.ToInt32(islemTutari - odenenMiktar);
                            else
                                borcAlacakTutari = 0;

                        }
                        else if (islemTuru == "Satın Alma İşlemi")
                        {
                            if (odemeDurumu == "Ödenmedi")
                                borcAlacakTutari = -Convert.ToInt32(islemTutari);
                            else if (odemeDurumu == "Belirli bir kısmı ödendi")
                                borcAlacakTutari = -Convert.ToInt32(islemTutari - odenenMiktar);
                            else
                                borcAlacakTutari = 0;
                        }

                        // İlgiliKurumVeyaKisi özelinde toplam borç/alacak hesapla
                        DataRow[] existingRows = borcAlacakTable.Select($"IlgiliKurumVeyaKisi = '{ilgiliKurumVeyaKisi}'");
                        if (existingRows.Length > 0)
                            existingRows[0]["BorcVeyaAlacakTutari"] = Convert.ToInt32(existingRows[0]["BorcVeyaAlacakTutari"]) + borcAlacakTutari;
                        else
                            borcAlacakTable.Rows.Add(ilgiliKurumVeyaKisi, "", borcAlacakTutari);
                    }
                }
            }

            // Borç ve Alacakları BorcVeyaAlacakTuru'ne göre belirle
            for (int i = borcAlacakTable.Rows.Count - 1; i >= 0; i--)
            {
                DataRow row = borcAlacakTable.Rows[i];
                int borcAlacakTutari = Convert.ToInt32(row["BorcVeyaAlacakTutari"]);

                if (borcAlacakTutari < 0)
                    row["BorcVeyaAlacakTuru"] = "Şirketin borcu bulunuyor.";
                else if (borcAlacakTutari > 0)
                    row["BorcVeyaAlacakTuru"] = "Şirketin alacağı bulunuyor.";
                else
                    borcAlacakTable.Rows.RemoveAt(i); // Remove the row when borcAlacakTutari is 0.
            }

            dgvBorcAlacak.DataSource = borcAlacakTable;
        }
        private void btnUrunNoTemizle_Click(object sender, EventArgs e)
        {
            tbxUrunNo.Clear();
        }
        private bool InsertPersonel(int personelNo, string ad, string soyad, DateTime isBaslangic, double maas)
        {
            string connectionString = "Data Source=MyDatabase.db;Version=3;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                // Personel ekleme sorgusu
                string insertQuery = "INSERT INTO Personel (PersonelNo, Ad, Soyad, IsBaslangic, Maas) " +
                                     "VALUES (@personelNo, @ad, @soyad, @isBaslangic, @maas);";

                using (SQLiteCommand command = new SQLiteCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@personelNo", personelNo);
                    command.Parameters.AddWithValue("@ad", ad);
                    command.Parameters.AddWithValue("@soyad", soyad);
                    command.Parameters.AddWithValue("@isBaslangic", isBaslangic.ToString("yyyy-MM-dd")); // DateTime tipini veritabanı tarih formatına çeviriyoruz
                    command.Parameters.AddWithValue("@maas", maas);

                    int result = command.ExecuteNonQuery();

                    return result > 0;
                }
            }
        }
        private bool InsertUrun(string urunTuru, string islemTuru, int islemMiktari, string ilgiliKurumVeyaKisi, double islemTutari, string odemeDurumu, double odenenMiktar, DateTime islemTarihi)
        {
            string connectionString = "Data Source=MyDatabase.db;Version=3;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                // Ürün ekleme sorgusu
                string insertQuery = "INSERT INTO Urunler ( UrunTuru, IslemTuru, IslemMiktari, IlgiliKurumVeyaKisi, IslemTutari, OdemeDurumu, OdenenMiktar, IslemTarihi) " +
                                     "VALUES (@urunTuru, @islemTuru, @islemMiktari, @ilgiliKurumVeyaKisi, @islemTutari, @odemeDurumu, @odenenMiktar, @islemTarihi);";

                using (SQLiteCommand command = new SQLiteCommand(insertQuery, connection))
                {
                   
                    command.Parameters.AddWithValue("@urunTuru", urunTuru);
                    command.Parameters.AddWithValue("@islemTuru", islemTuru);
                    command.Parameters.AddWithValue("@islemMiktari", islemMiktari);
                    command.Parameters.AddWithValue("@ilgiliKurumVeyaKisi", ilgiliKurumVeyaKisi);
                    command.Parameters.AddWithValue("@islemTutari", islemTutari);
                    command.Parameters.AddWithValue("@odemeDurumu", odemeDurumu);
                    command.Parameters.AddWithValue("@odenenMiktar", odenenMiktar);
                    command.Parameters.AddWithValue("@islemTarihi", islemTarihi.ToString("yyyy-MM-dd"));

                    int result = command.ExecuteNonQuery();

                    return result > 0;
                }
            }
        }  
        private void btnPersonelEkle_Click(object sender, EventArgs e)
        {
            try
            {
                int personelNo = Convert.ToInt32(tbxPersonelNo.Text);
                string ad = tbxAd.Text;
                string soyad = tbxSoyad.Text;
                DateTime isBaslangic = dtpIseGirisTarihi.Value;
                double maas = Convert.ToDouble(tbxAylıkUcret.Text);
                if (personelNoList.Contains(personelNo))
                {
                    MessageBox.Show("Bu personel numarası zaten mevcut!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }



                if (InsertPersonel(personelNo, ad, soyad, isBaslangic, maas))
                {
                    MessageBox.Show("Personel başarıyla eklendi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LoadPersonelData();
                    CalculateAndInsertPersonelRapor();
                    LoadPersonelRapor();
                    LoadGenelKarZararChart();

                    personelNoList.Add(personelNo);
                }
            }

            catch
            {
                MessageBox.Show("Personel eklenirken bir hata oluştu.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void btnUrunEkle_Click(object sender, EventArgs e)
        {
            try
            {
                double odenenMiktar;
                string urunTuru = tbxUrunTuru.Text;
                string islemTuru = cbxIslemTuru.Text;
                int islemMiktari = Convert.ToInt32(tbxIslemMiktari.Text);
                string ilgiliKurumVeyaKisi = tbxKurumVeyaKisi.Text;
                double islemTutari = Convert.ToDouble(tbxIslemTutari.Text);
                string odemeDurumu = cbxOdemeDurumu.Text;
                if (cbxOdemeDurumu.Text == "Ödendi")
                {
                    odenenMiktar = islemTutari;
                }
                else if (cbxOdemeDurumu.Text == "Ödenmedi")
                {
                    odenenMiktar = 0;
                }
                else
                {

                    odenenMiktar = Convert.ToDouble(tbxOdenenMiktar.Text);
                }
                DateTime islemTarihi = dtpIslemTarihi.Value;


                if (InsertUrun(urunTuru, islemTuru, islemMiktari, ilgiliKurumVeyaKisi, islemTutari, odemeDurumu, odenenMiktar, islemTarihi))
                {
                    MessageBox.Show("Ürün başarıyla eklendi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LoadUrunData();
                    LoadUrunRaporu();
                    CalculateAndInsertUrunRapor();
                    LoadUrunAylikKarZarariChart();
                    LoadBorcAlacakTablosu();
                    LoadGenelKarZararChart();
                }
            }
            catch
            {
                MessageBox.Show("Ürün eklenirken bir hata oluştu.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
 
        private bool DeletePersonel(int personelNo)
        {

            string connectionString = "Data Source=MyDatabase.db;Version=3;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                // Personel silme sorgusu
                string deleteQuery = "DELETE FROM Personel WHERE PersonelNo=@personelNo;";

                using (SQLiteCommand command = new SQLiteCommand(deleteQuery, connection))
                {
                    command.Parameters.AddWithValue("@personelNo", personelNo);

                    int result = command.ExecuteNonQuery();

                    return result > 0;
                }
            }
        }
        private bool DeleteUrun(int urunNo)
        {
            string connectionString = "Data Source=MyDatabase.db;Version=3;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                // Ürün silme sorgusu
                string deleteQuery = "DELETE FROM Urunler WHERE UrunNo=@urunNo;";

                using (SQLiteCommand command = new SQLiteCommand(deleteQuery, connection))
                {
                    command.Parameters.AddWithValue("@urunNo", urunNo);

                    int result = command.ExecuteNonQuery();

                    return result > 0;
                }
            }
        }
        private void btnPersonelSil_Click(object sender, EventArgs e)
        {
            try
            {
                int personelNo = Convert.ToInt32(tbxPersonelNo.Text);
                bool dene = personelNoList.Contains(personelNo);
                if (!dene)
                {
                    MessageBox.Show("Bu personel numarası sistemde bulunmuyor!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (DeletePersonel(personelNo))
                {
                    MessageBox.Show("Personel başarıyla silindi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadPersonelData();
                    CalculateAndInsertPersonelRapor();
                    LoadPersonelRapor();
                    LoadGenelKarZararChart();
                    personelNoList.Remove(personelNo);
                }
            }

            catch
            {
                MessageBox.Show("Personel silinirken bir hata oluştu.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void btnUrunSil_Click(object sender, EventArgs e)
        {
            try
            {
                int urunNo = Convert.ToInt32(tbxUrunNo.Text);
                bool dene = urunNoList.Contains(urunNo);
                if (!dene)
                {
                    MessageBox.Show("Bu ürün numarası sistemde bulunmuyor!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (DeleteUrun(urunNo))
                {
                    MessageBox.Show("Ürün başarıyla silindi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    urunNoList.Remove(urunNo);

                    LoadUrunData();
                    LoadUrunRaporu();
                    CalculateAndInsertUrunRapor();
                    LoadUrunAylikKarZarariChart();
                    LoadBorcAlacakTablosu();
                    LoadGenelKarZararChart();
                }
            }
            catch
            {
                MessageBox.Show("Ürün silinirken bir hata oluştu.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private bool UpdatePersonel(int personelNo, string ad, string soyad, DateTime isBaslangic, double maas)
        {
            string connectionString = "Data Source=MyDatabase.db;Version=3;";
            string updateQuery = "UPDATE Personel SET Ad=@ad, Soyad=@soyad, IsBaslangic=@isBaslangic, Maas=@maas WHERE PersonelNo=@personelNo;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@ad", ad);
                    command.Parameters.AddWithValue("@soyad", soyad);
                    command.Parameters.AddWithValue("@isBaslangic", isBaslangic.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@maas", maas);
                    command.Parameters.AddWithValue("@personelNo", personelNo);

                    int result = command.ExecuteNonQuery();

                    return result > 0;
                }
            }
        }
        private bool UpdateUrun(int urunNo, string urunTuru, string islemTuru, int islemMiktari, string ilgiliKurumVeyaKisi, double islemTutari, string odemeDurumu, double odenenMiktar, DateTime islemTarihi)
        {
            string connectionString = "Data Source=MyDatabase.db;Version=3;";
            string updateQuery = "UPDATE Urunler SET UrunTuru=@urunTuru, IslemTuru=@islemTuru, IslemMiktari=@islemMiktari, IlgiliKurumVeyaKisi=@ilgiliKurumVeyaKisi, IslemTutari=@islemTutari, OdemeDurumu=@odemeDurumu, OdenenMiktar=@odenenMiktar, IslemTarihi=@islemTarihi WHERE UrunNo=@urunNo;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@urunTuru", urunTuru);
                    command.Parameters.AddWithValue("@islemTuru", islemTuru);
                    command.Parameters.AddWithValue("@islemMiktari", islemMiktari);
                    command.Parameters.AddWithValue("@ilgiliKurumVeyaKisi", ilgiliKurumVeyaKisi);
                    command.Parameters.AddWithValue("@islemTutari", islemTutari);
                    command.Parameters.AddWithValue("@odemeDurumu", odemeDurumu);
                    command.Parameters.AddWithValue("@odenenMiktar", odenenMiktar);
                    command.Parameters.AddWithValue("@islemTarihi", islemTarihi.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@urunNo", urunNo);

                    int result = command.ExecuteNonQuery();

                    return result > 0;
                }
            }
        }
        private void btnPersonelGuncelle_Click(object sender, EventArgs e)
        {
            try
            {
                int personelNo = Convert.ToInt32(tbxPersonelNo.Text);
                string ad = tbxAd.Text;
                string soyad = tbxSoyad.Text;
                DateTime isBaslangic = dtpIseGirisTarihi.Value;
                double maas = Convert.ToDouble(tbxAylıkUcret.Text);

                if (UpdatePersonel(personelNo, ad, soyad, isBaslangic, maas))
                {
                    MessageBox.Show("Personel bilgileri güncellendi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadPersonelData();
                    CalculateAndInsertPersonelRapor();
                    LoadPersonelRapor();
                    LoadGenelKarZararChart();
                }
            }
            catch
            {
                MessageBox.Show("Personel bilgileri güncellenirken bir hata oluştu.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnUrunGuncelle_Click(object sender, EventArgs e)
        {
            try
            {
                int urunNo = Convert.ToInt32(tbxUrunNo.Text);
                string urunTuru = tbxUrunTuru.Text;
                string islemTuru = cbxIslemTuru.Text;
                int islemMiktari = Convert.ToInt32(tbxIslemMiktari.Text);
                string ilgiliKurumVeyaKisi = tbxKurumVeyaKisi.Text;
                double islemTutari = Convert.ToDouble(tbxIslemTutari.Text);
                string odemeDurumu = cbxOdemeDurumu.Text;
                double odenenMiktar = Convert.ToDouble(tbxOdenenMiktar.Text);
                DateTime islemTarihi = dtpIslemTarihi.Value;

                if (UpdateUrun(urunNo, urunTuru, islemTuru, islemMiktari, ilgiliKurumVeyaKisi, islemTutari, odemeDurumu, odenenMiktar, islemTarihi))
                {
                    MessageBox.Show("Ürün bilgileri güncellendi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LoadUrunData();
                    LoadUrunRaporu();
                    CalculateAndInsertUrunRapor();
                    LoadUrunAylikKarZarariChart();
                    LoadBorcAlacakTablosu();
                    LoadGenelKarZararChart();
                }
            }
            catch
            {
                MessageBox.Show("Ürün bilgileri güncellenirken bir hata oluştu.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void dgvPersonel_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0) // Eğer tıklanan bir satır varsa
            {
                DataGridViewRow row = dgvPersonel.Rows[e.RowIndex];

                // Satırdaki verileri TextBox'lara yerleştirme
                tbxPersonelNo.Text = row.Cells["PersonelNo"].Value.ToString();
                tbxAd.Text = row.Cells["Ad"].Value.ToString();
                tbxSoyad.Text = row.Cells["Soyad"].Value.ToString();
                dtpIseGirisTarihi.Value = Convert.ToDateTime(row.Cells["IsBaslangic"].Value);
                tbxAylıkUcret.Text = row.Cells["Maas"].Value.ToString();
            }
        }
        private void dgvUrunler_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0) // Eğer tıklanan bir satır varsa
            {
                DataGridViewRow row = dgvUrunler.Rows[e.RowIndex];

                // Satırdaki verileri TextBox'lara ve diğer kontrollere yerleştirme
                tbxUrunNo.Text = row.Cells["UrunNo"].Value.ToString();
                tbxUrunTuru.Text = row.Cells["UrunTuru"].Value.ToString();
                cbxIslemTuru.Text = row.Cells["IslemTuru"].Value.ToString();
                tbxIslemMiktari.Text = row.Cells["IslemMiktari"].Value.ToString();
                tbxKurumVeyaKisi.Text = row.Cells["IlgiliKurumVeyaKisi"].Value.ToString();
                tbxIslemTutari.Text = row.Cells["IslemTutari"].Value.ToString();
                cbxOdemeDurumu.Text = row.Cells["OdemeDurumu"].Value.ToString();
                tbxOdenenMiktar.Text = row.Cells["OdenenMiktar"].Value.ToString();
                dtpIslemTarihi.Value = Convert.ToDateTime(row.Cells["IslemTarihi"].Value);
            }
        }
        private void LoadPersonelData()
        {
            string connectionString = "Data Source=MyDatabase.db;Version=3;";
            string selectQuery = "SELECT PersonelNo, Ad, Soyad, IsBaslangic, Maas FROM Personel;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(selectQuery, connection))
                {
                    DataTable personelTable = new DataTable();
                    adapter.Fill(personelTable);

                    dgvPersonel.DataSource = personelTable;
                }
            }
        }      
        private void LoadUrunData()
        {
            string connectionString = "Data Source=MyDatabase.db;Version=3;";
            string selectQuery = "SELECT UrunNo, UrunTuru, IslemTuru, IslemMiktari, IlgiliKurumVeyaKisi, IslemTutari, OdemeDurumu, OdenenMiktar, IslemTarihi FROM Urunler;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(selectQuery, connection))
                {
                    DataTable urunTable = new DataTable();
                    adapter.Fill(urunTable);

                    dgvUrunler.DataSource = urunTable;
                }
            }
        }
        private void LoadGiderData()
        {
            string connectionString = "Data Source=MyDatabase.db;Version=3;";
            string selectQuery = "SELECT * FROM Giderdurumu;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(selectQuery, connection))
                {
                    DataTable GiderTable = new DataTable();
                    adapter.Fill(GiderTable);

                    dgvGider.DataSource = GiderTable;
                }
            }
        }
        private void tbxOdenenMiktar_Click(object sender, EventArgs e)
        {
            tbxOdenenMiktar.Clear();
          
        }
 

        private void CreatePersonelRaporTable()
        {
            string connectionString = "Data Source=MyDatabase.db;Version=3;";
            string createTableQuery = "CREATE TABLE IF NOT EXISTS PersonelRapor (ToplamPersonelSayisi INTEGER, ToplamMaas REAL);";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
        private void UpdatePersonelRapor(int personelSayisi, double toplamMaas)
        {
            string connectionString = "Data Source=MyDatabase.db;Version=3;";
            string selectQuery = "SELECT COUNT(*) FROM PersonelRapor;";
            string updateQuery = "UPDATE PersonelRapor SET ToplamPersonelSayisi=@toplamPersonelSayisi, ToplamMaas=@toplamMaas;";
            string insertQuery = "INSERT INTO PersonelRapor (ToplamPersonelSayisi, ToplamMaas) VALUES (@toplamPersonelSayisi, @toplamMaas);";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteCommand selectCommand = new SQLiteCommand(selectQuery, connection))
                {
                    int rowCount = Convert.ToInt32(selectCommand.ExecuteScalar());

                    using (SQLiteCommand command = new SQLiteCommand(rowCount > 0 ? updateQuery : insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@toplamPersonelSayisi", personelSayisi);
                        command.Parameters.AddWithValue("@toplamMaas", toplamMaas);

                        command.ExecuteNonQuery();
                    }
                }
            }
        }
        private void CalculateAndInsertPersonelRapor()
        {
            int toplamPersonelSayisi = dgvPersonel.Rows.Count;
            double toplamMaas = 0;

            foreach (DataGridViewRow row in dgvPersonel.Rows)
            {
                if (row.Cells["Maas"].Value != null && row.Cells["Maas"].Value != DBNull.Value)
                {
                    double maas = Convert.ToDouble(row.Cells["Maas"].Value);
                    toplamMaas += maas;
                }
            }

            UpdatePersonelRapor(toplamPersonelSayisi, toplamMaas);
        }
        private void LoadPersonelRapor()
        {
            string connectionString = "Data Source=MyDatabase.db;Version=3;";
            string selectQuery = "SELECT ToplamPersonelSayisi, ToplamMaas FROM PersonelRapor;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(selectQuery, connection))
                {
                    DataTable PersonelRaporTable = new DataTable();
                    adapter.Fill(PersonelRaporTable);

                    dgvPersonelRapor.DataSource = PersonelRaporTable;
                }
            }
        }
        private void dgvGider_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0) // Eğer tıklanan bir satır varsa
            {
                DataGridViewRow row = dgvGider.Rows[e.RowIndex];

                // Satırdaki verileri TextBox'lara ve diğer kontrollere yerleştirme
                tbxGiderID.Text = tbxGiderTutari.Text = row.Cells["GiderNo"].Value.ToString();
                cbxGiderTuru.Text = row.Cells["giderTuru"].Value.ToString();
                tbxGiderTutari.Text = row.Cells["giderMiktari"].Value.ToString();
                dtpGiderTarihi.Text = row.Cells["giderTarihi"].Value.ToString();
              
            }
        }
        private void btnGiderSil_Click(object sender, EventArgs e)
        {
            try
            {
                int giderNo = Convert.ToInt32(tbxGiderID.Text);


                if (DeleteGiderDurumu(giderNo))
                {
                    MessageBox.Show("Gider başarıyla silindi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadGiderData();
                    LoadGenelKarZararChart();
                }
            }
            catch
            {
                MessageBox.Show("Gider silinirken bir hata oluştu.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void CreateGiderDurumuTable()
        {
            string connectionString = "Data Source=MyDatabase.db;Version=3;";
            string createTableQuery = "CREATE TABLE IF NOT EXISTS Giderdurumu (GiderNo INTEGER PRIMARY KEY AUTOINCREMENT, GiderTuru TEXT, GiderMiktari INTEGER, GiderTarihi DATE);";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
        
        private bool InsertGiderDurumu(string giderTuru, int giderMiktari, DateTime giderTarihi)
        {
            string connectionString = "Data Source=MyDatabase.db;Version=3;";
            string insertQuery = "INSERT INTO Giderdurumu (GiderTuru, GiderMiktari, GiderTarihi) VALUES (@giderTuru, @giderMiktari, @giderTarihi);";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@giderTuru", giderTuru);
                    command.Parameters.AddWithValue("@giderMiktari", giderMiktari);
                    command.Parameters.AddWithValue("@giderTarihi", giderTarihi);

                    int result = command.ExecuteNonQuery();

                    return result > 0;
                }
            }
        }
        private bool UpdateGiderDurumu(int giderNo, string giderTuru, int giderMiktari, DateTime giderTarihi)
        {
            string connectionString = "Data Source=MyDatabase.db;Version=3;";
            string updateQuery = "UPDATE Giderdurumu SET GiderTuru=@giderTuru, GiderMiktari=@giderMiktari, GiderTarihi=@giderTarihi WHERE GiderNo=@giderNo;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@giderNo", giderNo);
                    command.Parameters.AddWithValue("@giderTuru", giderTuru);
                    command.Parameters.AddWithValue("@giderMiktari", giderMiktari);
                    command.Parameters.AddWithValue("@giderTarihi", giderTarihi);

                    int result = command.ExecuteNonQuery();

                    return result > 0;
                }
            }
        }
        private bool DeleteGiderDurumu(int giderNo)
        {
            string connectionString = "Data Source=MyDatabase.db;Version=3;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                // Ürün silme sorgusu
                string deleteQuery = "DELETE FROM Giderdurumu WHERE giderNo=@giderNo;";

                using (SQLiteCommand command = new SQLiteCommand(deleteQuery, connection))
                {
                    command.Parameters.AddWithValue("@giderNo", giderNo);

                    int result = command.ExecuteNonQuery();

                    return result > 0;
                }
            }
        }
        private void btnGiderGuncelle_Click(object sender, EventArgs e)
        {
            try
            {
                int giderNo = Convert.ToInt32(tbxGiderID.Text);
                string giderTuru = cbxGiderTuru.Text;
                int giderMiktari = Convert.ToInt32(tbxGiderTutari.Text);
                DateTime giderTarihi = dtpGiderTarihi.Value;


                if (UpdateGiderDurumu(giderNo, giderTuru, giderMiktari, giderTarihi))
                {
                    MessageBox.Show("Ürün bilgileri güncellendi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadGiderData();
                    LoadGenelKarZararChart();
                }
            }
            catch
            {
                MessageBox.Show("Ürün bilgileri güncellenirken bir hata oluştu.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void personelTbxTemizle_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            tbxPersonelNo.Clear();
            tbxAd.Clear();
            tbxSoyad.Clear();
            tbxAylıkUcret.Clear();
        }
        private void urunTbxTemizle_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            tbxUrunNo.Clear();
            tbxUrunTuru.Clear();
            tbxIslemMiktari.Clear();
            tbxKurumVeyaKisi.Clear();
            tbxIslemTutari.Clear();
            tbxOdenenMiktar.Clear();
            cbxOdemeDurumu.SelectedIndex=0;
           
        }
        private void tbxIslemTutari_Click(object sender, EventArgs e)
        {
            tbxIslemTutari.Clear();
        }
        private void tbxAylıkUcret_Click(object sender, EventArgs e)
        {
            tbxAylıkUcret.Clear();
        }
        private void tbxGiderTutari_Click(object sender, EventArgs e)
        {
            tbxGiderTutari.Clear();
        }
        private void label19_Click(object sender, EventArgs e)
        {
            CreatePersonelRaporTable();
            CalculateAndInsertPersonelRapor();
            LoadPersonelRapor();
            LoadUrunRaporu();
            DeleteAllData();
        }
        private void DeleteAllData()
        {
            string connectionString = "Data Source=MyDatabase.db;Version=3;";
            string deleteQuery = "DELETE FROM PersonelRapor;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(deleteQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
        private void CreateUrunRaporTablosu()
        {
            string connectionString = "Data Source=MyDatabase.db;Version=3;";
            string createTableQuery = "CREATE TABLE IF NOT EXISTS urunRaporTablosu (" +
                                      "urunTuru TEXT," +
                                      "satinAlinanUrunMiktari INTEGER," +
                                      "satilanUrunMiktari INTEGER," +
                                      "urunStok INTEGER," +
                                      "aylikSatinAlmaFiyati REAL," +
                                      "aylikSatisFiyati REAL," +
                                      "urununAylikKarVeyaZarari REAL" +
                                      ");";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
        private void CalculateAndInsertUrunRapor()
        {
            string connectionString = "Data Source=MyDatabase.db;Version=3;";
            string selectQuery = "SELECT urunTuru, IslemTuru, IslemMiktari, IslemTutari FROM Urunler;";
            string insertQuery = "INSERT INTO urunRaporTablosu (urunTuru, satinAlinanUrunMiktari, satilanUrunMiktari, urunStok, aylikSatinAlmaFiyati, aylikSatisFiyati, urununAylikKarVeyaZarari) " +
                                 "VALUES (@urunTuru, @satinAlinanUrunMiktari, @satilanUrunMiktari, @urunStok, @aylikSatinAlmaFiyati, @aylikSatisFiyati, @urununAylikKarVeyaZarari);";

            Dictionary<string, int> satinAlinanUrunler = new Dictionary<string, int>();
            Dictionary<string, int> satilanUrunler = new Dictionary<string, int>();
            Dictionary<string, double> aylikSatinAlmaFiyatlari = new Dictionary<string, double>();
            Dictionary<string, double> aylikSatisFiyatlari = new Dictionary<string, double>();

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteCommand selectCommand = new SQLiteCommand(selectQuery, connection))
                {
                    using (SQLiteDataReader reader = selectCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string urunTuru = reader["urunTuru"].ToString();
                            string islemTuru = reader["IslemTuru"].ToString();
                            int islemMiktari = Convert.ToInt32(reader["IslemMiktari"]);
                            double islemTutari = Convert.ToDouble(reader["IslemTutari"]);

                            if (islemTuru == "Satın Alma İşlemi")
                            {
                                if (satinAlinanUrunler.ContainsKey(urunTuru))
                                    satinAlinanUrunler[urunTuru] += islemMiktari;
                                else
                                    satinAlinanUrunler[urunTuru] = islemMiktari;

                                if (aylikSatinAlmaFiyatlari.ContainsKey(urunTuru))
                                    aylikSatinAlmaFiyatlari[urunTuru] += islemTutari;
                                else
                                    aylikSatinAlmaFiyatlari[urunTuru] = islemTutari;
                            }
                            else if (islemTuru == "Satış İşlemi")
                            {
                                if (satilanUrunler.ContainsKey(urunTuru))
                                    satilanUrunler[urunTuru] += islemMiktari;
                                else
                                    satilanUrunler[urunTuru] = islemMiktari;

                                if (aylikSatisFiyatlari.ContainsKey(urunTuru))
                                    aylikSatisFiyatlari[urunTuru] += islemTutari;
                                else
                                    aylikSatisFiyatlari[urunTuru] = islemTutari;
                            }
                        }
                    }
                }

                foreach (var urunTuru in satinAlinanUrunler.Keys)
                {
                    int satinAlinanMiktar = satinAlinanUrunler[urunTuru];
                    int satilanMiktar = satilanUrunler.ContainsKey(urunTuru) ? satilanUrunler[urunTuru] : 0;
                    int urunStok = satinAlinanMiktar - satilanMiktar;

                    double aylikSatinAlmaFiyati = aylikSatinAlmaFiyatlari.ContainsKey(urunTuru) ? aylikSatinAlmaFiyatlari[urunTuru] : 0;
                    double aylikSatisFiyati = aylikSatisFiyatlari.ContainsKey(urunTuru) ? aylikSatisFiyatlari[urunTuru] : 0;
                    double urununAylikKarVeyaZarari = aylikSatisFiyati - aylikSatinAlmaFiyati;


                    using (SQLiteCommand insertCommand = new SQLiteCommand(insertQuery, connection))
                    {
                        insertCommand.Parameters.AddWithValue("@urunTuru", urunTuru);
                        insertCommand.Parameters.AddWithValue("@satinAlinanUrunMiktari", satinAlinanMiktar);
                        insertCommand.Parameters.AddWithValue("@satilanUrunMiktari", satilanMiktar);
                        insertCommand.Parameters.AddWithValue("@urunStok", urunStok);
                        insertCommand.Parameters.AddWithValue("@aylikSatinAlmaFiyati", aylikSatinAlmaFiyati);
                        insertCommand.Parameters.AddWithValue("@aylikSatisFiyati", aylikSatisFiyati);
                        insertCommand.Parameters.AddWithValue("@urununAylikKarVeyaZarari", urununAylikKarVeyaZarari);

                        insertCommand.ExecuteNonQuery();
                    }
                }
            }
        }
        private void cbxUrunRaporuFiltrleme_SelectedIndexChanged(object sender, EventArgs e)
        {
            // ComboBox'tan seçilen değeri alın
            string selectedFilter = cbxUrunRaporuFiltrleme.SelectedItem.ToString();

            if (selectedFilter == "Bu Ay")
            {
                // Bu Ay seçildiyse, sadece güncel ayın verilerini yükle ve DataGridView'i güncelle
                
                LoadUrunRaporuForCurrentMonth();
            }
            else if (selectedFilter == "Tüm Zamanlar")
            {
                // Tüm Zamanlar seçildiyse, tüm verileri yükle ve DataGridView'i güncelle
                
                LoadUrunRaporu();
            }
        }

        private void LoadUrunRaporuForCurrentMonth()
        {
            string connectionString = "Data Source=MyDatabase.db;Version=3;";
            string selectQuery = "SELECT urunTuru, " +
                                 "SUM(CASE WHEN IslemTuru = 'Satın Alma İşlemi' THEN IslemMiktari ELSE 0 END) AS satinAlinanUrunMiktari, " +
                                 "SUM(CASE WHEN IslemTuru = 'Satış İşlemi' THEN IslemMiktari ELSE 0 END) AS satilanUrunMiktari, " +
                                 "SUM(CASE WHEN IslemTuru = 'Satış İşlemi' THEN -IslemMiktari ELSE IslemMiktari END) AS urunStok, " +
                                 "SUM(CASE WHEN IslemTuru = 'Satın Alma İşlemi' THEN IslemTutari ELSE 0 END) AS aylikSatinAlmaFiyati, " +
                                 "SUM(CASE WHEN IslemTuru = 'Satış İşlemi' THEN IslemTutari ELSE 0 END) AS aylikSatisFiyati, " +
                                 "SUM(CASE WHEN IslemTuru = 'Satış İşlemi' THEN IslemTutari ELSE -IslemTutari END) AS urununAylikKarVeyaZarari " +
                                 $"FROM Urunler WHERE strftime('%Y-%m', IslemTarihi) = '{DateTime.Now.Year}-{DateTime.Now.Month:00}' GROUP BY urunTuru;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(selectQuery, connection))
                {
                    DataTable urunRaporTable = new DataTable();
                    adapter.Fill(urunRaporTable);

                    dgvUrunRaporu.Rows.Clear();

                    foreach (DataRow row in urunRaporTable.Rows)
                    {
                        string urunTuru = row["urunTuru"].ToString();
                        int satinAlinanUrunMiktari = Convert.ToInt32(row["satinAlinanUrunMiktari"]);
                        int satilanUrunMiktari = Convert.ToInt32(row["satilanUrunMiktari"]);
                        int urunStok = Convert.ToInt32(row["urunStok"]);
                        double aylikSatinAlmaFiyati = Convert.ToDouble(row["aylikSatinAlmaFiyati"]);
                        double aylikSatisFiyati = Convert.ToDouble(row["aylikSatisFiyati"]);
                        double urununAylikKarVeyaZarari = Convert.ToDouble(row["urununAylikKarVeyaZarari"]);

                        dgvUrunRaporu.Rows.Add(urunTuru, satinAlinanUrunMiktari, satilanUrunMiktari, urunStok, aylikSatinAlmaFiyati, aylikSatisFiyati, urununAylikKarVeyaZarari);
                    }
                }
            }
        }

        private void LoadUrunRaporu()
        {
            string connectionString = "Data Source=MyDatabase.db;Version=3;";
            string selectQuery = "SELECT urunTuru, " +
                                 "SUM(CASE WHEN IslemTuru = 'Satın Alma İşlemi' THEN IslemMiktari ELSE 0 END) AS satinAlinanUrunMiktari, " +
                                 "SUM(CASE WHEN IslemTuru = 'Satış İşlemi' THEN IslemMiktari ELSE 0 END) AS satilanUrunMiktari, " +
                                 "SUM(CASE WHEN IslemTuru = 'Satış İşlemi' THEN -IslemMiktari ELSE IslemMiktari END) AS urunStok, " +
                                 "SUM(CASE WHEN IslemTuru = 'Satın Alma İşlemi' THEN IslemTutari ELSE 0 END) AS aylikSatinAlmaFiyati, " +
                                 "SUM(CASE WHEN IslemTuru = 'Satış İşlemi' THEN IslemTutari ELSE 0 END) AS aylikSatisFiyati, " +
                                 "SUM(CASE WHEN IslemTuru = 'Satış İşlemi' THEN IslemTutari ELSE -IslemTutari END) AS urununAylikKarVeyaZarari " +
                                 "FROM Urunler " +
                                 "GROUP BY urunTuru;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(selectQuery, connection))
                {
                    DataTable urunRaporTable = new DataTable();
                    adapter.Fill(urunRaporTable);


                    dgvUrunRaporu.Rows.Clear();


                    foreach (DataRow row in urunRaporTable.Rows)
                    {
                        string urunTuru = row["urunTuru"].ToString();
                        int satinAlinanUrunMiktari = Convert.ToInt32(row["satinAlinanUrunMiktari"]);
                        int satilanUrunMiktari = Convert.ToInt32(row["satilanUrunMiktari"]);
                        int urunStok = Convert.ToInt32(row["urunStok"]);
                        double aylikSatinAlmaFiyati = Convert.ToDouble(row["aylikSatinAlmaFiyati"]);
                        double aylikSatisFiyati = Convert.ToDouble(row["aylikSatisFiyati"]);
                        double urununAylikKarVeyaZarari = Convert.ToDouble(row["urununAylikKarVeyaZarari"]);

                        dgvUrunRaporu.Rows.Add(urunTuru, satinAlinanUrunMiktari, satilanUrunMiktari, urunStok, aylikSatinAlmaFiyati, aylikSatisFiyati, urununAylikKarVeyaZarari);
                    }
                }
            }
        }
        
        private void cbxOdemeDurumu_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxOdemeDurumu.SelectedIndex == 0 || cbxOdemeDurumu.SelectedIndex == 1)
            {
                tbxOdenenMiktar.Enabled = false;
            }
            else
            {
                tbxOdenenMiktar.Enabled = true;
            }
        }



        private void pictureBoxInformation_Click(object sender, EventArgs e)
        {
            Process.Start("https://drive.google.com/file/d/1bWfjTUbb6MMznL8BBIwUolyJKUMfUaLn/view");
        }

        private void btnGiderEkle_Click(object sender, EventArgs e)
        {
            try
            {
                string giderTuru = cbxGiderTuru.Text;
                int giderMiktari = Convert.ToInt32(tbxGiderTutari.Text);
                DateTime giderTarihi = dtpGiderTarihi.Value;

                if (InsertGiderDurumu(giderTuru, giderMiktari, giderTarihi))
                {
                    MessageBox.Show("Gider başarıyla eklendi!.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadGiderData();
                    LoadGenelKarZararChart();


                }
            }

            catch
            {
                MessageBox.Show("Gider eklenirken bir hata oluştu.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pictureBoxHelp_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Destek ve Daha Fazlası için:" + "\n" + "altaygokmn@gmail.com" + "\n" + "canercer3@gmail.com", "Yardım");
        }

        private void tbxPersonelNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
            }

        }

        private void tbxAylıkUcret_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
            }

        }

        private void tbxIslemMiktari_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
            }
        }

        private void tbxIslemTutari_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
            }
        }

        private void tbxOdenenMiktar_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
            }
        }

        private void tbxGiderTutari_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
            }
        }

        private void tbxAd_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
            }
        }

        private void tbxSoyad_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
            }
        }

        private void tbxUrunTuru_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
            }
        }

        private void tbxKurumVeyaKisi_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
            }
        }

        private void panel7_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label19_Click_1(object sender, EventArgs e)
        {

        }
    }

}

