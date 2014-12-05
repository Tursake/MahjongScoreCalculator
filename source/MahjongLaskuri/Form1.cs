using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace MahjongLaskuri
{
    public partial class Form1 : Form
    {
        //Settings
        private string player1, player2, player3, player4;
        private bool uma, loaded = false, sameCheckError = false;
        private int _Checker, check1 = 0, check2 = 0, check3 = 0, check4 = 0, writer = 0, gamenumber = 1, lineadder;
        private List<KeyValuePair<string, double>> points = new List<KeyValuePair<string, double>>();
        private List<KeyValuePair<string, double>> pointTemp = new List<KeyValuePair<string, double>>();
        private List<KeyValuePair<string, double>> pointFinal = new List<KeyValuePair<string, double>>();
        private List<KeyValuePair<string, decimal>> position = new List<KeyValuePair<string, decimal>>();
        Regex reg = new Regex(@"^-?\d+[.]?\d*$");

        //Initialize form
        public Form1()
        {
            InitializeComponent();
            Setup();
            LoadPoints();
        }

        //Button click event (write to file, read from file, calculate final score, print on screen)
        private void button1_Click(object sender, EventArgs e)
        {
            sameChecker();

            if (Convert.ToInt32(textBox6.Text) + Convert.ToInt32(textBox7.Text) + Convert.ToInt32(textBox8.Text) +
                Convert.ToInt32(textBox9.Text) == 100000)
            {
                textBox6.BackColor = Color.White;
                textBox7.BackColor = Color.White;
                textBox8.BackColor = Color.White;
                textBox9.BackColor = Color.White;

                if(sameCheckError == false)
                {
                    button1.Text = "Add points";
                    SavePoints();
                    LoadPoints();

                    textBox6.Text = "";
                    textBox7.Text = "";
                    textBox8.Text = "";
                    textBox9.Text = "";
                }
            }
            else
            {
                textBox6.BackColor = Color.Red;
                textBox7.BackColor = Color.Red;
                textBox8.BackColor = Color.Red;
                textBox9.BackColor = Color.Red;
            }
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
        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox6.Text))
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
        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox7.Text))
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
        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox8.Text))
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
        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox9.Text))
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

        //Allow only positive and negative numbers in textbox
        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar)) return;
            if (!reg.IsMatch(textBox6.Text.Insert(textBox6.SelectionStart, e.KeyChar.ToString()) + "1")) e.Handled = true;
        }
        private void textBox7_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar)) return;
            if (!reg.IsMatch(textBox7.Text.Insert(textBox7.SelectionStart, e.KeyChar.ToString()) + "1")) e.Handled = true;
        }
        private void textBox8_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar)) return;
            if (!reg.IsMatch(textBox8.Text.Insert(textBox8.SelectionStart, e.KeyChar.ToString()) + "1")) e.Handled = true;
        }
        private void textBox9_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar)) return;
            if (!reg.IsMatch(textBox9.Text.Insert(textBox9.SelectionStart, e.KeyChar.ToString()) + "1")) e.Handled = true;
        }

        //Write points to file
        private void SavePoints()
        {
            string[] points = new string[]
            {
                textBox6.Text, Convert.ToString(numericUpDown1.Value),
                textBox7.Text, Convert.ToString(numericUpDown2.Value),
                textBox8.Text, Convert.ToString(numericUpDown3.Value),
                textBox9.Text, Convert.ToString(numericUpDown4.Value)
            };

            using (StreamWriter sw = new StreamWriter("points.txt",append:true))
            {
                foreach (string s in points)
                {
                    sw.Write(s + "\r\n");
                }
            }
        }

        //Read points from file
        private void LoadPoints()
        {
            resetValues();

            //Load points
            if (File.Exists("points.txt"))
            {
                if(new FileInfo("points.txt").Length != 0)
                {
                    richTextBox1.Text = "";
                    richTextBox2.Text = "";
                    richTextBox3.Text = "";
                    richTextBox4.Text = "";
                    gamenumber = 1;

                    using (StreamReader w = new StreamReader("points.txt"))
                    {
                        string line = "";
                        while ((line = w.ReadLine()) != null)
                        {
                            if (writer == 7)
                            {
                                position[3] = new KeyValuePair<string, decimal>(player4, Convert.ToDecimal(line));
                                CalculatePoints();
                                writer = 0;
                            }
                            else if (writer == 6)
                            {
                                points[3] = new KeyValuePair<string, double>(player4,
                                    Convert.ToInt32(line));
                                richTextBox4.AppendText(gamenumber + ". " + line + Environment.NewLine);
                                writer++;
                                gamenumber++;
                            }
                            else if (writer == 5)
                            {
                                position[2] = new KeyValuePair<string, decimal>(player3, Convert.ToDecimal(line));
                                writer++;
                            }
                            else if (writer == 4)
                            {
                                points[2] = new KeyValuePair<string, double>(player3,
                                    Convert.ToInt32(line));
                                richTextBox3.AppendText(gamenumber + ". " + line + Environment.NewLine);
                                writer++;
                            }
                            else if (writer == 3)
                            {
                                position[1] = new KeyValuePair<string, decimal>(player2, Convert.ToDecimal(line));
                                writer++;
                            }
                            else if (writer == 2)
                            {
                                points[1] = new KeyValuePair<string, double>(player2,
                                    Convert.ToInt32(line));
                                richTextBox2.AppendText(gamenumber + ". " + line + Environment.NewLine);
                                writer++;
                            }
                            else if (writer == 1)
                            {
                                position[0] = new KeyValuePair<string, decimal>(player1, Convert.ToDecimal(line));
                                writer++;
                            }
                            else if (writer == 0)
                            {
                                points[0] = new KeyValuePair<string, double>(player1,
                                    Convert.ToInt32(line));
                                richTextBox1.AppendText(gamenumber + ". " + line + Environment.NewLine);
                                writer++;
                            }
                        }
                    }
                }
            }
        }

        //Sets form and variables
        private void Setup()
        {
            button1.Enabled = false;
            richTextBox1.ReadOnly = true;
            richTextBox2.ReadOnly = true;
            richTextBox3.ReadOnly = true;
            richTextBox4.ReadOnly = true;
            textBox5.ReadOnly = true;

            setNames();

            resetValues();

            label1.BackColor = Color.Transparent;
            label2.BackColor = Color.Transparent;
            label3.BackColor = Color.Transparent;
            label4.BackColor = Color.Transparent;
            label5.BackColor = Color.Transparent;
            label6.BackColor = Color.Transparent;
            label7.BackColor = Color.Transparent;
            label8.BackColor = Color.Transparent;
            label9.BackColor = Color.Transparent;
        }

        //Resets lists for calculation
        private void resetValues()
        {
            points.Clear();
            pointTemp.Clear();
            pointFinal.Clear();

            points.Add(new KeyValuePair<string, double>(player1, 0));
            points.Add(new KeyValuePair<string, double>(player2, 0));
            points.Add(new KeyValuePair<string, double>(player3, 0));
            points.Add(new KeyValuePair<string, double>(player4, 0));

            pointTemp.Add(new KeyValuePair<string, double>(player1, 0));
            pointTemp.Add(new KeyValuePair<string, double>(player2, 0));
            pointTemp.Add(new KeyValuePair<string, double>(player3, 0));
            pointTemp.Add(new KeyValuePair<string, double>(player4, 0));

            pointFinal.Add(new KeyValuePair<string, double>(player1, 0));
            pointFinal.Add(new KeyValuePair<string, double>(player2, 0));
            pointFinal.Add(new KeyValuePair<string, double>(player3, 0));
            pointFinal.Add(new KeyValuePair<string, double>(player4, 0));

            position.Add(new KeyValuePair<string, decimal>(player1, 0));
            position.Add(new KeyValuePair<string, decimal>(player2, 0));
            position.Add(new KeyValuePair<string, decimal>(player3, 0));
            position.Add(new KeyValuePair<string, decimal>(player4, 0));
        }

        //Sets player names
        public void setNames()
        {
            if (ConfigurationManager.AppSettings["player1"] != "")
            {
                player1 = ConfigurationManager.AppSettings["player1"];
                label1.Text = player1;
                label8.Text = player1;
            }
            if (ConfigurationManager.AppSettings["player2"] != "")
            {
                player2 = ConfigurationManager.AppSettings["player2"]; ;
                label2.Text = player2;
                label7.Text = player2;
            }
            if (ConfigurationManager.AppSettings["player3"] != "")
            {
                player3 = ConfigurationManager.AppSettings["player3"];
                label3.Text = player3;
                label6.Text = player3;
            }
            if (ConfigurationManager.AppSettings["player3"] != "")
            {
                player4 = ConfigurationManager.AppSettings["player4"];
                label4.Text = player4;
                label5.Text = player4;
            }
        }

        //Calculate final scoring
        private void CalculatePoints()
        {
            points = points.OrderByDescending(i => i.Value).ToList();



            for (int x = 1; x <= 3; x++)
            {

                Console.WriteLine(((points[x].Value / 1000) - 30) % 0.5);

                if (((points[x].Value / 1000) - 30) % 0.5 == 0 && ((points[x].Value / 1000) - 30) > 0)
                {
                    points[x] = new KeyValuePair<string, double>(points[x].Key,
                    Math.Floor(points[x].Value / 1000 -30 ));
                }
                else if (((points[x].Value / 1000) - 30) % 0.5 == 0 && ((points[x].Value / 1000) -30 ) < 0)
                {
                    points[x] = new KeyValuePair<string, double>(points[x].Key,
                    Math.Ceiling(points[x].Value / 1000 - 30 ));
                }
                else
                {
                    points[x] = new KeyValuePair<string, double>(points[x].Key,
                    Math.Round(points[x].Value/1000 -30 ));
                }
            }

            points[0] = new KeyValuePair<string, double>(points[0].Key, Math.Abs(points[1].Value + points[2].Value + points[3].Value));

            for (int x = 0; x <= 3; x++)
            {
                for (int y = 0; y <= 3; y++)
                {
                    if (pointTemp[x].Key == points[y].Key)
                    {
                        double lopullisetpoints = points[y].Value;

                        pointTemp[x] = new KeyValuePair<string, double>(pointTemp[x].Key, lopullisetpoints);
                        break;
                    }
                }
            }

            pointTemp = pointTemp.OrderByDescending(i => i.Value).ToList();

            for (int a = 0; a <= 3; a++)
            {
                for (int s = 0; s <= 3; s++)
                {
                    if (pointTemp[a].Value == pointTemp[s].Value)
                    {
                        if (points[a].Key != points[s].Key)
                        {
                            int pos = int.Parse(position.First(x => x.Key.Contains(pointTemp[a].Key)).Value.ToString());
                            int pos2 = int.Parse(position.First(x => x.Key.Contains(pointTemp[s].Key)).Value.ToString());
                            if (pos > pos2)
                            {
                                string tempKey = pointTemp[a].Key, tempKey2 = pointTemp[s].Key;

                                pointTemp[a] = new KeyValuePair<string, double>(tempKey2, pointTemp[a].Value);
                                pointTemp[s] = new KeyValuePair<string, double>(tempKey, pointTemp[s].Value);
                            }
                        }
                    }
                }
            }

            Console.WriteLine(Convert.ToBoolean(ConfigurationManager.AppSettings["uma"]));

            if (Convert.ToBoolean(ConfigurationManager.AppSettings["uma"]) == true)
            {
                for (int b = 0; b <= 3; b++)
                {
                    if (b == 0)
                    {
                        pointTemp[b] = new KeyValuePair<string, double>(pointTemp[b].Key,
                            pointTemp[b].Value + 20);
                        Console.Write((gamenumber-1) + pointTemp[b].Key + pointTemp[b].Value);
                    }
                    else if (b == 1)
                    {
                        pointTemp[b] = new KeyValuePair<string, double>(pointTemp[b].Key,
                            pointTemp[b].Value + 10);
                        Console.Write((gamenumber-1) + pointTemp[b].Key + pointTemp[b].Value);
                    }
                    else if (b == 2)
                    {
                        pointTemp[b] = new KeyValuePair<string, double>(pointTemp[b].Key,
                            pointTemp[b].Value - 10);
                        Console.Write((gamenumber-1) + pointTemp[b].Key + pointTemp[b].Value);
                    }
                    else if (b == 3)
                    {
                        pointTemp[b] = new KeyValuePair<string, double>(pointTemp[b].Key,
                            pointTemp[b].Value - 20);
                        Console.Write((gamenumber-1) + pointTemp[b].Key + pointTemp[b].Value);
                        Console.WriteLine("");
                    }
                }
            }

            for (int t = 0; t <= 3; t++)
            {
                for (int y = 0; y <= 3; y++)
                {
                    if (pointFinal[y].Key == pointTemp[t].Key)
                    {
                        pointFinal[y] = new KeyValuePair<string, double>(pointFinal[y].Key, pointFinal[y].Value + pointTemp[t].Value);
                        break;
                    }
                }
            }

            pointFinal = pointFinal.OrderByDescending(i => i.Value).ToList();

            lineadder = 0;
            textBox5.Text = "";
            foreach (KeyValuePair<string, double> pair in pointFinal)
            {
                textBox5.AppendText(pair.Key + ": " + pair.Value);
                Console.Write(pair.Key + pair.Value + Environment.NewLine);
                lineadder++;
                if (lineadder < 4)
                {
                    textBox5.AppendText(Environment.NewLine);
                }
            }
        }

        //Uma scoring options
        private void yesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            ConfigurationManager.AppSettings["uma"] = "true";
            config.Save(ConfigurationSaveMode.Modified);
            //MessageBox.Show("Please restart for changes to take effect!");
            LoadPoints();

        }
        private void noToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            ConfigurationManager.AppSettings["uma"] = "false";
            config.Save(ConfigurationSaveMode.Modified);
            //MessageBox.Show("Please restart for changes to take effect!");
            LoadPoints();
        }

        //Get player names
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            var playerForm = new Form2();
            playerForm.Show();
        }

        //Only allow 1-4 once in the NumericUpDowns
        private void sameChecker()
        {
            if (numericUpDown1.Value > 0 && numericUpDown2.Value > 0 && numericUpDown3.Value > 0 &&
                numericUpDown4.Value > 0)
            {
                int error = 0;

                decimal[] sameCheck =
                {
                    numericUpDown1.Value, numericUpDown2.Value, numericUpDown3.Value,
                    numericUpDown4.Value
                };

                for (int o = 0; o < 4; o++)
                {
                    for (int k = 0; k < 4; k++)
                    {
                        if (sameCheck[k] == sameCheck[o])
                        {
                            if (k != o)
                            {
                                error++;
                            }
                        }
                    }
                }

                if (error > 0)
                {
                    sameCheckError = true;
                    numericUpDown1.BackColor = Color.Red;
                    numericUpDown2.BackColor = Color.Red;
                    numericUpDown3.BackColor = Color.Red;
                    numericUpDown4.BackColor = Color.Red;
                }
                else
                {
                    sameCheckError = false;
                    numericUpDown1.BackColor = Color.White;
                    numericUpDown2.BackColor = Color.White;
                    numericUpDown3.BackColor = Color.White;
                    numericUpDown4.BackColor = Color.White;
                }
            }
            else
            {
                sameCheckError = true;
                numericUpDown1.BackColor = Color.Red;
                numericUpDown2.BackColor = Color.Red;
                numericUpDown3.BackColor = Color.Red;
                numericUpDown4.BackColor = Color.Red;
            }
        }
    }
}