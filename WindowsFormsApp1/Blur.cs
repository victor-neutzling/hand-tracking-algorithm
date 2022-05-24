using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public class Blur
    {
        
        
        public Bitmap applyBlur(Bitmap bmp, int BlurIndex)
        {
            int K = BlurIndex;
            
            var rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            var data = bmp.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            var A = new byte[data.Height * data.Stride];
            Marshal.Copy(data.Scan0, A, 0, A.Length);
            int index = -1;

            var B = new uint[A.Length];

            // Passo 1
            B[0] = A[0];
            B[1] = A[1];
            B[2] = A[2];

            for (int i = 1; i < bmp.Width; i++)
            {
                index = 3 * i;

                B[index] = A[index] + B[index - 3];
                B[index + 1] = A[index + 1] + B[(index + 1) - 3];
                B[index + 2] = A[index + 2] + B[(index + 2) - 3];
            }
            for (int j = 1; j < bmp.Height; j++)
            {
                index = j * data.Stride;
                B[index] = A[index] + B[index - data.Stride];
                B[index + 1] = A[index + 1] + B[index - data.Stride + 1];
                B[index + 2] = A[index + 2] + B[index - data.Stride + 2];
            }
            for (int j = 1; j < bmp.Height; j++)
            {
                for (int i = 1; i < bmp.Width; i++)
                {
                    index = 3 * i + j * data.Stride;
                    B[index] = A[index] + B[index - 3] + B[index - data.Stride] - B[(index - 3) - data.Stride];
                    B[index + 1] = A[index + 1] + B[(index + 1) - 3] + B[(index + 1) - data.Stride] - B[((index + 1) - 3) - data.Stride];
                    B[index + 2] = A[index + 2] + B[(index + 2) - 3] + B[(index + 2) - data.Stride] - B[((index + 2) - 3) - data.Stride];
                }
            }
            // Passo 2
            for (int i = K * (3 + data.Stride); i < A.Length - K * (3 + data.Stride); i++)
            {
                A[i] = (byte)((B[i] - B[i - data.Stride * K] - B[i - 3 * K] + B[i - (data.Stride + 3) * K]) / (double)(K * K));
            }
            Marshal.Copy(A, 0, data.Scan0, A.Length);
            bmp.UnlockBits(data);
            return bmp;
        }
    }
}
