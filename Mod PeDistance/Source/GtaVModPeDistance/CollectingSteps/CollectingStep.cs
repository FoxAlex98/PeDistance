using GTA;

namespace GtaVModPeDistance.CollectingSteps
{
    //TODO: fare refactor dei concretestep usando i delegate
    abstract class CollectingStep
    {
        private float start;
        public CollectingStep()
        {
            start = Game.GameTime;
        }
        public void Process()
        {
            if (Game.GameTime > start + GetDelay())
            {
                ExecuteStep();
                CollectingState.ActualStep = GetNextStep();
            }
        }

        public abstract int GetDelay();
        public abstract CollectingStep GetNextStep();
        public abstract void ExecuteStep();
    }
}
