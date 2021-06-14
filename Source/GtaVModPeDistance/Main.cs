using GTA;
using GTA.UI;
using GtaVModPeDistance.CollectingSteps;
using GtaVModPeDistance.Menu;
using NativeUI;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace GtaVModPeDistance
{
    public class Main : Script
    {
        string ModName = "PeDistance-ML";
        string Developer = "Danilo-AleS-AleC";
        string Version = "1.0";

        MenuPool modMenuPool;
        Menu.Menu mainMenu;
        MenuHelper menuHelper;

        public Main()
        {
            GtaVModPeDistance.Settings.LoadSettings();

            menuHelper = new MenuHelper();

            Notification.Show("~o~" + ModName + " " + Version + "~n~~w~by ~b~" + Developer + " ~w~Loaded");
            Notification.Show("Use F5 to open menu");
            MenuSetup();
            Tick += onTick;
            KeyDown += onKeyDown;
        }

        private void onTick(object sender, EventArgs e)
        {           
            if(modMenuPool != null)
                modMenuPool.ProcessMenus();

            if (UtilsFunctions.SetupMenu)
            {
                MenuSetup();
                UtilsFunctions.SetupMenu = false;
            }

            if (CollectingState.StartCollectingData)
                CollectingState.ActualStep.Process();


            if (UtilsFunctions.ped != null && menuHelper.DebugMode.Checked)
            {
                UtilsFunctions.ped.Draw3DPedBoundingBoxUsingVertex(System.Drawing.Color.Red);
                UtilsFunctions.ped.PrintPedAxis();
                Game.Player.Character.PrintPedAxis();
            }

            if (UtilsFunctions.ActiveNearbyEntitiesBoundingBox)
                Utilities.DrawBoundingBoxNearbyEntities(75);

            if(UtilsFunctions.DistancePoints.Count > 0)
            {
                if(UtilsFunctions.DistancePoints.Count == 2) Utilities.DrawLine(UtilsFunctions.DistancePoints[0], UtilsFunctions.DistancePoints[1], System.Drawing.Color.Red);
            }

            if(UtilsFunctions.MeterMode && Game.Player.Character.IsShooting)
                    UtilsFunctions.PrintDistancePoints(Game.Player.Character.LastWeaponImpactPosition);
        }

        private void onKeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.F5)
                mainMenu.ToggleMenu();
        }

        private void MenuSetup()
        {
            modMenuPool = new MenuPool();

            // Main menu
            List<Menu.MenuItem> mainMenuItem = new List<Menu.MenuItem>();
            mainMenuItem.Add(new Menu.MenuItem("Start Collecting Data", CollectingUtils.StartCollectingData));
            mainMenuItem.Add(new Menu.MenuItem("Stop Collecting Data", CollectingUtils.WannaStopCollectingData));
            mainMenuItem.Add(new Menu.MenuItem("Clear Data", CollectingUtils.ClearCollectedData));
            mainMenuItem.Add(new Menu.MenuItem("Reset", UtilsFunctions.Reset));

            mainMenu = new Menu.Menu("PeDistance Menu", "SELECT AN OPTION", mainMenuItem);
            modMenuPool.Add(mainMenu.ModMenu);

            UIMenu uiUtilsMenu = modMenuPool.AddSubMenu(mainMenu.ModMenu, "> Utils Menu");
            new Menu.Menu(uiUtilsMenu, menuHelper.GetUtilsMenu());

            UIMenu uiMlMenu = modMenuPool.AddSubMenu(mainMenu.ModMenu, "> Settings Menu", "Show up a menu to let you set your own preferred settings.");
            new Menu.Menu(uiMlMenu, menuHelper.GetConfigMenu());
            uiMlMenu.OnMenuOpen += OnSettingsMenuOpen;
            uiMlMenu.OnMenuClose += OnSettingsMenuClose;
        }

        private void OnSettingsMenuClose(UIMenu sender)
        {
            UtilsFunctions.DeleteAllNearPed();
            Game.Player.Character.Position = CoordinatesUtils.playerPosition;
            UtilsFunctions.Reset();
            menuHelper.SaveSettings();
            //confirm to Save
        }

        private void OnSettingsMenuOpen(UIMenu sender)
        {
            CoordinatesUtils.playerPosition = Game.Player.Character.Position;
            Game.Player.Character.IsVisible = false;
            UtilsFunctions.SpawnAtRandomSavedLocation();
            //UtilsFunctions.SpawnSettingPeds();//doesn't work
        }
    }
}