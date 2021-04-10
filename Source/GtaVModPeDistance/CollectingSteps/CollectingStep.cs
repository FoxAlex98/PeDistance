using GTA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtaVModPeDistance.CollectingSteps
{
    abstract class CollectingStep
    {
        private float start;
        public CollectingStep()
        {
            start = Game.GameTime;
        }
        public CollectingStep Process()
        {
            if (Game.GameTime > start + GetDelay())
            {
                CallFunction();
                CollectingState.ActualStep = GetNextStep();
            }
            return CollectingState.ActualStep;
        }

        public abstract int GetDelay();
        public abstract CollectingStep GetNextStep();
        public abstract void CallFunction();
    }
}
