using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows;

namespace Game_of_life
{
    public partial class Form1 : Form
    {
        public bool[,] cells = new bool[50, 50];
        public bool started = false;
        Bitmap kép = new Bitmap(500, 500);

        public Form1()
        {
            InitializeComponent();
        }

        void uresNegyzet(int x, int y)
        {
            for (int i = 0; i < 10; i++)
            {
                kép.SetPixel(x * 10 + i, y * 10, Color.LightGray);
                kép.SetPixel(x * 10 + i, y * 10 + 9, Color.LightGray);
                kép.SetPixel(x * 10, y * 10 + i, Color.LightGray);
                kép.SetPixel(x * 10 + 9, y * 10 + i, Color.LightGray);
            }
        }

        void teliNegyzet(int x, int y, Color szín)
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    kép.SetPixel(x * 10 + i, y * 10 + j, szín);
                }
            }
        }
        
        public int checkNeighbours(int x, int y)
        {
            if (!(x < 0 || x > 49 || y < 0 || y > 49))
            {
                if (cells[x, y] == true)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            return 0;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            pictureBox1.BackColor = Color.White;
            DoubleBuffered = true;
            pictureBox1.Image = kép;
            for (int i = 0; i < 50; i++)
            {
                for (int j = 0; j < 50; j++)
                {
                    teliNegyzet(i, j, Color.White);
                    uresNegyzet(i, j);
                }
            }
            for (int i = 0; i < 50; i++)
            {
                for (int j = 0; j < 50; j++)
                {
                    cells[i, j] = false;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            button2.Text = "Start";
            started = false;
            for (int i = 0; i < 50; i++)
            {
                for (int j = 0; j < 50; j++)
                {
                    teliNegyzet(i, j, Color.White);
                    uresNegyzet(i, j);
                }
            }
            for (int i = 0; i < 50; i++)
            {
                for (int j = 0; j < 50; j++)
                {
                    cells[i, j] = false;
                }
            }
            pictureBox1.Image = kép;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            Point coordinates = me.Location;
            int x = (coordinates.X - (coordinates.X % 10)) / 10;
            int y = (coordinates.Y - (coordinates.Y % 10)) / 10;
            if (!cells[x, y])
            {
                cells[x, y] = true;
                teliNegyzet(x, y, Color.Black);
            }
            else
            {
                cells[x, y] = false;
                teliNegyzet(x, y, Color.White);
                uresNegyzet(x, y);
            }
            pictureBox1.Image = kép;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!started)
            {
                timer1.Start();
                button2.Text = "Stop";
            }
            else if (started)
            {
                timer1.Stop();
                button2.Text = "Start";
            }
            started = !started;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            bool[,] futureCells = new bool[50, 50];
            for (int i = 0; i < 50; i++)
            {
                for (int j = 0; j < 50; j++)
                {
                    futureCells[i, j] = false;
                    int livingCellsNearby = 0;
                    livingCellsNearby += checkNeighbours(i - 1, j - 1) + checkNeighbours(i - 1, j) + checkNeighbours(i - 1, j + 1) +    //XXX
                        checkNeighbours(i, j - 1) + checkNeighbours(i, j + 1) +                                                         //XOX
                        checkNeighbours(i + 1, j - 1) + checkNeighbours(i + 1, j) + checkNeighbours(i + 1, j + 1);                      //XXX
                    if (cells[i,j] && (livingCellsNearby == 2 || livingCellsNearby == 3))
                    {
                        futureCells[i,j] = true;
                    }
                    else if (!cells[i, j] && livingCellsNearby == 3)
                    {
                        futureCells[i, j] = true;
                    }

                }
            }
            
            for (int i = 0; i < 50; i++)
            {
                for (int j = 0; j < 50; j++)
                {
                    if (futureCells[i, j] != cells[i, j])
                    {
                        if (futureCells[i, j] == true)
                        {
                            teliNegyzet(i, j, Color.Black);
                        }
                        else if (futureCells[i, j] == false)
                        {
                            teliNegyzet(i, j, Color.White);
                            uresNegyzet(i, j);
                        }
                    }
                }
            }

            cells = futureCells;
            pictureBox1.Image = kép;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && Convert.ToInt32(textBox1.Text) != 0)
            {
                timer1.Interval = Convert.ToInt32(textBox1.Text);
            }
            else if (textBox1.Text != "" && Convert.ToInt32(textBox1.Text) == 0)
            {
                MessageBox.Show("Nem lehet 0");
            }
        }
    }
}
