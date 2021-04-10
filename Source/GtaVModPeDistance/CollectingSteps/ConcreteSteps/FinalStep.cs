using GTA;
using GTA.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtaVModPeDistance.CollectingSteps.ConcreteSteps
{
    class FinalStep : CollectingStep
    {
        public override void CallFunction()
        {
            Notification.Show("Counter: " + CollectingState.CollectedDataCounter + "/" + GtaVModPeDistance.Settings.MaxCollectedData);
            if (CollectingState.CollectedDataCounter >= GtaVModPeDistance.Settings.MaxCollectedData || CollectingState.WannaStop)
            {
                Notification.Show("Stop collecting data...");
                Game.Player.Character.IsVisible = true;
                Game.Player.Character.Position = CollectingState.InitialPosition;
                Globals.ShowHud();
                World.RenderingCamera = null;
                CollectingState.Ped = null;
                CollectingState.StartCollectingData = false;
            }
        }

        public override int GetDelay()
        {
            return -1;
        }

        public override CollectingStep GetNextStep()
        {
            return new TeleportToRandomSavedLocationStep();
        }
    }
}
