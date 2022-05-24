using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1.Properties
{
    public class Silhouette
    {
        public Bitmap applySilhouette(Bitmap bmp, Bitmap fundo)
        {

            var rectB = new Rectangle(0, 0, bmp.Width, bmp.Height);
            var dataB = fundo.LockBits(rectB, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            var B = new byte[dataB.Height * dataB.Stride];
            Marshal.Copy(dataB.Scan0, B, 0, B.Length);

            var rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            var data = bmp.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            var A = new byte[data.Height * data.Stride];
            Marshal.Copy(data.Scan0, A, 0, A.Length);
            // Passo 2
            for (int i = 0; i < A.Length; i = i + 3)
            {
                int rFinal = B[i + 2] - A[i + 2];
                int gFinal = B[i + 1] - A[i + 1];
                int bFinal = B[i] - A[i];
                double erro = Math.Sqrt(rFinal * rFinal + gFinal * gFinal + bFinal * bFinal) / 3;

                if (erro < 20)
                {
                    A[i] = 255;
                    A[i + 1] = 255;
                    A[i + 2] = 255;
                }
                else
                {
                    A[i] = 0;
                    A[i + 1] = 0;
                    A[i + 2] = 0;
                }

            }
            Marshal.Copy(A, 0, data.Scan0, A.Length);
            bmp.UnlockBits(data);
            return bmp;
        }
    }
}
