using System;
using System.Windows.Forms;
using System.Collections.Generic;
using GTA;
using GTA.UI;
using GTA.Math;
using GTA.Native;
using NativeUI;
using GtaVModPeDistance.Models;

namespace GtaVModPeDistance
{
    public delegate void ActionToDo();

    public class Main : Script
    {
        bool firstTime = true;
        int changeCameraPlayerState = 0;
        string ModName = "Tostino-ML";
        string Developer = "Danilo-AleS-AleC";
        string Version = "1.0";

        MenuPool modMenuPool;
        Menu mainMenu, utilsMenu, mlMenu, fileMenu;
        LocationManager file;
        UIMenuListItem helicopterList, motorbikeList, boatList, weaponList;
        List<dynamic> listOfHelicopter, listOfMotorbike, listOfBoat;
        VehicleHash[] allVehiclesHash;

        List<Ped> pedList = new List<Ped>();
        Ped ped;
        int GameTimeReference = Game.GameTime + 1000;
        Random rand = new Random();

        Camera cameretta;
        //List<string> namesList;
        //List<ActionToDo> actionsList;

        public Main()
        {
            file = new LocationManager();
            MenuSetup();
            Tick += onTick;
            KeyDown += onKeyDown;
            GameTimeReference = Game.GameTime + 1000;
        }

        private void onTick(object sender, EventArgs e)
        {
            // Mod info
            if (firstTime)
            {
                Notification.Show("~o~" + ModName + " " + Version + " by ~o~" + Developer + " Loaded");
                firstTime = false;
            }

            //if you don't do that, the menu wont show up
            
            if(modMenuPool != null)
            {
                modMenuPool.ProcessMenus();
            }

            if(World.RenderingCamera.Equals(cameretta))
            {

                if (Game.Player.Character.IsInVehicle() && changeCameraPlayerState != 1)
                {
                    cameretta.Detach();
                    cameretta.AttachTo(Game.Player.Character, new Vector3(0, 0, 17));
                    changeCameraPlayerState = 1;
                } else if (Game.Player.IsTargetingAnything && changeCameraPlayerState != 2)
                {
                    cameretta.Detach();
                    cameretta.AttachTo(Game.Player.Character, new Vector3(0, 0, 5));
                    changeCameraPlayerState = 2;
                } else if(Game.Player.Character.IsOnFoot && !Game.Player.IsTargetingAnything && changeCameraPlayerState != 0)
                {
                    cameretta.Detach();
                    cameretta.AttachTo(Game.Player.Character, new Vector3(0, 0, 10));
                    changeCameraPlayerState = 0;
                }
            }
        }

        private void onKeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.F5 && !modMenuPool.IsAnyMenuOpen())
            {
                mainMenu.ShowHideMenu();
            }
        }

        private void MenuSetup()
        {
            modMenuPool = new MenuPool();

            List<MenuItem> itemList = new List<MenuItem>();
            List<MenuItem> utilsList = new List<MenuItem>();
            List<MenuItem> mlList = new List<MenuItem>();
            List<MenuItem> fileList = new List<MenuItem>();

            allVehiclesHash = (VehicleHash[])Enum.GetValues(typeof(VehicleHash));
            listOfHelicopter = new List<dynamic>();
            listOfMotorbike = new List<dynamic>();
            listOfBoat = new List<dynamic>();

            for (int i = 0; i < allVehiclesHash.Length; i++)
            {
                Model tempModel = new Model(allVehiclesHash[i]);

                if (tempModel.IsHelicopter) listOfHelicopter.Add(allVehiclesHash[i]);
                else if (tempModel.IsBike) listOfMotorbike.Add(allVehiclesHash[i]);
                else if (tempModel.IsBoat) listOfBoat.Add(allVehiclesHash[i]);
            }

            //TODO: find index problem
            helicopterList = new UIMenuListItem("Helicopters: ", listOfHelicopter, 1);
            motorbikeList = new UIMenuListItem("Motorbikes: ", listOfMotorbike, 1);
            boatList = new UIMenuListItem("Boats: ", listOfBoat, 1);

            //utils menu
            utilsList.Add(new MenuItem("Reset Time Midday", ResetTimeMidday));
            utilsList.Add(new MenuItem("Reset Wanted Level", ResetWantedLevel));
            utilsList.Add(new MenuItem("Shoot Ped", ()=> { World.CreateRandomPed(World.GetCrosshairCoordinates().HitPosition); }));
            utilsList.Add(new MenuItem("Never Wanted", ()=> { Game.Player.IgnoredByPolice = true; }));
            utilsList.Add(new MenuItem("Handle Camera", HandleMyCamera));
            utilsList.Add(new MenuItem("Disable Camera", DisableCamera));
            utilsList.Add(new MenuItem(helicopterList, () => { SpawnVehicle(helicopterList, listOfHelicopter); }));
            utilsList.Add(new MenuItem(motorbikeList, () => { SpawnVehicle(motorbikeList, listOfMotorbike); }));
            utilsList.Add(new MenuItem(boatList, () => { SpawnVehicle(boatList, listOfBoat); }));
            //utilsList.Add(new MenuItem("Spawn Elic", SpawnElicopter));
            //utilsList.Add(new MenuItem("Spawn Bike", SpawnBike));


            //ml menu
            mlList.Add(new MenuItem("SpawnPed", SpawnOnePed));
            mlList.Add(new MenuItem("DeleteSpawnedPed", DeleteSpawnedPed));
            mlList.Add(new MenuItem("AirportDesertTeleport", AirportDesertTeleport));
            mlList.Add(new MenuItem("DeleteAllNearPed", DeleteAllNearPed));
            mlList.Add(new MenuItem("KillAllSpawnedPed", KillAllSpawnedPed));
            mlList.Add(new MenuItem("TeleportInWaypoint", TeleportInWaypoint));
            mlList.Add(new MenuItem("Safe Ped SideWalk", ()=> { World.CreateRandomPed(World.GetSafeCoordForPed(Game.Player.Character.Position,true)); }));
            mlList.Add(new MenuItem("Safe Ped No Sidewalk", ()=> { World.CreateRandomPed(World.GetSafeCoordForPed(Game.Player.Character.Position,false)); }));
            mlList.Add(new MenuItem("SpawnRandomPoint", SpawnRandomPoint));

            //file menu
            fileList.Add(new MenuItem("ShowCoordinates", ShowCoordinates));
            fileList.Add(new MenuItem("SaveCoordinates", SaveCoordinates));
            fileList.Add(new MenuItem("DeleteLastSavedCoord", file.DeleteLastCoordinate));
            fileList.Add(new MenuItem("CloseFile", file.CloseLocationFile));
            fileList.Add(new MenuItem("Street Name", StreetName));


            mainMenu = new Menu("Tostino Menu", "SELECT AN OPTION", itemList);
            modMenuPool.Add(mainMenu.ModMenu);

            UIMenu uiUtilsMenu = modMenuPool.AddSubMenu(mainMenu.ModMenu, "Utils");
            UIMenu uiMlMenu = modMenuPool.AddSubMenu(mainMenu.ModMenu, "Machine Learning");
            UIMenu uiFileMenu = modMenuPool.AddSubMenu(mainMenu.ModMenu, "File");

            utilsMenu = new Menu(uiUtilsMenu, utilsList);
            mlMenu = new Menu(uiMlMenu, mlList);
            fileMenu = new Menu(uiFileMenu, fileList);
        }

        public void HideOrShowHud(bool hideOrShow) 
        {
            Function.Call(Hash.DISPLAY_RADAR, hideOrShow);
        }

        public void SpawnVehicle(UIMenuListItem vehicleTypeList, List<dynamic> typeList)
        {
            Vector3 pos = Game.Player.Character.GetOffsetPosition(new Vector3(0, 10, 0));
            VehicleHash vehicleHash = typeList[vehicleTypeList.Index];
            Vehicle vehicle = World.CreateVehicle(new Model(vehicleHash),pos);
            vehicle.PlaceOnGround();
        }

        private void SpawnRandomPoint()
        {
            if (cameretta != null) cameretta.Delete();
            cameretta = null;
            SpawnPoint spawnPoint = file.GetRandomPoint();
            cameretta = World.CreateCamera(new Vector3(spawnPoint.Position.X, spawnPoint.Position.Y, spawnPoint.Position.Z + 0.8f), spawnPoint.Rotation, 80);
            World.RenderingCamera = cameretta;
            Game.Player.Character.Position = spawnPoint.Position;
            HideOrShowHud(false);
            Notification.Show("Camera has been ~b~generated to ~o~" + spawnPoint.StreetName.ToString() + ", " + spawnPoint.ZoneLocalizedName.ToString());
        }

        public void SaveCoordinates()
        {
            Vector3 pos = Game.Player.Character.Position;
            Vector3 rot = Game.Player.Character.Rotation;
            string streetName = World.GetStreetName(pos);
            string zoneLocalizedName = World.GetZoneLocalizedName(pos);
            file.SaveCoordinates(new SpawnPoint(pos, rot, streetName, zoneLocalizedName));
        }

        public void StreetName()
        {
            Vector3 pos = Game.Player.Character.GetOffsetPosition(new Vector3(0, 0, 0));
            string streetName = World.GetStreetName(pos);
            string zoneDisplayName = World.GetZoneDisplayName(pos);
            string zoneLocalizedName = World.GetZoneLocalizedName(pos);
            Notification.Show("StreetName: " + streetName + "\nZoneDisplayName: " + zoneDisplayName + "\nZoneLocalizedName: " + zoneLocalizedName);
        }
        

        #region methods
        public void HandleMyCamera()
        {
            cameretta = null;
            Vector3 cameraPos = Vector3.Zero;
            cameretta = World.CreateCamera(cameraPos, Vector3.Zero, 80);
            cameretta.AttachTo(Game.Player.Character, new Vector3(0, 0, 10));                     
            cameretta.Rotation = new Vector3(-90, 0, 0);           
            World.RenderingCamera = cameretta;
            Notification.Show("" + cameretta.Rotation);
        }

        public void DisableCamera()
        {
            if (cameretta != null)
            {
                cameretta.Detach();
                cameretta.Delete();
            }           
            World.RenderingCamera = null;
        }

        public void SpawnElicopter()
        {
            World.CreateVehicle(new Model(VehicleHash.Frogger2), Game.Player.Character.GetOffsetPosition(new Vector3(0, 10, 0)));
        }
        
        public void SpawnBike()
        {
            World.CreateVehicle(new Model(VehicleHash.Akuma), Game.Player.Character.GetOffsetPosition(new Vector3(0, 5, 0)));
        }


        private void KillAllSpawnedPed()
        {
            pedList.ForEach(ped => ped.Kill());
            Notification.Show("All spawned ped ~r~killed");
        }

        private void DeleteSpawnedPed()
        {
            pedList.ForEach(ped => ped.Delete());
            pedList.Clear();
            Notification.Show("All spawned ped ~r~deleted");
        }

        private void DeleteAllNearPed()
        {
            Ped[] pedArray = World.GetAllPeds();
            foreach (Ped ped in pedArray)
            {
                ped.Delete();
            }
            Notification.Show("All ped ~r~deleted (I am legend)");
        }

        private void ResetTimeMidday()
        {
            World.CurrentTimeOfDay = new TimeSpan(12, 0, 0);
            Notification.Show("It's ~r~High Noon!");
        }

        private void TeleportInWaypoint()
        {
            float X = World.WaypointPosition.X;
            float Y = World.WaypointPosition.Y;
            float Z = World.GetGroundHeight(new Vector2(X, Y));
            Vector3 pos = new Vector3(X, Y, Z);
            Game.Player.Character.Position = pos;
            Notification.Show("Player has been ~b~teleported to ~o~" + pos.ToString());
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

        private void SpawnOnePed()
        {
            float x;
            float y;
            do
            {
                x = rand.Next(-5, 5);
                y = rand.Next(2, 15);
            } while (Math.Abs(x) > y);
            Ped ped = World.CreateRandomPed(World.RenderingCamera.GetOffsetPosition(new Vector3(x, y, 0)));                      
            ped.Heading = rand.Next(360);
            pedList.Add(ped);
            Notification.Show(ped.Speed.ToString());
        }

        private void AirportDesertTeleport()
        {
            Vector3 airportPos = new Vector3(1716.713f, 3254.09f, 41.12978f);
            Game.Player.Character.Position = airportPos;
            Notification.Show("Player has been ~b~teleported to ~o~" + airportPos.ToString());
        }

        private void ShowCoordinates()
        {
            Notification.Show("Player coord ~o~" + Game.Player.Character.Position.ToString());
        }

        #endregion
    }

    
}