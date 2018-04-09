using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace PictureAcc
{
    public partial class Form1
    {
        /// <summary>
        /// bitmap caching
        /// </summary>
        Bitmap[] bms = new Bitmap[Utils.bitmaps_count];
        /// <summary>
        /// release sources of bitmaps
        /// </summary>
        private void DisposeBitmaps()
        {
            for(int i = 0; i < bms.Length; i++)
            {
                if (bms[i] != null)
                {
                    bms[i].Dispose();
                    bms[i] = null;
                }
            }
        }




        /// <summary>
        /// pictureboxes attached to flowview.
        /// </summary>
        PictureBox[] boxes = new PictureBox[Utils.pictureboxes_count];
        /// <summary>
        /// init pictureBox with some size and attach them to some control
        /// </summary>
        /// <param name="control"></param>
        /// <param name="size"></param>
        private void InitBoxes(Control control,int size)
        {
            for (int i = 0; i < boxes.Length; i++)
            {
                boxes[i] = new PictureBox
                {
                    Name = "pictureBoxAcc" + i.ToString(),
                    Size = new Size(size, size),
                };
                control.Controls.Add(boxes[i]);
            }
        }
        /// <summary>
        /// clear all picturebox
        /// </summary>
        private void ClearBoxes()
        {
            foreach(var box in boxes)
            {
                box.Image = null;
            }
        }
    }
}
