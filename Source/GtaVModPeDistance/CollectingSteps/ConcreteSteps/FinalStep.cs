using GTA;
using GTA.UI;

namespace GtaVModPeDistance.CollectingSteps.ConcreteSteps
{
    class FinalStep : CollectingStep
    {
        public override void ExecuteStep()
        {
            Notification.Show("Counter: " + CollectingState.CollectedDataCounter + "/" + Settings.MaxCollectedData);
            if (CollectingState.CollectedDataCounter >= Settings.MaxCollectedData || CollectingState.WannaStop)
            {
                Notification.Show("Stop collecting data...");
                Game.Player.Character.IsVisible = true;
                Game.Player.Character.Position = CollectingState.InitialPosition;
                Globals.ShowHud();
                World.RenderingCamera = null;
                CollectingState.Ped = null;
                CollectingState.StartCollectingData = false;
            }
            if (CollectingState.WannaStop)
            {
                CollectingUtils.ForceWritingData();
            }
            CollectingState.WannaDraw = false;
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
