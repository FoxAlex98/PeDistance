using GTA;
using GTA.Math;
using GTA.UI;
using GtaVModPeDistance.CollectingSteps;
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
        Menu mainMenu;
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

            //if (CollectingState.Ped != null)
            //{
            //    CollectingState.Ped.Draw3DPedBoundingBoxUsingVertex(System.Drawing.Color.Red);
            //}


            #region DrawBoundingBoxNearbyEntities
            if (UtilsFunctions.ActiveNearbyEntitiesBoundingBox)
                Utilities.DrawBoundingBoxNearbyEntities(75);
            #endregion

            /*
            if (CollectingState.WannaDraw)
                UtilsFunctions.DrawLine(CollectingState.Ped);
            */

            if(UtilsFunctions.DistancePoints.Count > 0)
            {
                if(UtilsFunctions.DistancePoints.Count == 2) Utilities.DrawLine(UtilsFunctions.DistancePoints[0], UtilsFunctions.DistancePoints[1], System.Drawing.Color.Red);
            }

            if(UtilsFunctions.MeterMode)
            {
                if(Game.Player.Character.IsShooting)
                {
                    UtilsFunctions.PrintDistancePoints(Game.Player.Character.LastWeaponImpactPosition);
                }                                  

            }
        }

        private void onKeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.F5 && !modMenuPool.IsAnyMenuOpen())
                mainMenu.ToggleMenu();
        }

        private void MenuSetup()
        {
            modMenuPool = new MenuPool();

            // Main menu
            List<MenuItem> mainMenuItem = new List<MenuItem>();
            mainMenuItem.Add(new MenuItem("Start Collecting Data", CollectingUtils.StartCollectingData));
            mainMenuItem.Add(new MenuItem("Stop Collecting Data", CollectingUtils.StopCollectingData));

            mainMenu = new Menu("PeDistance Menu", "SELECT AN OPTION", mainMenuItem);
            modMenuPool.Add(mainMenu.ModMenu);

            UIMenu uiUtilsMenu = modMenuPool.AddSubMenu(mainMenu.ModMenu, "> Utils Menu");
            new Menu(uiUtilsMenu, menuHelper.GetUtilsMenu());

            UIMenu uiMlMenu = modMenuPool.AddSubMenu(mainMenu.ModMenu, "> Settings Menu", "Show up a menu to let you set your own preferred settings. ~r~Do not forget ~w~to use ~g~Save ~w~to save modified settings.");
            new Menu(uiMlMenu, menuHelper.GetConfigMenu());
            uiMlMenu.OnMenuOpen += OnSettingsMenuOpen;
            uiMlMenu.OnMenuClose += OnSettingsMenuClose;
        }

        private void OnSettingsMenuClose(UIMenu sender)
        {
            UtilsFunctions.DeleteAllNearPed();
            Game.Player.Character.Position = CoordinatesUtils.playerPosition;
            UtilsFunctions.Reset();
            //confirm to Save
        }

        private void OnSettingsMenuOpen(UIMenu sender)
        {
            CoordinatesUtils.playerPosition = Game.Player.Character.Position;
            Game.Player.Character.IsVisible = false;
            UtilsFunctions.SpawnAtRandomSavedLocation();
            UtilsFunctions.SpawnSettingPeds();//doesn't work
        }
    }
}