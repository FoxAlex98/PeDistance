using GTA.UI;

namespace GtaVModPeDistance.CollectingSteps.ConcreteSteps
{
    class FinalStep : CollectingStep
    {
        public override void ExecuteStep()
        {
            Notification.Show("Counter: " + (++CollectingState.CollectedDataCounter) + "/" + Settings.MaxCollectedData);
            if (CollectingState.CollectedDataCounter >= Settings.MaxCollectedData || CollectingState.WannaStop)
                CollectingUtils.EndingCollectingData();
            if (CollectingState.WannaStop)
                CollectingUtils.ForceWritingData();
            UtilsFunctions.SetupMenu = true;
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
