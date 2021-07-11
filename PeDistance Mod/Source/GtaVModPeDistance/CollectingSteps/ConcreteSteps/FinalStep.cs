using GTA;
using GTA.UI;

namespace GtaVModPeDistance.CollectingSteps.ConcreteSteps
{
    class FinalStep : CollectingStep
    {
        public override void ExecuteStep()
        {
            CollectingState.NotificationId = Notification.Show("Counter: " + (++CollectingState.CollectedDataCounter) + "/" + Settings.MaxCollectedData);
            Globals.HideHud();
            if (CollectingState.WannaStop)
            {
                CollectingUtils.ForceWritingData();
                UtilsFunctions.SetupMenu = true;
            }
            if(CollectingState.CollectedDataCounter % 5 == 0)
                World.DestroyAllCameras();
            if (CollectingState.CollectedDataCounter >= Settings.MaxCollectedData || CollectingState.WannaStop)
                CollectingUtils.EndingCollectingData();
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
