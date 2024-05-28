using System;
using System.Text;
using System.Windows.Forms;

namespace RfId
{
    public partial class kronik : Form
    {
        private string _tcKimlikNo;
        private string _connectionString;
        public string KronikHastaliklar { get; private set; } // Kronik hastalıkları tutacak değişken

        public kronik(string tcKimlikNo, string connectionString)
        {
            InitializeComponent();
            _tcKimlikNo = tcKimlikNo;
            _connectionString = connectionString;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in checkedListBox1.CheckedItems)
            {
                sb.Append(item.ToString() + ", ");
            }

            if (sb.Length > 0)
                sb.Length -= 2; // Son virgül ve boşluk karakterlerini kaldır

            KronikHastaliklar = sb.ToString(); // Seçilen hastalıkları değişkene atayın

            // Sadece bilgi mesajı gösterin, veritabanına ekleme yapmayın
            MessageBox.Show("Kronik hastalık bilgileri kaydedildi.");
            this.Close(); // Formu kapatın
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void kronik_Load(object sender, EventArgs e)
        {
        }
    }
}
