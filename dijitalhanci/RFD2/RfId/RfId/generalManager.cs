using RfId;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace RfId
{
    public partial class generalManager : Form
    {
        public generalManager()
        {
            InitializeComponent();

        }

        private void button5_Click(object sender, EventArgs e)
        {
            frontOffice diger = new frontOffice();
            diger.Show();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            roomService diger2 = new roomService();
            diger2.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            lobyLoungecs diger3 = new lobyLoungecs();
            diger3.Show();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            steakHouse diger4 = new steakHouse();
            diger4.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            poolBar bar = new poolBar();
            bar.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            mainRestourant mr = new mainRestourant();
            mr.Show();
        }

        private void generalManager_Load(object sender, EventArgs e)
        {

        }
    }
}
