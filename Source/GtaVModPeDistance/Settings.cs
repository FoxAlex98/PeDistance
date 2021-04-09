
using GTA.UI;
using System;
using System.IO;
using System.Threading.Tasks;

namespace GtaVModPeDistance
{
    public static class Settings
    {

        private static string FilePath;
        public static int MaxCollectedData { get; set; }
        public static int MaxCollectedSelectionable { get; set; }
        public static int PedMinSpawningDistance { get; set; }
        public static int PedMaxSpawningDistance { get; set; }
        public static int CameraMinSpawningHeight { get; set; }
        public static int CameraMaxSpawningHeight { get; set; }
        public static float CameraFixedHeight { get; set; }
        public static int TeleportingDelay { get; set; }
        public static int RenderingDelay { get; set; }
        public static int PedSpawningDelay { get; set; }
        public static int CollectingDataDelay { get; set; }
        public static int ClearCollectingDataDelay { get; set; }
        public static string SaveScreenShotLocally { get; set; }
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
                    string key = line.Split('=')[0];
                    string value = line.Split('=')[1];
                    if (key.Equals("MaxCollectedData")) MaxCollectedData = int.Parse(value.Trim());
                    else if (key.Equals("MaxCollectedSelectionable")) MaxCollectedSelectionable = int.Parse(value.Trim());
                    else if (key.Equals("PedMinSpawningDistance")) PedMinSpawningDistance = int.Parse(value.Trim());
                    else if (key.Equals("PedMaxSpawningDistance")) PedMaxSpawningDistance = int.Parse(value.Trim());
                    else if (key.Equals("CameraMinSpawningHeight")) CameraMinSpawningHeight = int.Parse(value.Trim());
                    else if (key.Equals("CameraMaxSpawningHeight")) CameraMaxSpawningHeight = int.Parse(value.Trim());
                    else if (key.Equals("CameraFixedHeight")) CameraFixedHeight = float.Parse(value.Trim());
                    else if (key.Equals("TeleportingDelay")) TeleportingDelay = int.Parse(value.Trim());
                    else if (key.Equals("RenderingDelay")) RenderingDelay = int.Parse(value.Trim());
                    else if (key.Equals("PedSpawningDelay")) PedSpawningDelay = int.Parse(value.Trim());
                    else if (key.Equals("CollectingDataDelay")) CollectingDataDelay = int.Parse(value.Trim());
                    else if (key.Equals("ClearCollectingDataDelay")) ClearCollectingDataDelay = int.Parse(value.Trim());
                    else if (key.Equals("SaveScreenShotLocally")) SaveScreenShotLocally = value.Trim();
                    else if (key.Equals("ImageFormat")) ImageFormat = value.Trim();
                    else if (key.Equals("DirectoryName")) DirectoryName = value.Trim();
                }                
            }           
        }

        public static void SaveSettings()
        {
            string[] lines =
            {
                "MaxCollectedData=" + MaxCollectedData.ToString().Trim(),
                "MaxCollectedSelectionable=" + MaxCollectedSelectionable.ToString().Trim(),
                "PedMinSpawningDistance=" + PedMinSpawningDistance.ToString().Trim(),
                "PedMaxSpawningDistance=" + PedMaxSpawningDistance.ToString().Trim(),
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
