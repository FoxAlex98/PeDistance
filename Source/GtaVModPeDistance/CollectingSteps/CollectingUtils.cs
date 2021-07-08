using GTA;
using GTA.UI;
using GtaVModPeDistance.CollectingSteps.ConcreteSteps;
using GtaVModPeDistance.File;

namespace GtaVModPeDistance.CollectingSteps
{
    class CollectingUtils
    {
        static ScreenShotManager screenShotManager = ScreenShotManager.GetInstance();
        static DataMananger dataManager = DataMananger.GetInstance();

        public static void StartCollectingData()
        {
            if (CollectingState.StartCollectingData) return;

            if(CollectingState.PeDistance == default)
                CollectingState.PeDistance = Utilities.GetYMinByFov();
            
            CollectingState.InitialPosition = Game.Player.Character.Position;
            Game.Player.Character.IsVisible = false;
            UtilsFunctions.ResetWantedLevel();
            Globals.HideHud();
            CollectingState.WannaStop = false;
            CollectingState.CollectedDataCounter = 0;
            CollectingState.Ped = null;
            CollectingState.ActualStep = GetFirstStep();
            Notification.Show("Starting collecting data...");
            CollectingState.StartCollectingData = true;
            if (Settings.RandomTime) CollectingState.InitialTime = World.CurrentTimeOfDay;
            if (Settings.RandomWeather) CollectingState.InitialWeather = World.Weather;
            UtilsFunctions.SetupMenu = true;
        }
        public static void EndingCollectingData()
        {
            Notification.Show("Stop collecting data...");
            Game.Player.Character.IsVisible = true;
            Game.Player.Character.Position = CollectingState.InitialPosition;
            Globals.ShowHud();
            World.RenderingCamera.Delete();
            World.RenderingCamera = null;
            CollectingState.Ped = null;
            if (Settings.RandomTime) World.CurrentTimeOfDay = CollectingState.InitialTime;
            if (Settings.RandomWeather) World.Weather = CollectingState.InitialWeather;
            CollectingState.StartCollectingData = false;
        }

        public static void WannaStopCollectingData()
        {
            Notification.Show("Stop collecting data...");
            CollectingState.WannaStop = true;
        }

        public static void ForceWritingData()
        {
            dataManager.WriteDataToFile();
        }

        public static CollectingStep GetFirstStep()
        {
            return new TeleportToRandomSavedLocationStep();
        }

        public static void ClearCollectedData()
        {
            if (CollectingState.StartCollectingData) return;

            dataManager.CleanFile();
            screenShotManager.DeleteAllScreenShot();
            Notification.Show("Dataset and Screenshot cleaned");
        }

    }
}
