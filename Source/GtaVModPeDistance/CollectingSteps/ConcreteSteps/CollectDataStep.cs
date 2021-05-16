using GTA;
using GTA.Math;
using GtaVModPeDistance.File;
using GtaVModPeDistance.Models;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace GtaVModPeDistance.CollectingSteps.ConcreteSteps
{
    class CollectDataStep : CollectingStep
    {
        ScreenShotManager screenShot = ScreenShotManager.GetInstance();
        DataMananger dataManager = DataMananger.GetInstance();

        public override void ExecuteStep()
        {

            screenShot.TakeScreenshot();

            Vector2 topRight = Utilities.World3DToScreen2d(CollectingState.Ped.GetOffsetPosition(CollectingState.Ped.Model.Dimensions.rearBottomLeft));
            Vector2 botLeft = Utilities.World3DToScreen2d(CollectingState.Ped.GetOffsetPosition(CollectingState.Ped.Model.Dimensions.frontTopRight));
            screenShot.DrawBoundingBox(new Vector2(botLeft.X.ParseToScreenWidth(), botLeft.Y.ParseToScreenHeight()), new Vector2(topRight.X.ParseToScreenWidth(), topRight.Y.ParseToScreenHeight()), Color.Red);

            Ped2DBoundingBox box = CoordinatesUtils.GetPedBoundingBox(CollectingState.Ped);
            screenShot.DrawBoundingBox(new Vector2(box.PedBottomLeftX, box.PedBottomLeftY), new Vector2(box.PedTopRightX, box.PedTopRightY), Color.Blue);

            ScreenShot image = screenShot.SaveScreenShot();

            Data data = new Data(
                    CollectingState.CollectedDataCounter++,
                    GetDistance(CollectingState.Ped.Position, World.RenderingCamera.Position),
                    box,
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
