//Cody Ngo, Johhny Le  11/7/15
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Model;
using Newtonsoft.Json;
using Network_Controller;
using System.Diagnostics;

namespace View
{
    public partial class View : Form
    {
        private bool Running;
        private bool info;
        private readonly object endGame;

        private world World;
        private SolidBrush myBrush;
        private Color color;
        private StringFormat drawFormat;
        private delegate PreservedState callBack(PreservedState state);

        private SolidBrush textBrush = new SolidBrush(Color.Gold);
        private Font drawFont = new Font("Arial", 16);
        private PreservedState state;

        private cubes playerCube;
        private double sF;
        //changing key preview


        private double x_diff, y_diff;
        private const double ORIGIN_X = 300;
        private const double ORIGIN_Y = 300;

        private string yourName;
        private double yourMass;
        private double yourCalories;
        private double yourTime;
        private string[] yourAward;
        private Stopwatch timer;
        private TimeSpan totalTime;


        //FPS
        private int FPSactual;
        private int frames;
        private int seconds;
        private int start_time;
        private int end_time;
        private int arb1;
        private int arb2;


        public View()
        {
            InitializeComponent();

            FPSactual = 5;
            frames = 0;
            seconds = 0;
            start_time = DateTime.Now.Second;

            endGame = new object();
            Running = false;
            info = false;
            color = new Color();
            drawFormat = new StringFormat();
            World = new world();
            state = new PreservedState();
            timer = new Stopwatch();
            yourAward = new string[2];
            Width = World.WIDTH;
            Height = World.HEIGHT;


            FPS.Text = "FPS";
            Mass.Text = "Mass";
            Food.Text = "Food Count = 0";
            Length.Text = "Length";

            DoubleBuffered = true;

            yourMass = 0;

        }

        private int CalcFPS()
        {
            end_time = DateTime.Now.Second;

            seconds = (end_time - start_time);

            int FPS = frames / Math.Abs(seconds);

            start_time = DateTime.Now.Second;

            frames = 0;

            return FPS;
        }

        /// <summary>
        /// Helper method that checks to see if the user has pressed enter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void enterPressed(object sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case (char)13:
                    if (ServerTextBox.TextLength > 0 && PlayerNameBox.TextLength > 0)
                    {
                        try
                        {
                            PlayerNameBox.Hide();
                            ServerTextBox.Hide();
                            PlayerNameLabel.Hide();
                            ServerLabel.Hide();
                            TitleLabel.Hide();
                            state.clientSocket = Network_Controller.Network_Controller.Connect_to_Server((callBack)sendConnect, ServerTextBox.Text.ToString());
                            Running = true;
                            yourName = PlayerNameBox.Text;
                            timer.Start();

                        }
                        catch (Exception)
                        {
                            MessageBox.Show("Unable to connect to server.");
                        }
                    }
                    break;
                case (char)32:
                    if (Running)
                        split();
                    break;
            }
        }


        private PreservedState updateIn(PreservedState state)
        {
            lock (World)
            {
                if (state.incoming != null && state.incoming.Length > 0)
                {
                    //Convert JSON file back to a list of cubes
                    string message = state.incoming.ToString();
                    string[] cubes = message.Split('\n');
                    if (cubes != null)
                    {
                        for (int i = 0; i < cubes.Length; i++)
                        {
                            if (cubes[i].EndsWith("}"))
                            {
                                cubes cube = JsonConvert.DeserializeObject<cubes>(cubes[i]);

                                {
                                    if (cube != null)
                                    {
                                        if (cube.Mass == 0)
                                        {
                                            World.remove(cube);
                                        }
                                        else
                                        {
                                            World.add(cube);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                state.incoming.Clear();
                                state.incoming.Append(cubes[i]);
                            }
                        }
                    }
                }
                return state;
            }
            //Where do you call i want more data again
        }
        private void split()
        {
            string data;
            string dest_x = Cursor.Position.X.ToString();
            string dest_y = Cursor.Position.Y.ToString();
            data = "(split, " + dest_x + ", " + dest_y + ")\n";
            Network_Controller.Network_Controller.Send(state.clientSocket, data);
        }

        private void updateOut()
        {
            try
            {
                string data;
                string dest_x = (Cursor.Position.X) + "";
                string dest_y = (Cursor.Position.Y) + "";
                data = "(move, " + dest_x + ", " + dest_y + ")\n";
                Network_Controller.Network_Controller.Send(state.clientSocket, data);
            }
            catch (Exception e)
            {
                Invalidate();

                InitializeComponent();

                Running = false;
                info = false;

                World = new world();

                Width = World.WIDTH;
                Height = World.HEIGHT;

                //PlayerNameBox.Show();
                //ServerTextBox.Show();
                //PlayerNameLabel.Show();
                //ServerLabel.Show();
                //TitleLabel.Show();

                MessageBox.Show("Server is not available.");
            }
        }

        public PreservedState sendConnect(PreservedState state)
        {
            if (state.exceptions == null)
            {
                //wouldn't cast if using action<state>
                state.callback = (callBack)moreData;
                this.state = state;
                Network_Controller.Network_Controller.Send(state.clientSocket, PlayerNameBox.Text);
            }
            else
            {
                MessageBox.Show("Server Not found");

                Application.Restart();
            }
            return this.state;
        }

        public PreservedState moreData(PreservedState state)
        {
            if (state.exceptions == null)
            {
                this.state = state;
                info = true;
                state = updateIn(state);
                Network_Controller.Network_Controller.i_want_more_data(state);
            }
            else
            {
                MessageBox.Show("Server Connection Lost.");

                Application.Restart();
            }
            return this.state;
        }






        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (Running && info)
            {
                lock (World)
                {
                    if (World.cubeCount() > 0)
                    {

                        end_time = DateTime.Now.Second;

                        int timeElapsed = end_time - start_time;

                        if (timeElapsed > 10)
                        {
                            FPSactual = CalcFPS();
                            FPS.Text = "FPS = " + FPSactual;
                        }

                        int offSet = 90;

                        if (World.playerCount() > 0)
                        {

                            playerCube = World.getPlayer(PlayerNameBox.Text);
                            if (playerCube != null && playerCube.Mass == 0)
                            {
                                Running = false;
                                info = false;
                            }

                            //View port code? Yes.
                            x_diff = playerCube.loc_x - ORIGIN_X + 90;
                            y_diff = playerCube.loc_y - ORIGIN_Y + 90;
                            sF = 100 / (playerCube.getLength() / 10);
                            double sF2 = playerCube.getLength() / (-2);

                            FPS.Update();
                            arb1 = end_time;
                            arb2 = start_time;
                            Mass.Text = "Mass = " + playerCube.Mass;
                            Mass.Update();
                            Length.Text = "Length = " + playerCube.getLength();
                            Length.Update();
                            Food.Text = "Cube Count = " + World.cubeCount();
                            Food.Update();

                            foreach (cubes ncube in World.GetPlayerCubes())
                            {
                                //Player cube
                                if (ncube.uid == playerCube.uid)
                                {
                                    color = Color.FromArgb(ncube.argb_color);
                                    myBrush = new SolidBrush(color);

                                    e.Graphics.FillRectangle(myBrush, new Rectangle((int)(ORIGIN_X - (ncube.getLength() / (1.5)) + offSet * 2), (int)(ORIGIN_Y - (ncube.getLength() / (1.5)) + offSet * 2), (int)(ncube.getLength() * 2), (int)(ncube.getLength() * 2)));

                                    string drawString = ncube.Name.ToString();

                                    e.Graphics.DrawString(drawString, drawFont, textBrush, ((int)(ORIGIN_X - (ncube.getLength() / (1.5)) + offSet * 2) + (int)ncube.getLength() / 2 - drawString.Length / 2 * 12), ((int)(ORIGIN_Y - (ncube.getLength() / (1.5)) + offSet * 2) + (int)ncube.getLength() / 2 - 10), drawFormat);
                                }
                                else
                                {
                                    color = Color.FromArgb(ncube.argb_color);
                                    myBrush = new SolidBrush(color);

                                    e.Graphics.FillRectangle(myBrush, new Rectangle((int)((ncube.loc_x - x_diff) * 2 + sF), (int)((ncube.loc_y - y_diff) * 2 + sF), (int)(ncube.getLength() * 2), (int)(ncube.getLength() * 2)));

                                    string drawString = ncube.Name.ToString();

                                    e.Graphics.DrawString(drawString, drawFont, textBrush, ((int)((ncube.loc_x - x_diff) * 2 + sF) + (int)ncube.getLength() / 2 - drawString.Length / 2 * 12), ((int)((ncube.loc_y - y_diff) * 2 + sF) + (int)ncube.getLength() / 2 - 10), drawFormat);

                                }

                            }
                        }
                        foreach (cubes ncube in World.GetFoodCubes())
                        {
                            color = Color.FromArgb(ncube.argb_color);
                            myBrush = new SolidBrush(color);

                            e.Graphics.FillRectangle(myBrush, new Rectangle((int)((ncube.loc_x - x_diff) * 2 + offSet + sF), (int)((ncube.loc_y - y_diff) * 2 + offSet + sF), (int)(ncube.getLength() * sF), (int)(ncube.getLength() * sF)));
                        }
                    }
                }

                if (playerCube != null && playerCube.Mass > yourMass)
                {
                    yourMass = playerCube.Mass;
                    yourCalories = yourMass * 4;
                }

                if (playerCube != null && playerCube.Mass <= 1 && !playerCube.food)
                {
                    timer.Stop();
                    totalTime = timer.Elapsed;

                    calcAwards();
                }

                updateOut();
                Invalidate();
                frames++;
            }
        }

        private void calcAwards()
        {
            lock (endGame)
            {
                yourAward[0] = "Starved";
                string elapsedTime = "";
                if (yourMass > 2000)
                {
                    yourAward[0] = "Basic";
                    yourAward[1] = " ";
                }
                if (yourMass > 5000)
                {
                    yourAward[0] = "Obese";
                    yourAward[1] = " ";
                }
                if (totalTime.Minutes > 1)
                {
                    yourAward[1] = ("Survivalist");
                    elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                        totalTime.Hours, totalTime.Minutes, totalTime.Seconds,
                        totalTime.Milliseconds / 10);
                }
                else
                {
                    yourAward[1] = "Casual";
                }
                MessageBox.Show(yourName + ", the " + yourAward.ElementAt(0) + " " + yourAward.ElementAt(1) + "\nHighest Mass: " + yourMass + "\nCalories Consumed: " + yourCalories + "\nTime Survived: " + "Not Long Enough.");
                Application.Restart();
            }
        }

    }
}