using System.Drawing;
using System.Windows.Forms;


namespace paixnidi2
{
    internal class Hrwas
    {
        public PictureBox swma = new PictureBox();
        public Hrwas(PictureBox p)
        {
            swma = p;
        }
        public void kinhshHrwa(KeyEventArgs e) {
            
                if (e.KeyCode.ToString().Equals("Left"))
                {
                    swma.Location = new Point(swma.Location.X - 15, swma.Location.Y);
                }
                else if (e.KeyCode.ToString().Equals("Right"))
                {
                    swma.Location = new Point(swma.Location.X + 15, swma.Location.Y);
                }
                else if (e.KeyCode.ToString().Equals("Up"))
                {
                    swma.Location = new Point(swma.Location.X, swma.Location.Y - 15);
                }
                else if (e.KeyCode.ToString().Equals("Down"))
                {
                    swma.Location = new Point(swma.Location.X, swma.Location.Y + 15);
                }
                
            
        }
    }
}
