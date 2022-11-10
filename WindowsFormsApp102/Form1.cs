using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using static System.Net.WebRequestMethods;

namespace WindowsFormsApp102
{
    public partial class Form1 : Form
    {
        int score = 0;
        int timeRemaining = 59;
        List<int> scores = new List<int>();
        List<PictureBox> bullets = new List<PictureBox>();
        List<PictureBox> enemyBullets = new List<PictureBox>();
        Random r;
        SoundPlayer s1, s2, s3, s4;
        public Form1()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            foreach(PictureBox p in bullets)
            {
                p.Location = new Point(p.Location.X, p.Location.Y - 10);
                if((p.Location.X > pictureBox2.Location.X - 52 && p.Location.X < pictureBox2.Location.X +52) && (Math.Abs(pictureBox2.Location.Y +100 - p.Location.Y)< 5))
                {
                    s3.Play();
                    score += 10;
                    label1.Text = "Score: " + score.ToString();
                    p.Location = new Point(-100, p.Location.Y+20);
                }
            }
            foreach(PictureBox p in enemyBullets)
            {
                p.Location = new Point(p.Location.X, p.Location.Y + 10);
                if((p.Location.X > pictureBox1.Location.X - 43 && p.Location.X < pictureBox1.Location.X + 43) && (Math.Abs(pictureBox1.Location.Y - 50 - p.Location.Y) < 5))
                {
                    s4.Play();
                    if (score >= 10) score -= 10;
                    else score = 0;
                    label1.Text = "Score: " + score.ToString();
                    p.Location = new Point(-100, p.Location.Y + 20);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            Stream s = System.IO.File.Open("temp.dat",FileMode.Open);
            BinaryFormatter b = new BinaryFormatter();
            scores = (List<int>)b.Deserialize(s);
            s.Close();
            pictureBox1.ImageLocation = "spaceship.png";
            r = new Random();
            s1 = new SoundPlayer("invaderkilled.wav");
            s2 = new SoundPlayer("bam2.wav");
            s3 = new SoundPlayer("yes.wav");
            s4 = new SoundPlayer("ouch.wav");
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.ToString().Equals("Left"))
            {
                pictureBox1.Location = new Point(pictureBox1.Location.X - 15, pictureBox1.Location.Y);
            }
            else if (e.KeyCode.ToString().Equals("Right"))
            {
                pictureBox1.Location = new Point(pictureBox1.Location.X + 15, pictureBox1.Location.Y);
            }
            else if (e.KeyCode.ToString().Equals("Up"))
            {
                pictureBox1.Location = new Point(pictureBox1.Location.X, pictureBox1.Location.Y - 15);
            }
            else if (e.KeyCode.ToString().Equals("Down"))
            {
                pictureBox1.Location = new Point(pictureBox1.Location.X, pictureBox1.Location.Y + 15);
            }
            else if (e.KeyCode.ToString().Equals("Space"))
            {
                createBullet(pictureBox1.Location.X);
            }
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            if (timeRemaining > 0)
            {
                timeRemaining--;
                countdown.Text = "Time Remaining: 00:" + (timeRemaining<10 ? "0" : "") + timeRemaining.ToString();
            }
            else
            {
                timer1.Enabled = false;
                timer2.Enabled = false;
                if(scores.Count < 10)
                {
                    scores.Add(score);
                    scores.Sort();
                    scores = Enumerable.Reverse(scores).ToList();
                }
                else if(scores[0] < score) scores[0] = score;
                score = 0;
                timeRemaining = 59;
                foreach (PictureBox p in bullets)
                {
                    p.Location = new Point(-100, p.Location.Y + 20);

                }
                foreach (PictureBox p in enemyBullets)
                {
                    p.Location = new Point(-100, p.Location.Y + 20);

                }
                timer3.Enabled = false;
                MessageBox.Show("Select an option from the menu to continue.");
                menuStrip1.Visible = true;

            }
        }

        private void playAgainToolStripMenuItem_Click(object sender, EventArgs e)
        {
            menuStrip1.Visible = false;
            timer1.Enabled = true;
            timer2.Enabled = true;
            timer3.Enabled = true;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Stream s = System.IO.File.Open("temp.dat",FileMode.Create);
            BinaryFormatter b = new BinaryFormatter();
            b.Serialize(s, scores);
            s.Close();
        }

        private void showTop10ScoresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int i = 1;
            String result = ""; 
            foreach(int s in scores)
            {
                result += i.ToString() + ". " + s + Environment.NewLine;
                i++;
            }
            MessageBox.Show(result);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void createBullet(int startX)
        {
            PictureBox p = new PictureBox();
            p.ImageLocation = "bullet_icon.png";
            p.Location = new Point(startX+5, pictureBox1.Location.Y-50);
            p.Size = new Size(60, 50);
            p.SizeMode = PictureBoxSizeMode.StretchImage;
            Controls.Add(p);
            bullets.Add(p);
            s1.Play();
        }

        private void createBulletEnemy(int startX)
        {
            PictureBox p = new PictureBox();
            p.ImageLocation = "bullet_icon2.png";
            p.Location = new Point(startX + 5, pictureBox2.Location.Y + 100);
            p.Size = new Size(60, 50);
            p.SizeMode = PictureBoxSizeMode.StretchImage;
            Controls.Add(p);
            enemyBullets.Add(p);
            s2.Play();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            createBulletEnemy(pictureBox2.Location.X);
            pictureBox2.Location = new Point(r.Next(Width - pictureBox2.Width), pictureBox2.Location.Y);
        }
    }
}
