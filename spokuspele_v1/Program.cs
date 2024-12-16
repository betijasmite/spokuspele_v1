using System;
using System.Drawing;
using System.Windows.Forms;

namespace AvoidGhostsGame
{
    public class MainForm : Form
    {
        private Button startButton;
        private Label titleLabel;
        private Label scoreLabel;
        private Label gameOverLabel;
        private Button[] doorButtons;
        private int score;
        private Random random;
        private int ghostDoorIndex;

        public MainForm()
        {
            this.Text = "Izvairies no spokiem!";
            this.Size = new Size(400, 300);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.Black;

            titleLabel = new Label
            {
                Text = "Izvairies no spokiem!",
                Font = new Font("Arial", 16, FontStyle.Bold),
                ForeColor = Color.Red,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 50
            };

            startButton = new Button
            {
                Text = "Sākt spēli",
                Dock = DockStyle.Bottom,
                Height = 40,
                ForeColor = Color.Red,
                BackColor = Color.Black 
            };
            startButton.Click += StartGame;

            scoreLabel = new Label
            {
                Text = "Punkti: 0",
                Font = new Font("Arial", 12, FontStyle.Bold),
                ForeColor = Color.Red, 
                Location = new Point(10, 10),
                AutoSize = true,
                Visible = false
            };

            gameOverLabel = new Label
            {
                Text = "Tevi nobiedēja spoks. Spēle beigusies!",
                Font = new Font("Arial", 14, FontStyle.Bold),
                ForeColor = Color.Red, 
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(50, 100), 
                Width = 300,
                Height = 50,
                Visible = false
            };

            this.Controls.Add(titleLabel);
            this.Controls.Add(startButton);
            this.Controls.Add(scoreLabel);
            this.Controls.Add(gameOverLabel);

            random = new Random();
        }

        private void StartGame(object sender, EventArgs e)
        {
            // Atiestatīt punktu skaitu
            score = 0;
            scoreLabel.Text = "Punkti: 0";
            scoreLabel.Visible = true;

            startButton.Visible = false;
            titleLabel.Visible = false;
            gameOverLabel.Visible = false;

            // Izveidot durvis kā pogas
            CreateDoors();
        }

        private void CreateDoors()
        {
            if (doorButtons != null)
            {
                foreach (var button in doorButtons)
                {
                    this.Controls.Remove(button);
                }
            }

            doorButtons = new Button[3];
            ghostDoorIndex = random.Next(0, 3);

            for (int i = 0; i < 3; i++)
            {
                var doorButton = new Button
                {
                    Text = "Durvis " + (i + 1),
                    Width = 100,
                    Height = 50,
                    Location = new Point(50 + i * 110, 150),
                    ForeColor = Color.Red, 
                    BackColor = Color.Black 
                };

                doorButton.Click += (sender, e) => OpenDoor(i);
                doorButtons[i] = doorButton;
                this.Controls.Add(doorButton);
            }
        }

        private void OpenDoor(int doorIndex)
        {
            bool hasGhost = doorIndex == ghostDoorIndex;

            if (hasGhost)
            {
                GameOver();
            }
            else
            {
                score++;
                scoreLabel.Text = $"Punkti: {score}";
                CreateDoors();
            }
        }

        private void GameOver()
        {
            foreach (var button in doorButtons)
            {
                this.Controls.Remove(button);
            }

            gameOverLabel.Visible = true; 
            gameOverLabel.BringToFront(); 

            var timer = new Timer { Interval = 30000};
            timer.Tick += (s, e) =>
            {
                timer.Stop();
                titleLabel.Visible = true;
                startButton.Visible = true;
                scoreLabel.Visible = false;
                gameOverLabel.Visible = false;
            };
            timer.Start();
        }

        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new MainForm());
        }
    }
}
