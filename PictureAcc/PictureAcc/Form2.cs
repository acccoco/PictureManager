using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PictureAcc
{
    public partial class Form2 : Form
    {
        private string path;
        private Bitmap bt;
        public Form2(string path)
        {
            InitializeComponent();
            this.path = path;
        }



        private void Form2_Load(object sender, EventArgs e)
        {
            bt = ShowPicture(path, pictureBox1.Size);
            pictureBox1.Image = bt;
        }
        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            bt.Dispose();
        }

        #region helper method
        private Bitmap ShowPicture(string sourcePath, Size size)
        {
            Image image = Image.FromFile(sourcePath);
            Bitmap bm = new Bitmap(size.Width, size.Height);

            Graphics gr = Graphics.FromImage(bm);
            if (image.Width * bm.Height >= image.Height * bm.Width)
            {
                int relHei = image.Height * bm.Width / image.Width;
                gr.DrawImage(image, new Rectangle(0, (bm.Height - relHei) / 2, bm.Width, relHei));
            }
            else
            {
                int relWi = image.Width * bm.Height / image.Height;
                gr.DrawImage(image, new Rectangle((bm.Width - relWi) / 2, 0, relWi, bm.Height));
            }

            image.Dispose();
            gr.Dispose();

            return bm;
        }

        #endregion
    }
}
