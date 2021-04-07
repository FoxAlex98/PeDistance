using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtaVModPeDistance
{
    static class Utilities
    {

        public static string ToBase64String(Image image)
        {
            return Convert.ToBase64String(ToByteArray(image, ImageFormat.Jpeg));           
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
