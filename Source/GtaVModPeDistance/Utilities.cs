using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace GtaVModPeDistance
{
    static class Utilities
    {
        static Random random = new Random();

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

        public static float NextFloat(float min, float max)
        {
            return (float)(random.NextDouble() * (max - min) + min);
        }

        public static float GetPosXByPosY(float y)
        {
            float range = Settings.PedSpawningDistanceRatio * y;
            return NextFloat(-range, range);
        }
    }
}
