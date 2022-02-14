using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

namespace ParseTable
{
    public partial class Form2 : Form
    {
        string[] sgf;
        int turn = 0;
        List<Stone> stones;
        public string ParsePlayers(string rawData)
        {
            Regex PW = new Regex(@"PW\[(.*?)\]");
            Regex PB = new Regex(@"PB\[(.*?)\]");
            Regex RE = new Regex(@"RE\[(.*?)\]");

            if (PW.Matches(rawData).Count > 0 | PB.Matches(rawData).Count > 0 | RE.Matches(rawData).Count > 0)
            {
                string PWU = PW.Match(rawData).Groups[1].Value;
                string PBU = PB.Match(rawData).Groups[1].Value;
                string REU = RE.Match(rawData).Groups[1].Value;
                return $"{PBU}(B) - {PWU}(W) ({REU})";
            }
            else
            {
                return "Error";
            }
        }

        public void ParseStones()
        {
            List<Stone> stones = new List<Stone>();
            for (int i = 2; i < sgf.Length - 1; i++)
            {
                string rawData = sgf[i];
                Regex parser = new Regex(@"^(B|W)\[(.*?)\]");
                Regex timereg = new Regex(@"(B|W)L\[(.*?)\]");
                Regex overtimereg = new Regex(@"OW\[(.*?)\]");

                if (parser.Match(rawData).Groups[2].Value != "")
                {
                    string stoneData = parser.Match(rawData).Value;
                    int x = stoneData[2] - 97;
                    int y = stoneData[3] - 97;
                    bool isWhite = stoneData[0] == 'W' ? true : false;
                    string time = timereg.Match(rawData).Groups[2].Value;
                    bool isOverTime = overtimereg.Matches(rawData).Count > 0;
                    string info = "";
                    if (isOverTime)
                    {
                        info = isWhite ? $"White's turn on overtime" : $"Black's turn on overtime";
                    }
                    else
                        info = isWhite ? $"White's turn with {time} seconds left" : $"Black's turn with {time} seconds left";
                    Stone stone = new Stone(x, y, isWhite,  info);
                    stones.Add(stone);
                }
            }
            this.stones = stones;
        }
        public Form2(string[] sgfdata)
        {
            InitializeComponent();

            this.sgf = sgfdata;

            ParseStones();

            richTextBox1.SelectionAlignment = HorizontalAlignment.Center;

            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            this.Width = pictureBox1.Width + 10;

            DrawTurn();
        }
        
        public void DrawTurn()
        {
            Regex SZ = new Regex(@"SZ\[(.*?)\]");
            int dimension = Math.Min(pictureBox1.Width, pictureBox1.Height);
            int size = 1;
            if (SZ.Matches(sgf[1]).Count > 0)
            {
                size = int.Parse(SZ.Match(sgf[1]).Groups[1].Value);
                dimension /= size + 1;
            }

            Graphics g1 = Graphics.FromImage(this.pictureBox1.Image);
            g1.Clear(Color.White);

           for (int i = 0; i < size; i++)
               for (int j = 0; j < size; j++)
                   DrawCross(dimension + 5, g1, (i + 1) * dimension, (j + 1) * dimension);

            for(int i = 0; i < turn; i++)
            {
                Pen pen = new Pen(stones[i].IsWhite ? Color.Gray : Color.Black, 6f);
                g1.DrawEllipse(pen, (stones[i].X + 1) * dimension - dimension / 4,
                                    (stones[i].Y + 1) * dimension - dimension / 4,
                                    dimension / 2, dimension / 2);
                if (i ==  turn - 1)
                {
                    richTextBox1.Text = $"{ParsePlayers(sgf[1])}" + $"\n{stones[i].StoneData}";
                    richTextBox1.Refresh();
                }
            }
        }
        public void DrawCross(int length, Graphics g, int x, int y)
        {
            Pen pen = new Pen(Color.Black, 2);
            g.DrawLine(pen, x - length / 2, y, x + length / 2, y);
            g.DrawLine(pen, x, y - length / 2, x, y + length / 2);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(turn < stones.Count)
            {
                turn++;
            }
            else
            {
                turn = 0;
            }
            DrawTurn();
            pictureBox1.Refresh();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (turn > 1)
            {
                turn--;
            }
            else
            {
                turn = stones.Count;
            }
            DrawTurn();
            pictureBox1.Refresh();
        }
    }
}
