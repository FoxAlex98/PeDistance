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
            
            CollectingState.InitialPosition = Game.Player.Character.Position;
            Game.Player.Character.IsVisible = false;
            UtilsFunctions.ResetWantedLevel();//sembra che non funzioni
            Globals.HideHud();
            CollectingState.WannaStop = false;
            CollectingState.CollectedDataCounter = 0;
            CollectingState.Ped = null;
            CollectingState.ActualStep = GetFirstStep();
            Notification.Show("Starting collecting data... Remove menu using ESC");
            CollectingState.StartCollectingData = true;
        }
        public static void EndingCollectingData()
        {
            Notification.Show("Stop collecting data...");
            Game.Player.Character.IsVisible = true;
            Game.Player.Character.Position = CollectingState.InitialPosition;
            Globals.ShowHud(); //TODO: check bug hud
            World.RenderingCamera = null;
            CollectingState.Ped = null;
            CollectingState.StartCollectingData = false;
        }

        public static void WannaStopCollectingData()
        {
            if (!CollectingState.StartCollectingData) return;

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
