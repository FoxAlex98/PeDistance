namespace GtaVModPeDistance.CollectingSteps.ConcreteSteps
{
    class ClearNearEntitiesStep : CollectingStep
    {
        public override void ExecuteStep()
        {
            UtilsFunctions.DeleteAllNearPed();
            UtilsFunctions.DeleteAllNearVehicles();
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
