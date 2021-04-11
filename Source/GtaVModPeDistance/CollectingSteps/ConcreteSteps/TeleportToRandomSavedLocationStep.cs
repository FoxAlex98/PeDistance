using GTA;
using GTA.Math;
using GtaVModPeDistance.CollectingSteps.ConcreteSteps;
using System;

namespace GtaVModPeDistance.CollectingSteps
{
    class TeleportToRandomSavedLocationStep : CollectingStep
    {
        private LocationManager locationManager = LocationManager.GetInstance();
        private Random rand = new Random();

        public override void CallFunction()
        {
            CollectingState.SpawnPoint = locationManager.GetRandomPoint();
            Vector3 Position = CollectingState.SpawnPoint.GetPosition();
            Vector3 Rotation = CollectingState.SpawnPoint.GetRotation();
            Game.Player.Character.Position = Position;
            float Z;
            if (Settings.CameraFixedHeight == 0)
            {
                // TODO sistemare l'orientamento della verso il basso della camera
                Z = (Position.Z - World.GetGroundHeight(Game.Player.Character.Position)) + rand.Next(Settings.CameraMinSpawningHeight, Settings.CameraMaxSpawningHeight);
            }
            else
            {
                Z = Position.Z + Settings.CameraFixedHeight;
            }
            Camera camera = World.CreateCamera(new Vector3(Position.X, Position.Y, Z), Rotation, 60);
            World.RenderingCamera = camera;
            // Notification.Show("Camera has been ~b~generated to ~o~" + spawnPoint.StreetName.ToString() + ", " + spawnPoint.ZoneLocalizedName.ToString());

        }

        public override int GetDelay()
        {
            return Settings.TeleportingDelay * 1000;
        }

        public override CollectingStep GetNextStep()
        {
            return new ClearNearEntitiesStep();
        }
    }
}
