using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing;
using GTA;
using GTA.UI;
using GTA.Math;
using GTA.Native;
using NativeUI;

namespace GtaVModPeDistance
{
    public delegate void ActionToDo();

    public class Main : Script
    {
        bool firstTime = true;
        string ModName = "Tostino-ML";
        string Developer = "Danilo-AleS-AleC";
        string Version = "1.0";

        MenuPool modMenuPool;
        Menu mainMenu;

        List<Vector3> placesList = new List<Vector3>();
        List<Ped> pedList = new List<Ped>();
        Ped ped;
        int GameTimeReference = Game.GameTime + 1000;
        Random rand = new Random();
        //List<string> namesList;
        //List<ActionToDo> actionsList;
        
        public Main()
        {
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
            itemList.Add(new MenuItem("Shoot Ped", ()=> { World.CreateRandomPed(World.GetCrosshairCoordinates().HitPosition); }));
            itemList.Add(new MenuItem("Safe Ped SideWalk", ()=> { World.CreateRandomPed(World.GetSafeCoordForPed(Game.Player.Character.Position,true)); }));
            itemList.Add(new MenuItem("Safe Ped No Sidewalk", ()=> { World.CreateRandomPed(World.GetSafeCoordForPed(Game.Player.Character.Position,false)); }));
            itemList.Add(new MenuItem("Handle Camera", HandleMyCamera));

            /*
            itemList.Add(new MenuItem("Test V", () => { SendKeys.SendWait("H"); }));
            itemList.Add(new MenuItem("Test F12", () => { SendKeys.SendWait("F12"); }));
            */
            mainMenu = new Menu("Tostino Menu", "SELECT AN OPTION", itemList);
            modMenuPool.Add(mainMenu.GetMainMenu());
        }
        
        public void HandleMyCamera()
        {
            Vector3 myPos = Game.Player.Character.Position;
            Vector3 cameraPos = new Vector3(myPos.X, myPos.Y - 40, myPos.Z);
            Camera cameretta = World.CreateCamera(cameraPos, Vector3.Zero, 100);
            cameretta.PointAt(Game.Player.Character);
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