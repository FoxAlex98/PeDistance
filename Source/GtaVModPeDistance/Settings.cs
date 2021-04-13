using System;
using System.IO;

namespace GtaVModPeDistance
{
    public static class Settings
    {

        private static string FilePath;
        public static int MaxCollectedData { get; set; }
        public static int MaxCollectedSelectionable { get; set; }
        public static float PedSpawningDistanceRatio { get; set; }
        public static int PedMinSpawningDistanceY { get; set; }
        public static int PedMaxSpawningDistanceY { get; set; }
        public static int CameraMinSpawningHeight { get; set; }
        public static int CameraMaxSpawningHeight { get; set; }
        public static float CameraFixedHeight { get; set; }
        public static int TeleportingDelay { get; set; }
        public static int RenderingDelay { get; set; }
        public static int PedSpawningDelay { get; set; }
        public static int CollectingDataDelay { get; set; }
        public static int ClearCollectingDataDelay { get; set; }
        public static bool SaveScreenShotLocally { get; set; }
        public static string ImageFormat { get; set; }
        public static string DirectoryName { get; set; }

        public static void LoadSettings()
        {
            string fileSettings = "";
            try
            {
                FilePath = Path.Combine(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory), "scripts", "GtaVModPeDistanceSettings.txt");                
                using (var sr = new StreamReader(FilePath))
                {
                    fileSettings = sr.ReadToEnd();
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
            AssignValueToSettings(fileSettings);
        }

        private static void AssignValueToSettings(string settings)
        {
            string[] lines = settings.Split('\n');
            foreach(string line in lines)
            {
                if (!line.Trim().Equals(""))
                {
                    string key = line.Split('=')[0].Trim();
                    string value = line.Split('=')[1].Trim();
                    if (key.Equals("MaxCollectedData")) MaxCollectedData = int.Parse(value);
                    else if (key.Equals("MaxCollectedSelectionable")) MaxCollectedSelectionable = int.Parse(value);
                    else if (key.Equals("PedSpawningDistanceRatio")) PedSpawningDistanceRatio = float.Parse(value);
                    else if (key.Equals("PedMinSpawningDistanceY")) PedMinSpawningDistanceY = int.Parse(value);
                    else if (key.Equals("PedMaxSpawningDistanceY")) PedMaxSpawningDistanceY = int.Parse(value);
                    else if (key.Equals("CameraMinSpawningHeight")) CameraMinSpawningHeight = int.Parse(value);
                    else if (key.Equals("CameraMaxSpawningHeight")) CameraMaxSpawningHeight = int.Parse(value);
                    else if (key.Equals("CameraFixedHeight")) CameraFixedHeight = float.Parse(value);
                    else if (key.Equals("TeleportingDelay")) TeleportingDelay = int.Parse(value);
                    else if (key.Equals("RenderingDelay")) RenderingDelay = int.Parse(value);
                    else if (key.Equals("PedSpawningDelay")) PedSpawningDelay = int.Parse(value);
                    else if (key.Equals("CollectingDataDelay")) CollectingDataDelay = int.Parse(value);
                    else if (key.Equals("ClearCollectingDataDelay")) ClearCollectingDataDelay = int.Parse(value);
                    else if (key.Equals("SaveScreenShotLocally")) SaveScreenShotLocally = bool.Parse(value);
                    else if (key.Equals("ImageFormat")) ImageFormat = value;
                    else if (key.Equals("DirectoryName")) DirectoryName = value;
                }                
            }
        }

        public static void SaveSettings()
        {
            string[] lines =
            {
                "MaxCollectedData=" + MaxCollectedData.ToString().Trim(),
                "MaxCollectedSelectionable=" + MaxCollectedSelectionable.ToString().Trim(),
                "PedMinSpawningDistanceY=" + PedMinSpawningDistanceY.ToString().Trim(),
                "PedMaxSpawningDistanceY=" + PedMaxSpawningDistanceY.ToString().Trim(),
                "PedSpawningDistanceRatio=" + PedSpawningDistanceRatio.ToString().Trim(),
                "CameraMinSpawningHeight=" + CameraMinSpawningHeight.ToString().Trim(),
                "CameraMaxSpawningHeight=" + CameraMaxSpawningHeight.ToString().Trim(),
                "CameraFixedHeight=" + CameraFixedHeight.ToString().Trim(),
                "TeleportingDelay=" + TeleportingDelay.ToString().Trim(),
                "RenderingDelay=" + RenderingDelay.ToString().Trim(),
                "PedSpawningDelay=" + PedSpawningDelay.ToString().Trim(),
                "CollectingDataDelay=" + CollectingDataDelay.ToString().Trim(),
                "ClearCollectingDataDelay=" + ClearCollectingDataDelay.ToString().Trim(),
                "SaveScreenShotLocally=" + SaveScreenShotLocally.ToString().Trim(),
                "ImageFormat=" + ImageFormat.ToString().Trim(),
                "DirectoryName=" + DirectoryName.ToString().Trim()
            };

            System.IO.File.WriteAllLines(FilePath, lines);
        }
    }
}
