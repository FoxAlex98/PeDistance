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
            return Settings.RenderingDelay * 1000;
        }

        public override CollectingStep GetNextStep()
        {
            return new SpawnPedStep();
        }

    }
}
