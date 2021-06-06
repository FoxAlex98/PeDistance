using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using GtaVModPeDistance.Models;
using NativeUI;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace GtaVModPeDistance
{
    class UtilsFunctions
    {

        static LocationManager locationManager = LocationManager.GetInstance();

        public static Ped ped;

        public static List<Vector3> DistancePoints = new List<Vector3>();

        public static bool ActiveNearbyEntitiesBoundingBox = false;

        public static bool MeterMode = false;

        public static bool SetupMenu = false;

        #region Utils

        //public static void DrawLine(Ped ped)
        //{
        //    Vector3 startLine = ped.Position;
        //    Vector3 endLine = new Vector3(ped.Position.X + 10, ped.Position.Y, ped.Position.Z);
        //    World.DrawLine(startLine, endLine, Color.Green);
        //    DrawBox(ped);
        //}

        //public static void DrawBox(Ped ped)
        //{
        //    if (ped == null) return;

        //    Ped2DBoundingBox ped2DBoundingBox = CoordinatesUtils.GetPedBoundingBoxInScreen(ped);
        //    Vector3 start = new Vector3(ped2DBoundingBox.PedTopLeftX, ped.Position.Y ,ped2DBoundingBox.PedTopLeftY);
        //    Vector3 end = new Vector3(ped2DBoundingBox.PedBottomRightX, ped.Position.Y ,ped2DBoundingBox.PedBottomRightY);
        //    DrawBox(start, end, Color.Green);
        //}

        private static void DrawBox(Vector3 start, Vector3 end, Color color)
        {            
            Function.Call(Hash.DRAW_BOX, start.X, start.Y, start.Z, end.X, end.Y, end.Z, color.R, color.G, color.B, color.A);
        }

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


            //ped1 = World.CreateRandomPed(Game.Player.Character.GetOffsetPosition(new Vector3(x + 1, y, 0)));
            //ped1.Heading = 90;
            //// Notification.Show(ped1.Rotation.ToString());

            //ped2 = World.CreateRandomPed(Game.Player.Character.GetOffsetPosition(new Vector3(x + 2, y, 0)));
            //ped2.Heading = 180;
            //// Notification.Show(ped2.Rotation.ToString());

            //ped3 = World.CreateRandomPed(Game.Player.Character.GetOffsetPosition(new Vector3(x + 3, y, 0)));
            //ped3.Heading = 270;
            //// Notification.Show(ped3.Rotation.ToString());
            //ped.Heading = Utilities.NextFloat(1, 360);
            //DrawBox(ped);

        }

        public static void ChangeWeather(UIMenuListItem weatherList, List<dynamic> weathers)
        {
            World.TransitionToWeather(weathers[weatherList.Index], 0f);
        }

        public static void SpawnVehicle(UIMenuListItem vehicleTypeList, List<dynamic> typeList)
        {

            if (Globals.Vehicle != null) Globals.Vehicle.Delete();
            Vector3 pos = Game.Player.Character.GetOffsetPosition(new Vector3(0, 10, 0));
            VehicleHash vehicleHash = typeList[vehicleTypeList.Index];
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

        public static void SetTime(int h, int m, int s)
        {
            World.CurrentTimeOfDay = new TimeSpan(h, m, s);
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
            if (Game.Player.WantedLevel == 0)
            {
                Screen.ShowSubtitle("you are innocent");
            }
            else
            {
                Game.Player.WantedLevel = 0;
                Notification.Show("Player WantedLevel set to 0");
            }
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
        public static void SpawnAtRandomSavedLocation()
        {
            SpawnPoint spawnPoint = locationManager.GetRandomPoint();
            Game.Player.Character.Position = spawnPoint.GetPosition();
            Game.Player.Character.Rotation = spawnPoint.GetRotation();
            SetCameraPosition(spawnPoint.GetPosition(), spawnPoint.GetRotation());
        }

        public static void SetCameraPosition(Vector3 Position, Vector3 Rotation)
        {
            float Z = 0;
            if (Settings.CameraFixedHeight == 0)
            {
                // TODO sistemare l'orientamento della verso il basso della camera
                Z = (Position.Z - World.GetGroundHeight(Game.Player.Character.Position)) + Utilities.NextFloat(Settings.CameraMinSpawningHeight, Settings.CameraMaxSpawningHeight);
            }
            else
            {
                Z = Position.Z + Settings.CameraFixedHeight;
            }
            Camera camera = World.CreateCamera(new Vector3(Position.X, Position.Y, Z), Rotation, 60);
            World.RenderingCamera = camera;
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

    }
}
