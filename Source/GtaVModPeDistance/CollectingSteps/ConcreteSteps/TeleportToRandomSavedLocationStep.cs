using GTA;
using GTA.Math;
using GtaVModPeDistance.CollectingSteps.ConcreteSteps;
using System;

namespace GtaVModPeDistance.CollectingSteps
{
    class TeleportToRandomSavedLocationStep : CollectingStep
    {
        LocationManager locationManager = new LocationManager();
        Random rand = new Random();

        public override void CallFunction()
        {
            CollectingState.SpawnPoint = locationManager.GetRandomPoint();
            Vector3 Position = CollectingState.SpawnPoint.GetPosition();
            Vector3 Rotation = CollectingState.SpawnPoint.GetRotation();
            Game.Player.Character.Position = Position;
            float Z;
            if (GtaVModPeDistance.Settings.CameraFixedHeight == 0)
            {
                // TODO sistemare l'orientamento della verso il basso della camera
                Z = (Position.Z - World.GetGroundHeight(Game.Player.Character.Position)) + rand.Next(GtaVModPeDistance.Settings.CameraMinSpawningHeight, GtaVModPeDistance.Settings.CameraMaxSpawningHeight);
            }
            else
            {
                Z = Position.Z + GtaVModPeDistance.Settings.CameraFixedHeight;
            }
            Camera camera = World.CreateCamera(new Vector3(Position.X, Position.Y, Z), Rotation, 60);
            World.RenderingCamera = camera;
            // Notification.Show("Camera has been ~b~generated to ~o~" + spawnPoint.StreetName.ToString() + ", " + spawnPoint.ZoneLocalizedName.ToString());

        }

        public override int GetDelay()
        {
            return GtaVModPeDistance.Settings.TeleportingDelay * 1000;
        }

        public override CollectingStep GetNextStep()
        {
            return new ClearNearEntitiesStep();
        }
    }
}
