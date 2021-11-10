using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMPLib;
using System.Windows.Forms;


namespace Simple_Space_Game_2
{
    public partial class Form1 : Form
    {
        WindowsMediaPlayer gameMedia;
        WindowsMediaPlayer shootgMedia;
        WindowsMediaPlayer explosion;

        PictureBox[] stars;
        int backgroundspeed;
        int playerSpeed;

        PictureBox[] munitions;
        int MunitionSpeed;

        PictureBox[] enemy;
        int enemySpeed;

        PictureBox[] enemiesMunition;
        int enemiesMunitionSpeed;

        Random rnd;

        int score;
        int level;
        int difficulty;
        bool pause;
        bool gameover;

        public Form1()
        {
            InitializeComponent();
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            pause = false;
            gameover = false;
            score = 0;
            level = 1;
            difficulty = 9;
            
            backgroundspeed = 4;
            playerSpeed = 4;
            enemySpeed = 4;
            enemiesMunitionSpeed = 4;
            MunitionSpeed = 20;

            munitions = new PictureBox[3];
            enemy = new PictureBox[10];

            // Load Images (Map from project debug folder where images are saved)
            Image munition = Image.FromFile(@"Images\Ammo.jpg");

            Image enemy1 = Image.FromFile(@"Images\E1.jpg");
            Image enemy2 = Image.FromFile(@"Images\E2.jpg");
            Image enemy3 = Image.FromFile(@"Images\E3.jpg");
            Image boss1 = Image.FromFile(@"Images\Enemy Boss.png");
            Image boss2 = Image.FromFile(@"Images\Enemy Boss 2.png");

            //Create WindowsMediaPlayer

            gameMedia = new WindowsMediaPlayer();
            shootgMedia = new WindowsMediaPlayer();
            explosion = new WindowsMediaPlayer();

            //Load all songs

            gameMedia.URL = "Sounds\\Background.mp4";
            shootgMedia.URL = "Sounds\\jetshooting2.mp4";
            explosion.URL = "Sounds\\explosion3.mp4";

            //Song Settings

            gameMedia.settings.setMode("loop", true);
            gameMedia.settings.volume = 5;
            shootgMedia.settings.volume = 4;
            explosion.settings.volume = 10;
            gameMedia.controls.play();

            //Initialize EnemyPictureBoxes

            for (int i = 0; i < enemy.Length; i++)
            {
                enemy[i] = new PictureBox();
                enemy[i].Size = new Size(50, 50);
                enemy[i].SizeMode = PictureBoxSizeMode.Zoom;
                enemy[i].BorderStyle = BorderStyle.None;
                enemy[i].Visible = false;
                this.Controls.Add(enemy[i]);
                enemy[i].Location = new Point((i + 1) * 50, -50);
            }

            enemy[0].Image = boss1;
            enemy[1].Image = enemy2;
            enemy[2].Image = enemy3;
            enemy[3].Image = enemy1;
            enemy[4].Image = enemy2;
            enemy[5].Image = enemy3;
            enemy[6].Image = enemy1;
            enemy[7].Image = enemy2;
            enemy[8].Image = enemy3;
            enemy[9].Image = boss2;

            
            for (int i = 0; i < munitions.Length; i++)
            {
                munitions[i] = new PictureBox();
                munitions[i].Size = new Size(8, 8);
                munitions[i].Image = munition;
                munitions[i].SizeMode = PictureBoxSizeMode.Zoom;
                munitions[i].BorderStyle = BorderStyle.None;
                this.Controls.Add(munitions[i]);
            }
            stars = new PictureBox[10];
            rnd = new Random();

            for (int i = 0; i < stars.Length; i++)
            {
                stars[i] = new PictureBox();
                stars[i].BorderStyle = BorderStyle.None;
                stars[i].Location = new Point(rnd.Next(20, 580), rnd.Next(-10, 400));
                if (i % 2 == 1)
                {
                    stars[i].Size = new Size(2, 2);
                    stars[i].BackColor = Color.White;
                }
                else
                {
                    stars[i].Size = new Size(3, 3);
                    stars[i].BackColor = Color.White;
                }

                this.Controls.Add(stars[i]);
            }

            //Ememies Munition
            enemiesMunition = new PictureBox[10];
            for (int i = 0; i < enemiesMunition.Length; i++)
            {
                enemiesMunition[i] = new PictureBox();
                enemiesMunition[i].Size = new Size(2, 10);
                enemiesMunition[i].Visible = false;
                enemiesMunition[i].BackColor = Color.Red;
                int x = rnd.Next(0, 10);
                enemiesMunition[i].Location = new Point(enemy[x].Location.X, enemy[x].Location.Y - 20);
                this.Controls.Add(enemiesMunition[i]);
            }

        }

        private void MoveBGTimer_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < stars.Length / 2; i++)
            {
                stars[i].Top += backgroundspeed;

                if (stars[i].Top >= this.Height)
                {
                    stars[i].Top = -stars[i].Height;
                }
            }
            for (int i = stars.Length / 2; i < stars.Length; i++)
            {
                stars[i].Top += backgroundspeed - 2;
                if (stars[i].Top >= this.Height)
                {
                    stars[i].Top = -stars[i].Height;
                }
            }
        }

        private void LeftMoveTimer_Tick(object sender, EventArgs e)
        {
            if (Player.Left > 10)
            {
                Player.Left -= playerSpeed;
            }

        }

        private void RightMoverTimer_Tick(object sender, EventArgs e)
        {
            if (Player.Right < 580)
            {
                Player.Left += playerSpeed;
            }
        }

        private void DownMoveTimer_Tick(object sender, EventArgs e)
        {
            if (Player.Top < 400)
            {
                Player.Top += playerSpeed;
            }
        }

        private void UpMoveTimer_Tick(object sender, EventArgs e)
        {
            if (Player.Top > 10)
            {
                Player.Top -= playerSpeed;
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (!pause)
            {
                if (e.KeyCode == Keys.Right)
                {
                    RightMoverTimer.Start();
                }
                if (e.KeyCode == Keys.Left)
                {
                    LeftMoveTimer.Start();
                }
                if (e.KeyCode == Keys.Down)
                {
                    DownMoveTimer.Start();
                }
                if (e.KeyCode == Keys.Up)
                {
                    UpMoveTimer.Start();
                }
            }
                          
         }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            RightMoverTimer.Stop();
            LeftMoveTimer.Stop();
            DownMoveTimer.Stop();
            UpMoveTimer.Stop();

            if(e.KeyCode == Keys.Space)
            {
                if (!gameover)
                {
                    if (pause)
                    {
                        StartTimers();
                        label3.Visible = false;
                        gameMedia.controls.play();
                        pause = false;
                    }
                    else
                    {
                        label3.Location = new Point(this.Width/2-120, 150);
                        label3.Text = "Paused";
                        label3.Visible = true;
                        gameMedia.controls.pause();
                        StopTimers();
                        pause = true;
                    }
                }
            }
        }

        private void MoveMunitionTimer_Tick(object sender, EventArgs e)
        {
            shootgMedia.controls.play();
            for (int i = 0; i < munitions.Length; i++)
            {
                if (munitions[i].Top > 0)
                {
                    munitions[i].Visible = true;
                    munitions[i].Top -= MunitionSpeed;

                    Collision();
                }
                else
                {
                    munitions[i].Visible = false;
                    munitions[i].Location = new Point(Player.Location.X + 20, Player.Location.Y - i * 30);
                }
            }
        }

        private void MoveEnemiesTimer_Tick(object sender, EventArgs e)
        {
            MoveEnemy(enemy, enemySpeed);
        }

        private void MoveEnemy(PictureBox[] array, int speed)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i].Visible = true;
                array[i].Top += speed;

                if (array[i].Top > this.Height)
                {
                    array[i].Location = new Point((i + 1) * 50, -200);
                }
            }
        }

        private void Collision()
        {
            for (int i = 0; i < enemy.Length; i++)
            {
                if (munitions[0].Bounds.IntersectsWith(enemy[i].Bounds) || munitions[1].Bounds.IntersectsWith(enemy[i].Bounds) || munitions[2].Bounds.IntersectsWith(enemy[i].Bounds))
                {
                    explosion.controls.play();

                    score += 1;
                    scorelbl.Text = (score < 10) ? "0" + score.ToString() : score.ToString();

                    if (score % 30 == 0)
                    {
                        level += 1;
                        levellbl.Text = (level < 10) ? "0" + level.ToString() : level.ToString();

                        if(enemySpeed <= 10 && enemiesMunitionSpeed <= 10 && difficulty >= 0)
                        {
                            difficulty--;
                            enemySpeed++;
                            enemiesMunitionSpeed++;
                        }

                        if (level == 10)
                        {
                            GameOver("CONGRATULATIONS YOU WIN");
                        }
                    }

                    enemy[i].Location = new Point((i + 1) * 50, -100);
                }
                if (Player.Bounds.IntersectsWith(enemy[i].Bounds))
                {
                    explosion.settings.volume = 30;
                    explosion.controls.play();
                    Player.Visible = false;
                    GameOver("Game Over");
                }
                
            }
        }

        private void GameOver(string str)
        {
            label3.Text = str;
            label3.Location = new Point(190, 170);
            label3.Visible = true;
            ReplayButton.Visible = true;
            ExitButton.Visible = true;

            gameMedia.controls.stop();
            StopTimers();
        }

        private void StopTimers()
        {
            MoveBGTimer.Stop();
            MoveEnemiesTimer.Stop();
            MoveMunitionTimer.Stop();
            EnemyMunitionTimer.Stop();
          
        }

        private void StartTimers()
        {
            MoveBGTimer.Start();
            MoveEnemiesTimer.Start();
            MoveMunitionTimer.Start();
            EnemyMunitionTimer.Start();
        }

        private void EnemyMunitionTimer_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < (enemiesMunition.Length - difficulty); i++)
            {
                if (enemiesMunition[i].Top < this.Height)
                {
                    enemiesMunition[i].Visible = true;
                    enemiesMunition[i].Top += enemiesMunitionSpeed;

                    CollisionWithEnemiesMunition();
                }
                else
                {
                    enemiesMunition[i].Visible = false;
                    int x = rnd.Next(0, 10);
                    enemiesMunition[i].Location = new Point(enemy[x].Location.X + 20, enemy[x].Location.Y + 10);
                }
            }
        }

        private void CollisionWithEnemiesMunition()
        {
            for (int i = 0; i < enemiesMunition.Length; i++)
            {
                if (enemiesMunition[i].Bounds.IntersectsWith(Player.Bounds))
                {
                    enemiesMunition[i].Visible = false;
                    explosion.settings.volume = 30;
                    explosion.controls.play();
                    Player.Visible = false;
                    GameOver("Game Over");
                }
            }
        }

        private void ReplayButton_Click(object sender, EventArgs e)
        {
            this.Controls.Clear();
            InitializeComponent();
            Form1_Load(e,e);

        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            Environment.Exit(1);
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void scorelbl_TextChanged(object sender, EventArgs e)
        {

        }
    }

}           
