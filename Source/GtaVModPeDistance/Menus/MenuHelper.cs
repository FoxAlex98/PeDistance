using GTA;
using GTA.Math;
using GTA.UI;
using NativeUI;
using System;
using GtaVModPeDistance.Menus.Impl;
using System.Collections.Generic;
using System.Drawing;

namespace GtaVModPeDistance.Menus
{
    class MenuHelper
    {
        // Utils
        UIMenuListItem planeList, helicopterList, motorbikeList, boatList;
        List<dynamic> listOfPlanes, listOfHelicopter, listOfMotorbike, listOfBoat;
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

            VehicleHash[] allVehiclesHash = (VehicleHash[])Enum.GetValues(typeof(VehicleHash));

            listOfPlanes = new List<dynamic>();
            listOfHelicopter = new List<dynamic>();
            listOfMotorbike = new List<dynamic>();
            listOfBoat = new List<dynamic>();

            for (int i = 0; i < allVehiclesHash.Length; i++)
            {
                Model tempModel = new Model(allVehiclesHash[i]);

                if (tempModel.IsPlane) listOfPlanes.Add(allVehiclesHash[i]);
                else if (tempModel.IsHelicopter) listOfHelicopter.Add(allVehiclesHash[i]);
                else if (tempModel.IsBike) listOfMotorbike.Add(allVehiclesHash[i]);
                else if (tempModel.IsBoat) listOfBoat.Add(allVehiclesHash[i]);
            }

            //TODO: find index problem
            planeList = new UIMenuListItem("Planes: ", listOfPlanes, 1);
            helicopterList = new UIMenuListItem("Helicopters: ", listOfHelicopter, 1);
            motorbikeList = new UIMenuListItem("Motorbikes: ", listOfMotorbike, 1);
            boatList = new UIMenuListItem("Boats: ", listOfBoat, 1);

            DebugMode.CheckboxEvent += UtilsFunctions.ToggleDebugMode;
            utilsList.Add(new MenuItem(DebugMode));
            if (DebugMode.Checked)
                AddDebugItemList(utilsList);
            utilsList.Add(new MenuItem("Delete All Near Ped", UtilsFunctions.DeleteAllNearPed));
            utilsList.Add(new MenuItem("Delete All Near Vehicle", UtilsFunctions.DeleteAllNearVehicles));
            utilsList.Add(new MenuItem("Ignored by everyone", () => { Game.Player.IgnoredByEveryone = true; }));
            utilsList.Add(new MenuItem("Reset Wanted Level", UtilsFunctions.ResetWantedLevel));
            utilsList.Add(new MenuItem("Set Time Midday", () => UtilsFunctions.SetTime(12, 0, 0)));
            utilsList.Add(new MenuItem("Set Time Midnight", () => UtilsFunctions.SetTime(0, 0, 0)));
            utilsList.Add(new MenuItem("Set Time Afternoon", () => UtilsFunctions.SetTime(17, 0, 0)));
            utilsList.Add(new MenuItem("Spawn Random Point", UtilsFunctions.SpawnAtRandomSavedLocation));
            utilsList.Add(new MenuItem(planeList, () => { UtilsFunctions.SpawnVehicle(planeList, listOfPlanes); }));
            utilsList.Add(new MenuItem(helicopterList, () => { UtilsFunctions.SpawnVehicle(helicopterList, listOfHelicopter); }));
            utilsList.Add(new MenuItem(motorbikeList, () => { UtilsFunctions.SpawnVehicle(motorbikeList, listOfMotorbike); }));
            utilsList.Add(new MenuItem(boatList, () => { UtilsFunctions.SpawnVehicle(boatList, listOfBoat); }));
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

            //Checkbox
            configList.Add(new MenuItem(saveScreenShotLocally));
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