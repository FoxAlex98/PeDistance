﻿using GTA;
using GTA.Math;
using System;

namespace GtaVModPeDistance.CollectingSteps.ConcreteSteps
{
    class SpawnPedStep : CollectingStep
    {
        public override void CallFunction()
        {
            float y = Utilities.NextFloat(Settings.PedMinSpawningDistanceY, Settings.PedMaxSpawningDistanceY);
            float x = Utilities.GetPosXByPosY(y);
            CollectingState.Ped = World.CreateRandomPed(World.RenderingCamera.GetOffsetPosition(new Vector3(x, y, 0)));
            CollectingState.Ped.Heading = Utilities.NextFloat(1, 360);
        }

        public override int GetDelay()
        {
            return Settings.PedSpawningDelay * 1000;
        }

        public override CollectingStep GetNextStep()
        {
            return new CollectDataStep();
        }
    }
}
