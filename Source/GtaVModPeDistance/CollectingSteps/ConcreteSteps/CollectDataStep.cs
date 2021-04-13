using GTA;
using GTA.Math;
using GtaVModPeDistance.File;
using GtaVModPeDistance.Models;
using System;

namespace GtaVModPeDistance.CollectingSteps.ConcreteSteps
{
    class CollectDataStep : CollectingStep
    {
        ScreenShotManager screenShot = ScreenShotManager.GetInstance();
        DataMananger dataManager = DataMananger.GetInstance();

        public override void CallFunction()
        {
            ScreenShot image = screenShot.TakeScreenshot();

            Data data = new Data(
                    CollectingState.CollectedDataCounter++,
                    GetDistance(CollectingState.Ped.Position, World.RenderingCamera.Position),
                    GetPedBoundingBox(),
                    GetPedHeight(),
                    CollectingState.Ped.Rotation.Z,
                    World.RenderingCamera.Position.Z - World.GetGroundHeight(Game.Player.Character.Position),
                    image.Name,
                    image.b64String,
                    World.CurrentTimeOfDay.ToString()
                );

            dataManager.AddElement(data);
        }
        private double GetDistance(Vector3 pedPosition, Vector3 cameraPosition)
        {
            return Math.Sqrt(Math.Pow(pedPosition.X - cameraPosition.X, 2) + Math.Pow(pedPosition.Y - cameraPosition.Y, 2) + Math.Pow(pedPosition.Z - cameraPosition.Z, 2));
        }


        private Ped2DBoundingBox GetPedBoundingBox()
        {
            float pedHeighestBodyPart = GetPedHighestBodyPart();
            float pedLowestBodyPart = GetPedLowestBodyPart();
            float pedFurthestRightBodyPart = GetPedFurthestRightBodyPart();
            float pedFurthestLeftBodyPart = GetPedFurthestLeftBodyPart();

            Vector2 BoundingBoxTopLeft = new Vector2(pedFurthestLeftBodyPart, pedHeighestBodyPart);
            Vector2 BoundingBoxTopRight = new Vector2(pedFurthestRightBodyPart, pedHeighestBodyPart);
            Vector2 BoundingBoxBottomLeft = new Vector2(pedFurthestLeftBodyPart, pedLowestBodyPart);
            Vector2 BoundingBoxBottomRight = new Vector2(pedFurthestRightBodyPart, pedLowestBodyPart);

            GTA.UI.Notification.Show(BoundingBoxTopLeft.ToString());
            GTA.UI.Notification.Show(BoundingBoxTopRight.ToString());
            GTA.UI.Notification.Show(BoundingBoxBottomLeft.ToString());
            GTA.UI.Notification.Show(BoundingBoxBottomRight.ToString());

            return new Ped2DBoundingBox(BoundingBoxTopLeft, BoundingBoxTopRight, BoundingBoxBottomLeft, BoundingBoxBottomRight);
        }

        private float GetPedHeight()
        {
            float max = CollectingState.Ped.Bones[0].Position.Z;
            foreach (PedBone pedBone in CollectingState.Ped.Bones)
            {
                max = pedBone.Position.Z > max ? pedBone.Position.Z : max;
            }
            return max - World.GetGroundHeight(CollectingState.Ped.Position);
        }

        private float GetPedHighestBodyPart()
        {
            float max = CollectingState.Ped.Bones[0].Position.Z;
            foreach (PedBone pedBone in CollectingState.Ped.Bones)
            {
                max = pedBone.Position.Z > max ? pedBone.Position.Z : max;
            }
            return max;
        }
        private float GetPedLowestBodyPart()
        {
            float min = CollectingState.Ped.Bones[0].Position.Z;
            foreach (PedBone pedBone in CollectingState.Ped.Bones)
            {
                min = pedBone.Position.Z < min ? pedBone.Position.Z : min;
            }
            return min;
        }
        private float GetPedFurthestRightBodyPart()
        {
            float max = CollectingState.Ped.Bones[0].Position.X;
            foreach (PedBone pedBone in CollectingState.Ped.Bones)
            {
                max = pedBone.Position.X > max ? pedBone.Position.X : max;
            }
            return max;
        }
        private float GetPedFurthestLeftBodyPart()
        {
            float min = CollectingState.Ped.Bones[0].Position.X;
            foreach (PedBone pedBone in CollectingState.Ped.Bones)
            {
                min = pedBone.Position.X < min ? pedBone.Position.X : min;
            }
            return min;
        }


        public override int GetDelay()
        {
            return Settings.CollectingDataDelay * 1000;
        }

        public override CollectingStep GetNextStep()
        {
            return new ClearCollectingDataSettingsStep();
        }

    }
}
