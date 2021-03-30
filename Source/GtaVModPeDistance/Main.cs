using System;
using System.Windows.Forms;
using GTA;
using GTA.Native;
using GTA.Math;
using System.Collections.Generic;

namespace GtaVModPeDistance
{
    public class Main : Script
    {
        bool firstTime = true;
        string ModName = "Tostino-ML";
        string Developer = "Danilo-AleS-AleC";
        string Version = "1.0";
        List<Vector3> placesList = new List<Vector3> { new Vector3(1716.713f, 3254.09f, 41.12978f) };
        List<Ped> pedList = new List<Ped>();
        Ped ped;
        int GameTimeReference = Game.GameTime + 1000;
        Random rand = new Random();
        public Main()
        {
            Tick += onTick;
            KeyDown += onKeyDown;
            GameTimeReference = Game.GameTime + 1000;
        }

        private void onTick(object sender, EventArgs e)
        {
            // Mod info
            if (firstTime)
            {
                UI.Notify("~o~" + ModName + " " + Version + " by ~o~" + Developer + " Loaded");
                firstTime = false;
            }
            // start your script here:
        }

        private void onKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.NumPad1)
            {
                UI.Notify("Player coord ~o~" + Game.Player.Character.Position.ToString());
            }
            if (e.KeyCode == Keys.NumPad2)
            {
                Game.Player.Character.Position = placesList[0];
                UI.Notify("Player has been ~b~teleported to ~o~" + placesList[0].ToString());
            }
            if (e.KeyCode == Keys.NumPad3)
            {
                pedList.Add(World.CreateRandomPed(Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0, rand.Next(50), 50))));
                UI.Notify("Ped has been ~b~spawned!");
            }
            if (e.KeyCode == Keys.NumPad4)
            {
                Vector3 pos = World.GetWaypointPosition();
                Game.Player.Character.Position = pos;
                UI.Notify("Player has been ~b~teleported to ~o~" + pos.ToString());
            }
            if (e.KeyCode == Keys.NumPad5)
            {
                Game.Player.WantedLevel = 0;
                UI.Notify("Player WantedLevel set to 0");
            }
            if (e.KeyCode == Keys.NumPad6)
            {
                pedList.ForEach(ped => ped.Kill());
                UI.Notify("All spawned ped ~r~killed");
            }
            if (e.KeyCode == Keys.NumPad7)
            {
                pedList.ForEach(ped => ped.Delete());
                pedList.Clear();
                UI.Notify("All spawned ped ~r~deleted");
            }
            if (e.KeyCode == Keys.NumPad8)
            {
                Ped[] pedArray = World.GetAllPeds();
                foreach (Ped ped in pedArray)
                {
                    ped.Delete();
                }
                UI.Notify("All ped ~r~deleted (I am legend)");
            }
            if (e.KeyCode == Keys.NumPad9)
            {
                World.CurrentDayTime = new TimeSpan(12, 0, 0);
                UI.Notify("It's ~r~High Noon!");
            }
        }
    }
}