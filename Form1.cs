using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using static ParseTable.JsonFormsDownsream;
using Newtonsoft.Json;

namespace ParseTable
{
    public partial class Form1 : Form
    {
        List<Player> Players = new List<Player>();
        DataTable table = new DataTable();

        public void GetPlayers(ref List<Player> Players)
        {
            string line = "";
            using (WebClient webClient = new WebClient())
            {
                line = webClient.DownloadString("https://gokgs.com/top100.jsp");
            }
            Regex name = new Regex("user=(.*?)\"");
            Regex rating = new Regex(@"(\dd|\?)</");
            MatchCollection names = name.Matches(line);
            MatchCollection ratings = rating.Matches(line);
            for (int i = 0; i < 100; i++)
            {
                Player player = new Player(names[i].Groups[1].Value, ratings[i].Groups[1].Value, "-", "-");
                Players.Add(player);
            }


        }

        public Form1()
        {
            InitializeComponent();

        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            this.Width = dataGridView1.Width;

            table.Columns.Add("Number", typeof(int));
            table.Columns.Add("Name", typeof(string));
            table.Columns.Add("Rating", typeof(string));
            table.Columns.Add("Last game", typeof(string));
            table.Columns.Add("Previous game", typeof(string));

            GetPlayers(ref Players);

            ParseGames(Players);

            for (int i = 0; i < 100; i++)
            {
                table.Rows.Add(i + 1, Players[i].Name, Players[i].Rating, Players[i].FirstGame, Players[i].SecondGame);
            }


            dataGridView1.DataSource = table;
        }

        async Task ParseGames(List<Player> players)
        {
            await API.Login();

            for (int i = 0; i < players.Count; i++)
            {
                JsonFormsDownsream.Message msg = await API.GetAllGames(players[i].Name);
                for (int j = 0; j < 2; j++)
                {
                    if (msg.type == "ARCHIVE_JOIN")
                    {
                        try
                        {
                            players[i].FirstGame = msg.games[0].ToString();
                            players[i].SecondGame = msg.games[1].ToString();
                            table.Rows[i][3] = msg.games[0].score.ToString();
                            table.Rows[i][4] = msg.games[1].score.ToString();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(JsonConvert.SerializeObject(msg));
                            Console.WriteLine(ex.Message);
                        }
                    }
                    else
                    {
                        players[i].Exception =  msg.type.ToString();
                    }
                }


            }

            await API.Logout();

        }
        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if ((e.ColumnIndex == 3 | e.ColumnIndex == 4) & e.RowIndex != -1)
                {
                    string nameDesired = table.Rows[e.RowIndex][1].ToString();
                    string link = "";
                    foreach (Player player in Players)
                    {
                        if (player.Name == nameDesired)
                        {
                            if (player.Exception == null)
                            {
                                link = e.ColumnIndex == 3 ? player.FirstGame : player.SecondGame;
                            }
                            else
                            {
                                throw new Exception($"Server returned:  {player.Exception}");
                            }
                        }
                    }
                    string gamedata = "";

                    using (WebClient webClient = new WebClient())
                    {
                        gamedata = webClient.DownloadString(link);
                    }

                    if (gamedata.Split(";").Length < 3)
                        throw new ArgumentException("Game had no turns.");
                    Form2 form2 = new Form2(gamedata.Split(";"));
                    form2.Show();
                    form2.Text = e.ColumnIndex == 3 ? "Last game" : "Previous game";
                    form2.Focus();
                }
            }

            //  на  forbidden
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
    }

}

