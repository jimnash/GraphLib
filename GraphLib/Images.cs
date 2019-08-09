using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
// ReSharper disable UnusedMember.Global

namespace GraphLib
{
    /// <summary>
    /// 256 Grey scale image 
    /// </summary>
    public class Images
    {
        private readonly ColorPalette _monoPalette;
        public byte[] LastData;

        public Images()
        {
            var bmp = new Bitmap(1024, 1024, PixelFormat.Format8bppIndexed);
            _monoPalette = bmp.Palette;
            var entries = _monoPalette.Entries;
            for (var i = 0; i < 256; i++)
                entries[i] = Color.FromArgb(i, i, i);
        }

        /// <summary>
        /// If passed null just  use the first set of data it had received
        /// Assumes square src arbitrary dest
        /// </summary>
        /// <param name="data"></param>
        /// <param name="destW"></param>
        /// <param name="destH"></param>
        /// <returns></returns>
        public Bitmap CreateBitmapImage(byte[] data, int destW, int destH)
        {
            if (data == null)
            {
                if (LastData == null)
                    return null;
                data = LastData;
            }
            else if (LastData == null)
               LastData = (byte[])data.Clone();

            var srcSize = (int) Math.Sqrt(data.Length);            
            return CreateBitmapImage(data, srcSize, srcSize, destW, destH);
          
        }

        /// <summary>
        /// Arbitrary src width & height and dest width & height
        /// </summary>
        /// <param name="data"></param>
        /// <param name="srcW"></param>
        /// <param name="srcH"></param>
        /// <param name="destW"></param>
        /// <param name="destH"></param>
        /// <returns></returns>
        public Bitmap CreateBitmapImage(byte[] data,int srcW,int srcH, int destW, int destH)
        {
            if (data == null)
                return null;
            var bitmap = new Bitmap(srcW, srcH, PixelFormat.Format8bppIndexed);
            var bmData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadWrite, bitmap.PixelFormat);
            var pNative = bmData.Scan0;
            Marshal.Copy(data, 0, pNative, srcW * srcH);
            bitmap.UnlockBits(bmData);

            bitmap.Palette = _monoPalette;

            var newImage = new Bitmap(bitmap, destW, destH);
            return newImage;
        }

        public void ResetLastData()
        {
            LastData = null;
        }

        // Normalised Gaussian Distribution Set 
        // Just here to have it somewhere to remember it.................
        public double[] GetGaussDistribution(int count)
        {
            var data = new double[count];
            var max = 0.0;
            var step = 6.0 / count;
            var pi = 1.0 / Math.Sqrt(2 * Math.PI);
            for (var i = 0; i < count; ++i)
            {
                var x = -3.0 + (i * step);
                data[i] = pi * Math.Exp(-0.5 * Math.Pow(x, 2));
                if (data[i] > max)
                    max = data[i];
            }
            for (var i = 0; i < count; ++i)
                data[i] /= max;

            return data;
        }
    }
}