using GTA;
using GTA.Math;
using GtaVModPeDistance.File;
using GtaVModPeDistance.Models;
using System;
using System.Drawing;

namespace GtaVModPeDistance.CollectingSteps.ConcreteSteps
{
    class CollectDataStep : CollectingStep
    {
        ScreenShotManager screenShotManager = ScreenShotManager.GetInstance();
        DataMananger dataManager = DataMananger.GetInstance();

        public override void ExecuteStep()
        {
            screenShotManager.TakeScreenshot();
            Ped2DBoundingBox box = CoordinatesUtils.GetPedBoundingBoxInScreen(CollectingState.Ped);
            if(Settings.PrintBox)
                screenShotManager.DrawBoundingBox(new Vector2(box.PedBottomLeftX, box.PedBottomLeftY), new Vector2(box.PedTopRightX, box.PedTopRightY), Color.Blue);
            ScreenShot image = screenShotManager.SaveScreenShot();

            Data data = new Data(
                    Math.Round(World.GetDistance(CollectingState.Ped.Position, World.RenderingCamera.Position), 3),
                    box,
                    image.Name,
                    image.b64String,
                    World.CurrentTimeOfDay.ToString(),
                    CollectingState.Ped.Rotation.Z,
                    World.RenderingCamera.Position.Z - World.GetGroundHeight(Game.Player.Character.Position)                  
                );
            dataManager.AddElement(data);
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
