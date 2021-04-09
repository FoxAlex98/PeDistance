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

        MenuPool modMenuPool;
        Menu mainMenu;
        MenuHelper menuHelper;

        Random rand = new Random();

        // Managers
        LocationManager location;
        ScreenShotManager screenShot;
        DataMananger dataManager;       

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

            menuHelper = new MenuHelper();

            Notification.Show("~o~" + ModName + " " + Version + " by ~o~" + Developer + " Loaded");
            Notification.Show("Use F5 to open menu");
            MenuSetup();
            Tick += onTick;
            KeyDown += onKeyDown;
        }

        private void onTick(object sender, EventArgs e)
        {
            //if you don't do that, the menu wont show up            
            if(modMenuPool != null)
                modMenuPool.ProcessMenus();

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
                                if (collectedDataCounter >= GtaVModPeDistance.Settings.MaxCollectedData || wannaStop)
                                {
                                    EndingCollectingData();
                                } else
                                {
                                    start = Game.GameTime;
                                    collectingStep = 0;
                                    Notification.Show("Counter: " + collectedDataCounter + "/" + GtaVModPeDistance.Settings.MaxCollectedData);
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
                mainMenu.ToggleMenu();
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

            UIMenu uiUtilsMenu = modMenuPool.AddSubMenu(mainMenu.ModMenu, "Utils Menu");
            new Menu(uiUtilsMenu, menuHelper.GetUtilsMenu());

            UIMenu uiMlMenu = modMenuPool.AddSubMenu(mainMenu.ModMenu, "Settings Menu");
            new Menu(uiMlMenu, menuHelper.GetConfigMenu());           
        }

        #region Collecting Data Functions
        public void StartCollectingData()
        {
            initialPosition = Game.Player.Character.Position;
            Notification.Show("Starting collecting data... Remove menu using ESC");           
            Game.Player.Character.IsVisible = false;
            Globals.HideHud();
            screenShot.DeleteAllScreenShot();
            dataManager.CleanFile();
            start = Game.GameTime;            
            wannaStop = false;
            collectingStep = 0;
            collectedDataCounter = 0;            
            ped = null;
            startCollectingData = true;
        }

        public void StopCollectingData()
        {
            if(startCollectingData)
            {
                Notification.Show("Stop collecting data...");
                wannaStop = true;
            }           
        }

        public void EndingCollectingData()
        {
            if (startCollectingData)
            {
                Notification.Show("Stop collecting data...");
                startCollectingData = false;
                Game.Player.Character.IsVisible = true;
                Game.Player.Character.Position = initialPosition;
                Globals.ShowHud();
                World.RenderingCamera = null;
                collectingStep = 0;
                collectedDataCounter = 0;
                ped = null;
                dataManager.WriteDataToFile();
            }
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



        private void SpawnRandomPoint()
        {            
            spawnPoint = location.GetRandomPoint();
            Vector3 Position = new Vector3(spawnPoint.PosX, spawnPoint.PosY, spawnPoint.PosZ);
            Vector3 Rotation = new Vector3(spawnPoint.RotX, spawnPoint.RotY, spawnPoint.RotZ);
            Game.Player.Character.Position = Position;
            float Z = 0;
            if (GtaVModPeDistance.Settings.CameraFixedHeight == 0)
            {
                // TODO sistemare l'orientamento della verso il basso della camera
                Z = (Position.Z - World.GetGroundHeight(Game.Player.Character.Position)) + rand.Next(GtaVModPeDistance.Settings.CameraMinSpawningHeight, GtaVModPeDistance.Settings.CameraMaxSpawningHeight);
            } else
            {
                Z = Position.Z + GtaVModPeDistance.Settings.CameraFixedHeight;
            }
            Camera camera = World.CreateCamera(new Vector3(Position.X, Position.Y, Z), Rotation, 60);
            World.RenderingCamera = camera;
            // Notification.Show("Camera has been ~b~generated to ~o~" + spawnPoint.StreetName.ToString() + ", " + spawnPoint.ZoneLocalizedName.ToString());
        }

        private void CollectingData()
        {            
            ScreenShot image = screenShot.TakeScreenshot();

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
            return Math.Sqrt(Math.Pow(pedPosition.X - cameraPosition.X, 2) + Math.Pow(pedPosition.Y - cameraPosition.Y, 2) + Math.Pow(pedPosition.Z - cameraPosition.Z, 2));
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

    }

    
}