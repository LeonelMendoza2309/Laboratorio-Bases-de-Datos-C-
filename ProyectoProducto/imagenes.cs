using System;
using System.Drawing;
using System.IO;

namespace ProyectoProducto
{
    public static class Imagenes
    {
        public static byte[] ImageToByteArray(Image image)
        {
            if (image == null)
                return null;

            using (var mMemoryStream = new MemoryStream())
            {
                image.Save(mMemoryStream, System.Drawing.Imaging.ImageFormat.Png);
                return mMemoryStream.ToArray();
            }
        }

        public static Image LoadImage(string path)
        {
            if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
                return null;
            return Image.FromFile(path);
        }

        public static byte[] ImageFileToBytes(string path)
        {
            if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
                return null;
            return File.ReadAllBytes(path);
        }
        public static Image AdjustImageForCell(Image image, int maxWidth, int maxHeight)
        {
            if (image == null) return null;

            try
            {
                int w = image.Width;
                int h = image.Height;
                double scale = 1.0;
                if (w > maxWidth || h > maxHeight)
                {
                    double sx = (double)maxWidth / w;
                    double sy = (double)maxHeight / h;
                    scale = Math.Min(sx, sy);
                }

                int newW = (int)(w * scale);
                int newH = (int)(h * scale);

                return new Bitmap(image, new Size(Math.Max(1, newW), Math.Max(1, newH)));
            }
            catch
            {
                return null;
            }
        }
    }
}
