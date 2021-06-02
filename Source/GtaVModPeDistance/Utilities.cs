using GTA;
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
            float range = (Settings.CameraFov / 100f) * y;
            return NextFloat(-range, range);
        }

        public static Vector2 World3DToScreen2d(Vector3 pos)
        {
            var x2dp = new OutputArgument();
            var y2dp = new OutputArgument();
            Function.Call<bool>(Hash.GET_SCREEN_COORD_FROM_WORLD_COORD, pos.X, pos.Y, pos.Z, x2dp, y2dp);

            return new Vector2(x2dp.GetResult<float>().ParseToScreenWidth(), y2dp.GetResult<float>().ParseToScreenHeight());
            
            //PointF screenCoords = GTA.UI.Screen.WorldToScreen(pos);
            //return new Vector2(screenCoords.X, screenCoords.Y);
        }

        public static Vector2 World3DToScreen2d(Vector3 pos, Vector3 support)
        {
            Vector2 pos2D = World3DToScreen2d(pos);
            float x = pos2D.X;
            float y = pos2D.Y;

            if (y < 0)
            {
                //GTA.UI.Notification.Show("Testone " + pos2D.ToString());
                x = World3DToScreen2d(support).X;
                y = -y;
                //GTA.UI.Notification.Show("Testone MOD" + x + " " + y);
            }

            return new Vector2(x, y);
        }

        #region ToCheck
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
        #endregion

        public static void Draw3DPedBoundingBoxUsingVertex(this GTA.Entity entity, Color color)
        {
            float Z = entity.Rotation.Z;
            entity.Rotation = Vector3.Zero;

            Vector3 DBL = entity.Model.Dimensions.rearBottomLeft;     // Down Behind left
            Vector3 TFR = entity.Model.Dimensions.frontTopRight;      // Top front right

            // aggiustamenti dei margini
            DBL.X += 0.25f;
            TFR.X -= 0.25f;
            //TFR.Z -= 0.12f;   

            Vector3 DBR = new Vector3(TFR.X, DBL.Y, DBL.Z);        // Down Behind right
            Vector3 DFR = new Vector3(TFR.X, TFR.Y, DBL.Z);        // Down front right
            Vector3 DFL = new Vector3(DBL.X, TFR.Y, DBL.Z);        // Down front left

            Vector3 TBL = new Vector3(DBL.X, DBL.Y, TFR.Z);        // Top Behind left
            Vector3 TBR = new Vector3(TFR.X, DBL.Y, TFR.Z);        // Top Behind right
            Vector3 TFL = new Vector3(DBL.X, TFR.Y, TFR.Z);        // Top front left

            entity.Rotation = new Vector3(0, 0, Z);

            Draw3DBoundingBox(
                entity.GetOffsetPosition(DBL),
                entity.GetOffsetPosition(DBR),
                entity.GetOffsetPosition(DFL),
                entity.GetOffsetPosition(DFR),
                entity.GetOffsetPosition(TBL),
                entity.GetOffsetPosition(TBR),
                entity.GetOffsetPosition(TFL),
                entity.GetOffsetPosition(TFR),
                color);

            //Draw3DBoundingBox(
            //  DBL,
            //  DBR,
            //  DFL,
            //  DFR,
            //  TBL,
            //  TBR,
            //  TFL,
            //  TFR,
            //  color);

        }

        public static void Draw3BBoundingBoxUsingEntityPosition(this GTA.Entity entity, Color color)
        {
            float Z = entity.Rotation.Z;
            entity.Rotation = Vector3.Zero;

            Vector3 LRA = new Vector3(entity.LeftPosition.X, entity.RearPosition.Y, entity.AbovePosition.Z);       // Left Rear Above
            Vector3 LFA = new Vector3(entity.LeftPosition.X, entity.FrontPosition.Y, entity.AbovePosition.Z);      // Left Front Above
            Vector3 RRA = new Vector3(entity.RightPosition.X, entity.RearPosition.Y, entity.AbovePosition.Z);      // Right Rear Above
            Vector3 RFA = new Vector3(entity.RightPosition.X, entity.FrontPosition.Y, entity.AbovePosition.Z);     // Right Front Above

            Vector3 LRB = new Vector3(entity.LeftPosition.X, entity.RearPosition.Y, entity.BelowPosition.Z);       // Left Rear Below
            Vector3 LFB = new Vector3(entity.LeftPosition.X, entity.FrontPosition.Y, entity.BelowPosition.Z);      // Left Front Below
            Vector3 RRB = new Vector3(entity.RightPosition.X, entity.RearPosition.Y, entity.BelowPosition.Z);      // Right Rear Below
            Vector3 RFB = new Vector3(entity.RightPosition.X, entity.FrontPosition.Y, entity.BelowPosition.Z);     // Right Front Below

            //DBL.X += 0.2f;
            //TFR.X -= 0.2f;

            entity.Rotation = new Vector3(0, 0, Z);

            //Draw3DBoundingBox(LRB, RRB, LFB, RFB, LRA, RRA, LFA, RFA, color);

            Draw3DBoundingBox(
                entity.GetOffsetPosition(LRB),
                entity.GetOffsetPosition(RRB),
                entity.GetOffsetPosition(LFB),
                entity.GetOffsetPosition(RFB),
                entity.GetOffsetPosition(LRA),
                entity.GetOffsetPosition(RRA),
                entity.GetOffsetPosition(LFA),
                entity.GetOffsetPosition(RFA),
                color);
        }

        public static void DrawBoundingBoxNearbyEntities(float radius)
        {
            Entity[] entity = World.GetNearbyEntities(Game.Player.Character.Position, radius);
            foreach (Entity ent in entity)
            {
                if(ent.IsOnScreen)
                {
                    if (ent is Vehicle)
                        ent.Draw3DPedBoundingBoxUsingVertex(Color.Magenta);
                    else if (ent is Ped)
                        ent.Draw3DPedBoundingBoxUsingVertex(Color.Red);
                    else if(ent is Weapon)
                        ent.Draw3DPedBoundingBoxUsingVertex(Color.Blue);
                    else
                        ent.Draw3DPedBoundingBoxUsingVertex(Color.Aqua);
                }
              
            }
        }

        public static void Draw3DBoundingBox(Vector3 LeftRearBelow, Vector3 RightRearBelow, Vector3 LeftFrontBelow, Vector3 RightFrontBelow,
                Vector3 LeftRearAbove, Vector3 RightRearAbove, Vector3 LeftFrontAbove, Vector3 RightFrontAbove, Color color)
        {
            DrawLine(LeftRearBelow, RightRearBelow, color);
            DrawLine(LeftRearBelow, LeftFrontBelow, color);
            DrawLine(LeftRearBelow, LeftRearAbove, color);

            DrawLine(RightFrontBelow, RightRearBelow, color);
            DrawLine(RightFrontBelow, LeftFrontBelow, color);
            DrawLine(RightFrontBelow, RightFrontAbove, color);

            DrawLine(LeftFrontAbove, RightFrontAbove, color);
            DrawLine(LeftFrontAbove, LeftRearAbove, color);
            DrawLine(LeftFrontAbove, LeftFrontBelow, color);

            DrawLine(RightRearAbove, RightFrontAbove, color);
            DrawLine(RightRearAbove, LeftRearAbove, color);
            DrawLine(RightRearAbove, RightRearBelow, color);

        }

        public static void PrintPedAxis(this GTA.Ped ped)
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

        public static void FacePosition(this Ped ped, Vector3 pos) {
            ped.Heading = Function.Call<float>(Hash.GET_HEADING_FROM_VECTOR_2D, pos.X - ped.Position.X, pos.Y - ped.Position.Y); 
        }

    }
}
