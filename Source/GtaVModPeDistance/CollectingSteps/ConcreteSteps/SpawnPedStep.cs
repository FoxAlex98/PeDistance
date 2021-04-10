using GTA;
using GTA.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtaVModPeDistance.CollectingSteps.ConcreteSteps
{
    class SpawnPedStep : CollectingStep
    {
        Random rand = new Random();

        public override void CallFunction()
        {
            float x, y;
            do
            {
                x = rand.Next(-4, 4);
                y = rand.Next(GtaVModPeDistance.Settings.PedMinSpawningDistance, GtaVModPeDistance.Settings.PedMaxSpawningDistance);
            } while (Math.Abs(x) > y);
            CollectingState.Ped = World.CreateRandomPed(World.RenderingCamera.GetOffsetPosition(new Vector3(x, y, 0)));
            CollectingState.Ped.Heading = rand.Next(360);
        }

        public override int GetDelay()
        {
            return GtaVModPeDistance.Settings.PedSpawningDelay * 1000;
        }

        public override CollectingStep GetNextStep()
        {
            return new CollectDataStep();
        }
    }
}
