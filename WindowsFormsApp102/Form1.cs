using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Windows.Forms;

namespace paixnidi2
{
    public partial class Form1 : Form
    {
        SQLiteConnection connection;
        String connectionString = "Data source=identifier.sqlite;Version=3";
        public bool playing = false;
        int score = 0;
        int timeRemaining = 59;
        List<int> scores = new List<int>(); // lista me top 10 scores
        List<PictureBox> bullets = new List<PictureBox>();
        List<PictureBox> enemyBullets = new List<PictureBox>();
        SoundPlayer s1, s2, s3, s4; // hxhtika efe
        Enemy lagos;
        Hrwas farmer;

        public Form1()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // Elegxos an xtypithike o lagos
            foreach (PictureBox p in bullets)
            {
                p.Location = new Point(p.Location.X, p.Location.Y - 10);
                if ((p.Location.X > pictureBox2.Location.X - 52 && p.Location.X < pictureBox2.Location.X + 52) && (Math.Abs(pictureBox2.Location.Y + 100 - p.Location.Y) < 5))
                {
                    s3.Play();
                    score += 10;
                    label1.Text = "Score: " + score.ToString();
                    p.Location = new Point(-100, p.Location.Y + 20);
                }
            }
            // Elegxos an xtypithike o agrwths
            foreach (PictureBox p in enemyBullets)
            {
                p.Location = new Point(p.Location.X, p.Location.Y + 10);
                if ((p.Location.X > pictureBox1.Location.X - 43 && p.Location.X < pictureBox1.Location.X + 43) && (Math.Abs(pictureBox1.Location.Y - 50 - p.Location.Y) < 5))
                {
                    s4.Play();
                    if (score >= 10) score -= 10;
                    else score = 0;
                    label1.Text = "Score: " + score.ToString();
                    p.Location = new Point(-100, p.Location.Y + 20);
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            pictureBox2.BringToFront(); // Fernoyme to lago brosta apo ta labels
            pictureBox1.ImageLocation = "farmer.png";
            // Anaktoyme ta scores apo th bash 
            connection = new SQLiteConnection(connectionString);
            connection.Open();
            String selectSQL = "Select score from Stats";
            SQLiteCommand command = new SQLiteCommand(selectSQL, connection);
            SQLiteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                scores.Add(reader.GetInt32(0));
            }
            s1 = new SoundPlayer("invaderkilled.wav");
            s2 = new SoundPlayer("bam2.wav");
            s3 = new SoundPlayer("yes.wav");
            s4 = new SoundPlayer("ouch.wav");
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (playing)
            {
                farmer.kinhshHrwa(e);
                if (e.KeyCode.ToString().Equals("Space"))
                {
                    createBullet(pictureBox1.Location.X);
                }
            }
        }
        private void timer3_Tick(object sender, EventArgs e)
        {
            if (timeRemaining > 0)
            {
                timeRemaining--;
                countdown.Text = "Time Remaining: 00:" + (timeRemaining < 10 ? "0" : "") + timeRemaining.ToString();
            }
            else
            {
                timer1.Enabled = false;
                timer2.Enabled = false;
                label1.Visible = false;
                countdown.Visible = false;
                if (scores.Count < 10)
                {
                    scores.Add(score);
                    scores.Sort();
                    scores = Enumerable.Reverse(scores).ToList();
                }
                else if (scores[0] < score) scores[0] = score;

                label1.Text = "Score: " + score.ToString();
                timeRemaining = 59;
                playing = false;
                // Diagrafh sfairwn
                foreach (PictureBox p in bullets)
                {
                    p.Location = new Point(-100, p.Location.Y + 20);

                }
                foreach (PictureBox p in enemyBullets)
                {
                    p.Location = new Point(-100, p.Location.Y + 20);

                }
                // Apothikeysh dedomenwn sth bash
                connection = new SQLiteConnection(connectionString);
                connection.Open();
                String createSQL = "Create table if not exists Stats(id integer auto increment primary key, name text" +
                    "score integer)";
                SQLiteCommand command = new SQLiteCommand(createSQL, connection);
                command.ExecuteNonQuery();
                String insertSQL = "Insert into Stats(name,score) values('" + textBox1.Text + "'," + score.ToString() + ")";
                command = new SQLiteCommand(insertSQL, connection);
                command.ExecuteNonQuery();
                connection.Close();
                score = 0;
                timer3.Enabled = false;
                menuStrip1.Visible = true;
                label2.Visible = true;
                label3.Visible = true;
                textBox1.Text = "";
                textBox1.Visible = true;
                textBox1.Enabled = true;
                radioButton1.Visible = true;
                radioButton1.Enabled = true;
                radioButton2.Visible = true;
                radioButton2.Enabled = true;
                button1.Enabled = true;
                button1.Visible = true;

            }
        }

        private void showTop10ScoresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int i = 1;
            String result = "";
            foreach (int s in scores)
            {
                connection = new SQLiteConnection(connectionString);
                connection.Open();
                String selectSQL = "Select name from Stats where score = " + s.ToString();
                SQLiteCommand command = new SQLiteCommand(selectSQL, connection);
                SQLiteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    if (!result.Contains(reader.GetString(0)))
                    {
                        result += i.ToString() + ". " + reader.GetString(0) + Environment.NewLine;
                        i++;
                    }
                }
                connection.Close();
            }
            MessageBox.Show(result);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            lagos = new Enemy(pictureBox2, radioButton1.Checked);
            farmer = new Hrwas(pictureBox1);
            label5.Text = "User: " + textBox1.Text;
            menuStrip1.Visible = false;
            label1.Visible = true;
            countdown.Visible = true;
            label2.Visible = false;
            label3.Visible = false;
            textBox1.Visible = false;
            textBox1.Enabled = false;
            radioButton1.Visible = false;
            radioButton1.Enabled = false;
            radioButton2.Visible = false;
            radioButton2.Enabled = false;
            button1.Enabled = false;
            button1.Visible = false;
            playing = true;
            timer1.Enabled = true;
            timer2.Enabled = true;
            timer3.Enabled = true;
        }

        // Hrwas pyrobola
        public void createBullet(int startX)
        {
            PictureBox p = new PictureBox();
            p.ImageLocation = "karoto.png";
            p.Location = new Point(startX + 5, pictureBox1.Location.Y - 50);
            p.Size = new Size(60, 50);
            p.SizeMode = PictureBoxSizeMode.StretchImage;
            Controls.Add(p);
            bullets.Add(p);
            s1.Play();
        }

        // Exthros pyrobola
        private void createBulletEnemy(int startX)
        {
            PictureBox p = new PictureBox();
            p.ImageLocation = "karoto1.png";
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
            lagos.kinhshExthroy();
        }
    }
}
