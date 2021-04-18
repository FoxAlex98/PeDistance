using GTA;
using GTA.Math;
using GtaVModPeDistance.Models;

namespace GtaVModPeDistance
{
    class CoordinatesUtils
    {
        public static Ped2DBoundingBox GetPedBoundingBox(Ped ped)
        {
            float pedHeighestBodyPart = GetPedHighestBodyPart(ped);
            float pedLowestBodyPart = GetPedLowestBodyPart(ped);
            float pedFurthestRightBodyPart = GetPedFurthestRightBodyPart(ped);
            float pedFurthestLeftBodyPart = GetPedFurthestLeftBodyPart(ped);

            Vector2 BoundingBoxTopLeft = new Vector2(pedFurthestLeftBodyPart, pedHeighestBodyPart);
            Vector2 BoundingBoxTopRight = new Vector2(pedFurthestRightBodyPart, pedHeighestBodyPart);
            Vector2 BoundingBoxBottomLeft = new Vector2(pedFurthestLeftBodyPart, pedLowestBodyPart);
            Vector2 BoundingBoxBottomRight = new Vector2(pedFurthestRightBodyPart, pedLowestBodyPart);

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

        private static float GetPedHighestBodyPart(Ped ped)
        {
            float max = ped.Bones[0].Position.Z;
            foreach (PedBone pedBone in ped.Bones)
            {
                max = pedBone.Position.Z > max ? pedBone.Position.Z : max;
            }
            return max;
        }
        private static float GetPedLowestBodyPart(Ped ped)
        {
            float min = ped.Bones[0].Position.Z;
            foreach (PedBone pedBone in ped.Bones)
            {
                min = pedBone.Position.Z < min ? pedBone.Position.Z : min;
            }
            return min;
        }
        private static float GetPedFurthestRightBodyPart(Ped ped)
        {
            float max = ped.Bones[0].Position.X;
            foreach (PedBone pedBone in ped.Bones)
            {
                max = pedBone.Position.X > max ? pedBone.Position.X : max;
            }
            return max;
        }
        private static float GetPedFurthestLeftBodyPart(Ped ped)
        {
            float min = ped.Bones[0].Position.X;
            foreach (PedBone pedBone in ped.Bones)
            {
                min = pedBone.Position.X < min ? pedBone.Position.X : min;
            }
            return min;
        }

    }
}
