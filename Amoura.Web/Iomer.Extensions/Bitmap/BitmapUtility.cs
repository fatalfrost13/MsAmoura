using System;
using System.Drawing;

namespace Iomer.Extensions.GFX
{
    public static class BitmapUtility
    {
        public static Bitmap ScaledResize(this Bitmap target, int maxWidth, int maxHeight)
        {
            var width = target.Width;
            var height = target.Height;

            maxWidth = Math.Min(maxWidth, width);
            maxHeight = Math.Min(maxHeight, height);

            if (width > maxWidth)
            {
                height = height * maxWidth / width;
                width = maxWidth;
            }

            if (height > maxHeight)
            {
                width = width * maxHeight / height;
                height = maxHeight;
            }

            var scaled = new Bitmap(width, height);
            using (var g = Graphics.FromImage(scaled))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                g.DrawImage(target, new Rectangle(0, 0, width, height));
            }

            return scaled;
        }
    }
}
