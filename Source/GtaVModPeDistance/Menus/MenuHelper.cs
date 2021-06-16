﻿using GTA;
using GTA.UI;
using GtaVModPeDistance.Menus.Impl;
using NativeUI;
using System.Collections.Generic;
using System.Drawing;

namespace GtaVModPeDistance.Menus
{
    class MenuHelper
    {
        // Utils
        public UIMenuCheckboxItem DebugMode;

        // Config
        UIMenuCheckboxItem saveScreenShotLocally, printBox;

        //Impl
        CameraFixedHeightListItem cameraFixedHeightListItem = new CameraFixedHeightListItem();
        CameraFovListItem cameraFovListItem = new CameraFovListItem();
        ImageFormatListItem imageFormatListItem = new ImageFormatListItem();
        MaxCollectedDataListItem maxCollectedDataListItem = new MaxCollectedDataListItem();
        CameraAngleListItem cameraAngleListItem = new CameraAngleListItem();
        TeleportingDelayListItem teleportingDelayListItem = new TeleportingDelayListItem();
        RenderingDelayListItem renderingDelayListItem = new RenderingDelayListItem();
        PedSpawningDelayListItem pedSpawningDelayListItem = new PedSpawningDelayListItem();
        CollectingDataDelayListItem collectingDataDelayListItem = new CollectingDataDelayListItem();
        ClearCollectingDataDelayListItem clearCollectingDataDelayListItem = new ClearCollectingDataDelayListItem();


        public MenuHelper()
        {
            DebugMode = new UIMenuCheckboxItem("Debug Mode", false);
        }

        public List<MenuItem> GetUtilsMenu()
        {
            //utils menu
            List<MenuItem> utilsList = new List<MenuItem>();

            DebugMode.CheckboxEvent += UtilsFunctions.ToggleDebugMode;
            utilsList.Add(new MenuItem(DebugMode));
            if (DebugMode.Checked)
                AddDebugItemList(utilsList);
            utilsList.Add(new MenuItem("Delete All Near Ped", UtilsFunctions.DeleteAllNearPed));
            utilsList.Add(new MenuItem("Delete All Near Vehicle", UtilsFunctions.DeleteAllNearVehicles));
            utilsList.Add(new MenuItem("Ignored by everyone", () => { Game.Player.IgnoredByEveryone = true; }));
            utilsList.Add(new MenuItem("Reset Wanted Level", UtilsFunctions.ResetWantedLevel));
            utilsList.Add(new MenuItem("Spawn Random Point", UtilsFunctions.SpawnAtRandomSavedLocation));
            utilsList.Add(new DayTimeListItem());
            
            utilsList.Add(new VehicleListItem("Car", Globals.VehicleType.CARS));
            utilsList.Add(new VehicleListItem("Motorbike", Globals.VehicleType.MOTORBIKES));
            utilsList.Add(new VehicleListItem("Helicopter", Globals.VehicleType.HELICOPTERS));
            utilsList.Add(new VehicleListItem("Plane", Globals.VehicleType.PLANES));
            utilsList.Add(new VehicleListItem("Boat", Globals.VehicleType.BOATS));
            
            utilsList.Add(new WeatherListItem());

            return utilsList;
        }

        private static void AddDebugItemList(List<MenuItem> utilsList)
        {
            utilsList.Add(new MenuItem(new UIMenuColoredItem("Dist between 2 point", Color.Green, Color.White), () => UtilsFunctions.PrintDistancePoints(Game.Player.Character.Position)));
            utilsList.Add(new MenuItem(new UIMenuColoredItem("Clear between 2 point", Color.Green, Color.White), () => UtilsFunctions.DistancePoints.Clear()));
            utilsList.Add(new MenuItem(new UIMenuColoredItem("Meter Mode", Color.Green, Color.White), () => UtilsFunctions.ToggleMeterMode()));
            utilsList.Add(new MenuItem(new UIMenuColoredItem("Spawn Ped", Color.Green, Color.White), UtilsFunctions.SpawnOnePed));
            utilsList.Add(new MenuItem(new UIMenuColoredItem("Teleport To Waypoint", Color.Green, Color.White), UtilsFunctions.TeleportToWaypoint));
            utilsList.Add(new MenuItem(new UIMenuColoredItem("Toggle nearby entity box", Color.Green, Color.White), UtilsFunctions.ToggleNearbyEntityBoundingBox));
            utilsList.Add(new MenuItem(new UIMenuColoredItem("Save Point Coordinates", Color.Green, Color.White), UtilsFunctions.SaveCoordinates));
        }

        public List<MenuItem> GetConfigMenu()
        {
            // Config menu
            saveScreenShotLocally = new UIMenuCheckboxItem("Save ScreenShot Locally", true);
            printBox = new UIMenuCheckboxItem("Print Box", true);

            List<MenuItem> configList = new List<MenuItem>();
            configList.Add(cameraFovListItem);
            configList.Add(cameraAngleListItem);
            configList.Add(cameraFixedHeightListItem);
            configList.Add(imageFormatListItem);
            configList.Add(maxCollectedDataListItem);
            configList.Add(teleportingDelayListItem);
            configList.Add(renderingDelayListItem);
            configList.Add(pedSpawningDelayListItem);
            configList.Add(collectingDataDelayListItem);
            configList.Add(clearCollectingDataDelayListItem);

            configList.Add(new MenuItem(saveScreenShotLocally));//Checkbox
            configList.Add(new MenuItem(printBox));

            return configList;
        }

        public void SaveSettings()
        {
            maxCollectedDataListItem.Save();
            cameraFovListItem.Save();
            cameraFixedHeightListItem.Save();
            imageFormatListItem.Save();
            cameraAngleListItem.Save();
            teleportingDelayListItem.Save();
            renderingDelayListItem.Save();
            pedSpawningDelayListItem.Save();
            collectingDataDelayListItem.Save();
            clearCollectingDataDelayListItem.Save();

            Settings.SaveScreenShotLocally = saveScreenShotLocally.Checked;
            Settings.PrintBox = printBox.Checked;
            Settings.SaveSettings();
            Notification.Show("Settings saved");
        }

    }
}