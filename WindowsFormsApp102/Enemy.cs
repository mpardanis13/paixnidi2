using System;
using System.Drawing;
using System.Windows.Forms;

namespace paixnidi2
{
    internal class Enemy
    {
        public PictureBox swma = new PictureBox();
        Random r = new Random();
        bool dyskolia;

        public Enemy(PictureBox p, bool d)
        {
            swma = p;
            dyskolia= d;
        }
        public void kinhshExthroy()
        {
            Form f1 = Application.OpenForms[0];
            if(dyskolia)
            swma.Location = new Point(r.Next(f1.Width - swma.Width), swma.Height);
            else swma.Location = new Point(r.Next(f1.Width - swma.Width), r.Next(f1.Height - swma.Height));

        }
    }
}
