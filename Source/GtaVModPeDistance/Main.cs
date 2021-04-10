using System;
using System.Windows.Forms;
using System.Collections.Generic;
using GTA;
using GTA.UI;
using NativeUI;
using GtaVModPeDistance.CollectingSteps;

namespace GtaVModPeDistance
{
    public class Main : Script
    {
        string ModName = "Tostino-ML";
        string Developer = "Danilo-AleS-AleC";
        string Version = "1.0";

        MenuPool modMenuPool;
        Menu mainMenu;
        MenuHelper menuHelper;

        CollectingStep collectingStep;

        public Main()
        {
            GtaVModPeDistance.Settings.LoadSettings();

            collectingStep = CollectingUtils.GetFirstStep();
            menuHelper = new MenuHelper();

            Notification.Show("~o~" + ModName + " " + Version + " by ~o~" + Developer + " Loaded");
            Notification.Show("Use F5 to open menu");
            MenuSetup();
            Tick += onTick;
            KeyDown += onKeyDown;
        }

        private void onTick(object sender, EventArgs e)
        {           
            if(modMenuPool != null)
                modMenuPool.ProcessMenus();

            if(CollectingState.StartCollectingData)
                collectingStep.Process();
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

            mainMenu = new Menu("Tostino Menu", "SELECT AN OPTION", mainMenuItem);
            modMenuPool.Add(mainMenu.ModMenu);

            UIMenu uiUtilsMenu = modMenuPool.AddSubMenu(mainMenu.ModMenu, "Utils Menu");
            new Menu(uiUtilsMenu, menuHelper.GetUtilsMenu());

            UIMenu uiMlMenu = modMenuPool.AddSubMenu(mainMenu.ModMenu, "Settings Menu");
            new Menu(uiMlMenu, menuHelper.GetConfigMenu());           
        }
    }
}