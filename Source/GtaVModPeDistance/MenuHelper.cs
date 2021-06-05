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
        Weather[] allWeather;
        UIMenuListItem planeList, helicopterList, motorbikeList, boatList, weatherList;
        List<dynamic> listOfPlanes, listOfHelicopter, listOfMotorbike, listOfBoat, listOfWeather;
        
        // Config
        UIMenuListItem maxCollectedDataList, cameraMinSpawningHeightList, cameraMaxSpawningHeightList, imageFormatList;
        UIMenuListItem cameraFixedHeightList, cameraFovList, teleportingDelayList, renderingDelayList, pedSpawningDelayList, collectingDataDelayList, clearCollectingDataDelayList;
        List<dynamic> listOfMaxCollectedData, listOfCameraMinSpawningHeight, listOfCameraMaxSpawningHeight, listOfCameraFixedHeight, listOfCameraFov;
        List<dynamic> listOfTeleportingDelay, listOfRenderingDelay, listOfPedSpawningDelay, listOfCollectingDataDelay, listOfClearCollectingDataDelay, listOfImageFormat;
        UIMenuCheckboxItem saveScreenShotLocally, printBox;


        public List<MenuItem> GetUtilsMenu()
        {
            //utils menu
            List<MenuItem> utilsList = new List<MenuItem>();

            allVehiclesHash = (VehicleHash[])Enum.GetValues(typeof(VehicleHash));

            listOfPlanes = new List<dynamic>();
            listOfHelicopter = new List<dynamic>();
            listOfMotorbike = new List<dynamic>();
            listOfBoat = new List<dynamic>();
            listOfWeather = new List<dynamic>();

            for (int i = 0; i < allVehiclesHash.Length; i++)
            {
                Model tempModel = new Model(allVehiclesHash[i]);

                if (tempModel.IsPlane) listOfPlanes.Add(allVehiclesHash[i]);
                else if (tempModel.IsHelicopter) listOfHelicopter.Add(allVehiclesHash[i]);
                else if (tempModel.IsBike) listOfMotorbike.Add(allVehiclesHash[i]);
                else if (tempModel.IsBoat) listOfBoat.Add(allVehiclesHash[i]);
            }

            allWeather = (Weather[])Enum.GetValues(typeof(Weather));

            for(int i = 0; i < allWeather.Length; i++)
            {
                listOfWeather.Add(allWeather[i]);
            }

            //TODO: find index problem
            planeList = new UIMenuListItem("Planes: ", listOfPlanes, 1);
            helicopterList = new UIMenuListItem("Helicopters: ", listOfHelicopter, 1);
            motorbikeList = new UIMenuListItem("Motorbikes: ", listOfMotorbike, 1);
            boatList = new UIMenuListItem("Boats: ", listOfBoat, 1);
            weatherList = new UIMenuListItem("Weather: ", listOfWeather, 1);

            utilsList.Add(new MenuItem("Delete All Near Ped", UtilsFunctions.DeleteAllNearPed));
            utilsList.Add(new MenuItem("Delete All Near Vehicle", UtilsFunctions.DeleteAllNearVehicles));
            utilsList.Add(new MenuItem("Ignored by everyone", () => { Game.Player.IgnoredByEveryone = true; }));
            utilsList.Add(new MenuItem("Reset Wanted Level", UtilsFunctions.ResetWantedLevel));
            utilsList.Add(new MenuItem("Set Time Midday", () => UtilsFunctions.SetTime(12, 0, 0)));
            utilsList.Add(new MenuItem("Set Time Midnight", () => UtilsFunctions.SetTime(0, 0, 0)));
            utilsList.Add(new MenuItem("Set Time Afternoon", () => UtilsFunctions.SetTime(17, 0, 0)));
            utilsList.Add(new MenuItem("Spawn Ped", UtilsFunctions.SpawnOnePed));
            utilsList.Add(new MenuItem("Spawn Random Point", UtilsFunctions.SpawnAtRandomSavedLocation));
            utilsList.Add(new MenuItem("Teleport To Waypoint", UtilsFunctions.TeleportToWaypoint));
            utilsList.Add(new MenuItem("Toggle nearby entity box", UtilsFunctions.ToggleNearbyEntityBoundingBox));
            utilsList.Add(new MenuItem("Save Point Coordinates", UtilsFunctions.SaveCoordinates));
            utilsList.Add(new MenuItem("Reset", Reset));
            utilsList.Add(new MenuItem(planeList, () => { UtilsFunctions.SpawnVehicle(planeList, listOfPlanes); }));
            utilsList.Add(new MenuItem(helicopterList, () => { UtilsFunctions.SpawnVehicle(helicopterList, listOfHelicopter); }));
            utilsList.Add(new MenuItem(motorbikeList, () => { UtilsFunctions.SpawnVehicle(motorbikeList, listOfMotorbike); }));
            utilsList.Add(new MenuItem(boatList, () => { UtilsFunctions.SpawnVehicle(boatList, listOfBoat); }));
            utilsList.Add(new MenuItem(weatherList, () => { UtilsFunctions.ChangeWeather(weatherList, listOfWeather); }));

            return utilsList;
        }

        public List<MenuItem> GetConfigMenu()
        {
            // Config menu
            listOfMaxCollectedData = new List<dynamic>();
            listOfCameraMinSpawningHeight = new List<dynamic>();
            listOfCameraMaxSpawningHeight = new List<dynamic>();
            listOfCameraFixedHeight = new List<dynamic>();
            listOfTeleportingDelay = new List<dynamic>();
            listOfRenderingDelay = new List<dynamic>();
            listOfPedSpawningDelay = new List<dynamic>();
            listOfCollectingDataDelay = new List<dynamic>();
            listOfClearCollectingDataDelay = new List<dynamic>();
            saveScreenShotLocally = new UIMenuCheckboxItem("Save ScreenShot Locally", true);
            printBox = new UIMenuCheckboxItem("Print Box", true);
            listOfImageFormat = new List<dynamic>();
            listOfCameraFov = new List<dynamic>();

            for (int i = 0; i <= 100; i++)
            {
                listOfCameraMinSpawningHeight.Add(i);
                listOfCameraMaxSpawningHeight.Add(i);
                listOfTeleportingDelay.Add(i);
                listOfRenderingDelay.Add(i);
                listOfPedSpawningDelay.Add(i);
                listOfCollectingDataDelay.Add(i);
                listOfClearCollectingDataDelay.Add(i);
            }

            for(int i = 20; i <= 90; i += 5)
            {
                listOfCameraFov.Add(i);
            }

            listOfMaxCollectedData.Add(5);
            for (int i = 30; i <= Settings.MaxCollectedSelectionable; i += 30)
            {
                listOfMaxCollectedData.Add(i);
            }

            for (float i = 0; i <= 15; i += 0.2f)
            {
                listOfCameraFixedHeight.Add(i);
            }

            listOfImageFormat.Add("Jpeg");
            listOfImageFormat.Add("Png");

            maxCollectedDataList = new UIMenuListItem("Max Collected Data: ", listOfMaxCollectedData, listOfMaxCollectedData.IndexOf(Settings.MaxCollectedData));
            cameraMinSpawningHeightList = new UIMenuListItem("Camera Min Spawning Height (m): ", listOfCameraMinSpawningHeight, listOfCameraMinSpawningHeight.IndexOf(Settings.CameraMinSpawningHeight));
            cameraMaxSpawningHeightList = new UIMenuListItem("Camera Max Spawning Height (m): ", listOfCameraMaxSpawningHeight, listOfCameraMaxSpawningHeight.IndexOf(Settings.CameraMaxSpawningHeight));
            cameraFixedHeightList = new UIMenuListItem("Camera Fixed Height (m): ", listOfCameraFixedHeight, listOfCameraFixedHeight.IndexOf(Settings.CameraFixedHeight));
            cameraFovList = new UIMenuListItem("Camera Fov: ", listOfCameraFov, listOfCameraFov.IndexOf(Settings.CameraFov));
            teleportingDelayList = new UIMenuListItem("Teleporting Delay (s): ", listOfTeleportingDelay, listOfTeleportingDelay.IndexOf(Settings.TeleportingDelay));
            renderingDelayList = new UIMenuListItem("Rendering Delay (s): ", listOfRenderingDelay, listOfRenderingDelay.IndexOf(Settings.RenderingDelay));
            pedSpawningDelayList = new UIMenuListItem("Ped Spawning Delay (s): ", listOfPedSpawningDelay, listOfPedSpawningDelay.IndexOf(Settings.PedSpawningDelay));
            collectingDataDelayList = new UIMenuListItem("Collecting Data Delay (s): ", listOfCollectingDataDelay, listOfCollectingDataDelay.IndexOf(Settings.CollectingDataDelay));
            clearCollectingDataDelayList = new UIMenuListItem("Clear Collecting Data Delay (s): ", listOfClearCollectingDataDelay, listOfClearCollectingDataDelay.IndexOf(Settings.ClearCollectingDataDelay));
            imageFormatList = new UIMenuListItem("Image Format: ", listOfImageFormat, listOfImageFormat.IndexOf(Settings.ImageFormat));


            List<MenuItem> configList = new List<MenuItem>();
            configList.Add(new MenuItem(maxCollectedDataList));
            configList.Add(new MenuItem(cameraMinSpawningHeightList));
            configList.Add(new MenuItem(cameraMaxSpawningHeightList));
            configList.Add(new MenuItem(cameraFixedHeightList));
            configList.Add(new MenuItem(cameraFovList));
            configList.Add(new MenuItem(teleportingDelayList));
            configList.Add(new MenuItem(renderingDelayList));
            configList.Add(new MenuItem(pedSpawningDelayList));
            configList.Add(new MenuItem(collectingDataDelayList));
            configList.Add(new MenuItem(clearCollectingDataDelayList));
            configList.Add(new MenuItem(saveScreenShotLocally));
            configList.Add(new MenuItem(printBox));
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
            Settings.CameraMinSpawningHeight = listOfCameraMinSpawningHeight[cameraMinSpawningHeightList.Index];
            Settings.CameraMaxSpawningHeight = listOfCameraMaxSpawningHeight[cameraMaxSpawningHeightList.Index];
            Settings.CameraFixedHeight = listOfCameraFixedHeight[cameraFixedHeightList.Index];
            Settings.CameraFov = listOfCameraFov[cameraFovList.Index];
            Settings.TeleportingDelay = listOfTeleportingDelay[teleportingDelayList.Index];
            Settings.RenderingDelay = listOfRenderingDelay[renderingDelayList.Index];
            Settings.PedSpawningDelay = listOfPedSpawningDelay[pedSpawningDelayList.Index];
            Settings.CollectingDataDelay = listOfCollectingDataDelay[collectingDataDelayList.Index];
            Settings.ClearCollectingDataDelay = listOfClearCollectingDataDelay[clearCollectingDataDelayList.Index];
            Settings.SaveScreenShotLocally = saveScreenShotLocally.Checked;
            Settings.PrintBox = printBox.Checked;
            Settings.ImageFormat = listOfImageFormat[imageFormatList.Index];
            Settings.SaveSettings();
            Notification.Show("Settings saved");
        }

    }
}