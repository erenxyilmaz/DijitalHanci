using System;
using System.Windows.Forms;
using System.IO.Ports;
using System.Data.OleDb;
using System.Data;

namespace RfId
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            serialPort1.DataReceived += SerialPort1_DataReceived;
        }
        OleDbConnection baglantı = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\onur\\Desktop\\dijitalhancı.accdb");


        private void buttonConnect_Click(object sender, EventArgs e)
        {
            // Bağlan butonuna tıklandığında seri portu aç
            if (!serialPort1.IsOpen)
            {
                try
                {
                    serialPort1.Open(); // Seri portu aç
                    textBoxPuan.AppendText("Bağlantı kuruldu, kartınızı okutunuz.\r\n");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Seri porta bağlanırken hata oluştu: " + ex.Message);
                }
            }
            else
            {
                textBoxPuan.AppendText("Seri port zaten açık.\r\n");
            }
        }

        private void SerialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            // Seri porttan veri geldiğinde çalışacak metod
            string data = serialPort1.ReadLine(); // Seri porttan gelen satırı oku
            ShowData(data); // Gelen veriyi TextBox'a yazdır
            SaveToDatabase(data); // Veritabanına kaydet
        }

        private void ShowData(string data)
        {
            // Bu metodun içeriği UI thread'inde çalışmalı
            Invoke(new MethodInvoker(delegate ()
            {
                textBoxPuan.AppendText(data + Environment.NewLine);
            }));
        }
        private void SaveToDatabase(string data)
        {
            // Veriyi virgülle ayırarak parçalara böl
            string[] parts = data.Split(',');
            if (parts.Length == 1)
            {
                // Yalnızca kart ID'si var, diğer bilgiler eksik.
                MessageBox.Show($"Yalnızca Kart ID alındı: {parts[0]}");
            }
            else if (parts.Length == 3)
            {
                // Kart ID, Geçen Süre ve Puan bilgileri mevcut.
                string kartId = parts[0].Trim();
                // Geçen Süre ve Puan'ın sayısal değerlere dönüştürülmesi
                if (int.TryParse(parts[1].Trim(), out int gecenSure) && int.TryParse(parts[2].Trim(), out int puan))
                {
                    try
                    {
                        baglantı.Open();
                        using (OleDbCommand command = new OleDbCommand("INSERT INTO KartOkuma (KartId, GecenSure, Puan) VALUES (?, ?, ?)", baglantı))
                        {
                            command.Parameters.AddWithValue("@KartId", kartId);
                            command.Parameters.AddWithValue("@GecenSure", gecenSure);
                            command.Parameters.AddWithValue("@Puan", puan);

                            command.ExecuteNonQuery();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Veritabanına kaydedilirken hata oluştu: " + ex.Message);
                    }
                    finally
                    {
                        if (baglantı.State == System.Data.ConnectionState.Open)
                        {
                            baglantı.Close();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Geçen süre veya puan sayısal bir değere dönüştürülemedi.");
                }
            }
            else
            {
                MessageBox.Show($"Beklenen veri formatı karşılanamadı: '{data}'");
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }
    }
}
//"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\onur\\Desktop\\Database1.accdb"\\onur\\Desktop\\Database1.accdb"