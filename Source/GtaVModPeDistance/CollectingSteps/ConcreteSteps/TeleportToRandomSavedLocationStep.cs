using GtaVModPeDistance.File;

namespace GtaVModPeDistance.CollectingSteps.ConcreteSteps
{
    class TeleportToRandomSavedLocationStep : CollectingStep
    {
        private LocationManager locationManager = LocationManager.GetInstance();

        public override void ExecuteStep()
        {
            CollectingState.SpawnPoint = locationManager.GetRandomPoint();
            UtilsFunctions.SetCameraPosition(CollectingState.SpawnPoint);
            if (Settings.RandomWeather)
                UtilsFunctions.SetRandomWeather();
            if (Settings.RandomTime)
                UtilsFunctions.SetRandomTime();

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
