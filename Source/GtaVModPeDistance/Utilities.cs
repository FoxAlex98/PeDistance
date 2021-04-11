using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace GtaVModPeDistance
{
    static class Utilities
    {

        public static string ToBase64String(Image image, ImageFormat imageFormat)
        {
            return Convert.ToBase64String(ToByteArray(image, imageFormat));           
        }

        private static byte[] ToByteArray(this Image image, ImageFormat format)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, format);
                return ms.ToArray();
            }
        }
    }
}
