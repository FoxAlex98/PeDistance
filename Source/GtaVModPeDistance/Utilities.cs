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

        //public static void getMaxBotLeft(Vector3 dbl)
        //{
        //    Vector3 dbr = new Vector3(TFR.X, DBL.Y, DBL.Z);
        //    Vector3 dfr = new Vector3(TFR.X, TFR.Y, DBL.Z);
        //    Vector3 dfl = new Vector3(DBL.X, TFR.Y, DBL.Z);

        //}

        //public static void getMaxTopRight()
        //{

        //}

        //private void DrawBoundingBox(Entity e, Vector3 dim, Vector3 FUL, Vector3 BLR)
        //{
        //    Vector3[] vertices = new Vector3[8];

        //    vertices[0] = FUL;
        //    vertices[1] = FUL - dim.X * e.RightVector;
        //    vertices[2] = FUL - dim.Z * e.UpVector;
        //    vertices[3] = FUL - dim.Y * Vector3.Cross(e.UpVector, e.RightVector);

        //    vertices[4] = BLR;
        //    vertices[5] = BLR - dim.X * e.RightVector;
        //    vertices[6] = BLR - dim.Z * e.UpVector;
        //    vertices[7] = BLR - dim.Y * Vector3.Cross(e.UpVector, e.RightVector);

        //    int xMin = int.MaxValue;
        //    int yMin = int.MaxValue;
        //    int xMax = 0;
        //    int yMax = 0;

        //    foreach (Vector3 v in vertices)
        //    {
        //        int x = (int)get2Dfrom3D(v).X;
        //        int y = (int)get2Dfrom3D(v).Y;

        //        if (x < xMin)
        //            xMin = x;
        //        if (x > xMax)
        //            xMin = x;
        //        if (y < yMin)
        //            yMin = y;
        //        if (y > yMax)
        //            yMax = y;
        //    }

        //    if (xMin < 0)
        //        xMin = 0;
        //    if (yMin < 0)
        //        yMin = 0;

        //    if (xMax > UI.WIDTH)
        //        xMax = UI.WIDTH;
        //    if (yMax > UI.HEIGHT)
        //        yMax = UI.HEIGHT;

        //    int width = xMax - xMin;
        //    int height = yMax - yMin;

        //    xMin = (int)(IMAGE_WIDTH / (1.0f * UI.WIDTH) * xMin);
        //    xMax = (int)(IMAGE_WIDTH / (1.0f * UI.WIDTH) * xMax);
        //    yMin = (int)(IMAGE_HEIGHT / (1.0f * UI.HEIGHT) * yMin);
        //    yMax = (int)(IMAGE_HEIGHT / (1.0f * UI.HEIGHT) * yMax);

        //    width = xMax - xMin;
        //    height = yMax - yMin;

        //}

        public static void Draw3DPedBoundingBox(this GTA.Ped ped, Color color)
        {
            float Z = ped.Rotation.Z;
            ped.Rotation = Vector3.Zero;

            Vector3 DBL = ped.Model.Dimensions.rearBottomLeft;     // Down Behind left
            Vector3 TFR = ped.Model.Dimensions.frontTopRight;      // Top front right

            // aggiustamenti dei margini
            DBL.X += 0.2f;
            TFR.X -= 0.2f;
            //TFR.Z -= 0.12f;   

            Vector3 DBR = new Vector3(TFR.X, DBL.Y, DBL.Z);        // Down Behind right
            Vector3 DFR = new Vector3(TFR.X, TFR.Y, DBL.Z);        // Down front right
            Vector3 DFL = new Vector3(DBL.X, TFR.Y, DBL.Z);        // Down front left

            Vector3 TBL = new Vector3(DBL.X, DBL.Y, TFR.Z);        // Top Behind left
            Vector3 TBR = new Vector3(TFR.X, DBL.Y, TFR.Z);        // Top Behind right
            Vector3 TFL = new Vector3(DBL.X, TFR.Y, TFR.Z);        // Top front left

            ped.Rotation = new Vector3(0, 0, Z);

            Draw3DBoundingBox(
                ped.GetOffsetPosition(DBL),
                ped.GetOffsetPosition(DBR),
                ped.GetOffsetPosition(DFL),
                ped.GetOffsetPosition(DFR),
                ped.GetOffsetPosition(TBL),
                ped.GetOffsetPosition(TBR),
                ped.GetOffsetPosition(TFL),
                ped.GetOffsetPosition(TFR), 
                color);
        }

        //public static Vector3 RotateOnZ(Vector3 point, Matrix matrix)
        //{
        //    return matrix.TransformPoint(point);
        //}

        //public static Matrix GetRotationMatrix(float angle)
        //{
        //    double rad = Deg2rad(angle);
        //    return Matrix.RotationZ((float) rad);
        //}

        //public static double Deg2rad(double angle)
        //{
        //    return (Math.PI / 180) * angle;
        //}

        public static void Draw3DBoundingBox(Vector3 DownBehindLeft, Vector3 DownBehindRight, Vector3 DownForwardLeft, Vector3 DownForwardRight,
                Vector3 TopBehindLeft, Vector3 TopBehindRight, Vector3 TopForwardLeft, Vector3 TopForwardRight, Color color)
        {
            DrawLine(DownBehindLeft, DownBehindRight, color);
            DrawLine(DownBehindLeft, DownForwardLeft, color);
            DrawLine(DownBehindLeft, TopBehindLeft, color);

            DrawLine(DownForwardRight, DownBehindRight, color);
            DrawLine(DownForwardRight, DownForwardLeft, color);
            DrawLine(DownForwardRight, TopForwardRight, color);

            DrawLine(TopForwardLeft, TopForwardRight, color);
            DrawLine(TopForwardLeft, TopBehindLeft, color);
            DrawLine(TopForwardLeft, DownForwardLeft, color);

            DrawLine(TopBehindRight, TopForwardRight, color);
            DrawLine(TopBehindRight, TopBehindLeft, color);
            DrawLine(TopBehindRight, DownBehindRight, color);

        }

        public static void printPedAxis(this GTA.Ped ped)
        {
            Vector3 origin = Vector3.Zero;
            Vector3 pedOrigin = ped.GetOffsetPosition(origin);
            // X
            DrawLine(pedOrigin, ped.GetOffsetPosition(new Vector3(50, 0 , 0)), Color.Red);    
            DrawLine(pedOrigin, ped.GetOffsetPosition(new Vector3(-50, 0, 0)), Color.Red);

            // Y
            DrawLine(pedOrigin, ped.GetOffsetPosition(new Vector3(0, 50, 0)), Color.Blue);
            DrawLine(pedOrigin, ped.GetOffsetPosition(new Vector3(0, -50, 0)), Color.Blue);

            // Z
            DrawLine(pedOrigin, ped.GetOffsetPosition(new Vector3(0, 0, 50)), Color.Yellow);       
        }

        public static float ParseToScreenWidth(this float value)
        {
            return value * Screen.PrimaryScreen.Bounds.Width;
        }

        public static float ParseToScreenHeight(this float value)
        {
            return value * Screen.PrimaryScreen.Bounds.Height;
        }

        public static void DrawBox(Vector3 a, Vector3 b, Color col)
        {         
            Function.Call(Hash.DRAW_BOX, a.X, a.Y, a.Z, b.X, b.Y, b.Z, col.R, col.G, col.B, col.A);
        }

        public static void DrawLine(Vector3 a, Vector3 b, Color col)
        {
            Function.Call(Hash.DRAW_LINE, a.X, a.Y, a.Z, b.X, b.Y, b.Z, col.R, col.G, col.B, col.A);
        }
    }
}
