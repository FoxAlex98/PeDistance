﻿using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing;
using GTA;
using GTA.UI;
using GTA.Math;
using GTA.Native;
using NativeUI;

using System.Reflection;

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
        Menu mainMenu;
        FileManager file;

        List<Ped> pedList = new List<Ped>();
        Ped ped;
        int GameTimeReference = Game.GameTime + 1000;
        Random rand = new Random();

        Camera cameretta;
        //List<string> namesList;
        //List<ActionToDo> actionsList;

        public Main()
        {
            file = new FileManager();
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
            itemList.Add(new MenuItem("SpawnPed", SpawnOnePed));
            itemList.Add(new MenuItem("DeleteAllNearPed", DeleteAllNearPed));
            itemList.Add(new MenuItem("KillAllSpawnedPed", KillAllSpawnedPed));
            itemList.Add(new MenuItem("ResetTimeMidday", ResetTimeMidday));
            itemList.Add(new MenuItem("ResetWantedLevel", ResetWantedLevel));
            itemList.Add(new MenuItem("TeleportInWaypoint", TeleportInWaypoint));
            itemList.Add(new MenuItem("AirportDesertTeleport", AirportDesertTeleport));
            itemList.Add(new MenuItem("DeleteSpawnedPed", DeleteSpawnedPed));
            itemList.Add(new MenuItem("ShowCoordinates", ShowCoordinates));
            itemList.Add(new MenuItem("SaveCoordinates", SaveCoordinates));
            itemList.Add(new MenuItem("CloseFile", file.CloseLocationFile));
            itemList.Add(new MenuItem("Never Wanted", ()=> { Game.Player.IgnoredByPolice = true; }));
            itemList.Add(new MenuItem("Shoot Ped", ()=> { World.CreateRandomPed(World.GetCrosshairCoordinates().HitPosition); }));
            itemList.Add(new MenuItem("Safe Ped SideWalk", ()=> { World.CreateRandomPed(World.GetSafeCoordForPed(Game.Player.Character.Position,true)); }));
            itemList.Add(new MenuItem("Safe Ped No Sidewalk", ()=> { World.CreateRandomPed(World.GetSafeCoordForPed(Game.Player.Character.Position,false)); }));
            itemList.Add(new MenuItem("Handle Camera", HandleMyCamera));
            itemList.Add(new MenuItem("Disable Camera", DisableCamera));
            itemList.Add(new MenuItem("Street Name", StreetName));

            /*
            itemList.Add(new MenuItem("Test V", () => { SendKeys.SendWait("H"); }));
            itemList.Add(new MenuItem("Test F12", () => { SendKeys.SendWait("F12"); }));
            */
            mainMenu = new Menu("Tostino Menu", "SELECT AN OPTION", itemList);
            modMenuPool.Add(mainMenu.GetMenu());
        }

        public void SaveCoordinates()
        {
            Vector3 pos = Game.Player.Character.Position;
            Vector3 rot = Game.Player.Character.Rotation;
            string streetName = World.GetStreetName(pos);
            string zoneLocalizedName = World.GetZoneLocalizedName(pos);
            file.SaveCoordinates(pos, rot, streetName, zoneLocalizedName);
        }

        public void StreetName()
        {
            Vector3 pos = Game.Player.Character.GetOffsetPosition(new Vector3(0, 0, 0));
            String streetName = World.GetStreetName(new Vector2(pos.X, pos.Y));
            String zoneDisplayName = World.GetZoneDisplayName(new Vector2(pos.X, pos.Y));
            String zoneLocalizedName = World.GetZoneLocalizedName(new Vector2(pos.X, pos.Y));
            Notification.Show("StreetName: " + streetName + "\nZoneDisplayName: " + zoneDisplayName + "\nZoneLocalizedName: " + zoneLocalizedName);
        }
        
        public void HandleMyCamera()
        {
            cameretta = null;
            Vector3 myPos = Game.Player.Character.Position;
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

        #region methods

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
            pedList.Add(World.CreateRandomPed(Game.Player.Character.GetOffsetPosition(new Vector3(0, rand.Next(50), 50))));
            Notification.Show("Ped has been ~b~spawned!");
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