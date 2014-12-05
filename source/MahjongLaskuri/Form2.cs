using System;
using System.Configuration;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;

namespace MahjongLaskuri
{
    public partial class Form2 : Form
    {
        private int _Checker, check1 = 0, check2 = 0, check3 = 0, check4 = 0;

        public Form2()
        {
            InitializeComponent();
            button1.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings["player1"].Value = textBox1.Text;
            config.AppSettings.Settings["player2"].Value = textBox2.Text;
            config.AppSettings.Settings["player3"].Value = textBox3.Text;
            config.AppSettings.Settings["player4"].Value = textBox4.Text;
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
            this.Close();
        }

        //For disabling the button if no text in all fields
        public int Checker
        {
            get { return _Checker; }
            set
            {
                _Checker = value;
                if (_Checker == 4)
                {
                    button1.Enabled = true;
                }
                if (_Checker < 4)
                {
                    button1.Enabled = false;
                }
            }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                Checker--;
                check1 = 0;
            }
            else
            {
                if (check1 == 0)
                {
                    Checker++;
                    check1++;
                }
            }
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                Checker--;
                check2 = 0;
            }
            else
            {
                if (check2 == 0)
                {
                    Checker++;
                    check2++;
                }
            }
        }
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox3.Text))
            {
                Checker--;
                check3 = 0;
            }
            else
            {
                if (check3 == 0)
                {
                    Checker++;
                    check3++;
                }
            }
        }
        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox4.Text))
            {
                Checker--;
                check4 = 0;
            }
            else
            {
                if (check4 == 0)
                {
                    Checker++;
                    check4++;
                }
            }
        }
    }
}
