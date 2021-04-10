using GTA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtaVModPeDistance.CollectingSteps.ConcreteSteps
{
    class ClearNearEntitiesStep : CollectingStep
    {
        public override void CallFunction()
        {
            UtilsFunctions.DeleteAllNearPed();
            UtilsFunctions.DeleteAllNearVehicles();
        }

        public override int GetDelay()
        {
            return GtaVModPeDistance.Settings.RenderingDelay * 1000;
        }

        public override CollectingStep GetNextStep()
        {
            return new SpawnPedStep();
        }

    }
}
