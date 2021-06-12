using GTA;
using GTA.Math;
using GTA.UI;
using GtaVModPeDistance.CollectingSteps;
using NativeUI;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace GtaVModPeDistance
{
    class MenuHelper
    {
        // Utils
        VehicleHash[] allVehiclesHash;
        Weather[] allWeather;
        UIMenuListItem planeList, helicopterList, motorbikeList, boatList, weatherList;
        List<dynamic> listOfPlanes, listOfHelicopter, listOfMotorbike, listOfBoat, listOfWeather;
        public UIMenuCheckboxItem DebugMode;

        // Config
        UIMenuListItem maxCollectedDataList, imageFormatList;
        UIMenuListItem cameraFixedHeightList, cameraFovList, cameraAngleList, teleportingDelayList, renderingDelayList, pedSpawningDelayList, collectingDataDelayList, clearCollectingDataDelayList;
        List<dynamic> listOfMaxCollectedData, listOfCameraFixedHeight, listOfCameraFov, listOfCameraAngle;
        List<dynamic> listOfTeleportingDelay, listOfRenderingDelay, listOfPedSpawningDelay, listOfCollectingDataDelay, listOfClearCollectingDataDelay, listOfImageFormat;
        UIMenuCheckboxItem saveScreenShotLocally, printBox;

        public MenuHelper()
        {
            DebugMode = new UIMenuCheckboxItem("Debug Mode", false);
        }

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

            for (int i = 0; i < allWeather.Length; i++)
            {
                listOfWeather.Add(allWeather[i]);
            }

            //TODO: find index problem
            planeList = new UIMenuListItem("Planes: ", listOfPlanes, 1);
            helicopterList = new UIMenuListItem("Helicopters: ", listOfHelicopter, 1);
            motorbikeList = new UIMenuListItem("Motorbikes: ", listOfMotorbike, 1);
            boatList = new UIMenuListItem("Boats: ", listOfBoat, 1);
            weatherList = new UIMenuListItem("Weather: ", listOfWeather, 1);

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
            utilsList.Add(new MenuItem(weatherList, () => { UtilsFunctions.ChangeWeather(weatherList, listOfWeather); }));

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
            listOfMaxCollectedData = new List<dynamic>();
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
            listOfCameraAngle = new List<dynamic>();

            for (int i = 0; i <= 100; i++)
            {
                listOfTeleportingDelay.Add(i);
                listOfRenderingDelay.Add(i);
                listOfPedSpawningDelay.Add(i);
                listOfCollectingDataDelay.Add(i);
                listOfClearCollectingDataDelay.Add(i);
            }

            for(int i = -90; i <= 90; i++)
            {
                listOfCameraAngle.Add(i);
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

            for (float i = 0; i <= 15; i += 0.1f)
            {
                listOfCameraFixedHeight.Add((float) Math.Round(i, 1));
            }

            listOfImageFormat.Add("Jpeg");
            listOfImageFormat.Add("Png");

            maxCollectedDataList = new UIMenuListItem("Max Collected Data: ", listOfMaxCollectedData, listOfMaxCollectedData.IndexOf(Settings.MaxCollectedData));
            cameraFixedHeightList = new UIMenuListItem("Camera Fixed Height (m): ", listOfCameraFixedHeight, listOfCameraFixedHeight.IndexOf(Settings.CameraFixedHeight));
            cameraFovList = new UIMenuListItem("Camera Fov: ", listOfCameraFov, listOfCameraFov.IndexOf(Settings.CameraFov));
            cameraAngleList = new UIMenuListItem("Camera Angle: ", listOfCameraAngle, listOfCameraAngle.IndexOf(Settings.CameraAngle));
            teleportingDelayList = new UIMenuListItem("Teleporting Delay (s): ", listOfTeleportingDelay, listOfTeleportingDelay.IndexOf(Settings.TeleportingDelay));
            renderingDelayList = new UIMenuListItem("Rendering Delay (s): ", listOfRenderingDelay, listOfRenderingDelay.IndexOf(Settings.RenderingDelay));
            pedSpawningDelayList = new UIMenuListItem("Ped Spawning Delay (s): ", listOfPedSpawningDelay, listOfPedSpawningDelay.IndexOf(Settings.PedSpawningDelay));
            collectingDataDelayList = new UIMenuListItem("Collecting Data Delay (s): ", listOfCollectingDataDelay, listOfCollectingDataDelay.IndexOf(Settings.CollectingDataDelay));
            clearCollectingDataDelayList = new UIMenuListItem("Clear Collecting Data Delay (s): ", listOfClearCollectingDataDelay, listOfClearCollectingDataDelay.IndexOf(Settings.ClearCollectingDataDelay));
            imageFormatList = new UIMenuListItem("Image Format: ", listOfImageFormat, listOfImageFormat.IndexOf(Settings.ImageFormat));


            List<MenuItem> configList = new List<MenuItem>();
            configList.Add(new MenuItem(maxCollectedDataList));
            configList.Add(new MenuItem(cameraFixedHeightList));
            configList.Add(new MenuItem(cameraFovList));
            configList.Add(new MenuItem(cameraAngleList));
            configList.Add(new MenuItem(teleportingDelayList));
            configList.Add(new MenuItem(renderingDelayList));
            configList.Add(new MenuItem(pedSpawningDelayList));
            configList.Add(new MenuItem(collectingDataDelayList));
            configList.Add(new MenuItem(clearCollectingDataDelayList));
            configList.Add(new MenuItem(saveScreenShotLocally));
            configList.Add(new MenuItem(printBox));
            configList.Add(new MenuItem(imageFormatList));

            cameraFovList.OnListChanged += CameraFovList_OnListChanged;
            cameraAngleList.OnListChanged += CameraAngleList_OnListChanged;
            cameraFixedHeightList.OnListChanged += CameraFixedHeightList_OnListChanged;
            

            return configList;
        }

        private void CameraAngleList_OnListChanged(UIMenuListItem sender, int newIndex)
        {
            Vector3 cameraRot = World.RenderingCamera.Rotation;
            World.RenderingCamera.Rotation = new Vector3(listOfCameraAngle[newIndex], cameraRot.Y, cameraRot.Z);
        }

        private void CameraFixedHeightList_OnListChanged(UIMenuListItem sender, int newIndex)
        {
            float Z = (Game.Player.Character.Position.Z - 1) + listOfCameraFixedHeight[newIndex];
            Vector3 cameraPos = World.RenderingCamera.Position;
            World.RenderingCamera.Position = new Vector3(cameraPos.X, cameraPos.Y, Z);
        }

        private void CameraFovList_OnListChanged(UIMenuListItem sender, int newIndex)
        {
            World.RenderingCamera.FieldOfView = listOfCameraFov[newIndex];
            UtilsFunctions.SpawnSettingPeds();
        }

        public void SaveSettings()
        {
            Settings.MaxCollectedData = listOfMaxCollectedData[maxCollectedDataList.Index];
            Settings.CameraFixedHeight = listOfCameraFixedHeight[cameraFixedHeightList.Index];
            Settings.CameraFov = listOfCameraFov[cameraFovList.Index];
            Settings.CameraAngle = listOfCameraAngle[cameraAngleList.Index];
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