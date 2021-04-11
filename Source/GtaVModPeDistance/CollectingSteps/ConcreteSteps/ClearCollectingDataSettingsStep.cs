using GTA;

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
            return Settings.ClearCollectingDataDelay * 1000;
        }

        public override CollectingStep GetNextStep()
        {
            return new FinalStep();
        }
    }
}
