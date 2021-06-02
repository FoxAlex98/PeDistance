using GTA;
using GTA.Math;

namespace GtaVModPeDistance.CollectingSteps.ConcreteSteps
{
    class SpawnPedStep : CollectingStep
    {
        public override void ExecuteStep()
        {
            float y = Utilities.GetYByFov();
            float x = Utilities.GetPosXByPosY(y);
            CollectingState.Ped = World.CreateRandomPed(World.RenderingCamera.GetOffsetPosition(new Vector3(x, y, 0)));
            CollectingState.Ped.Heading += GetRotationUsingCameraAxis();
            //CollectingState.Ped.Heading += GetRotationUsingCameraPosition();
            CollectingState.WannaDraw = true;
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

        private int GetRotationUsingCameraPosition()
        {
            CollectingState.Ped.FacePosition(World.RenderingCamera.Position);
            return GetRandomHeadingAngle();
        }

        private int GetRandomHeadingAngle()
        {
           return (int) Utilities.NextFloat(0, 359);
        }
    }
}
