using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using WindowsFormsApp1.Properties;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        Blur blur = new Blur();
        int size = 100;
        Silhouette silhouette = new Silhouette();
        Bitmap bmp;
        Bitmap back;
        public Form1()
        {
            InitializeComponent();

            
            back = Properties.Resources.fundo2;
            bmp = Properties.Resources.bruno2;

            Blur blur = new Blur();
            int K = 25;
            bmp = silhouette.applySilhouette(blur.applyBlur(bmp, K), blur.applyBlur(back, K));


            this.WindowState = FormWindowState.Maximized;
            this.KeyPreview = true;
            this.FormBorderStyle = FormBorderStyle.None;
            this.KeyDown += (o, e) =>
            {
                if (e.KeyCode == Keys.Escape)
                    Application.Exit();
            };

            var rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            var data = bmp.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            var array = new byte[(data.Height / 2) * data.Stride];
            Marshal.Copy(data.Scan0, array, 0, array.Length);

            int max = 0;
            int maxindex = 0;


            for (int i = 0; i < array.Length; i += 3)
            {
                byte b = array[i + 0],
                     g = array[i + 1],
                     r = array[i + 2];

                if (r < 10)
                {
                    int _x = i % data.Stride;
                    if (_x > max)
                    {
                        max = _x;
                        maxindex = i;
                    }
                }

                array[i + 0] = b;
                array[i + 1] = g;
                array[i + 2] = r;
            }

            int x = (maxindex % data.Stride) / 3;
            int y = maxindex / data.Stride;
            (x, y) = centralize(array, data.Stride, x, y, size);

            bool inhand = false;

            while (!inhand)
            {
                inhand = true;
                int xl = x - 25,
                    yl = y - 25;
                for (int i = 3 * xl + data.Stride * yl; i < (3 * xl + data.Stride * yl) + 150; i += 3)
                {
                    byte b = array[i + 0],
                         g = array[i + 1],
                         r = array[i + 2];
                    if (r < 10)
                    {
                        inhand = false;
                        y -= 10;
                        break;
                    }
                }
            }
            (x, y) = centralize(array, data.Stride, x, y, size);


            Marshal.Copy(array, 0, data.Scan0, array.Length);
            bmp.UnlockBits(data);
            Graphics.FromImage(bmp).FillEllipse(Brushes.Red, x - 5, y - 5, 10, 10);
            Graphics.FromImage(bmp).DrawRectangle(Pens.Red, x - size, y - size, 2 * size, 2 * size);

            this.BackgroundImage = bmp;
        }

        (int, int) centralize(byte[] array, int stride, int x, int y, int size = 100)
        {
            int sx = 0;
            int sy = 0;
            int count = 0;

            for (int j = y - size; j < y + size; j++)
            {
                for (int i = x - size; i < x + size; i++)
                {
                    int index = j * stride + 3 * i; //linha
                    if (index >= array.Length)
                        continue;
                    byte b = array[index + 0],
                         g = array[index + 1],
                         r = array[index + 2];

                    if (r < 10 && g < 10 && b < 10)
                    {
                        sx += i;
                        sy += j;
                        count++;
                    }
                }
            }
            x = sx / count;
            y = sy / count;
            return (x, y);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Blur blur = new Blur();
            //int K = 25;
            //bmp = silhouette.applySilhouette(blur.applyBlur(bmp, K), blur.applyBlur(back, K));
            //this.BackgroundImage = bmp; 
        }

        private void button2_Click(object sender, EventArgs e)
        {
           
        }
    }
}
