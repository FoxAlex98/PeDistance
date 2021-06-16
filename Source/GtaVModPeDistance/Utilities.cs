using GTA;
using GTA.Math;
using GTA.Native;
using GtaVModPeDistance.Models;
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

        public static float GetYByFov()
        {
            float yMin = 10f - Settings.CameraFov / 10f;
            float yMax = 20f;
            return NextFloat(yMin, yMax);
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
        }

        public static Vector2 World3DToScreen2d(Vector3 pos, Vector3 support)
        {
            Vector2 pos2D = World3DToScreen2d(pos);
            float x = pos2D.X;
            float y = pos2D.Y;

            if (y < 0)
            {
                x = World3DToScreen2d(support).X;
                y = -y;
            }

            return new Vector2(x, y);
        }

        public static void Draw3DPedBoundingBoxUsingVertex(this Entity entity, Color color)
        {
            Entity3DBoundingBox box3D = new Entity3DBoundingBox(entity);
            Draw3DBoundingBox(
                box3D.DBL,
                box3D.DBR,
                box3D.DFL,
                box3D.DFR,
                box3D.TBL,
                box3D.TBR,
                box3D.TFL,
                box3D.TFR,
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

        public static void DrawLine(Vector3 a, Vector3 b, Color col)
        {
            Function.Call(Hash.DRAW_LINE, a.X, a.Y, a.Z, b.X, b.Y, b.Z, col.R, col.G, col.B, col.A);
        }

        public static void FacePosition(this Ped ped, Vector3 pos) {
            ped.Heading = Function.Call<float>(Hash.GET_HEADING_FROM_VECTOR_2D, pos.X - ped.Position.X, pos.Y - ped.Position.Y); 
        }

        public static bool IsVehicleType(this VehicleHash vehicleHash, Globals.VehicleType vehicleType)
        {
            Model model = new Model(vehicleHash);
            switch (vehicleType)
            {
                case Globals.VehicleType.CARS: return model.IsCar ? true : false;
                case Globals.VehicleType.BOATS: return model.IsBoat ? true : false;
                case Globals.VehicleType.MOTORBIKES: return model.IsBike ? true : false;
                case Globals.VehicleType.HELICOPTERS: return model.IsHelicopter ? true : false;
                case Globals.VehicleType.PLANES: return model.IsPlane ? true : false;
            }
            return false;
        }

    }
}
