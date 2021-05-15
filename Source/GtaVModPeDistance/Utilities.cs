using GTA.Math;
using GTA.Native;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

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

        public static Vector2 World3DToScreen2d(Vector3 pos)
        {
            var x2dp = new OutputArgument();
            var y2dp = new OutputArgument();

            Function.Call<bool>(Hash.GET_SCREEN_COORD_FROM_WORLD_COORD, pos.X, pos.Y, pos.Z, x2dp, y2dp);
            return new Vector2(x2dp.GetResult<float>(), y2dp.GetResult<float>());
        }

        public static float ParseToScreenWidth(this float value)
        {
            return value * Screen.PrimaryScreen.WorkingArea.Width;
        }

        public static float ParseToScreenHeight(this float value)
        {
            return value * Screen.PrimaryScreen.WorkingArea.Height;
        }
    }
}
