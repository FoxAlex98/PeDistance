using GTA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtaVModPeDistance.CollectingSteps.ConcreteSteps
{
    class ClearCollectingDataSettingsStep : CollectingStep
    {
        public override void CallFunction()
        {
            World.RenderingCamera.Delete();
            UtilsFunctions.DeleteAllNearPed();
        }

        public override int GetDelay()
        {
            return GtaVModPeDistance.Settings.ClearCollectingDataDelay * 1000;
        }

        public override CollectingStep GetNextStep()
        {
            return new FinalStep();
        }
    }
}
