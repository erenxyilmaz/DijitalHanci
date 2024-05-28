using System;
using System.Text;
using System.Windows.Forms;

namespace RfId
{
    public partial class Alerji : Form
    {
        private string _tcKimlikNo;
        private string _connectionString;
        public string Alerjenler { get; private set; } // Alerjen bilgilerini tutacak değişken

        public Alerji(string tcKimlikNo, string connectionString)
        {
            InitializeComponent();
            _tcKimlikNo = tcKimlikNo;
            _connectionString = connectionString;
            this.button3.Click += new EventHandler(button1_Click);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StringBuilder alerjenler = new StringBuilder();
            CollectCheckedItems(checkedListBox1, alerjenler); // Meyveler
            CollectCheckedItems(checkedListBox2, alerjenler); // Sebzeler
            CollectCheckedItems(checkedListBox3, alerjenler); // Hayvansal Ürünler
            CollectCheckedItems(checkedListBox4, alerjenler); // Kuruyemişler

            if (alerjenler.Length > 0)
                alerjenler.Length--; // Son virgülü kaldır

            Alerjenler = alerjenler.ToString(); // Alerjen bilgilerini değişkene atayın

            // Sadece bilgi mesajı gösterin, veritabanına ekleme yapmayın
            MessageBox.Show("Alerjen bilgileri kaydedildi.");
            this.Close(); // Alerji formunu kapat
        }

        private void CollectCheckedItems(CheckedListBox checkedListBox, StringBuilder stringBuilder)
        {
            foreach (var item in checkedListBox.CheckedItems)
            {
                stringBuilder.Append(item.ToString() + ",");
            }
        }

        private void Alerji_Load(object sender, EventArgs e)
        {
            // Form yüklendiğinde yapılacak işlemler
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Seçim değişikliği olduğunda yapılacak işlemler
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void checkedListBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkedListBox8_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void checkedListBox4_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
