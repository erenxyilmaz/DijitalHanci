using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Globalization;
using System.Windows.Forms;

namespace RfId
{
    public partial class misafir : Form
    {
        private string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\Monster\Desktop\dijitalhanci\dijitalhancı.accdb;Persist Security Info=False;";
        private string kronikHastaliklar; // Kronik hastalıkları tutacak değişken
        private string alerjenler; // Alerjen bilgilerini tutacak değişken

        public misafir()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string ad = txtAd.Text;
            string soyad = txtSoyad.Text;
            DateTime dogumTarihi;
            bool validDate = DateTime.TryParseExact(maskedTextBoxDogumTarihi.Text, "dd.MM.yyyy",
                                                     CultureInfo.InvariantCulture,
                                                     DateTimeStyles.None,
                                                     out dogumTarihi);

            if (!validDate)
            {
                MessageBox.Show("Lütfen geçerli bir doğum tarihi girin (GG.AA.YYYY).");
                return;
            }
            string kimlikNo = txtKimlikNo.Text;
            string telefon = txtTelefon.Text;
            string email = txtEposta.Text;
            DateTime giris = dateTimePickerGiris.Value;
            DateTime cikis = dateTimePickerCikis.Value;
            string odaNumarasi = txtOdaNumarasi.Text;
            string kartId = txtKartId.Text;
            double boy = double.Parse(txtBoy.Text); // Örneğin cm cinsinden girildiği varsayılmıştır
            double kilo = double.Parse(txtKilo.Text); // Örneğin kg cinsinden girildiği varsayılmıştır
            double bmi = 0;

            if (checkboxKgCm.Checked) // Eğer Kg/Cm seçildiyse
            {
                boy = boy / 100; // cm'yi metre'ye çevir
                bmi = kilo / (boy * boy); // BMI hesapla
            }
            else if (checkboxLbsFeet.Checked) // Eğer Lbs/Feet seçildiyse
            {
                boy = boy * 0.0254; // inç'i metreye çevir (1 inç = 0.0254 metre)
                kilo = kilo * 0.453592; // pound'u kilograma çevir (1 pound = 0.453592 kilogram)
                bmi = kilo / (boy * boy); // BMI hesapla
            }
            else
            {
                MessageBox.Show("Lütfen bir ölçü birimi seçiniz.");
                return;
            }

            string cinsiyet = ""; // Cinsiyet için boş bir string tanımla

            // Cinsiyet seçimini kontrol et
            if (checkBox1.Checked && !checkBox2.Checked)
            {
                cinsiyet = "Kadın";
            }
            else if (!checkBox1.Checked && checkBox2.Checked)
            {
                cinsiyet = "Erkek";
            }
            else
            {
                MessageBox.Show("Lütfen cinsiyet seçiniz ve sadece birini işaretleyiniz.");
                return;
            }

            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "INSERT INTO Ziyaretciler (Ad, Soyad, DogumTarihi, KimlikNo, Telefon, Email, GirisTarihi, CikisTarihi, OdaNumarasi, KartId, Boy, Kilo, Ortalama, Cinsiyet, KronikHastaliklar, AlerjenMaddeler) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";

                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                    {
                        // Parametreleri sırasıyla ekleyin
                        cmd.Parameters.Add("@Ad", OleDbType.VarChar).Value = ad;
                        cmd.Parameters.Add("@Soyad", OleDbType.VarChar).Value = soyad;
                        cmd.Parameters.Add("@DogumTarihi", OleDbType.Date).Value = dogumTarihi;
                        cmd.Parameters.Add("@KimlikNo", OleDbType.VarChar).Value = kimlikNo;
                        cmd.Parameters.Add("@Telefon", OleDbType.VarChar).Value = telefon;
                        cmd.Parameters.Add("@Email", OleDbType.VarChar).Value = email;
                        cmd.Parameters.Add("@GirisTarihi", OleDbType.Date).Value = giris;
                        cmd.Parameters.Add("@CikisTarihi", OleDbType.Date).Value = cikis;
                        cmd.Parameters.Add("@OdaNumarasi", OleDbType.VarChar).Value = odaNumarasi;
                        cmd.Parameters.Add("@KartId", OleDbType.VarChar).Value = kartId;
                        cmd.Parameters.Add("@Boy", OleDbType.Double).Value = boy;
                        cmd.Parameters.Add("@Kilo", OleDbType.Double).Value = kilo;
                        cmd.Parameters.Add("@Ortalama", OleDbType.Double).Value = bmi;
                        cmd.Parameters.Add("@Cinsiyet", OleDbType.VarChar).Value = cinsiyet;
                        cmd.Parameters.Add("@KronikHastaliklar", OleDbType.VarChar).Value = kronikHastaliklar; // Kronik hastalıkları ekleyin
                        cmd.Parameters.Add("@AlerjenMaddeler", OleDbType.VarChar).Value = alerjenler; // Alerjen bilgilerini ekleyin

                        // ExecuteNonQuery'yi sadece bir kere çağırın
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Yeni ziyaretçi başarıyla eklendi.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Bir hata oluştu: " + ex.Message);
                }
            }
            // Formu temizle
            ClearForm();
        }

        private void ClearForm()
        {
            // Formdaki tüm metin kutularını temizle
            txtAd.Clear();
            txtSoyad.Clear();
            maskedTextBoxDogumTarihi.Clear();
            txtKimlikNo.Clear();
            txtTelefon.Clear();
            txtEposta.Clear();
            dateTimePickerGiris.Value = DateTime.Now;
            dateTimePickerCikis.Value = DateTime.Now;
            txtOdaNumarasi.Clear();
            txtKartId.Clear();
            txtBoy.Clear();
            txtKilo.Clear();
            kronikHastaliklar = null; // Kronik hastalıkları temizle
            alerjenler = null; // Alerjen bilgilerini temizle
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string kimlikNo = txtKimlikNo.Text;

            // Veritabanında TC Kimlik Numarası'na göre silme işlemi yapılıyor
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                conn.Open();

                // Önce veritabanında bu T.C. Kimlik Numarası'na sahip bir kayıt olup olmadığını kontrol ediyoruz
                string checkQuery = "SELECT COUNT(*) FROM Ziyaretciler WHERE KimlikNo = ?";
                using (OleDbCommand checkCmd = new OleDbCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("?", kimlikNo);
                    int count = (int)checkCmd.ExecuteScalar();

                    if (count > 0)
                    {
                        // Kayıt varsa, silme işlemi yapılıyor
                        string deleteQuery = "DELETE FROM Ziyaretciler WHERE KimlikNo= ?";
                        using (OleDbCommand cmd = new OleDbCommand(deleteQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("?", kimlikNo);
                            int result = cmd.ExecuteNonQuery();

                            if (result > 0)
                            {
                                MessageBox.Show("Kullanıcı veritabanından başarıyla silindi.");
                            }
                            else
                            {
                                MessageBox.Show("Kullanıcı silinirken bir hata oluştu.");
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Bu T.C. Kimlik Numarasına sahip bir kullanıcı yok.");
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Formdan alınan TCKimlik numarası
            string KimlikNo = txtKimlikNo.Text;
            // Tarihleri kontrol etmek için bir referans tarihi belirle
            DateTime referansTarih = DateTime.Today; // veya uygulamanız için mantıklı olan başka bir tarih

            // SQL sorgusu ve parametreler listesi
            List<string> setClauses = new List<string>();
            List<OleDbParameter> parameters = new List<OleDbParameter>();

            // Ad alanı güncelleniyorsa
            if (!string.IsNullOrEmpty(txtAd.Text))
            {
                setClauses.Add("Ad = ?");
                parameters.Add(new OleDbParameter("?", txtAd.Text));
            }

            // Soyad alanı güncelleniyorsa
            if (!string.IsNullOrEmpty(txtSoyad.Text))
            {
                setClauses.Add("Soyad = ?");
                parameters.Add(new OleDbParameter("?", txtSoyad.Text));
            }
            if (DateTime.TryParse(maskedTextBoxDogumTarihi.Text, out DateTime dogumTarihi))
            {
                setClauses.Add("DogumTarihi = ?");
                parameters.Add(new OleDbParameter("?", dogumTarihi));
            }
            // Telefon alanı güncelleniyorsa
            if (!string.IsNullOrEmpty(txtTelefon.Text))
            {
                setClauses.Add("Telefon = ?");
                parameters.Add(new OleDbParameter("?", txtTelefon.Text));
            }

            // Email alanı güncelleniyorsa
            if (!string.IsNullOrEmpty(txtEposta.Text))
            {
                setClauses.Add("Email = ?");
                parameters.Add(new OleDbParameter("?", txtEposta.Text));
            }
            // Giriş tarihi alanı güncelleniyorsa ve değeri referans tarihten farklıysa
            if (dateTimePickerGiris.Value.Date != referansTarih)
            {
                setClauses.Add("GirisTarihi = ?");
                parameters.Add(new OleDbParameter("?", dateTimePickerGiris.Value));
            }

            // Çıkış tarihi alanı güncelleniyorsa ve değeri referans tarihten farklıysa
            if (dateTimePickerCikis.Value.Date != referansTarih)
            {
                setClauses.Add("CikisTarihi = ?");
                parameters.Add(new OleDbParameter("?", dateTimePickerCikis.Value));
            }

            // Oda numarası alanı güncelleniyorsa
            if (!string.IsNullOrEmpty(txtOdaNumarasi.Text))
            {
                setClauses.Add("OdaNumarasi = ?");
                parameters.Add(new OleDbParameter("?", txtOdaNumarasi.Text));
            }

            // Kart Id
            if (!string.IsNullOrEmpty(txtKartId.Text))
            {
                setClauses.Add("KartId = ?");
                parameters.Add(new OleDbParameter("?", txtKartId.Text));
            }

            // Kronik hastalıklar güncelleniyorsa
            if (!string.IsNullOrEmpty(kronikHastaliklar))
            {
                setClauses.Add("KronikHastaliklar = ?");
                parameters.Add(new OleDbParameter("?", kronikHastaliklar));
            }

            // Alerjen bilgileri güncelleniyorsa
            if (!string.IsNullOrEmpty(alerjenler))
            {
                setClauses.Add("AlerjenMaddeler = ?");
                parameters.Add(new OleDbParameter("?", alerjenler));
            }

            // Eğer hiçbir alan güncellenmiyorsa, işlemi bitir.
            if (setClauses.Count == 0)
            {
                MessageBox.Show("Güncellenecek bir bilgi girilmedi.");
                return;
            }

            // SQL sorgusunu oluştur.
            string updateQuery = $"UPDATE Ziyaretciler SET {string.Join(", ", setClauses)} WHERE KimlikNo = ?";

            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                conn.Open();

                using (OleDbCommand updateCmd = new OleDbCommand(updateQuery, conn))
                {
                    // Parametreleri komuta ekle
                    foreach (var parameter in parameters)
                    {
                        updateCmd.Parameters.Add(parameter);
                    }

                    // Son parametre olarak TCKimlik numarasını ekle
                    updateCmd.Parameters.Add(new OleDbParameter("?", KimlikNo));

                    // Güncelleme işlemini çalıştır
                    int result = updateCmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        MessageBox.Show("Ziyaretçi bilgileri başarıyla güncellendi.");
                    }
                    else
                    {
                        MessageBox.Show("Güncelleme sırasında bir hata oluştu ya da belirtilen kimlik numarasıyla eşleşen bir kayıt bulunamadı.");
                    }
                }
            }
        }

        private void HesaplaBMI()
        {
            double boy = double.Parse(txtBoy.Text); // txtBoy TextBox'ından boy değerini al
            double kilo = double.Parse(txtKilo.Text); // txtKilo TextBox'ından kilo değerini al

            if (checkboxKgCm.Checked) // Eğer Kg/Cm checkbox'ı işaretliyse
            {
                double boyMetre = boy / 100; // boyu cm'den metre'ye çevir
                double bmi = kilo / (boyMetre * boyMetre); // BMI hesapla
                MessageBox.Show("BMI (Kg/Cm): " + bmi.ToString("0.00")); // BMI'yi göster
            }
            else if (checkboxLbsFeet.Checked) // Eğer Lbs/Feet checkbox'ı işaretliyse
            {
                double boyInch = boy * 0.393701; // boyu inç'e çevir
                double bmi = (kilo * 703) / (boyInch * boyInch); // BMI hesapla
                MessageBox.Show("BMI (Lbs/Feet): " + bmi.ToString("0.00")); // BMI'yi göster
            }
            else
            {
                MessageBox.Show("Lütfen bir ölçü birimi seçin.");
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void misafir_Load(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            string tcKimlikNo = txtKimlikNo.Text; // Misafir formundan TC Kimlik Numarası alınır
            Alerji alerjiForm = new Alerji(tcKimlikNo, connectionString); // Alerji formu başlatılır
            alerjiForm.ShowDialog(); // Alerji formu modal olarak açılır

            // Alerjen bilgilerini formdan alın
            alerjenler = alerjiForm.Alerjenler;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string tcKimlikNo = txtKimlikNo.Text; // Misafir formundan TC Kimlik Numarası alınır
            kronik kronikForm = new kronik(tcKimlikNo, connectionString);
            kronikForm.ShowDialog(); // Formu modal olarak aç

            // Kronik hastalıkları formdan alın
            kronikHastaliklar = kronikForm.KronikHastaliklar;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
