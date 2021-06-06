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

        public override void ExecuteStep()
        {
            CollectingState.WannaDraw = false;
            CollectingState.SpawnPoint = locationManager.GetRandomPoint();
            UtilsFunctions.SetCameraPosition(CollectingState.SpawnPoint);
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
