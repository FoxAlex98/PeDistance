using GTA;
using GTA.Math;
using GtaVModPeDistance.File;
using GtaVModPeDistance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtaVModPeDistance.CollectingSteps.ConcreteSteps
{
    class CollectDataStep : CollectingStep
    {
        ScreenShotManager screenShot = new ScreenShotManager();
        DataMananger dataManager = new DataMananger();

        public override void CallFunction()
        {
            ScreenShot image = screenShot.TakeScreenshot();

            Data data = new Data(
                    CollectingState.CollectedDataCounter++,
                    GetDistance(CollectingState.Ped.Position, World.RenderingCamera.Position),
                    GetEntityHeight(),
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


        private float GetEntityHeight()
        {
            float max = CollectingState.Ped.Bones[0].Position.Z;
            foreach (PedBone pedBone in CollectingState.Ped.Bones)
            {
                max = pedBone.Position.Z > max ? pedBone.Position.Z : max;
            }
            return max - World.GetGroundHeight(CollectingState.Ped.Position);
        }


        public override int GetDelay()
        {
            return GtaVModPeDistance.Settings.CollectingDataDelay * 1000;
        }

        public override CollectingStep GetNextStep()
        {
            return new ClearCollectingDataSettingsStep();
        }

    }
}
