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

        public override void ExecuteStep()
        {
            ScreenShot image = screenShot.TakeScreenshot();

            Data data = new Data(
                    CollectingState.CollectedDataCounter++,
                    GetDistance(CollectingState.Ped.Position, World.RenderingCamera.Position),
                    CoordinatesUtils.GetPedBoundingBox(CollectingState.Ped),
                    CoordinatesUtils.GetPedHeight(CollectingState.Ped),
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
