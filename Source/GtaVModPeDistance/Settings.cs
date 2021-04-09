
namespace GtaVModPeDistance
{
    public static class Settings
    {

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
        public static bool AdvancedMode { get; set; }
        public static string SaveScreenShotLocally { get; set; }
        public static string ImageFormat { get; set; }
        public static string DirectoryName { get; set; }

        public static void LoadSettings()
        {
            MaxCollectedSelectionable = Properties.Settings.Default.MaxCollectedSelectionable;
            MaxCollectedData = Properties.Settings.Default.MaxCollectedData;
            PedMinSpawningDistance = Properties.Settings.Default.PedMinSpawningDistance;
            PedMaxSpawningDistance = Properties.Settings.Default.PedMaxSpawningDistance;
            CameraMinSpawningHeight = Properties.Settings.Default.CameraMinSpawningHeight;
            CameraMaxSpawningHeight = Properties.Settings.Default.CameraMaxSpawningHeight;
            CameraFixedHeight = Properties.Settings.Default.CameraFixedHeight;
            TeleportingDelay = Properties.Settings.Default.TeleportingDelay;
            RenderingDelay = Properties.Settings.Default.RenderingDelay;
            PedSpawningDelay = Properties.Settings.Default.PedSpawningDelay;
            CollectingDataDelay = Properties.Settings.Default.CollectingDataDelay;
            ClearCollectingDataDelay = Properties.Settings.Default.ClearCollectingDataDelay;
            AdvancedMode = Properties.Settings.Default.AdvancedMode;
            SaveScreenShotLocally = Properties.Settings.Default.SaveScreenShotLocally;
            ImageFormat = Properties.Settings.Default.ImageFormat;
            DirectoryName = Properties.Settings.Default.DirectoryName;
        }

        public static void SaveSettings()
        {
            Properties.Settings.Default.MaxCollectedData = MaxCollectedData;
            Properties.Settings.Default.PedMinSpawningDistance = PedMinSpawningDistance;
            Properties.Settings.Default.PedMaxSpawningDistance = PedMaxSpawningDistance;
            Properties.Settings.Default.CameraMinSpawningHeight = CameraMinSpawningHeight;
            Properties.Settings.Default.CameraMaxSpawningHeight = CameraMaxSpawningHeight;
            Properties.Settings.Default.CameraFixedHeight = CameraFixedHeight;
            Properties.Settings.Default.TeleportingDelay = TeleportingDelay;
            Properties.Settings.Default.RenderingDelay = RenderingDelay;
            Properties.Settings.Default.PedSpawningDelay = PedSpawningDelay;
            Properties.Settings.Default.CollectingDataDelay = CollectingDataDelay;
            Properties.Settings.Default.ClearCollectingDataDelay = ClearCollectingDataDelay;
            Properties.Settings.Default.AdvancedMode = AdvancedMode;
            Properties.Settings.Default.SaveScreenShotLocally = SaveScreenShotLocally;
            Properties.Settings.Default.ImageFormat = ImageFormat;
            Properties.Settings.Default.Save();
        }

        public static void ToggleAdvancedMode()
        {
            AdvancedMode = !AdvancedMode;
            Properties.Settings.Default.AdvancedMode = AdvancedMode;
            Properties.Settings.Default.Save();
        }
    }
}
