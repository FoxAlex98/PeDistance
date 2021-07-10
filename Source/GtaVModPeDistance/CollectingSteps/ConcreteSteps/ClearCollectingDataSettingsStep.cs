namespace GtaVModPeDistance.CollectingSteps.ConcreteSteps
{
    class ClearCollectingDataSettingsStep : CollectingStep
    {
        public override void ExecuteStep()
        {
            UtilsFunctions.SetTime(UtilsFunctions.GetClosestTimeInRange(), 0, 0, false);
            UtilsFunctions.DeleteAllNearPed();
        }

        public override int GetDelay()
        {
            return Settings.ClearCollectingDataDelay * 1000;
        }

        public override CollectingStep GetNextStep()
        {
            return new FinalStep();
        }
    }
}
