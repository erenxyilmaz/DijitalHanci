using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RfId
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string girilenKod = kodTextBox.Text; // Kodu giriniz textbox'ından alınan kod
            string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\onur\\Desktop\\dijitalhancı.accdb";

            // Kontrol edilecek tabloların listesi
            string[] tables = { "etevi", "analokanta", "havuzkafe", "lobi", "odaservisi" };

            bool kodBulundu = false; // Kodun bulunup bulunmadığını takip etmek için bir flag

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();

                // Her bir tablo için sorgulama yap
                foreach (string table in tables)
                {
                    // Kodun varlığını kontrol etmek için sorgu
                    string queryCheckCode = $"SELECT COUNT(1) FROM {table} WHERE Kod = @Kod";
                    using (OleDbCommand commandCheck = new OleDbCommand(queryCheckCode, connection))
                    {
                        commandCheck.Parameters.Clear();
                        commandCheck.Parameters.AddWithValue("@Kod", girilenKod);
                        int kodVarMi = (int)commandCheck.ExecuteScalar();

                        if (kodVarMi > 0)
                        {
                            // Kod bulundu, şimdi kod alanını güncelle
                            string queryUpdateCode = $"UPDATE {table} SET Kod = NULL WHERE Kod = @Kod";
                            using (OleDbCommand commandUpdate = new OleDbCommand(queryUpdateCode, connection))
                            {
                                commandUpdate.Parameters.AddWithValue("@Kod", girilenKod);
                                int etkilenenSatirSayisi = commandUpdate.ExecuteNonQuery();

                                if (etkilenenSatirSayisi > 0)
                                {
                                    MessageBox.Show($"Kod bulundu ve {table} tablosunda sıfırlandı.");
                                    kodBulundu = true;
                                    break; // Kod bulundu ve sıfırlandı, döngüden çık.
                                }
                            }
                        }
                    }
                }

                if (!kodBulundu)
                {
                    MessageBox.Show("Bu kod veritabanında hiçbir tabloda bulunamadı.");
                }
            }
        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }
    }
}
