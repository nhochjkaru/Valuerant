using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Valuerant
{
    public partial class Recorder : Form
    {
        public int w, h;
        public Recorder(int width, int height)
        {
            w = width;
            h = height;

            Console.WriteLine($"{w}:{h}");
            InitializeComponent();
        }

        public Image ScaleImage(Image image, int maxWidth, int maxHeight)
        {
            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            var newImage = new Bitmap(maxWidth, maxHeight);
            using (var graphics = Graphics.FromImage(newImage))
            {
                // Calculate x and y which center the image
                int y = (maxHeight / 2) - newHeight / 2;
                int x = (maxWidth / 2) - newWidth / 2;

                // Draw image on x and y with newWidth and newHeight
                graphics.DrawImage(image, x, y, newWidth, newHeight);
            }

            return newImage;
        }

        private void CaptureScreen()
        {
            Rectangle screenSize = Screen.PrimaryScreen.Bounds;
            Size Size = new Size(screenSize.Width, screenSize.Height);



            while (recEnabled)
            {
                Bitmap target = new Bitmap(screenSize.Width, screenSize.Height);

                //target = ReSizeImage(target, Size);

                using (Graphics g = Graphics.FromImage(target))
                {
                    //g.CopyFromScreen((Size.Width / 2) - w, (Size.Height / 2) - h, 0, 0, Size);
                    //(xSize - fovX) / 2, (ySize - fovY) / 2, fovX, fovY
                    g.CopyFromScreen((Size.Width - w) / 2, (Size.Height - h) / 2, 0, 0, Size);
                    using (Image prev_bitmap = pictureBox1.Image)
                    {
                        pictureBox1.Image = target;
                    }
                }


                Thread.Sleep(33);
            }
            x2.Abort();
            // return target;
        }

        public bool recEnabled = true;
        Thread x2;
        private void Recorder_Load(object sender, EventArgs e)
        {
            pictureBox1.Size = new Size(w, h);
            pictureBox1.Location = new Point(0, 0);
            Size = new Size(w, h);

        }

        public void startThread()
        {
            pictureBox1.Size = new Size(w, h);
            pictureBox1.Location = new Point(0, 0);
            Width = w;
            Height = h;
            x2 = new Thread(CaptureScreen);
            x2.Start();
        }
    }
}
