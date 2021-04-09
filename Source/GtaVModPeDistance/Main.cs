using System;
using System.Windows.Forms;
using System.Collections.Generic;
using GTA;
using GTA.UI;
using GTA.Math;
using GTA.Native;
using NativeUI;
using GtaVModPeDistance.Models;
using GtaVModPeDistance.File;

namespace GtaVModPeDistance
{
    public delegate void ActionToDo();

    public class Main : Script
    {
        string ModName = "Tostino-ML";
        string Developer = "Danilo-AleS-AleC";
        string Version = "1.0";
        bool reloadMenu = false;

        MenuPool modMenuPool;
        Menu mainMenu;

        Random rand = new Random();

        // Managers
        LocationManager location;
        ScreenShotManager screenShot;
        DataMananger dataManager;

        // Utils
        Vehicle vehicle = null;
        VehicleHash[] allVehiclesHash;
        UIMenuListItem planeList, helicopterList, motorbikeList, boatList;
        List<dynamic> listOfPlanes, listOfHelicopter, listOfMotorbike, listOfBoat;

        // Config
        UIMenuListItem maxCollectedDataList, pedMinSpawningDistanceList, pedMaxSpawningDistanceList, cameraMinSpawningHeightList, cameraMaxSpawningHeightList;
        UIMenuListItem cameraFixedHeightList, teleportingDelayList, renderingDelayList, pedSpawningDelayList, collectingDataDelayList, clearCollectingDataDelayList, saveScreenShotLocallyList;
        List<dynamic> listOfMaxCollectedData, listOfPedMinSpawningDistance, listOfPedMaxSpawningDistance, listOfCameraMinSpawningHeight, listOfCameraMaxSpawningHeight, listOfCameraFixedHeight;
        List<dynamic> listOfTeleportingDelay, listOfRenderingDelay, listOfPedSpawningDelay, listOfCollectingDataDelay, listOfClearCollectingDataDelay, listOfSaveScreenShotLocally;

        // Collecting Data variables
        Vector3 initialPosition;
        SpawnPoint spawnPoint;
        bool startCollectingData = false;
        bool wannaStop = false;
        float start = 0;
        int collectingStep = 0;
        int collectedDataCounter = 0;
        Ped ped;

        public Main()
        {
            GtaVModPeDistance.Settings.LoadSettings();

            location = new LocationManager();
            screenShot = new ScreenShotManager();
            dataManager = new DataMananger();

            Notification.Show("~o~" + ModName + " " + Version + " by ~o~" + Developer + " Loaded");
            MenuSetup();
            Tick += onTick;
            KeyDown += onKeyDown;
        }

        private void onTick(object sender, EventArgs e)
        {
            //if you don't do that, the menu wont show up            
            if(modMenuPool != null || reloadMenu)
            {
                modMenuPool.ProcessMenus();
                reloadMenu = false;
            }

            #region Collecting Data region

            if(startCollectingData)
            {
                switch(collectingStep)
                {
                    case 0:
                        {
                            if (Game.GameTime > start + (GtaVModPeDistance.Settings.TeleportingDelay * 1000))
                            {
                                SpawnRandomPoint();
                                start = Game.GameTime;
                                collectingStep++;
                            }
                            break;
                        }                                                  
                    case 1:
                        {
                            if (Game.GameTime > start + (GtaVModPeDistance.Settings.RenderingDelay * 1000))
                            {
                                InitCollettingDataOperations();
                                start = Game.GameTime;
                                collectingStep++;
                            }
                            break;
                        }
                    case 2:
                        {
                            if (Game.GameTime > start + (GtaVModPeDistance.Settings.PedSpawningDelay * 1000))
                            {
                                SpawnPed();
                                start = Game.GameTime;
                                collectingStep++;
                            }
                            break;
                        }
                    case 3:
                        {
                            if (Game.GameTime > start + (GtaVModPeDistance.Settings.CollectingDataDelay * 1000))
                            {
                                CollectingData();
                                start = Game.GameTime;
                                collectingStep++;
                            }
                            break;
                        }
                    case 4:
                        {
                            if (Game.GameTime > start + (GtaVModPeDistance.Settings.ClearCollectingDataDelay * 1000))
                            {
                                ClearCollettingDataSettings();
                                start = Game.GameTime;
                                collectingStep++;
                            }
                            break;
                        }
                    case 5:
                        {
                            if (Game.GameTime > start)
                            {
                                if (collectedDataCounter % 5 == 0) dataManager.WriteDataToFile();
                                if (collectedDataCounter >= Properties.Settings.Default.MaxCollectedData || wannaStop)
                                {
                                    EndingCollectingData();
                                } else
                                {
                                    start = Game.GameTime;
                                    collectingStep = 0;
                                    Notification.Show("Counter: " + collectedDataCounter + "/" + Properties.Settings.Default.MaxCollectedData);
                                }                               
                            }
                            break;
                        }
                    default:
                        break;
                }
                 
            }

            #endregion
        }

        private void onKeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.F5 && !modMenuPool.IsAnyMenuOpen())
            {
                mainMenu.ToggleMenu();
            }
        }

        private void MenuSetup()
        {
            modMenuPool = new MenuPool();

            // Main menu
            List<MenuItem> mainMenuItem = new List<MenuItem>();
            mainMenuItem.Add(new MenuItem("Start Collecting Data", StartCollectingData));
            mainMenuItem.Add(new MenuItem("Stop Collecting Data", StopCollectingData));
           // mainMenuItem.Add(new MenuItem("Advanced Mode", ActivateAdvancedMode));

            mainMenu = new Menu("Tostino Menu", "SELECT AN OPTION", mainMenuItem);
            modMenuPool.Add(mainMenu.ModMenu);

            //utils menu
            // if (GtaVModPeDistance.Settings.AdvancedMode)
            if(true)
            {
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
                                     
                utilsList.Add(new MenuItem("Delete All Near Ped", DeleteAllNearPed));
                utilsList.Add(new MenuItem("Ignored by everyone", () => { Game.Player.IgnoredByEveryone = true; }));
                utilsList.Add(new MenuItem("Reset Wanted Level", ResetWantedLevel));
                utilsList.Add(new MenuItem("Set Time Midday", () => SetTime(12, 0, 0)));
                utilsList.Add(new MenuItem("Set Time Midnight", () => SetTime(0, 0, 0)));
                utilsList.Add(new MenuItem("Set Time Afternoon", () => SetTime(17, 0, 0)));
                utilsList.Add(new MenuItem("Spawn Ped", SpawnOnePed));
                utilsList.Add(new MenuItem("Spawn Random Point", SpawnRandomPoint));
                utilsList.Add(new MenuItem("Teleport To Waypoint", TeleportToWaypoint));
                utilsList.Add(new MenuItem(planeList, () => { SpawnVehicle(planeList, listOfPlanes); }));
                utilsList.Add(new MenuItem(helicopterList, () => { SpawnVehicle(helicopterList, listOfHelicopter); }));
                utilsList.Add(new MenuItem(motorbikeList, () => { SpawnVehicle(motorbikeList, listOfMotorbike); }));
                utilsList.Add(new MenuItem(boatList, () => { SpawnVehicle(boatList, listOfBoat); }));

                UIMenu uiUtilsMenu = modMenuPool.AddSubMenu(mainMenu.ModMenu, "Utils Menu");
                new Menu(uiUtilsMenu, utilsList);
            }

            // Config menu
            listOfMaxCollectedData = new List<dynamic>();
            listOfPedMinSpawningDistance = new List<dynamic>();
            listOfPedMaxSpawningDistance = new List<dynamic>();
            listOfCameraMinSpawningHeight = new List<dynamic>();
            listOfCameraMaxSpawningHeight = new List<dynamic>();
            listOfCameraFixedHeight = new List<dynamic>();
            listOfTeleportingDelay = new List<dynamic>();
            listOfRenderingDelay = new List<dynamic>();
            listOfPedSpawningDelay = new List<dynamic>();
            listOfCollectingDataDelay = new List<dynamic>();
            listOfClearCollectingDataDelay = new List<dynamic>();
            listOfSaveScreenShotLocally = new List<dynamic>();

            for (int i = 0; i <= 15; i++)
            {
                if(i >= 2)
                {
                    listOfPedMinSpawningDistance.Add(i);
                    listOfPedMaxSpawningDistance.Add(i);                    
                }
                listOfCameraMinSpawningHeight.Add(i);
                listOfCameraMaxSpawningHeight.Add(i);
                listOfTeleportingDelay.Add(i);
                listOfRenderingDelay.Add(i);
                listOfPedSpawningDelay.Add(i);
                listOfCollectingDataDelay.Add(i);
                listOfClearCollectingDataDelay.Add(i);               
            }

            for (int i = 0; i <= GtaVModPeDistance.Settings.MaxCollectedSelectionable; i+=30)
            {
                listOfMaxCollectedData.Add(i);
            }
            
            for (float i = 0; i <= 15; i+=0.2f)
            {
                listOfCameraFixedHeight.Add(i);
            }

            listOfSaveScreenShotLocally.Add("Yes");
            listOfSaveScreenShotLocally.Add("No");

            maxCollectedDataList = new UIMenuListItem("Max Collected Data: ", listOfMaxCollectedData, listOfMaxCollectedData.IndexOf(GtaVModPeDistance.Settings.MaxCollectedData));
            pedMinSpawningDistanceList = new UIMenuListItem("Ped Min Spawning Distance: ", listOfPedMinSpawningDistance, listOfPedMinSpawningDistance.IndexOf(GtaVModPeDistance.Settings.PedMinSpawningDistance));
            pedMaxSpawningDistanceList = new UIMenuListItem("Ped Max Spawning Distance: ", listOfPedMaxSpawningDistance, listOfPedMaxSpawningDistance.IndexOf(GtaVModPeDistance.Settings.PedMaxSpawningDistance));
            cameraMinSpawningHeightList = new UIMenuListItem("Camera Min Spawning Height: ", listOfCameraMinSpawningHeight, listOfCameraMinSpawningHeight.IndexOf(GtaVModPeDistance.Settings.CameraMinSpawningHeight));
            cameraMaxSpawningHeightList = new UIMenuListItem("Camera Max Spawning Height: ", listOfCameraMaxSpawningHeight, listOfCameraMaxSpawningHeight.IndexOf(GtaVModPeDistance.Settings.CameraMaxSpawningHeight));
            cameraFixedHeightList = new UIMenuListItem("Camera Fixed Height: ", listOfCameraFixedHeight, listOfCameraFixedHeight.IndexOf(GtaVModPeDistance.Settings.CameraFixedHeight));
            teleportingDelayList = new UIMenuListItem("Teleporting Delay: ", listOfTeleportingDelay, listOfTeleportingDelay.IndexOf(GtaVModPeDistance.Settings.TeleportingDelay));
            renderingDelayList = new UIMenuListItem("Rendering Delay: ", listOfRenderingDelay, listOfRenderingDelay.IndexOf(GtaVModPeDistance.Settings.RenderingDelay));
            pedSpawningDelayList = new UIMenuListItem("Ped Spawning Delay: ", listOfPedSpawningDelay, listOfPedSpawningDelay.IndexOf(GtaVModPeDistance.Settings.PedSpawningDelay));
            collectingDataDelayList = new UIMenuListItem("Collecting Data Delay: ", listOfCollectingDataDelay, listOfCollectingDataDelay.IndexOf(GtaVModPeDistance.Settings.CollectingDataDelay));
            clearCollectingDataDelayList = new UIMenuListItem("Clear Collecting Data Delay: ", listOfClearCollectingDataDelay, listOfClearCollectingDataDelay.IndexOf(GtaVModPeDistance.Settings.ClearCollectingDataDelay));
            saveScreenShotLocallyList = new UIMenuListItem("Save ScreenShot Locally: ", listOfSaveScreenShotLocally, listOfSaveScreenShotLocally.IndexOf(GtaVModPeDistance.Settings.SaveScreenShotLocally));

            List<MenuItem> configList = new List<MenuItem>();
            configList.Add(new MenuItem(maxCollectedDataList));
            configList.Add(new MenuItem(pedMinSpawningDistanceList));
            configList.Add(new MenuItem(pedMaxSpawningDistanceList));
            configList.Add(new MenuItem(cameraMinSpawningHeightList));
            configList.Add(new MenuItem(cameraMaxSpawningHeightList));
            configList.Add(new MenuItem(cameraFixedHeightList));
            configList.Add(new MenuItem(teleportingDelayList));
            configList.Add(new MenuItem(renderingDelayList));
            configList.Add(new MenuItem(pedSpawningDelayList));
            configList.Add(new MenuItem(collectingDataDelayList));
            configList.Add(new MenuItem(clearCollectingDataDelayList));
            configList.Add(new MenuItem(saveScreenShotLocallyList));
            configList.Add(new MenuItem("Save", SaveSettings));

            UIMenu uiMlMenu = modMenuPool.AddSubMenu(mainMenu.ModMenu, "Settings Menu");
            new Menu(uiMlMenu, configList);

            // Spawn Points menu
            List<MenuItem> spawnPointMenuList = new List<MenuItem>();
            spawnPointMenuList.Add(new MenuItem("Close File", location.CloseLocationFile));
            spawnPointMenuList.Add(new MenuItem("Delete Last Saved Coord", location.DeleteLastCoordinate));
            spawnPointMenuList.Add(new MenuItem("Save Coordinates", SaveCoordinates));
            
            UIMenu uiFileMenu = modMenuPool.AddSubMenu(mainMenu.ModMenu, "Spawn Points Menu");
            new Menu(uiFileMenu, spawnPointMenuList);
        }

        #region Collecting Data Functions
        public void StartCollectingData()
        {
            initialPosition = Game.Player.Character.Position;
            Notification.Show("Starting collecting data... Remove menu using ESC");           
            Game.Player.Character.IsVisible = false;
            HideOrShowHud(false);

            start = Game.GameTime;
            startCollectingData = true;
            wannaStop = false;
            collectingStep = 0;
            collectedDataCounter = 0;            
            ped = null;
        }

        public void StopCollectingData()
        {
            Notification.Show("Stop collecting data...");
            wannaStop = true;
        }

        public void EndingCollectingData()
        {
            Notification.Show("Stop collecting data...");
            Game.Player.Character.IsVisible = true;
            Game.Player.Character.Position = initialPosition;
            HideOrShowHud(true);
            World.RenderingCamera = null;
            start = Game.GameTime;
            startCollectingData = false;
            collectingStep = 0;
            collectedDataCounter = 0;
            ped = null;
            dataManager.WriteDataToFile();
        }

        private void InitCollettingDataOperations()
        {
            DeleteAllNearPed();
            DeleteAllNearVehicles();                
        }

        private void ClearCollettingDataSettings()
        {
            World.RenderingCamera.Delete();
            DeleteAllNearPed();
        }

        public void HideOrShowHud(bool hideOrShow) 
        {
            Function.Call(Hash.DISPLAY_RADAR, hideOrShow);
        }

        private void SpawnRandomPoint()
        {            
            spawnPoint = location.GetRandomPoint();
            Game.Player.Character.Position = spawnPoint.Position;
            float Z = 0;
            if (GtaVModPeDistance.Settings.CameraFixedHeight == 0)
            {
                // TODO sistemare l'orientamento della verso il basso della camera
                Z = (spawnPoint.Position.Z - World.GetGroundHeight(Game.Player.Character.Position)) + rand.Next(GtaVModPeDistance.Settings.CameraMinSpawningHeight, GtaVModPeDistance.Settings.CameraMaxSpawningHeight);
            } else
            {
                Z = (spawnPoint.Position.Z - World.GetGroundHeight(Game.Player.Character.Position)) + GtaVModPeDistance.Settings.CameraFixedHeight;
            }
            Camera camera = World.CreateCamera(new Vector3(spawnPoint.Position.X, spawnPoint.Position.Y, Z), spawnPoint.Rotation, 60);
            World.RenderingCamera = camera;
            // Notification.Show("Camera has been ~b~generated to ~o~" + spawnPoint.StreetName.ToString() + ", " + spawnPoint.ZoneLocalizedName.ToString());
        }

        private void CollectingData()
        {            
            ScreenShot image = screenShot.TakeScreenshot(spawnPoint);

            Data data = new Data(
                    collectedDataCounter++,
                    GetDistance(ped.Position, World.RenderingCamera.Position),
                    getEntityHeight(),
                    ped.Rotation.Z,
                    World.RenderingCamera.Position.Z - World.GetGroundHeight(Game.Player.Character.Position),
                    image.Name,
                    image.b64String,
                    World.CurrentTimeOfDay.ToString()
                );

            dataManager.addElement(data);
        }
       
        private float getEntityHeight()
        {
            float max = ped.Bones[0].Position.Z;
            foreach(PedBone pedBone in ped.Bones)
            {
                max = pedBone.Position.Z > max ? pedBone.Position.Z : max;
            }
            return max - World.GetGroundHeight(ped.Position);
        }

        private double GetDistance(Vector3 pedPosition, Vector3 cameraPosition)
        {
            return Math.Sqrt(Math.Pow(pedPosition.X - cameraPosition.X, 2) + Math.Pow(pedPosition.Y - cameraPosition.Y, 2));
        }

        private void SpawnPed()
        {
            float x, y;
            do
            {
                x = rand.Next(-4, 4);
                y = rand.Next(GtaVModPeDistance.Settings.PedMinSpawningDistance, GtaVModPeDistance.Settings.PedMaxSpawningDistance);
            } while (Math.Abs(x) > y);
            ped = World.CreateRandomPed(World.RenderingCamera.GetOffsetPosition(new Vector3(x, y, 0)));
            ped.Heading = rand.Next(360);
        }

        private void DeleteAllNearPed()
        {
            Ped[] pedArray = World.GetAllPeds();
            foreach (Ped ped in pedArray)
            {
                ped.Delete();
            }
        }

        private void DeleteAllNearVehicles()
        {
            Vehicle[] vehicleArray = World.GetAllVehicles();
            foreach (Vehicle vc in vehicleArray)
            {
                vc.Delete();
            }
        }

        #endregion

        #region Utils
        private void ActivateAdvancedMode()
        {
            GtaVModPeDistance.Settings.ToggleAdvancedMode();
            MenuSetup();
            reloadMenu = true;
            if (GtaVModPeDistance.Settings.AdvancedMode)
                Notification.Show("Advanced Mode activated!");
            else
                Notification.Show("Advanced Mode deactivated!");
        }
        private void SaveSettings()
        {
            GtaVModPeDistance.Settings.MaxCollectedData = listOfMaxCollectedData[maxCollectedDataList.Index];
            GtaVModPeDistance.Settings.PedMinSpawningDistance = listOfPedMinSpawningDistance[pedMinSpawningDistanceList.Index];
            GtaVModPeDistance.Settings.PedMaxSpawningDistance = listOfPedMaxSpawningDistance[pedMaxSpawningDistanceList.Index];
            GtaVModPeDistance.Settings.CameraMinSpawningHeight = listOfCameraMinSpawningHeight[cameraMinSpawningHeightList.Index];
            GtaVModPeDistance.Settings.CameraMaxSpawningHeight = listOfCameraMaxSpawningHeight[cameraMaxSpawningHeightList.Index];
            GtaVModPeDistance.Settings.CameraFixedHeight = listOfCameraFixedHeight[cameraFixedHeightList.Index];
            GtaVModPeDistance.Settings.TeleportingDelay = listOfTeleportingDelay[teleportingDelayList.Index];
            GtaVModPeDistance.Settings.RenderingDelay = listOfRenderingDelay[renderingDelayList.Index];
            GtaVModPeDistance.Settings.PedSpawningDelay = listOfPedSpawningDelay[pedSpawningDelayList.Index];
            GtaVModPeDistance.Settings.CollectingDataDelay = listOfCollectingDataDelay[collectingDataDelayList.Index];
            GtaVModPeDistance.Settings.ClearCollectingDataDelay = listOfClearCollectingDataDelay[clearCollectingDataDelayList.Index];
            GtaVModPeDistance.Settings.SaveScreenShotLocally = listOfSaveScreenShotLocally[saveScreenShotLocallyList.Index];
            GtaVModPeDistance.Settings.SaveSettings();
            Notification.Show("Settings saved");
        }
        private void SpawnOnePed()
        {
            float x;
            float y;
            do
            {
                x = rand.Next(-5, 5);
                y = rand.Next(2, 15);
            } while (Math.Abs(x) > y);
            ped = World.CreateRandomPed(Game.Player.Character.GetOffsetPosition(new Vector3(x, y, 0)));
            ped.Heading = rand.Next(360);           
        }

        public void SpawnVehicle(UIMenuListItem vehicleTypeList, List<dynamic> typeList)
        {
            if (vehicle != null) vehicle.Delete();
            Vector3 pos = Game.Player.Character.GetOffsetPosition(new Vector3(0, 10, 0));
            VehicleHash vehicleHash = typeList[vehicleTypeList.Index];
            vehicle = World.CreateVehicle(new Model(vehicleHash), pos);
            vehicle.PlaceOnGround();
            vehicle.IsInvincible = true;
        }
       
        public void SaveCoordinates()
        {
            Vector3 pos = Game.Player.Character.Position;
            Vector3 rot = Game.Player.Character.Rotation;
            string streetName = World.GetStreetName(pos);
            string zoneLocalizedName = World.GetZoneLocalizedName(pos);
            location.SaveCoordinates(new SpawnPoint(pos.X, pos.Y, pos.Z, rot.X, rot.Y, rot.Z, streetName, zoneLocalizedName));
        }
      
        private void SetTime(int h, int m, int s)
        {
            World.CurrentTimeOfDay = new TimeSpan(h, m, s);
            Notification.Show("Time set to: " + h + ":" + m + ":" + s);
        }

        private void TeleportToWaypoint()
        {
            float X = World.WaypointPosition.X;
            float Y = World.WaypointPosition.Y;
            float Z = World.GetGroundHeight(new Vector2(X, Y)) + 2;
            Vector3 pos = new Vector3(X, Y, Z);
            Game.Player.Character.Position = pos;
            Notification.Show("Player has been ~b~teleported to  ~g~" + World.GetStreetName(pos) + ", " + World.GetZoneDisplayName(pos) + ": ~o~" + pos.ToString());
        }

        private void ResetWantedLevel()
        {
            if(Game.Player.WantedLevel == 0)
            {
                GTA.UI.Screen.ShowSubtitle("you are innocent");
            }
            else
            {
                Game.Player.WantedLevel = 0;
                Notification.Show("Player WantedLevel set to 0");
            }
        }
        #endregion
    }

    
}