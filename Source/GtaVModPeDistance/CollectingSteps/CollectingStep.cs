﻿using GTA;

namespace GtaVModPeDistance.CollectingSteps
{
    abstract class CollectingStep
    {
        private float _start;
        public CollectingStep()
        {
            _start = Game.GameTime;
        }
        public void Process()
        {
            if (Game.GameTime > _start + GetDelay())
            {
                CallFunction();
                CollectingState.ActualStep = GetNextStep();
            }
        }

        public abstract int GetDelay();
        public abstract CollectingStep GetNextStep();
        public abstract void CallFunction();
    }
}
