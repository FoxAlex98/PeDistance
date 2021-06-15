using System;
using System.IO;

namespace GtaVModPeDistance
{
    public static class Settings
    {

        private static string FilePath;
        public static int MaxCollectedData { get; set; }
        public static int MaxCollectedSelectionable { get; set; }
        public static float CameraFixedHeight { get; set; }
        public static int CameraFov { get; set; }
        public static int CameraAngle { get; set; }
        public static int TeleportingDelay { get; set; }
        public static int RenderingDelay { get; set; }
        public static int PedSpawningDelay { get; set; }
        public static int CollectingDataDelay { get; set; }
        public static int ClearCollectingDataDelay { get; set; }
        public static bool SaveScreenShotLocally { get; set; }
        public static bool PrintBox { get; set; }
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
                fileSettings = GetDefaultSettings();
                System.IO.File.WriteAllText(FilePath, fileSettings);
            }
            AssignValueToSettings(fileSettings);
        }

        private static string GetDefaultSettings()
        {
            return @"MaxCollectedData=30
MaxCollectedSelectionable=5000
CameraMinSpawningHeight=4
CameraMaxSpawningHeight=15
CameraFixedHeight=0,8
CameraFov=30
CameraAngle=0
TeleportingDelay=5
RenderingDelay=6
PedSpawningDelay=1
CollectingDataDelay=3
ClearCollectingDataDelay=1
SaveScreenShotLocally=True
PrintBox=True
ImageFormat=Jpeg
DirectoryName=MachineLearningProject";
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
                    else if (key.Equals("CameraFixedHeight")) CameraFixedHeight = float.Parse(value);
                    else if (key.Equals("CameraFov")) CameraFov = int.Parse(value);
                    else if (key.Equals("CameraAngle")) CameraAngle = int.Parse(value);
                    else if (key.Equals("TeleportingDelay")) TeleportingDelay = int.Parse(value);
                    else if (key.Equals("RenderingDelay")) RenderingDelay = int.Parse(value);
                    else if (key.Equals("PedSpawningDelay")) PedSpawningDelay = int.Parse(value);
                    else if (key.Equals("CollectingDataDelay")) CollectingDataDelay = int.Parse(value);
                    else if (key.Equals("ClearCollectingDataDelay")) ClearCollectingDataDelay = int.Parse(value);
                    else if (key.Equals("SaveScreenShotLocally")) SaveScreenShotLocally = bool.Parse(value);
                    else if (key.Equals("PrintBox")) PrintBox = bool.Parse(value);
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
                "CameraFixedHeight=" + CameraFixedHeight.ToString().Trim(),
                "CameraFov=" + CameraFov.ToString().Trim(),
                "CameraAngle=" + CameraAngle.ToString().Trim(),
                "TeleportingDelay=" + TeleportingDelay.ToString().Trim(),
                "RenderingDelay=" + RenderingDelay.ToString().Trim(),
                "PedSpawningDelay=" + PedSpawningDelay.ToString().Trim(),
                "CollectingDataDelay=" + CollectingDataDelay.ToString().Trim(),
                "ClearCollectingDataDelay=" + ClearCollectingDataDelay.ToString().Trim(),
                "SaveScreenShotLocally=" + SaveScreenShotLocally.ToString().Trim(),
                "PrintBox=" + PrintBox.ToString().Trim(),
                "ImageFormat=" + ImageFormat.ToString().Trim(),
                "DirectoryName=" + DirectoryName.ToString().Trim()
            };

            System.IO.File.WriteAllLines(FilePath, lines);
        }

        public enum SettingToSave
        {
            NONE,
            MAX_COLLECTED_DATA,
            CAMERA_FIXED_HEIGHT,
            CAMERA_FOV,
            CAMERA_ANGLE,
            TELEPORTING_DELAY,
            RENDERING_DELAY,
            PED_SPAWNING_DELAY,
            COLLECTING_DATA_DELAY,
            CLEAR_COLLECTING_DATA_DELAY,
            SAVE_SCREENSHOT_LOCALLY,
            PRINT_BOX,
            IMAGE_FORMAT,
            COUNT
        }

        public static void SaveSetting(SettingToSave setting, dynamic value)
        {
            switch (setting)
            {
                case SettingToSave.CAMERA_ANGLE:
                    CameraAngle = value;
                    break;
                case SettingToSave.CAMERA_FOV:
                    CameraFov = value;
                    break;
                case SettingToSave.CAMERA_FIXED_HEIGHT:
                    CameraFixedHeight = value;
                    break;
                case SettingToSave.CLEAR_COLLECTING_DATA_DELAY:
                    ClearCollectingDataDelay = value;
                    break;
                case SettingToSave.COLLECTING_DATA_DELAY:
                    CollectingDataDelay = value;
                    break;
                case SettingToSave.MAX_COLLECTED_DATA:
                    MaxCollectedData = value;
                    break;
                case SettingToSave.IMAGE_FORMAT:
                    ImageFormat = value;
                    break;
                case SettingToSave.PED_SPAWNING_DELAY:
                    PedSpawningDelay = value;
                    break;
                case SettingToSave.PRINT_BOX:
                    PrintBox = value;
                    break;
                case SettingToSave.SAVE_SCREENSHOT_LOCALLY:
                    SaveScreenShotLocally = value;
                    break;
                case SettingToSave.TELEPORTING_DELAY:
                    TeleportingDelay = value;
                    break;
                    //altri
            }
        }
    }
}
