using GTA;
using GTA.Math;
using GTA.UI;
using GtaVModPeDistance.CollectingSteps;
using GtaVModPeDistance.CollectingSteps.ConcreteSteps;
using GtaVModPeDistance.File;
using GtaVModPeDistance.Models;
using NativeUI;
using System;
using System.Collections.Generic;

namespace GtaVModPeDistance
{
    class UtilsFunctions
    {

        static LocationManager locationManager = LocationManager.GetInstance();

        public static Ped ped, pedMin, pedMax;

        public static List<Vector3> DistancePoints = new List<Vector3>();

        public static bool ActiveNearbyEntitiesBoundingBox = false;

        public static bool MeterMode = false;

        public static bool SetupMenu = false;

        static Random rand = new Random();

        #region Utils

        public static void SpawnOnePed()
        {
            float y = Utilities.GetYByFov();           
            float x = Utilities.GetPosXByPosY(y);
            if (ped != null) ped.Delete();
            ped = World.CreateRandomPed(Game.Player.Character.GetOffsetPosition(new Vector3(x, y, 0)));
            ped.FacePosition(Game.Player.Character.Position);
            float angle = Utilities.NextFloat(1, 360);
            Notification.Show(angle.ToString());
            ped.Heading += angle;

        }

        public static void SpawnSettingPeds()
        {
            float yMin = 10f - World.RenderingCamera.FieldOfView / 10f;
            if (pedMin != null) pedMin.Delete();
            pedMin = World.CreateRandomPed(World.RenderingCamera.GetOffsetPosition(new Vector3(-2f, yMin, 0)));
            pedMin.FacePosition(Game.Player.Character.Position);

            if (pedMax != null) pedMax.Delete();
            pedMax = World.CreateRandomPed(World.RenderingCamera.GetOffsetPosition(new Vector3(2f, 20f, 0)));
            pedMax.FacePosition(Game.Player.Character.Position);
        }

        public static void SpawnVehicle(VehicleHash vehicleHash)
        {
            if (Globals.Vehicle != null) Globals.Vehicle.Delete();
            Vector3 pos = Game.Player.Character.GetOffsetPosition(new Vector3(0, 10, 0));
            Globals.Vehicle = World.CreateVehicle(new Model(vehicleHash), pos);
            Globals.Vehicle.PlaceOnGround();
            Globals.Vehicle.IsInvincible = true;
        }

        public static void SaveCoordinates()
        {
            Vector3 pos = Game.Player.Character.Position;
            Vector3 rot = Game.Player.Character.Rotation;
            string streetName = World.GetStreetName(pos);
            string zoneLocalizedName = World.GetZoneLocalizedName(pos);
            locationManager.SaveCoordinates(new SpawnPoint(pos.X, pos.Y, pos.Z, rot.X, rot.Y, rot.Z, streetName, zoneLocalizedName));
        }

        public static void SetRandomWeather(bool notify = false)
        {
            Weather[] allWeather = (Weather[])Enum.GetValues(typeof(Weather));
            int index = rand.Next(0,allWeather.Length);
            World.TransitionToWeather(allWeather[index], 0f);
            if (notify)
                Notification.Show(allWeather[index].ToString());
        }

        public static void SetRandomTime(bool notify = false)
        {
            int h = rand.Next(0, 24);
            int m = rand.Next(0, 60);
            SetTime(h, m, 0, false);
            if (notify)
                Notification.Show(h + " " + m);
        }

        public static void SetTime(int h, int m = 0, int s = 0, bool notify = true)
        {
            World.CurrentTimeOfDay = new TimeSpan(h, m, s);
            if(notify)
                Notification.Show("Time set to: " + h + ":" + m + ":" + s);
        }

        public static void TeleportToWaypoint()
        {
            float X = World.WaypointPosition.X;
            float Y = World.WaypointPosition.Y;
            float Z = World.GetGroundHeight(new Vector2(X, Y)) + 2;
            Vector3 pos = new Vector3(X, Y, Z);
            Game.Player.Character.Position = pos;
            Notification.Show("Player has been ~b~teleported to  ~g~" + World.GetStreetName(pos) + ", " + World.GetZoneDisplayName(pos) + ": ~o~" + pos.ToString());
        }

        public static void ResetWantedLevel()
        {
            Game.Player.WantedLevel = 0;
        }

        public static void DeleteAllNearPed()
        {
            Ped[] pedArray = World.GetAllPeds();
            foreach (Ped ped in pedArray)
            {
                ped.Delete();
            }
        }

        public static void DeleteAllNearVehicles()
        {
            Vehicle[] vehicleArray = World.GetAllVehicles();
            foreach (Vehicle vc in vehicleArray)
            {
                vc.Delete();
            }
        }

        public static void SetCameraPosition(SpawnPoint spawnPoint)
        {
            Vector3 position = spawnPoint.GetPosition();
            Vector3 rotation = spawnPoint.GetRotation();
            Game.Player.Character.Position = position;
            Game.Player.Character.Rotation = rotation;
            Vector3 camRotation = new Vector3(Settings.CameraAngle, rotation.Y, rotation.Z);
            //Game.Player.Character.Heading = 0f;
            float Z = (Game.Player.Character.Position.Z - 1) + Settings.CameraFixedHeight;
            Camera camera = World.CreateCamera(new Vector3(position.X, position.Y, Z), camRotation, Settings.CameraFov);
            World.RenderingCamera = camera;
        }

        public static void Reset()
        {
            Game.Player.Character.IsVisible = true;
            Globals.ShowHud();
            World.RenderingCamera = null;
            CollectingState.StartCollectingData = false;
            CollectingState.ActualStep = new TeleportToRandomSavedLocationStep();
            CollectingState.CollectedDataCounter = 0;
            CollectingState.Ped = null;
        }

        public static void SpawnAtRandomSavedLocation()
        {
            SpawnPoint spawnPoint = locationManager.GetRandomPoint();
            SetCameraPosition(spawnPoint);
        }

        public static void SpawnNextSavedLocation()
        {
            SpawnPoint spawnPoint = locationManager.GetNextPoint();
            SetCameraPosition(spawnPoint);
        }

        public static void SpawnPrevSavedLocation()
        {
            SpawnPoint spawnPoint = locationManager.GetPrevPoint();
            SetCameraPosition(spawnPoint);
        }

        public static void ToggleNearbyEntityBoundingBox() => ActiveNearbyEntitiesBoundingBox = !ActiveNearbyEntitiesBoundingBox;
        #endregion

        public static void PrintDistancePoints(Vector3 point)
        {
            if (DistancePoints.Count % 2 == 0) DistancePoints.Clear();
            DistancePoints.Add(point);
            Notification.Show("Point saved: " + point.ToString());
            if (DistancePoints.Count == 2)
            {
                Notification.Show("Distance: " + World.GetDistance(DistancePoints[0], DistancePoints[1]) + " meter");               
            }                            
        }
        
        public static void ToggleMeterMode()
        {
            MeterMode = !MeterMode;
            Notification.Show("Meter mode " + (MeterMode ? "activated" : "deactivated"));
        } 
        
        public static void ToggleDebugMode(UIMenuCheckboxItem debugMode, bool value)
        {
            debugMode.Checked = value;
            SetupMenu = true;
            Notification.Show("Advanced Mode " + (debugMode.Checked ? "activated" : "deactivated"));
            Notification.Show("F5 to open menu");
        }


        public static int GetClosestTimeInRange()
        {
            int currenTime = World.CurrentTimeOfDay.Hours;
            if (currenTime > 6 && currenTime < 10) return 8;
            if (currenTime > 10 && currenTime < 14) return 12;
            if (currenTime > 14 && currenTime < 18) return 16;
            if (currenTime > 18 && currenTime < 22) return 20;
            if (currenTime > 20 && currenTime < 2) return 0;
            else return 4;
        }

    }
}
