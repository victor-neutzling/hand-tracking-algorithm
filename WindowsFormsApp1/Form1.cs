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
        Silhouette silhouette = new Silhouette();
        Bitmap bmp;
        Bitmap back;
        public Form1()
        {
            InitializeComponent();
            back = Properties.Resources.fundo2;
            bmp = Properties.Resources.bruno2;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Blur blur = new Blur();
            int K = 25;
            bmp = silhouette.applySilhouette(blur.applyBlur(bmp, K), blur.applyBlur(back, K));
            this.BackgroundImage = bmp; 
        }

        private void button2_Click(object sender, EventArgs e)
        {
           
        }
    }
}
