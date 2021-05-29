using GTA;
using GTA.Math;
using GtaVModPeDistance.Models;
using System.Windows.Forms;

namespace GtaVModPeDistance
{
    class CoordinatesUtils
    {
        private static float paddingTop = 0.12f;
        private static float paddingLeft = 0.1f;
        private static float paddingRight = 0.1f;
        private static float paddingBottom = 0.05f;

        /*
        public static Ped2DBoundingBox GetPedBoundingBoxByBones(Ped ped)
        {
            float pedHeighestBodyPart = GetPedHighestBodyPart(ped);
            float pedLowestBodyPart = GetPedLowestBodyPart(ped);
            float pedFurthestRightBodyPart = GetPedFurthestRightBodyPart(ped);
            float pedFurthestLeftBodyPart = GetPedFurthestLeftBodyPart(ped);

            Vector2 BoundingBoxTopLeft = new Vector2(pedFurthestLeftBodyPart, pedHeighestBodyPart);
            Vector2 BoundingBoxTopRight = new Vector2(pedFurthestRightBodyPart, pedHeighestBodyPart);
            Vector2 BoundingBoxBottomLeft = new Vector2(pedFurthestLeftBodyPart, pedLowestBodyPart);
            Vector2 BoundingBoxBottomRight = new Vector2(pedFurthestRightBodyPart, pedLowestBodyPart);

            Game.RadioStation = RadioStation.RadioOff;
            GTA.UI.Notification.Show(BoundingBoxTopLeft.ToString());
            GTA.UI.Notification.Show(BoundingBoxTopRight.ToString());
            GTA.UI.Notification.Show(BoundingBoxBottomLeft.ToString());
            GTA.UI.Notification.Show(BoundingBoxBottomRight.ToString());
            

            return new Ped2DBoundingBox(BoundingBoxTopLeft, BoundingBoxTopRight, BoundingBoxBottomLeft, BoundingBoxBottomRight);
        }
        */

        public static Ped2DBoundingBox GetPedBoundingBoxInScreen(Ped ped)
        {
            float Z = ped.Rotation.Z;
            ped.Rotation = Vector3.Zero;

            Vector3 DBL = ped.Model.Dimensions.rearBottomLeft;     // Down Behind left
            Vector3 TFR = ped.Model.Dimensions.frontTopRight;      // Top front right

            // aggiustamenti dei margini
            DBL.X += 0.25f;
            DBL.Z += 0.4f;
            TFR.X -= 0.25f;
            //TFR.Z -= 0.12f;   

            ped.Rotation = new Vector3(0, 0, Z);

            Vector3 DBR = new Vector3(TFR.X, DBL.Y, DBL.Z);        // Down Behind right
            Vector3 DFR = new Vector3(TFR.X, TFR.Y, DBL.Z);        // Down front right
            Vector3 DFL = new Vector3(DBL.X, TFR.Y, DBL.Z);        // Down front left

            Vector3 TBL = new Vector3(DBL.X, DBL.Y, TFR.Z);        // Top Behind left
            Vector3 TBR = new Vector3(TFR.X, DBL.Y, TFR.Z);        // Top Behind right
            Vector3 TFL = new Vector3(DBL.X, TFR.Y, TFR.Z);        // Top front left



            DBL = ped.GetOffsetPosition(DBL);
            DBR = ped.GetOffsetPosition(DBR);        
            DFR = ped.GetOffsetPosition(DFR);        
            DFL = ped.GetOffsetPosition(DFL);
            
            TFR = ped.GetOffsetPosition(TFR);
            TBL = ped.GetOffsetPosition(TBL);        
            TBR = ped.GetOffsetPosition(TBR);        
            TFL = ped.GetOffsetPosition(TFL);

            Vector2[] vertices = new Vector2[8];

            vertices[0] = Utilities.World3DToScreen2d(TFR);
            vertices[1] = Utilities.World3DToScreen2d(TBL);
            vertices[2] = Utilities.World3DToScreen2d(TBR);
            vertices[3] = Utilities.World3DToScreen2d(TFL);
            vertices[4] = Utilities.World3DToScreen2d(DBL, TBL);
            vertices[5] = Utilities.World3DToScreen2d(DBR, TBR);
            vertices[6] = Utilities.World3DToScreen2d(DFR, TFR);
            vertices[7] = Utilities.World3DToScreen2d(DFL, TFL);
            /*
            GTA.UI.Notification.Show("TFR " + vertices[0]);
            GTA.UI.Notification.Show("TBL " + vertices[1]);
            GTA.UI.Notification.Show("TBR " + vertices[2]);
            GTA.UI.Notification.Show("TFL " + vertices[3]);
            GTA.UI.Notification.Show("DBL " + vertices[4]);
            GTA.UI.Notification.Show("DBR " + vertices[5]);
            GTA.UI.Notification.Show("DFR " + vertices[6]);
            GTA.UI.Notification.Show("DFL " + vertices[7]);
            */
            int xMin = int.MaxValue;
            int yMin = int.MaxValue;
            int xMax = 0;
            int yMax = 0;

            foreach (Vector2 v2 in vertices)
            {
                //GTA.UI.Notification.Show(v.ToString());
                int x = (int) v2.X;
                int y = (int) v2.Y;
                //GTA.UI.Notification.Show(v2.ToString());

                if (x < xMin)
                    xMin = x;
                if (x > xMax)
                    xMax = x;
                if (y < yMin)
                    yMin = y;
                if (y > yMax)
                    yMax = y;
            }

            if (xMin < 0)
                xMin = 0;
            if (yMin < 0)
                yMin = 0;

            
            Vector2 BoundingBoxTopLeft = new Vector2(xMin, yMin);
            Vector2 BoundingBoxTopRight = new Vector2(xMax, yMin);
            Vector2 BoundingBoxBottomLeft = new Vector2(xMin, yMax);
            Vector2 BoundingBoxBottomRight = new Vector2(xMax, yMax);

            return new Ped2DBoundingBox(BoundingBoxTopLeft, BoundingBoxTopRight, BoundingBoxBottomLeft, BoundingBoxBottomRight);
        }

        public static float GetPedHeight(Ped ped)
        {
            float max = ped.Bones[0].Position.Z;
            foreach (PedBone pedBone in ped.Bones)
            {
                max = pedBone.Position.Z > max ? pedBone.Position.Z : max;
            }
            return max - World.GetGroundHeight(ped.Position);
        }

        /*
        private static float GetPedLowestBodyPart(Ped ped)
        {
            Vector3 pos = ped.Bones[0].Position;
            pos.Z -= paddingBottom;
            float yMax = Utilities.World3DToScreen2d(pos).Y;
            foreach (PedBone pedBone in ped.Bones)
            {
                Vector3 posMax = pedBone.Position;
                posMax.Z -= paddingBottom;
                yMax = Utilities.World3DToScreen2d(posMax).Y > yMax ? Utilities.World3DToScreen2d(posMax).Y : yMax;
            }

            //yMax = yMax > Screen.PrimaryScreen.Bounds.Height ? Screen.PrimaryScreen.Bounds.Height : yMax;
            return yMax;
        }
        private static float GetPedHighestBodyPart(Ped ped)
        {
            Vector3 pos = ped.Bones[0].Position;
            pos.Z += paddingTop;
            float yMin = Utilities.World3DToScreen2d(pos).Y;
            foreach (PedBone pedBone in ped.Bones)
            {
                Vector3 posMin = pedBone.Position;
                posMin.Z += paddingTop;
                yMin = Utilities.World3DToScreen2d(posMin).Y < yMin ? Utilities.World3DToScreen2d(posMin).Y : yMin;
            }
            //useless check???
            //yMin = yMin > 0 ? 0 : yMin;

            return yMin;
        }
        private static float GetPedFurthestRightBodyPart(Ped ped)
        {
            Vector3 pos = ped.Bones[0].Position;
            pos.X += paddingRight;
            float xMax = Utilities.World3DToScreen2d(ped.Bones[0].Position).X;
            foreach (PedBone pedBone in ped.Bones)
            {
                Vector3 posMax = pedBone.Position;
                posMax.X += paddingRight;
                xMax = Utilities.World3DToScreen2d(posMax).X > xMax ? Utilities.World3DToScreen2d(posMax).X : xMax;
            }
            //xMax = xMax > Screen.PrimaryScreen.Bounds.Width ? Screen.PrimaryScreen.Bounds.Width : xMax;
            return xMax;
        }
        private static float GetPedFurthestLeftBodyPart(Ped ped)
        {
            Vector3 pos = ped.Bones[0].Position;
            pos.X -= paddingLeft;
            float xMin = Utilities.World3DToScreen2d(ped.Bones[0].Position).X;
            foreach (PedBone pedBone in ped.Bones)
            {
                Vector3 posMin = pedBone.Position;
                posMin.X -= paddingLeft;
                xMin = Utilities.World3DToScreen2d(posMin).X < xMin ? Utilities.World3DToScreen2d(posMin).X : xMin;
            }
            //xMin = xMin > 0 ? 0 : xMin;
            return xMin;
        }
        */

    }
}
