using GTA;
using GTA.Math;
using GtaVModPeDistance.Models;

namespace GtaVModPeDistance
{
    class CoordinatesUtils
    {
        private static float paddingTop = 0.12f;
        private static float paddingLeft = 0.1f;
        private static float paddingRight = 0.1f;
        private static float paddingBottom = 0.05f;

        public static Ped2DBoundingBox GetPedBoundingBox(Ped ped)
        {
            float pedHeighestBodyPart = GetPedHighestBodyPart(ped);
            float pedLowestBodyPart = GetPedLowestBodyPart(ped);
            float pedFurthestRightBodyPart = GetPedFurthestRightBodyPart(ped);
            float pedFurthestLeftBodyPart = GetPedFurthestLeftBodyPart(ped);

            Vector2 BoundingBoxTopLeft = new Vector2(pedFurthestLeftBodyPart.ParseToScreenWidth(), pedHeighestBodyPart.ParseToScreenHeight());
            Vector2 BoundingBoxTopRight = new Vector2(pedFurthestRightBodyPart.ParseToScreenWidth(), pedHeighestBodyPart.ParseToScreenHeight());
            Vector2 BoundingBoxBottomLeft = new Vector2(pedFurthestLeftBodyPart.ParseToScreenWidth(), pedLowestBodyPart.ParseToScreenHeight());
            Vector2 BoundingBoxBottomRight = new Vector2(pedFurthestRightBodyPart.ParseToScreenWidth(), pedLowestBodyPart.ParseToScreenHeight());

            /*
            Game.RadioStation = RadioStation.RadioOff;
            GTA.UI.Notification.Show(BoundingBoxTopLeft.ToString());
            GTA.UI.Notification.Show(BoundingBoxTopRight.ToString());
            GTA.UI.Notification.Show(BoundingBoxBottomLeft.ToString());
            GTA.UI.Notification.Show(BoundingBoxBottomRight.ToString());
            */

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

        private static float GetPedLowestBodyPart(Ped ped)
        {
            Vector3 pos = ped.Bones[0].Position;
            pos.Z -= paddingBottom;
            float max = Utilities.World3DToScreen2d(pos).Y;
            foreach (PedBone pedBone in ped.Bones)
            {
                Vector3 posMax = pedBone.Position;
                posMax.Z -= paddingBottom;
                max = Utilities.World3DToScreen2d(posMax).Y > max ? Utilities.World3DToScreen2d(posMax).Y : max;
            }
            return max;
        }
        private static float GetPedHighestBodyPart(Ped ped)
        {
            Vector3 pos = ped.Bones[0].Position;
            pos.Z += paddingTop;
            float min = Utilities.World3DToScreen2d(pos).Y;
            foreach (PedBone pedBone in ped.Bones)
            {
                Vector3 posMin = pedBone.Position;
                posMin.Z += paddingTop;
                min = Utilities.World3DToScreen2d(posMin).Y < min ? Utilities.World3DToScreen2d(posMin).Y : min;
            }
            return min;
        }
        private static float GetPedFurthestRightBodyPart(Ped ped)
        {
            Vector3 pos = ped.Bones[0].Position;
            pos.X += paddingRight;
            float max = Utilities.World3DToScreen2d(ped.Bones[0].Position).X;
            foreach (PedBone pedBone in ped.Bones)
            {
                Vector3 posMax = pedBone.Position;
                posMax.X += paddingRight;
                max = Utilities.World3DToScreen2d(posMax).X > max ? Utilities.World3DToScreen2d(posMax).X : max;
            }
            return max;
        }
        private static float GetPedFurthestLeftBodyPart(Ped ped)
        {
            Vector3 pos = ped.Bones[0].Position;
            pos.X -= paddingLeft;
            float min = Utilities.World3DToScreen2d(ped.Bones[0].Position).X;
            foreach (PedBone pedBone in ped.Bones)
            {
                Vector3 posMin = pedBone.Position;
                posMin.X -= paddingLeft;
                min = Utilities.World3DToScreen2d(posMin).X < min ? Utilities.World3DToScreen2d(posMin).X : min;
            }
            return min;
        }

    }
}
