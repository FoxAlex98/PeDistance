using GTA.UI;

namespace GtaVModPeDistance.CollectingSteps.ConcreteSteps
{
    class ClearNearEntitiesStep : CollectingStep
    {
        public override void ExecuteStep()
        {
            UtilsFunctions.DeleteAllNearPed();
            UtilsFunctions.DeleteAllNearVehicles();
            Notification.Hide(CollectingState.NotificationId);
        }

        public override int GetDelay()
        {
            return Settings.RenderingDelay * 1000;
        }

        public override CollectingStep GetNextStep()
        {
            return new SpawnPedStep();
        }

    }
}
