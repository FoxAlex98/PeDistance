using GTA;
using GTA.UI;
using GtaVModPeDistance.CollectingSteps;
using NativeUI;
using System;
using System.Collections.Generic;

namespace GtaVModPeDistance
{
    class MenuHelper
    {
        // Utils
        VehicleHash[] allVehiclesHash;
        UIMenuListItem planeList, helicopterList, motorbikeList, boatList;
        List<dynamic> listOfPlanes, listOfHelicopter, listOfMotorbike, listOfBoat;

        // Config
        UIMenuListItem maxCollectedDataList, pedMinSpawningDistanceYList, pedMaxSpawningDistanceYList, cameraMinSpawningHeightList, cameraMaxSpawningHeightList, imageFormatList;
        UIMenuListItem cameraFixedHeightList, teleportingDelayList, renderingDelayList, pedSpawningDelayList, collectingDataDelayList, clearCollectingDataDelayList, saveScreenShotLocallyList;
        List<dynamic> listOfMaxCollectedData, listOfPedMinSpawningDistanceY, listOfPedMaxSpawningDistanceY, listOfCameraMinSpawningHeight, listOfCameraMaxSpawningHeight, listOfCameraFixedHeight;
        List<dynamic> listOfTeleportingDelay, listOfRenderingDelay, listOfPedSpawningDelay, listOfCollectingDataDelay, listOfClearCollectingDataDelay, listOfSaveScreenShotLocally, listOfImageFormat;

        public List<MenuItem> GetUtilsMenu()
        {
            //utils menu
            List<MenuItem> utilsList = new List<MenuItem>();

            allVehiclesHash = (VehicleHash[])Enum.GetValues(typeof(VehicleHash));
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

            utilsList.Add(new MenuItem("Delete All Near Ped", UtilsFunctions.DeleteAllNearPed));
            utilsList.Add(new MenuItem("Ignored by everyone", () => { Game.Player.IgnoredByEveryone = true; }));
            utilsList.Add(new MenuItem("Reset Wanted Level", UtilsFunctions.ResetWantedLevel));
            utilsList.Add(new MenuItem("Set Time Midday", () => UtilsFunctions.SetTime(12, 0, 0)));
            utilsList.Add(new MenuItem("Set Time Midnight", () => UtilsFunctions.SetTime(0, 0, 0)));
            utilsList.Add(new MenuItem("Set Time Afternoon", () => UtilsFunctions.SetTime(17, 0, 0)));
            utilsList.Add(new MenuItem("Spawn Ped", UtilsFunctions.SpawnOnePed));
            utilsList.Add(new MenuItem("Spawn Random Point", UtilsFunctions.SpawnAtRandomSavedLocation));
            utilsList.Add(new MenuItem("Teleport To Waypoint", UtilsFunctions.TeleportToWaypoint));
            utilsList.Add(new MenuItem("Save Point Coordinates", UtilsFunctions.SaveCoordinates));
            utilsList.Add(new MenuItem("Reset", Reset));
            utilsList.Add(new MenuItem(planeList, () => { UtilsFunctions.SpawnVehicle(planeList, listOfPlanes); }));
            utilsList.Add(new MenuItem(helicopterList, () => { UtilsFunctions.SpawnVehicle(helicopterList, listOfHelicopter); }));
            utilsList.Add(new MenuItem(motorbikeList, () => { UtilsFunctions.SpawnVehicle(motorbikeList, listOfMotorbike); }));
            utilsList.Add(new MenuItem(boatList, () => { UtilsFunctions.SpawnVehicle(boatList, listOfBoat); }));

            return utilsList;
        }

        public List<MenuItem> GetConfigMenu()
        {
            // Config menu
            listOfMaxCollectedData = new List<dynamic>();
            listOfPedMinSpawningDistanceY = new List<dynamic>();
            listOfPedMaxSpawningDistanceY = new List<dynamic>();
            listOfCameraMinSpawningHeight = new List<dynamic>();
            listOfCameraMaxSpawningHeight = new List<dynamic>();
            listOfCameraFixedHeight = new List<dynamic>();
            listOfTeleportingDelay = new List<dynamic>();
            listOfRenderingDelay = new List<dynamic>();
            listOfPedSpawningDelay = new List<dynamic>();
            listOfCollectingDataDelay = new List<dynamic>();
            listOfClearCollectingDataDelay = new List<dynamic>();
            listOfSaveScreenShotLocally = new List<dynamic>();
            listOfImageFormat = new List<dynamic>();

            for (int i = 0; i <= 100; i++)
            {
                if (i >= 2)
                {
                    listOfPedMinSpawningDistanceY.Add(i);
                    listOfPedMaxSpawningDistanceY.Add(i);
                }
                listOfCameraMinSpawningHeight.Add(i);
                listOfCameraMaxSpawningHeight.Add(i);
                listOfTeleportingDelay.Add(i);
                listOfRenderingDelay.Add(i);
                listOfPedSpawningDelay.Add(i);
                listOfCollectingDataDelay.Add(i);
                listOfClearCollectingDataDelay.Add(i);
            }

            for (int i = 0; i <= Settings.MaxCollectedSelectionable; i += 30)
            {
                listOfMaxCollectedData.Add(i);
            }

            for (float i = 0; i <= 15; i += 0.2f)
            {
                listOfCameraFixedHeight.Add(i);
            }

            listOfSaveScreenShotLocally.Add("Yes");
            listOfSaveScreenShotLocally.Add("No");

            listOfImageFormat.Add("Jpeg");
            listOfImageFormat.Add("Png");

            maxCollectedDataList = new UIMenuListItem("Max Collected Data: ", listOfMaxCollectedData, listOfMaxCollectedData.IndexOf(Settings.MaxCollectedData));
            pedMinSpawningDistanceYList = new UIMenuListItem("Ped Min Spawning Distance Y: ", listOfPedMinSpawningDistanceY, listOfPedMinSpawningDistanceY.IndexOf(Settings.PedMinSpawningDistanceY));
            pedMaxSpawningDistanceYList = new UIMenuListItem("Ped Max Spawning Distance Y: ", listOfPedMaxSpawningDistanceY, listOfPedMaxSpawningDistanceY.IndexOf(Settings.PedMaxSpawningDistanceY));
            cameraMinSpawningHeightList = new UIMenuListItem("Camera Min Spawning Height: ", listOfCameraMinSpawningHeight, listOfCameraMinSpawningHeight.IndexOf(Settings.CameraMinSpawningHeight));
            cameraMaxSpawningHeightList = new UIMenuListItem("Camera Max Spawning Height: ", listOfCameraMaxSpawningHeight, listOfCameraMaxSpawningHeight.IndexOf(Settings.CameraMaxSpawningHeight));
            cameraFixedHeightList = new UIMenuListItem("Camera Fixed Height: ", listOfCameraFixedHeight, listOfCameraFixedHeight.IndexOf(Settings.CameraFixedHeight));
            teleportingDelayList = new UIMenuListItem("Teleporting Delay: ", listOfTeleportingDelay, listOfTeleportingDelay.IndexOf(Settings.TeleportingDelay));
            renderingDelayList = new UIMenuListItem("Rendering Delay: ", listOfRenderingDelay, listOfRenderingDelay.IndexOf(Settings.RenderingDelay));
            pedSpawningDelayList = new UIMenuListItem("Ped Spawning Delay: ", listOfPedSpawningDelay, listOfPedSpawningDelay.IndexOf(Settings.PedSpawningDelay));
            collectingDataDelayList = new UIMenuListItem("Collecting Data Delay: ", listOfCollectingDataDelay, listOfCollectingDataDelay.IndexOf(Settings.CollectingDataDelay));
            clearCollectingDataDelayList = new UIMenuListItem("Clear Collecting Data Delay: ", listOfClearCollectingDataDelay, listOfClearCollectingDataDelay.IndexOf(Settings.ClearCollectingDataDelay));
            saveScreenShotLocallyList = new UIMenuListItem("Save ScreenShot Locally: ", listOfSaveScreenShotLocally, listOfSaveScreenShotLocally.IndexOf(Settings.SaveScreenShotLocally));
            imageFormatList = new UIMenuListItem("Save ScreenShot Locally: ", listOfImageFormat, listOfImageFormat.IndexOf(Settings.ImageFormat));

            List<MenuItem> configList = new List<MenuItem>();
            configList.Add(new MenuItem(maxCollectedDataList));
            configList.Add(new MenuItem(pedMinSpawningDistanceYList));
            configList.Add(new MenuItem(pedMaxSpawningDistanceYList));
            configList.Add(new MenuItem(cameraMinSpawningHeightList));
            configList.Add(new MenuItem(cameraMaxSpawningHeightList));
            configList.Add(new MenuItem(cameraFixedHeightList));
            configList.Add(new MenuItem(teleportingDelayList));
            configList.Add(new MenuItem(renderingDelayList));
            configList.Add(new MenuItem(pedSpawningDelayList));
            configList.Add(new MenuItem(collectingDataDelayList));
            configList.Add(new MenuItem(clearCollectingDataDelayList));
            configList.Add(new MenuItem(saveScreenShotLocallyList));
            configList.Add(new MenuItem(imageFormatList));
            configList.Add(new MenuItem("Save", SaveSettings));

            return configList;
        }

        public void Reset()
        {
            Game.Player.Character.IsVisible = true;
            Globals.ShowHud();
            World.RenderingCamera = null;
            CollectingState.StartCollectingData = false;
            CollectingState.ActualStep = new TeleportToRandomSavedLocationStep();
            CollectingState.CollectedDataCounter = 0;
            CollectingState.Ped = null;
        }

        private void SaveSettings()
        {
            Settings.MaxCollectedData = listOfMaxCollectedData[maxCollectedDataList.Index];
            Settings.PedMinSpawningDistanceY = listOfPedMinSpawningDistanceY[pedMinSpawningDistanceYList.Index];
            Settings.PedMaxSpawningDistanceY = listOfPedMaxSpawningDistanceY[pedMaxSpawningDistanceYList.Index];
            Settings.CameraMinSpawningHeight = listOfCameraMinSpawningHeight[cameraMinSpawningHeightList.Index];
            Settings.CameraMaxSpawningHeight = listOfCameraMaxSpawningHeight[cameraMaxSpawningHeightList.Index];
            Settings.CameraFixedHeight = listOfCameraFixedHeight[cameraFixedHeightList.Index];
            Settings.TeleportingDelay = listOfTeleportingDelay[teleportingDelayList.Index];
            Settings.RenderingDelay = listOfRenderingDelay[renderingDelayList.Index];
            Settings.PedSpawningDelay = listOfPedSpawningDelay[pedSpawningDelayList.Index];
            Settings.CollectingDataDelay = listOfCollectingDataDelay[collectingDataDelayList.Index];
            Settings.ClearCollectingDataDelay = listOfClearCollectingDataDelay[clearCollectingDataDelayList.Index];
            Settings.SaveScreenShotLocally = listOfSaveScreenShotLocally[saveScreenShotLocallyList.Index];
            Settings.ImageFormat = listOfImageFormat[imageFormatList.Index];
            Settings.SaveSettings();
            Notification.Show("Settings saved");
        }

    }
}