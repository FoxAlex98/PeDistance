using GTA;
using GTA.UI;
using GtaVModPeDistance.File;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtaVModPeDistance.CollectingSteps
{
    class CollectingUtils
    {
        static ScreenShotManager screenShotManager = ScreenShotManager.GetInstance();
        static DataMananger dataManager = DataMananger.GetInstance();

        public static void StartCollectingData()
        {
            CollectingState.InitialPosition = Game.Player.Character.Position;
            Game.Player.Character.IsVisible = false;
            Globals.HideHud();
            screenShotManager.DeleteAllScreenShot();
            dataManager.CleanFile();
            CollectingState.WannaStop = false;
            CollectingState.CollectedDataCounter = 0;
            CollectingState.Ped = null;
            CollectingState.StartCollectingData = true;
            CollectingState.ActualStep = GetFirstStep();
            Notification.Show("Starting collecting data... Remove menu using ESC");

        }
        public static void EndingCollectingData()
        {
            Notification.Show("Stop collecting data...");
            Game.Player.Character.IsVisible = true;
            Game.Player.Character.Position = CollectingState.InitialPosition;
            Globals.ShowHud();
            World.RenderingCamera = null;
            CollectingState.Ped = null;
            CollectingState.StartCollectingData = false;
        }

        public static void StopCollectingData()
        {
            if (CollectingState.StartCollectingData)
            {
                Notification.Show("Stop collecting data...");
                CollectingState.WannaStop = true;
            }
        }

        public static CollectingStep GetFirstStep()
        {
            return new TeleportToRandomSavedLocationStep();
        }

    }
}
