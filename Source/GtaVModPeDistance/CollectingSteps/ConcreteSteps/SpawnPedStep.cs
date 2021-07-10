using GTA;
using GTA.Math;

namespace GtaVModPeDistance.CollectingSteps.ConcreteSteps
{
    class SpawnPedStep : CollectingStep
    {
        public override void ExecuteStep()
        {
            float y = GetNextYPosition();
            float x = Utilities.GetPosXByPosY(y);
            CollectingState.Ped = World.CreateRandomPed(World.RenderingCamera.GetOffsetPosition(new Vector3(x, y, 0)));
            CollectingState.Ped.Heading += GetRotationUsingCameraAxis();
            //CollectingState.Ped.Heading += GetRotationUsingCameraPosition(); //TODO: controllare se togliere
        }

        public override int GetDelay()
        {
            return Settings.PedSpawningDelay * 1000;
        }

        public override CollectingStep GetNextStep()
        {
            return new CollectDataStep();
        }

        private int GetRotationUsingCameraAxis()
        {
            return (int) World.RenderingCamera.Rotation.Z + 180 + GetRandomHeadingAngle();
        }

        private int GetRandomHeadingAngle()
        {
           return (int) Utilities.NextFloat(0, 359);
        }

        private float GetNextYPosition()
        {
            float yPos = CollectingState.PeDistance;
            if (yPos > 30f)
                return CollectingState.PeDistance = Utilities.GetYMinByFov();
            CollectingState.PeDistance += 0.1f;
            return yPos;
        }
    }
}
